Imports System.Security.Principal

Namespace Runtime
    Public Class Web
        Implements IContext


        
        Public Function Storage() As IDictionary(Of String, Object) Implements IContext.Storage
            If System.Web.HttpContext.Current.Items(Constants.StoreName) Is Nothing Then
                System.Web.HttpContext.Current.Items(Constants.StoreName) = New Dictionary(Of String, Object)
            End If
            Return CType(System.Web.HttpContext.Current.Items(Constants.StoreName), Dictionary(Of String, Object))
        End Function

        Public Sub ContextSet() Implements IContext.ContextSet

        End Sub

        Public Property ChickenMode As Boolean = False Implements IContext.ChickenMode

        Public Property CurrentUser As IPrincipal Implements IContext.CurrentUser
            Get
                Return System.Web.HttpContext.Current.User
            End Get
            Set(value As IPrincipal)
                System.Web.HttpContext.Current.User = value
            End Set
        End Property
    End Class
End NameSpace
