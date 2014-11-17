Imports System.Web

Namespace Utils
    Public Class LazyFrameworkRequestModule
        Implements IHttpModule

        Public Sub Dispose() Implements IHttpModule.Dispose

        End Sub

        Public Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
            AddHandler context.BeginRequest, AddressOf StartRequest
            AddHandler context.EndRequest, AddressOf EndRequest
        End Sub

        Private Sub StartRequest(ByVal sender As Object, ByVal e As EventArgs)
            ResponseThread.Current.Clear()
        End Sub

        Private Sub EndRequest(ByVal sender As Object, ByVal e As EventArgs)
            ResponseThread.Current.Clear()
        End Sub
    End Class
End NameSpace
