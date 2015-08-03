Namespace Dto
    Public MustInherit Class DtoBase
        Implements ISupportActionList

        Private ReadOnly _Actionlist As New List(Of IActionDescriptor)
        Public ReadOnly Property Actions As List(Of IActionDescriptor) Implements ISupportActionList.Actions
            Get
                Return _Actionlist
            End Get
        End Property
    End Class
End Namespace
