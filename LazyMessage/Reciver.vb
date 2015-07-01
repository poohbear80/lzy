Public Class Reciver
    Private ReadOnly _config As ReciverConfig

    Public sub New (config As ReciverConfig)
        _config = config
    End Sub

    Private _handlers As new Dictionary(Of String, Action(Of Object()))

    Public Sub  RegisterHandler(messageId As Guid , handler As Action(Of Object))
        If _handlers.ContainsKey(messageId.ToString) Then
            Throw New DuplicateNameException(messageId.ToString)
        End If

        _handlers.Add(messageId.ToString, handler)

    End Sub

    Public Sub  StartReciving()
        _config.Connection.OnMessage(AddressOf CanHandle, AddressOf MessageRecived)
    End Sub

    Public Function  CanHandle(message As String) As Boolean
        Dim envelope As Envelope
        envelope = LazyFramework.Utils.Json.Reader.StringToObject(Of Envelope)(message)
        Return _handlers.ContainsKey(envelope.Type)
    End Function

    Public Sub MessageRecived(message As String)
        Dim envelope As Envelope
        envelope = LazyFramework.Utils.Json.Reader.StringToObject(Of Envelope)(message)

        If _handlers.ContainsKey(envelope.Type) Then

        End If

    End Sub



End Class