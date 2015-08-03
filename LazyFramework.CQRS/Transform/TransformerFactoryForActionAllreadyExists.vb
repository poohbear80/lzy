Imports System.Runtime.Serialization

Namespace Transform
    <Serializable>
    Friend Class TransformerFactoryForActionAllreadyExists
        Inherits Exception

        Private key As Type
        Private t As Type
        Private transformerFactory As ITransformerFactory

        Public Sub New(key As Type, t As Type, transformerFactory As ITransformerFactory)

            MyBase.New(String.Format("Duplicate definition of transformer factory. Action:{0}, Found:{1}, Allready{2}", key.FullName, t.FullName, transformerFactory.GetType.FullName))
            Me.key = key
            Me.t = t
            Me.transformerFactory = transformerFactory


        End Sub

    End Class
End Namespace
