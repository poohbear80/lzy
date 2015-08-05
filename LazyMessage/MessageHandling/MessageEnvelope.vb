Namespace MessageHandling
    Public Class MessageEnvelope
        Implements IAmAMessage

        Public Sub New()
            _Created = Now
            _Identifier = Guid.NewGuid
        End Sub

        Public Property Created As Date Implements IAmAMessage.Created
        Public Property CreatedBy As String Implements IAmAMessage.CreatedBy
        Public Property Name As String Implements IAmAMessage.Name
        Public Property Identifier As Guid Implements IAmAMessage.Identifier
        Public Property Content As String Implements IAmAMessage.Content

    End Class
End Namespace
