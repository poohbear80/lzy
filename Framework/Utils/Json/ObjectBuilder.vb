Namespace Utils.Json
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
End NameSpace