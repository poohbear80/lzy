Namespace Runtime
    Public Class Context

        Private Shared _current As IContext
        Public Shared Property Current As IContext
            Get
                Return _current
            End Get
            Set(value As IContext)
                _current = value
            End Set
        End Property

        
    End Class

End NameSpace
