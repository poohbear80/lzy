Imports System.Reflection

Namespace Logging
    Public Interface IDoEventLogging
        Sub LogSuccessEvent(e As IAmAnEvent, ByVal target As MethodInfo)
    End Interface
End NameSpace