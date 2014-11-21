Namespace Utils.Json
    Public MustInherit Class Builder
        Public InnerResult As Object
        Private _Complete As Boolean = False

        Public MustOverride Function Parse(nextChar As IReader) As Boolean

        Public Property Complete As Boolean
            Get
                Return _Complete
            End Get
            Protected Set(value As Boolean)
                _Complete = value
            End Set
        End Property
    End Class

    Public MustInherit Class Builder(Of T As New)
        Inherits Builder
        Public Property Result As T
            Get
                Return CType(InnerResult, T)
            End Get
            Set(value As T)
                InnerResult = value
            End Set
        End Property
    End Class
End NameSpace