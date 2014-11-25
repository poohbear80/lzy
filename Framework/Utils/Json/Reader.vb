Imports System.IO

Namespace Utils.Json
    Public Class Reader

        Public Shared Function StringToObject(Of T As New)(input As String) As T
            Dim mem As New MemoryStream
            Dim write As New StreamWriter(mem, Text.Encoding.UTF8)
            write.Write(input)
            write.Flush()
            write.BaseStream.Position = 0
            Return StringToObject(Of T)(New StreamReader(mem, System.Text.Encoding.UTF8))
        End Function

        'Here we can add another function that accepts stream as parameter
        Public Shared Function StringToObject(Of T As New)(input As StreamReader) As T
            Dim builder As New ObjectBuilder(Of T)

            Return DirectCast(builder.Parse(New ReadStream(input)), T)

        End Function
    End Class


    Friend Class PropertyNotFoundException
        Inherits Exception

        Public Sub New(ByVal result As String)
            MyBase.New(result)
        End Sub
    End Class

    Public Class UncompleteException
        Inherits Exception

    End Class

    Public Class MissingTokenException
        Inherits Exception

        Private ReadOnly _S As String

        Public Sub New(ByVal s As String)
            MyBase.New(s)
            _S = s
        End Sub

        Public ReadOnly Property Token As String
            Get
                Return _S
            End Get
        End Property
    End Class
End Namespace