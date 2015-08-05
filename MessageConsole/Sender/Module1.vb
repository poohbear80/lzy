Imports LazyFramework.Message

Module Module1

    Sub Main()

        Dim m As New GeneratePaySlips

        m.Id = 1
        m.Message = "Dette er en teste."

        Dim mysender As New LazyFramework.Message.Sender(New Config)


        mysender.SendMessage(New Guid("0DD42853-2E40-4C29-A88A-9578C5697B6E"), m)

    End Sub

End Module



Public Class Config
    Inherits LazyFramework.Message.SenderConfig

    Public Overrides ReadOnly Property SystemName As String
        Get
            Return "TEST CONSOLE"
        End Get
    End Property

    Public Overrides Function CreateConnection() As ISenderConnection
        Return New LazyMessage.MSMQ.Sender
    End Function
End Class

Public Class GeneratePaySlips
    Public Id As Integer
    Public Message As String
End Class