Namespace Data
    Public MustInherit Class ServerConnectionInfo
        Public Address As String
        Public UserName As String
        Public Password As String
        Public Database As String
        Public Pooling As Boolean = True

        Public MustOverride Function GetProvider() As IDataAccessProvider

    End Class
End Namespace