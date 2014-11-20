Namespace Utils.Json
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
            While AscW(nextchar.PeekToBuffer) <= 32
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
            Dim w = AscW(nextChar.PeekToBuffer)

            While (w > 64 AndAlso w < 91) OrElse (w > 96 AndAlso w < 123) 'This is A-Z a-z  The only characters allowed in attribute names.
                w = AscW(nextChar.PeekToBuffer)
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
                        Dim stringParser As StringParser = New StringParser
                        stringParser.Parse(nextChar)

                        fInfo.SetValue(result, stringParser.InnerResult)
                    End If
                End If
            Loop While CanFindValueSeparator(nextChar)
        End Sub

        Private Shared Function CanFindValueSeparator(ByVal nextChar As IReader) As Boolean
            WhiteSpace(nextChar)
            If nextChar.PeekToBuffer = "," Then
                nextChar.Read()
                Return True
            End If
            Return False
        End Function

    End Class
End NameSpace