Namespace MessageHandling
    Public Interface IAmAMessage
        Property Identifier As Guid
        Property Created As DateTime
        Property CreatedBy As String
        Property Name As String
        Property Content As String

    End Interface
End NameSpace
