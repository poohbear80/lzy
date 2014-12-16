
Imports LazyFramework.CQRS.Command

Namespace CQRS.EventHandling
    Public Class EventBase
        Implements IAmAnEvent

        Private _guid As Guid
        Private _endTimestamp As Long
        Private _TimeStamp As Long

        Public Sub New()
            _guid = Guid.NewGuid
            _TimeStamp = Now.Ticks
        End Sub
        
        Public Overridable ReadOnly Property RunAsync As Boolean Implements IAmAnEvent.RunAsync
            Get
                Return False
            End Get
        End Property

        Public Property CommandSource As IAmACommand Implements IAmAnEvent.CommandSource

        Public Function ActionName() As String Implements IActionBase.ActionName
            Return Me.GetType.FullName
        End Function

        Public Function IsAvailable() As Boolean Implements IActionBase.IsAvailable
            Return True
        End Function

        Public Function IsAvailable(user As Security.Principal.IPrincipal) As Boolean Implements IActionBase.IsAvailable
            Return True
        End Function

        Public Function IsAvailable(user As Security.Principal.IPrincipal, o As Object) As Boolean Implements IActionBase.IsAvailable
            Return True
        End Function

        Public Sub ActionComplete() Implements IAmAnAction.ActionComplete
            _endTimestamp = Now.Ticks
        End Sub

        Public Function EndTimeStamp() As Long Implements IAmAnAction.EndTimeStamp
            Return _endTimestamp
        End Function

        Public Function Guid() As Guid Implements IAmAnAction.Guid
            Return _guid
        End Function

        Public Function TimeStamp() As Long Implements IAmAnAction.TimeStamp
            Return _TimeStamp
        End Function

        Public Function User() As Security.Principal.IPrincipal Implements IAmAnAction.User
            Return Nothing
        End Function
    End Class

End Namespace
