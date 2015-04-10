Namespace Data
    Public Class FillStatus(Of T As New)
        Public Sub New(v As T)
            Value = v
        End Sub
        Public Sub New()
            Value = New T
        End Sub
        Public Property FillResult As FillResultEnum
        Public Property Timestamp As Long
        Public Property Value As T

        Public Shared Narrowing Operator CType(o As FillStatus(Of T)) As T
            Return o.Value
        End Operator

        Public Shared Widening Operator CType(o As T) As FillStatus(Of T)
            Return New FillStatus(Of T)(o)
        End Operator


    End Class
End Namespace