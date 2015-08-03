Namespace ActionContext
    Public MustInherit Class ActionContext
        Public ReadOnly Property Name As String
            Get
                Return Me.GetType.Name
            End Get
        End Property

        Public MustOverride ReadOnly Property Actions As IEnumerable(Of IActionBase)


    End Class

End Namespace