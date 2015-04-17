

Public Class ServerConnectionInfo
    Inherits Data.ServerConnectionInfo

    Public Overrides Function GetProvider() As Data.IDataAccessProvider
        Return New ElasticProvider
    End Function
End Class