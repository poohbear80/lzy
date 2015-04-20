Imports LazyFramework.Data

Public Class Provider
    Implements IDataAccessProvider

    Public Function CreateCommand(cmd As CommandInfo) As IDbCommand Implements IDataAccessProvider.CreateCommand
        Dim ret As New DbCommand
        ret.CommandText = cmd.CommandText

    End Function

    Public Function CreateConnection(connectionInfo As Data.ServerConnectionInfo) As IDbConnection Implements IDataAccessProvider.CreateConnection

    End Function
End Class