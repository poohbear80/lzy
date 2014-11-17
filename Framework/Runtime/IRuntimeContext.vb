Imports System.Security.Principal

Namespace Runtime
    Public Interface IContext
        Function CurrentUser() As IPrincipal
        Function Storage() As Dictionary(Of String, Object)

        Sub ContextSet()


    End Interface
    
End Namespace




