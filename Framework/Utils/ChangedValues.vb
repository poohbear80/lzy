<Serializable()> _
Public NotInheritable Class ChangedValue
    Implements IComparable(Of String)

    Public FieldName As String
    Public value As Object

    Public Sub New()

    End Sub

    Public Sub New(ByVal Name As String, ByVal V As Object)
        FieldName = Name
        value = V
    End Sub

    Public Function CompareTo(ByVal other As String) As Integer Implements IComparable(Of String).CompareTo
        Return StrComp(FieldName, other, CompareMethod.Text)
    End Function
End Class

<Serializable()> _
Public NotInheritable Class ChangedValueCollection
    Inherits List(Of ChangedValue)
End Class
