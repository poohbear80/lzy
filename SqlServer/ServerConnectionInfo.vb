Imports LazyFramework.Data

<CLSCompliant(True)>
Public Class ServerConnectionInfo
    Inherits Data.ServerConnectionInfo

    Public Overrides Function GetProvider() As IDataAccessProvider
        Return New DataProvider
    End Function
End Class