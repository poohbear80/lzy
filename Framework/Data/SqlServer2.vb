Public Class SqlServer2
    Implements IDataAccessProvider
    
    Public Function CreateCommand(cmd As CommandInfo) As IDbCommand Implements IDataAccessProvider.CreateCommand
        Dim ret As New SqlCommand()

        ret.CommandText = cmd.CommandText
        ret.CommandType = cmd.CommandType

        Return ret
    End Function

    Public Function CreateConnection(connectionInfo As ServerConnectionInfo) As IDbConnection Implements IDataAccessProvider.CreateConnection
        '"server=13-testsql;Database=Hr;User ID=sa;Password=supermann;"
        Return New SqlConnection(String.Format("server={0};Database={1};User ID={2};Password={3};", connectionInfo.Address, connectionInfo.Database, connectionInfo.UserName, connectionInfo.Password))
    End Function

    Public Function CreateParameter() As IDbDataParameter Implements IDataAccessProvider.CreateParameter
        Return New SqlParameter
    End Function

End Class
