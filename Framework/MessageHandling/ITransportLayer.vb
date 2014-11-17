Namespace MessageHandling
    Public Interface ITransportLayer
        Sub Send(msg As IAmAMessage)
        Sub Recieve(callback As ReciveMsg)
    End Interface

    Public Delegate Sub ReciveMsg(msg As String)

End NameSpace
