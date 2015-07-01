Public Interface IReciveConnection

    Sub OnMessage(canHandle As Func(Of String, Boolean), handle As Action(Of String))
    Sub UnknownMessageId(msg As String)

End Interface