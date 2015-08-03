Imports System.Reflection

Namespace EventHandling
    Public Class DebugLogger
        Implements IDoEventLogging

        Public Sub LogSuccessEvent(e As IAmAnEvent, target As MethodInfo) Implements IDoEventLogging.LogSuccessEvent
            Debug.WriteLine(String.Format("{0}-{1}-{2}-{3}-{4}", e.GetType.ToString, e.Guid, e.TimeStamp, e.RunAsync, target.Module.Name & "." & target.Name))
        End Sub

    End Class
End Namespace
