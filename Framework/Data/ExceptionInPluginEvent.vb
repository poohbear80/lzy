
Namespace Data
    Public Class ExceptionInPluginEvent
        'Inherits EventBase

        Public Sub New(pre As String, exception As Exception)
            Me.Pre = pre
            Me.Exception = exception
        End Sub

        Public Property Exception As Exception
        Public Property Pre As String
    End Class
End NameSpace