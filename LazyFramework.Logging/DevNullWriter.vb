Imports LazyFramework.Logging

Friend Class DevNullWriter
    Implements ILogWriter

    Public Sub Write(info As LogInfo) Implements ILogWriter.Write
        
    End Sub

    Public Function Level() As LogLevelEnum Implements ILogWriter.Level
        Return 0
    End Function
End Class


Public Class MemoryWriter
    Implements ILogWriter

    Public LogEntries As New List(Of Object)

    Public Sub Write(info As LogInfo) Implements ILogWriter.Write
        
        LogEntries.Add(info)

    End Sub

    Public Function Level() As LogLevelEnum Implements ILogWriter.Level
        Return LogLevelEnum.Verbose
    End Function
End Class