Public Interface ISenderConnection
    ''' <summary>
    ''' This must be implementet. Should send the actual message to the configured msg system.
    ''' </summary>
    ''' <param name="message">This is the serialized envelope from LazyMessage</param>
    Sub Send(message As String)
End Interface