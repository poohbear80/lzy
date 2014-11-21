Namespace Utils.Json
    Friend Class IntegerParser
        Inherits Builder

        Public Overrides Function Parse(nextChar As IReader) As Boolean

            While IsNumeric(nextChar.PeekToBuffer)

            End While
            InnerResult = Integer.Parse(nextChar.Buffer)
            Complete = True
        End Function

    End Class
End NameSpace