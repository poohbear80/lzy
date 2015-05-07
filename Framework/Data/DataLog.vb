Imports LazyFramework.Logging

Namespace Data

    Public Class DataLog
        Inherits GenericLogEvent
        Public Sub New(msg As String)
            MyBase.New(msg)
        End Sub
    End Class
End Namespace