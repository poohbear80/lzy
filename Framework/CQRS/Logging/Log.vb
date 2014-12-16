
Imports LazyFramework.CQRS.Command
Imports LazyFramework.CQRS.EventHandling

Namespace CQRS.Logging
    Public Class Log

        Private Class DevNullWriter
            Implements ILogWriter

            Public Sub WriteCommand(cmd As CommandInfo) Implements ILogWriter.WriteCommand

            End Sub

            Public Sub WriteEvent(cmd As EventInfo) Implements ILogWriter.WriteEvent

            End Sub

            Public Sub WriteQuery(cmd As QueryInfo) Implements ILogWriter.WriteQuery

            End Sub

            Public Sub WriteError(erroInfo As ErrorInfo) Implements ILogWriter.WriteError

            End Sub
        End Class


        Private Shared ReadOnly PadLock As New Object
        Private Shared _writer As ILogWriter

        Public Shared ReadOnly Property Writer As ILogWriter
            Get
                If _writer Is Nothing Then
                    Debug.Print("Writer is nothing")
                    SyncLock PadLock
                        If _writer Is Nothing Then
                            _writer = ClassFactory.GetTypeInstance(Of ILogWriter, DevNullWriter)()
                        End If
                    End SyncLock
                End If
                Return _writer
            End Get
        End Property

        Public Shared Sub Command(e As IAmACommand)
            Dim input As New CommandInfo

            input.Name = e.ActionName
            input.Type = e.GetType.FullName
            input.InstanceId = e.Guid
            input.User = e.User
            input.StartTime = e.TimeStamp
            input.EndTime = e.EndTimeStamp
            input.Data = e
            If TypeOf (e) Is CommandBase Then
                If Not DirectCast(e, CommandBase).GetInnerEntity Is Nothing Then
                    input.EntityType = DirectCast(e, CommandBase).GetInnerEntity.GetType.FullName
                End If
            End If


            Writer.WriteCommand(input)
        End Sub

        Public Shared Sub [Event](ByVal e As IAmAnEvent)
            Dim input As New EventInfo

            input.Timestamp = e.TimeStamp
            input.EventId = e.Guid

            If e.CommandSource IsNot Nothing Then
                input.CommandData = e.CommandSource
                input.SourceCommand = e.CommandSource.Guid
            End If

            Writer.WriteEvent(input)
        End Sub

        Public Shared Sub Query(query As Query.IAmAQuery)
            Dim input As New QueryInfo
            Writer.WriteQuery(input)
        End Sub


        Public Shared Sub [Error](action As IAmAnAction, ex As Exception)
            Dim input As New ErrorInfo

            input.ActionType = If(TypeOf (action) Is IAmACommand, "Command", If(TypeOf (action) Is CQRS.Query.IAmAQuery, "Query", "Event"))
            input.Source = action.ActionName
            input.SourceGuid = action.Guid
            input.Message = ex.Message
            input.Type = ex.GetType.FullName
            input.Params = action
            Writer.WriteError(input)

        End Sub

    End Class
End Namespace
