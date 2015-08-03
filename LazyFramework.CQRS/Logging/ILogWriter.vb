Imports LazyFramework.CQRS.Command

Namespace Logging
    Public Interface ILogWriter
        Sub WriteCommand(cmd As CommandInfo, orginalcommand As IAmACommand)
        Sub WriteEvent(cmd As EventInfo)
        Sub WriteQuery(cmd As QueryInfo)
        Sub WriteError(erroInfo As ErrorInfo)
    End Interface
End Namespace
