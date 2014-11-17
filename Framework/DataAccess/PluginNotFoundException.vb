<Serializable> Public Class PluginNotFoundException
    Inherits ApplicationException
    Public Overrides Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        MyBase.GetObjectData(info, context)
    End Sub
    Public PluginType As String
End Class
