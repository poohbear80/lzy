Imports LazyFramework.EventHandling

Namespace Query
    Public Class QueryLogEvent
        Inherits EventBase
        Private ReadOnly _amAQuery As IAmAQuery

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
