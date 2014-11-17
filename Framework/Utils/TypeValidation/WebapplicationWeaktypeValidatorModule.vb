Public Class WebapplicationWeaktypeValidatorModule
    Implements System.Web.IHttpModule

    Public Sub Dispose() Implements System.Web.IHttpModule.Dispose

    End Sub

    Public Sub Init(ByVal context As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
        Dim validate As List(Of TypeValidation.ICheckTypeProvider)

        
        Try
            validate = TypeValidation.ValidateApplicationWeakReferances
        Catch ex As Exception
            Return
        End Try
        
        For Each i In validate
            If Not i.Result Then
                Throw New NotImplementedException(i.Name & ":" & Join(i.MissingClasses.ToArray, "|"))
            End If
        Next

    End Sub
End Class
