

Namespace CQRS.Transform
    Public Interface ISortingFunction
        Function SortingFunc() As Comparison(Of Object)
    End Interface

    Public Interface ITransformerFactory
        Inherits ISortingFunction
        Function GetTransformer(ByVal action As IAmAnAction, ByVal ent As Object) As ITransformEntityToDto
    End Interface
End Namespace
