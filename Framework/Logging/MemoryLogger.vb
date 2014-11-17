Namespace Logging
    Public Class MemoryLogger
        Implements ILogger

        Public List As New List(Of LogInfoBase)
        Public Sub Log(infoBase As LogInfoBase) Implements ILogger.Log
            List.Add(infoBase)
        End Sub
    End Class
End NameSpace
