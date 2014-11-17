Namespace MessageHandling
    Public Class Sender
        
        Public Shared Sub Send(name As String, data As Object)
            Dim toSend = New MessageEnvelope
            toSend.Name = name
            toSend.Content = ClassFactory.GetTypeInstance(Of IObjectSerializer).Serialize(data)

            ClassFactory.GetTypeInstance(Of ITransportLayer).Send(toSend)

        End Sub
        

    End Class
End NameSpace
