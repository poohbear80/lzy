
Public Class ACtionContext
    Inherits LazyFramework.CQRS.ActionContext.ActionContext
    
    Public Overrides ReadOnly Iterator Property Actions As IEnumerable(Of CQRS.ActionBase)
        Get
            Yield New MockAction

        End Get
    End Property
End Class

Public Class MockAction
    Inherits CQRS.ActionBase
    Public Overloads Overrides Function IsAvailable() As Boolean

    End Function

    Public Overloads Overrides Function IsAvailable(user As Security.Principal.IPrincipal) As Boolean

    End Function

    Public Overloads Overrides Function IsAvailable(user As Security.Principal.IPrincipal, o As Object) As Boolean

    End Function
End Class