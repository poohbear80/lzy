Imports System.Reflection

Namespace EventHandling
    Public Interface IDoEventLogging
        Sub LogSuccessEvent(e As IAmAnEvent, ByVal target As MethodInfo)
    End Interface
End Namespace
