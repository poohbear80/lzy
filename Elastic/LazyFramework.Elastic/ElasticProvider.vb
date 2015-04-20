Imports LazyFramework.Data

Public Class Provider
    Implements IDataAccessProvider

    Public Function CreateCommand(cmd As CommandInfo) As IDbCommand Implements IDataAccessProvider.CreateCommand
        Dim ret As New DbCommand
        ret.CommandText = cmd.CommandText

    End Function

    Public Function CreateConnection(connectionInfo As Data.ServerConnectionInfo) As IDbConnection Implements IDataAccessProvider.CreateConnection

    End Function
<<<<<<< Updated upstream
End Class
=======
End Class

Public Class CommandTextGenerator


    Public Shared Function Insert(template) As String

    End Function

    Public Shared Function Read(type As String, params As ) As String

    End Function

    Public Shared Function Search()

    End Function

End Class
>>>>>>> Stashed changes
