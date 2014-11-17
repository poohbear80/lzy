Public Class ServerConnectionInfo
    Public Address As String
    Public UserName As String
    Public Password As String
    Public Database As String
    
    Private _Provider As IDataAccessProvider
    Public Overridable Function GetProvider() As IDataAccessProvider
        If _Provider Is Nothing Then
            _Provider = New SqlServer2
        End If
        Return _Provider
    End Function
End Class
