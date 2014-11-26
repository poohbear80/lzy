Namespace Utils.Json
    Public Class ObjectBuilder(Of T As New)
        Inherits Builder(Of T)

        Public Overrides Function Parse(nextChar As IReader) As Object
            TokenAcceptors.EatUntil(TokenAcceptors.ObjectStart, nextChar)
            Dim Result = New T
            TokenAcceptors.Attributes(Result, nextChar)

            TokenAcceptors.EatUntil(TokenAcceptors.ObjectEnd, nextChar)

            Return Result
        End Function
    End Class
End NameSpace