Imports System.Security.Principal

Namespace Runtime
    Public Class Web
        Implements IContext
        
        Public Function CurrentUser() As IPrincipal Implements IContext.CurrentUser
            Return System.Web.HttpContext.Current.User
        End Function

        Public Function Storage() As Dictionary(Of String, Object) Implements IContext.Storage
            If System.Web.HttpContext.Current.Items(Constants.StoreName) Is Nothing Then
                System.Web.HttpContext.Current.Items(Constants.StoreName) = New Dictionary(Of String, Object)
            End If
            Return CType(System.Web.HttpContext.Current.Items(Constants.StoreName), Dictionary(Of String, Object))
        End Function

        Public Sub ContextSet() Implements IContext.ContextSet

        End Sub

        Public Property ChickenMode As Boolean = False Implements IContext.ChickenMode
    End Class
End NameSpace
