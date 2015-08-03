Namespace Command
    Public Interface IHandleCommand

    End Interface

    Public Interface IHandleCommand(Of TCommand As IAmACommand)
        Inherits IHandleCOmmand

        Sub Handle(cmd As TCommand)

    End Interface

End Namespace
