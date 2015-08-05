Imports System.Security.Principal
Imports LazyFramework.CQRS.EventHandling

Namespace Logging
    Public Class CommandInfo
        Public Name As String
        Public Type As String
        Public InstanceId As Guid
        Public User As IPrincipal
        Public StartTime As Int64
        Public EndTime As Int64
        Public State As String
        Public Data As Object
        Public EntityType As String
    End Class

    Public MustInherit Class BaseLogEvent
        Inherits EventBase

    End Class
End Namespace


