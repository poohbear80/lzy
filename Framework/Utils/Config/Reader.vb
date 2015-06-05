Imports System.IO

Namespace Utils.Config
    Public Class Reader

        Public Shared Function Load(Of T As New)() As T
            Return Load(Of T)(GetType(T).Name & ".json")
        End Function

        Public Shared Function Load(Of T As New)(path As String) As T
            Dim stream = System.IO.File.OpenRead(path)
            Return Load(Of T)(New StreamReader(stream))
        End Function

        Public Shared Function Load(Of T As New)(stream As StreamReader) As T

            Return Utils.Json.Reader.StringToObject(Of T)(stream)

        End Function

    End Class
End Namespace

