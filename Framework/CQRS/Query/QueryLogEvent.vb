Imports LazyFramework.CQRS.EventHandling

Namespace CQRS.Query
    Public Class QueryLogEvent
        Inherits EventBase
        Private _AmAQuery As IAmAQuery

        Public Sub New(q As IAmAQuery)
            _AmAQuery = q
        End Sub

        Public ReadOnly Property Query As IAmAQuery
            Get
                Return _AmAQuery
            End Get
        End Property
    End Class
End Namespace
