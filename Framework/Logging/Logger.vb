Imports LazyFramework.CQRS.EventHandling

Namespace Logging
    Public Class Logger
        Public Shared Property LogLevel As Integer = -1

        Public Shared Sub Log(level As Integer, log As LogInfoBase)
            If level > LogLevel Then
                Return
            End If


            log.Caller = New StackFrame(1, True).GetMethod().Name
            log.Level = level
            log.Thread = Threading.Thread.CurrentThread.ManagedThreadId
            log.User = Threading.Thread.CurrentPrincipal.Identity.Name
            log.Message = log.Message

            If ClassFactory.ContainsKey(Of ILogger)() Then
                ClassFactory.GetTypeInstance(Of ILogger).Log(log)
            End If

            EventHub.Publish(log)
        End Sub
    End Class
End Namespace
