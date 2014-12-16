Imports System.Security.Principal

Namespace Runtime
    Public Class TestContext
        Implements IContext

        Public Items As New Dictionary(Of String, Object)
        Private _Principal As IPrincipal

        Public Sub New()
        End Sub

        Public Sub New(p As IPrincipal)
            _Principal = p
        End Sub

        Public Function CurrentUser() As IPrincipal Implements IContext.CurrentUser
            Return _Principal
        End Function

        Public Function Storage() As Dictionary(Of String, Object) Implements IContext.Storage
            If Not Items.ContainsKey(Constants.StoreName) Then
                Items.Add(Constants.StoreName, New Dictionary(Of String, Object))
            End If
            Return CType(Items(Constants.StoreName), Dictionary(Of String, Object))

        End Function

        Public Sub ContextSet() Implements IContext.ContextSet

        End Sub

        Public Property ChickenMode As Boolean = False Implements IContext.ChickenMode
    End Class
End NameSpace
