Public Interface IDataAccessProvider
    Function CreateCommand(cmd As CommandInfo) As IDbCommand
    Function CreateConnection(connectionInfo As ServerConnectionInfo) As IDbConnection
    Function CreateParameter() As IDbDataParameter
End Interface
