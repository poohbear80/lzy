Namespace Utils.Json
    Friend Class ArrayBuilder
        Inherits Builder

        Public Sub New(t As Type)
            MyBase.New(t)
        End Sub

        Public Overrides Function Parse(nextChar As IReader) As Object
            TokenAcceptors.EatUntil(TokenAcceptors.ListStart, nextChar)
            Dim res = Activator.CreateInstance(type)
            Dim innerType As Type

            If type.IsGenericType Then
                innerType = type.GetGenericArguments(0)
            Else
                Throw New NonGenericListIsNotSupportedException
            End If


            Do
                If innerType.IsValueType Or innerType = GetType(String) Then
                    Dim value = TokenAcceptors.TypeParserMapper(innerType).Parse(nextChar)
                    CType(res, IList).Add(value)
                Else
                    Dim v As Object = Reader.StringToObject(nextChar, innerType)
                    If v IsNot Nothing Then
                        CType(res, IList).Add(v)
                    End If
                End If
            Loop While TokenAcceptors.CanFindValueSeparator(nextChar)

            TokenAcceptors.EatUntil(TokenAcceptors.ListEnd, nextChar)

            Return res
        End Function
    End Class
End Namespace
