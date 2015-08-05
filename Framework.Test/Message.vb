Imports LazyFramework.CQRS
Imports LazyFramework.CQRS.Command
Imports LazyMessage.MessageHandling
Imports NUnit.Framework

<TestFixture> Public Class Message

    <SetUp> Public Sub SetUp()
        Runtime.Context.Current = New Runtime.WinThread
        LazyFramework.ClassFactory.Clear()
        Dim serializer = New MessageConcrete

        LazyFramework.ClassFactory.SetTypeInstance(Of IMessageHandling)(serializer)
        LazyFramework.ClassFactory.SetTypeInstance(Of IActionSecurity)(New TestSecurity)

    End Sub

    <TearDown> Public Sub Tear()
        LazyFramework.ClassFactory.Clear()
    End Sub

    <Test> Public Sub First()

        MessageHandling.Sender.Send("opw_mycommand", New MyCommand)

    End Sub

    <Test> Public Sub Second()
        Dim recived As New MessageHandling.MessageEnvelope

        MessageHandling.Reciver.Start()

    End Sub

    Public Class DustCommand
        Inherits CommandBase

        Public Command As String
        Public Id As Integer

        Public Overrides Function ActionName() As String

            Return "jkjklj"

        End Function
    End Class

    Public Class MyCommand
        Public Command As String = "kljskdlf"
        Public Id As Integer = 1
    End Class


End Class


Public Class CommandHandlerTest
    Implements IHandleCommand

    Public Shared Sub SoSomething(data As Message.DustCommand)
        Debug.WriteLine("lkjskdf")
    End Sub

End Class


Public Class MessageConcrete
    Implements IMessageHandling

    Private _Callback As ReciveMsg

    Public Sub Send(msg As MessageHandling.IAmAMessage) Implements MessageHandling.ITransportLayer.Send
        Debug.WriteLine(msg.Name)
    End Sub

    Public Sub Recieve(callback As MessageHandling.ReciveMsg) Implements MessageHandling.ITransportLayer.Recieve
        _Callback = callback

        'Ta i mot melding
        Dim msg As New MessageEnvelope
        msg.Content = Newtonsoft.Json.JsonConvert.SerializeObject(New Message.MyCommand)

        _Callback.Invoke(Newtonsoft.Json.JsonConvert.SerializeObject(msg))

    End Sub

    Public Function Serialize(data As Object) As String Implements MessageHandling.IObjectSerializer.Serialize
        Return Newtonsoft.Json.JsonConvert.SerializeObject(data)
    End Function

    Public Function DeSerialize(type As Type, data As String) As Object Implements IObjectSerializer.Deserialize
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(data, type)
    End Function

    Public Function DeSerialize(Of T)(ByVal msg As String) As T Implements IObjectSerializer.Deserialize
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(Of T)(msg)
    End Function

    Public Sub Dispatch(o As Object) Implements IDispatchMessage.Dispatch
        Debug.WriteLine(o.GetType.ToString)

        Handling.ExecuteCommand(CType(o, IAmACommand))

    End Sub

    Public Function [GetType](name As String) As Type Implements INameMapping.GetType

        Return GetType(Message.DustCommand)

    End Function
End Class


Public Class TransportOverride
    Implements ITransportLayer

    Public Sub Recieve(callback As ReciveMsg) Implements ITransportLayer.Recieve

    End Sub

    Public Sub Send(msg As IAmAMessage) Implements ITransportLayer.Send

    End Sub
End Class

