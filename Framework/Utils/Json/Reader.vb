Imports System.IO
Imports System.Text
Imports System.Reflection

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

            builder.Parse(New ReadStream(input))

            If builder.Complete Then
                Return builder.Result
            End If

            Throw New uncompleteException


        End Function

        Public Class ReadStream
            Implements IReader
            
            Private ReadOnly _StreamReader As StreamReader
            Private internaleBuffer As New Queue(Of Char)


            Public Sub New(s As StreamReader)
                _StreamReader = s
            End Sub

            Public Function Peek() As Char Implements IReader.Peek
                Dim ret = ChrW(_StreamReader.Read)
                internaleBuffer.Enqueue(ret)
                Return ret
            End Function

            Public Function Read() As Char Implements IReader.Read
                If internaleBuffer.Count = 0 Then
                    Return ChrW(_StreamReader.Read)
                Else
                    Return internaleBuffer.Dequeue
                End If
            End Function

            Public ReadOnly Property Buffer As String Implements IReader.Buffer
                Get

                    Dim ret As New StringBuilder
                    While internaleBuffer.Count > 1
                        ret.Append(internaleBuffer.Dequeue)
                    End While
                    Return ret.ToString
                End Get
            End Property

            Public ReadOnly Property PeekBuffer As String Implements IReader.PeekBuffer
                Get
                    Return New String(internaleBuffer.ToArray)
                End Get
            End Property

            Public Function Current() As Char Implements IReader.Current
                Return internaleBuffer(internaleBuffer.Count - 1)
            End Function
        End Class


        Public MustInherit Class Builder
            Public InnerResult As Object
            Private _Complete As Boolean = False
            Public MustOverride Function Parse(nextChar As IReader) As Boolean

            Public Property Complete As Boolean
                Get
                    Return _Complete
                End Get
                Protected Set(value As Boolean)
                    _Complete = value
                End Set
            End Property
        End Class

        Public MustInherit Class Builder(Of T As New)
            Inherits Builder
            Public Property Result As T
                Get
                    Return CType(InnerResult, T)
                End Get
                Set(value As T)
                    InnerResult = value
                End Set
            End Property
        End Class

        Public Class ObjectBuilder(Of T As New)
            Inherits Builder(Of T)

            Public Overrides Function Parse(nextChar As IReader) As Boolean
                TokenAcceptors.EatUntil(TokenAcceptors.ObjectStart, nextChar)
                Result = New T
                TokenAcceptors.Attributes(Result, nextChar)

                TokenAcceptors.EatUntil(TokenAcceptors.ObjectEnd, nextChar)
                Complete = True
            End Function
        End Class

    End Class

    Public Interface IReader
        Function Read() As Char
        Function Peek() As Char
        Function Current() As Char
        ReadOnly Property Buffer As String
        ReadOnly Property PeekBuffer As String
    End Interface
    
    Public Class TokenAcceptors
        Public Const ObjectStart = "{"c
        Public Const ObjectEnd = "}"c
        Public Const Qualifier = ":"c
        Public Const Separator = ","c

        Public Shared Sub ConsumeComments(nextChar As IReader)
            WhiteSpace(nextChar)

        End Sub

        Public Shared Sub EatUntil(c As Char, nextChar As IReader)
            WhiteSpace(nextChar)
            If nextChar.Read <> c Then
                Throw New MissingTokenException(c)
            End If
        End Sub

        Public Shared Sub WhiteSpace(nextchar As IReader)
            While AscW(nextchar.Peek) <= 32
                nextchar.Read()
            End While
        End Sub

        Public Shared Sub Quote(nextChar As IReader)
            If nextChar.Read <> Chr(34) Then
                Throw New MissingTokenException(Chr(34))
            End If
        End Sub

        Public Shared Function Attribute(nextChar As IReader) As String
            'Dim buffer As New StringBuilder
            Quote(nextChar)
            Dim w = AscW(nextChar.Peek)

            While (w > 64 AndAlso w < 91) OrElse (w > 96 AndAlso w < 123) 'This is A-Z a-z  The only characters allowed in attribute names.
                w = AscW(nextChar.Peek)
            End While
            Dim ret = nextChar.Buffer
            Quote(nextChar)
            Return ret
        End Function

        Public Shared Sub Attributes(ByVal result As Object, ByVal nextChar As IReader)
            Do
                WhiteSpace(nextChar)
                Dim name = Attribute(nextChar)

                EatUntil(Qualifier, nextChar)

                Dim fInfo = result.GetType().GetField(name)

                If fInfo.FieldType.IsValueType Or fInfo.FieldType Is GetType(String) Then
                    If fInfo.FieldType Is GetType(String) Then
                        fInfo.SetValue(result, ParseText(nextChar))
                    End If
                End If
            Loop While CanFindValueSeparator(nextChar)
        End Sub

        Private Shared Function CanFindValueSeparator(ByVal nextChar As IReader) As Boolean
            WhiteSpace(nextChar)
            If nextChar.Peek = "," Then
                nextChar.Read()
                Return True
            End If
            Return False
        End Function

        Private Shared Function ParseText(ByVal nextChar As IReader) As String
            Dim buffer As New StringBuilder
            WhiteSpace(nextChar)
            Quote(nextChar)

            Dim w = nextChar.Peek
            While (w <> Chr(34))
                If w = "\" Then
                    buffer.Append(nextChar.Buffer)
                    buffer.Append(ParseTextEscape(nextChar))
                    w = nextChar.Current
                    Continue While
                End If
                w = nextChar.Peek
            End While

            If nextChar.PeekBuffer.Length > 1 Then
                buffer.Append(nextChar.Buffer)
            End If

            Quote(nextChar)

            Return buffer.ToString
        End Function


        Private Shared ReadOnly Match As New Dictionary(Of String, String) From {
            {"\\", "\"},
            {"\n", vbCrLf}
        }

        Private Shared Function ParseTextEscape(ByVal nextChar As IReader) As String
            nextChar.Peek()
            While Match.ContainsKey(nextChar.PeekBuffer)
                nextChar.Peek()
            End While
            Dim key = nextChar.Buffer
            Return Match(key)
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