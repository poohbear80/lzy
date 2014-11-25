Imports System.Reflection
Imports LazyFramework.CQRS.EventHandling
Imports LazyFramework.Utils

Namespace CQRS.Command
    Public Class Handling
        Implements IPublishEvent

        Private Shared ReadOnly PadLock As New Object
        Private Shared _handlers As Dictionary(Of Type, List(Of MethodInfo))
        Private Shared _commadList As Dictionary(Of String, Type)


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property CommandList As Dictionary(Of String, Type)
            Get
                If _commadList Is Nothing Then
                    SyncLock PadLock
                        If _commadList Is Nothing Then
                            Dim temp As New Dictionary(Of String, Type)
                            For Each t In TypeValidation.FindAllClassesOfTypeInApplication(GetType(IAmACommand))
                                If t.IsAbstract Then Continue For 'Do not map abstract commands. 

                                Dim c As IAmACommand = CType(Activator.CreateInstance(t), IAmACommand)
                                temp.Add(c.ActionName, t)
                            Next
                            _commadList = temp
                        End If
                    End SyncLock
                End If

                Return _commadList
            End Get
        End Property


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared ReadOnly Property AllHandlers() As Dictionary(Of Type, List(Of MethodInfo))
            Get
                If _handlers Is Nothing Then
                    Dim temp As Dictionary(Of Type, List(Of MethodInfo)) = Nothing
                    SyncLock PadLock
                        If _handlers Is Nothing Then
                            temp = FindHandlers.FindAllHandlerDelegates(Of IHandleCommand, IAmACommand)(False)
                        End If
                        _handlers = temp
                    End SyncLock
                End If
                Return _handlers
            End Get
        End Property

        ''' <summary>
        ''' Executes a command by finding the mapping to the type of command passed in. 
        ''' </summary>
        ''' <param name="command"></param>
        ''' <remarks>Any command can have only 1 handler. An exception will be thrown if there is found more than one for any given command. </remarks>
        <PublishesEventOfType(GetType(NoAccess), GetType(HandlerNotFound), GetType(CommandHasBeenExecutedEvent))>
        Public Shared Sub ExecuteCommand(command As IAmACommand)

            If AllHandlers.ContainsKey(command.GetType) Then
                If TypeOf (command) Is CommandBase Then
                    If Not IsCommandAvailable(CType(command, CommandBase)) Then
                        EventHub.Publish(New NoAccess(command))
                        Throw New ActionIsNotAvailableException(command, command.User)
                    End If
                End If
                If Not CanUserRunCommand(CType(command, CommandBase)) Then
                    EventHub.Publish(New NoAccess(command))
                    Throw New ActionSecurityAuthorizationFaildException(command, command.User)
                End If

                Validation.Handling.ValidateAction(command)

                Try
                    Dim temp = AllHandlers(command.GetType)(0).Invoke(Nothing, {command})
                    If temp IsNot Nothing Then
                        command.SetResult(Transform.Handling.TransformResult(command, temp))
                    End If
                Catch ex As TargetInvocationException
                    EventHub.Publish(New CommandFailureEvent(command))
                    Logging.Log.Error(command, ex)
                    Throw ex.InnerException
                Catch ex As Exception
                    Logging.Log.Error(command, ex)
                    Throw
                End Try
            Else
                Dim notImplementedException As NotImplementedException = New NotImplementedException(command.ActionName)
                Logging.Log.Error(command, notImplementedException)
                Throw notImplementedException
            End If


            command.ActionComplete()
            Logging.Log.Command(command)
        End Sub

        Public Shared Function IsCommandAvailable(cmd As CommandBase) As Boolean
            Return cmd.IsAvailable()
        End Function

        Public Shared Function CanUserRunCommand(cmd As CommandBase) As Boolean
            Return ActionSecurity.Current.UserCanRunThisAction(cmd.User, cmd)
        End Function


    End Class
End Namespace
