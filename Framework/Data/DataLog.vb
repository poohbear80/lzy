Imports LazyFramework.Logging

Public Class DataLog
    Inherits GenericLogEvent
    Public Sub New(msg As String)
        MyBase.New(msg)
    End Sub
End Class
