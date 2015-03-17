Imports System.Security.Principal

Namespace Runtime
    Public Interface IContext
        Property CurrentUser() As IPrincipal
        Function Storage() As Dictionary(Of String, Object)

        Sub ContextSet()
        
        Property ChickenMode() As Boolean

    End Interface
    
End Namespace




