''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable> Public Class ValueNameNotFoundException
    Inherits Exception

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class


''' <summary>
''' Thrown if the server specified does not exist
''' </summary>
<Serializable> Public Class ServerNotSpecifiedException
    Inherits Exception

    Public Sub New(ByVal msg As String)
        MyBase.New(msg)
    End Sub
End Class
