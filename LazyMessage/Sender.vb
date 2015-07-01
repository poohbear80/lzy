
''' <summary>
''' This class is for sending messages. 
''' </summary>
Public Class Sender
    Private ReadOnly _config As SenderConfig

    Private Connection As ISenderConnection


    Public Sub  new(config As SenderConfig)
        _config = config
        Connection = config.CreateConnection
    End Sub
    
    Public Sub SendMessage(messageId As Guid, d As Object)
        
        Dim toSend As New Envelope
        toSend.UniqueId = Guid.NewGuid().ToString
        toSend.Type = messageId.ToString
        
        toSend.Sender = _config.SystemName

        toSend.Content = LazyFramework.Utils.Json.Writer.ObjectToString(d)

        Connection.Send(LazyFramework.Utils.Json.Writer.ObjectToString(toSend))

    End Sub

End Class