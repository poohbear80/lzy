Namespace Utils.Json
    Public MustInherit Class Builder
        Public MustOverride Function Parse(nextChar As IReader) As Object
    End Class

    Public MustInherit Class Builder(Of T As New)
        Inherits Builder
        
    End Class
End NameSpace