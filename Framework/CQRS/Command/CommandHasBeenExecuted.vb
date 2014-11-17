Namespace CQRS.Command

    Public MustInherit Class CommandLogEvent
        Inherits BaseLogEvent

        Public Sub New(cmd As IAmACommand)
            Me.CommandSource = cmd
        End Sub
    End Class

    Public Class CommandHasBeenExecutedEvent
        Inherits CommandLogEvent

        Public Sub New(cmd As IAmACommand)
            MyBase.New(cmd)
        End Sub

    End Class

    Public Class CommandFailureEvent
        Inherits CommandLogEvent

        Public Sub New(cmd As IAmACommand)
            MyBase.New(cmd)
        End Sub

    End Class

End Namespace
