Public MustInherit Class GenericBaseAggregate(Of TInterface, TDefaultType As {TInterface, New})
    Inherits LazyBaseAggregate

    Private ReadOnly _Repository As TInterface = ClassFactory.GetTypeInstance(Of TInterface, TDefaultType)()
    Public ReadOnly Property Repository As TInterface
        Get
            Return _Repository
        End Get
    End Property
End Class

