Imports System.ServiceModel

Namespace Utils

    Public Class ServiceBag
        Implements IExtension(Of OperationContext)

        Public Sub Attach(ByVal owner As OperationContext) Implements IExtension(Of OperationContext).Attach

        End Sub

        Public Sub Detach(ByVal owner As OperationContext) Implements IExtension(Of OperationContext).Detach

        End Sub

        Public ReadOnly Property AllItems As Dictionary(Of String, Object)
            Get
                Return _Items
            End Get
        End Property

        Private ReadOnly _Items As New Dictionary(Of String, Object)

        Default Public Property Items(o As String) As Object
            Get
                Return _Items(o)
            End Get
            Set(value As Object)
                _Items(o) = value
            End Set
        End Property
    End Class
End Namespace
