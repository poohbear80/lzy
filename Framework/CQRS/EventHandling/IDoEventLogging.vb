Imports System.Reflection

Namespace CQRS.EventHandling
    Public Interface IDoEventLogging
        Sub LogSuccessEvent(e As IAmAnEvent, ByVal target As MethodInfo)
    End Interface
End Namespace
