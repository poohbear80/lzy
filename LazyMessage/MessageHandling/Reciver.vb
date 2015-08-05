Imports LazyFramework

Namespace MessageHandling
    Public Class Reciver

        Public Shared Sub Start()
            ClassFactory.GetTypeInstance(Of ITransportLayer).Recieve(AddressOf Recive)
        End Sub

        Private Shared Sub Recive(msg As MessageEnvelope)
            Dim type = ClassFactory.GetTypeInstance(Of INameMapping).GetType(msg.Name)
            Dim res As Object

            res = ClassFactory.GetTypeInstance(Of IObjectSerializer).Deserialize(type, msg.Content)

            ClassFactory.GetTypeInstance(Of IDispatchMessage).Dispatch(res)

        End Sub

        Public Shared Sub Recive(msg As String)

            'decode

            Recive(ClassFactory.GetTypeInstance(Of IObjectSerializer).Deserialize(Of MessageEnvelope)(msg))

        End Sub

    End Class
End Namespace
