Imports System.Security.Principal

Namespace CQRS.Logging
    Public Class QueryInfo
        Public Name As String
        Public Timestamp As String
        Public User As IIdentity
        Public Params As Object
    End Class
End NameSpace
