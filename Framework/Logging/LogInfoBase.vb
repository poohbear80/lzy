Imports LazyFramework.CQRS.EventHandling

Namespace Logging
    Public Class LogInfoBase
        Inherits EventBase

        Public Sub New(msg As String)
            Message = msg
        End Sub

        Public Caller As String
        Public Message As String
        Public User As String
        Public Level As Integer
        Public Thread As Integer

    End Class

    Public Class GenericLogEvent
        Inherits LogInfoBase
        Public Sub New()
            MyBase.New("")
        End Sub


        Public Sub New(msg As String)
            MyBase.New(msg)
        End Sub

    End Class

End Namespace
