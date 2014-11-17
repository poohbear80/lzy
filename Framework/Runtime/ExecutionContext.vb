Imports System.Security.Principal

Namespace Runtime
    Public Class ExecutionContext
        <ThreadStatic> Public Shared CurrentUser As IIdentity
        <ThreadStatic> Public Shared Store As Dictionary(Of String, Object)
    End Class
End NameSpace
