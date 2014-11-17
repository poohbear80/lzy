Imports System.Messaging

Namespace Events

    ''' <summary>
    ''' Dette er bare et marker interface som gjør det mulig å plukke opp alle klasser av denne typen
    ''' og få satt opp eventhandlere
    ''' </summary>
    ''' <remarks></remarks>
    <Obsolete> Public Interface IInitEvents
    End Interface

    <Obsolete> Public MustInherit Class EventHandlerType(Of TDataType)
        Implements IInitEvents

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")> Protected Sub New()
            AddEventHandler(EventType)
        End Sub


        Public Property InvokeCount As Integer
        Public MustOverride Function HandleEvent(data As TDataType) As Boolean
        Public MustOverride ReadOnly Property EventType As EventType(Of TDataType)


        Public Sub Invoke(data As TDataType)
            InvokeCount += 1
            HandleEvent(data)
        End Sub

        Public Overridable ReadOnly Property [Async]() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Sub AddEventHandler(e As EventType(Of TDataType))
            e.Subscribe(Me)
        End Sub
    End Class

    <Obsolete> Public Class EventType(Of TT)

        Public Shared Function Create() As EventType(Of TT)
            Return New EventType(Of TT)
        End Function


        Private Sub New()
        End Sub


        Private _subscribers As List(Of EventHandlerType(Of TT))
        Public ReadOnly Property Subcribers As List(Of EventHandlerType(Of TT))
            Get
                If _subscribers Is Nothing Then
                    _subscribers = New List(Of EventHandlerType(Of TT))
                End If
                Return _subscribers
            End Get
        End Property


        Public Sub Subscribe(f As EventHandlerType(Of TT))
            Subcribers.Add(f)
        End Sub

        Public Function Publish(data As TT) As Boolean

            For Each l In Subcribers 'her kan vi gjøre noe lureri for å gjøre dette mere paralellt...
                Try
                    If l.Async Then

                        Dim a As New Handler(Of TT)(AddressOf l.Invoke)
                        a.BeginInvoke(data, Sub(o) o.AsyncWaitHandle.Close(), LazyFramework.ClassFactory.Session)
                    Else
                        l.Invoke(data)
                    End If

                Catch ex As Exception

                End Try
            Next
            Return True
        End Function

    End Class

    <Obsolete> Public Delegate Sub Handler(Of TT)(data As TT)

    <Obsolete> Public Class Init

        Private Shared _allListeners As New List(Of IInitEvents)


        Private Shared ReadOnly Lock As New Object
        Private Shared _hasRun As Boolean = False

        Public Shared ReadOnly Property AllListeners As List(Of IInitEvents)
            Get
                Return _allListeners
            End Get
        End Property

        Public Shared Sub Run()
            If Not _hasRun Then
                _hasRun = True
                SyncLock Lock
                    TypeValidation.FindAllClassesOfTypeInApplication(GetType(IInitEvents)).ForEach(Sub(t As Type)
                                                                                                       Dim i = CType(Activator.CreateInstance(t), IInitEvents)
                                                                                                       _allListeners.Add(i)
                                                                                                   End Sub)
                End SyncLock
            End If
        End Sub
    End Class

    

End Namespace
