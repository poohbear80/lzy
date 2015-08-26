
Public Class LogInfo
    Public Sub New()
        TimeStamp = Now
    End Sub
    Public DataType As String
    Public TimeStamp As DateTime
    Public LogData As Object
End Class

Public Interface ILogWriter
    Sub Write(info As LogInfo)
    Function Level As LogLevelEnum
End Interface


