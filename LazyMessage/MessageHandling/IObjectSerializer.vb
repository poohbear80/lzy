Namespace MessageHandling
    Public Interface IObjectSerializer
        Function Serialize(data As Object) As String
        Function Deserialize(type As Type, data As String) As Object
        Function Deserialize(Of T)(ByVal msg As String) As T
    End Interface
End NameSpace
