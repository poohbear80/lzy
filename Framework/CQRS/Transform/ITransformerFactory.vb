

Namespace CQRS.Transform
    Public Interface ITransformerFactory
        Function GetTransformer(ByVal action As IAmAnAction, ByVal ent As Object) As ITransformEntityToDto
        Function SortingFunc() As Comparison(Of Object)
    End Interface
End Namespace
