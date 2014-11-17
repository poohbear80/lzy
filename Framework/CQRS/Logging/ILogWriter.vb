Namespace CQRS.Logging
    Public Interface ILogWriter
        Sub WriteCommand(cmd As CommandInfo)
        Sub WriteEvent(cmd As EventInfo)
        Sub WriteQuery(cmd As QueryInfo)
        Sub WriteError(erroInfo As ErrorInfo)
    End Interface
End NameSpace
