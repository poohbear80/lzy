Namespace Utils.Json
    Public Class DateParser
        Inherits Builder

        Public Overrides Function Parse(nextChar As IReader) As Object
            TokenAcceptors.WhiteSpace(nextChar)
            TokenAcceptors.BufferLegalCharacters(nextChar, "0123456789.:T+ ")



        End Function
    End Class
End NameSpace