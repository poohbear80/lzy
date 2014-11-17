Namespace CQRS.Query
    Public MustInherit Class ParalellQueryBase(Of TQuery As IAmAQuery, TResult As New)
        Implements IParalellQuery

        Public Query As TQuery
        Public Result As TResult

        Public Sub New()
            Result = New TResult
        End Sub


        Friend Property InnerQuery As Object Implements IParalellQuery.InnerQuery
            Set(value As Object)
                Query = CType(value, TQuery)
            End Set
            Get
                Return Query
            End Get
        End Property

        Public ReadOnly Property InnerResult As Object Implements IParalellQuery.InnerResult
            Get
                Return Result
            End Get
        End Property
    End Class

    Public Interface IParalellQuery
        ReadOnly Property InnerResult As Object
        Property InnerQuery As Object
    End Interface

End Namespace
