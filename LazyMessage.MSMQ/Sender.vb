Imports System.Messaging
Imports LazyFramework.Message

Public Class Sender
    Implements ISenderConnection

    Private _messageQueue as MessageQueue = New Messaging.MessageQueue(".\private$\SystemMessage")

    Public Sub Send(message As String) Implements ISenderConnection.Send

        Dim msg As New Messaging.Message

        msg.Label = "TESTING"
        msg.Body = message

        _messageQueue.Send(msg)
    End Sub
End Class

