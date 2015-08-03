Namespace Command
    Public Interface IAmACommand
        Inherits IAmAnAction

        Function Result() As Object
        Sub SetResult(o As Object)
    End Interface
End Namespace
