Namespace Data
    Public Class DataModificationPluginContext
        Private ReadOnly _serverConnectionInfo As ServerConnectionInfo
        Private ReadOnly _commandInfo As CommandInfo
        Private ReadOnly _data As Object

        Friend Sub New(serverConnectionInfo As ServerConnectionInfo, commandInfo As CommandInfo, data As Object)
            _serverConnectionInfo = serverConnectionInfo
            _commandInfo = commandInfo
            _data = data
        End Sub

        Public ReadOnly Property Data As Object
            Get
                Return _data
            End Get
        End Property

        Public ReadOnly Property Info As CommandInfo
            Get
                Return _commandInfo
            End Get
        End Property

        Public ReadOnly Property ConnectionInfo As ServerConnectionInfo
            Get
                Return _serverConnectionInfo
            End Get
        End Property
    End Class
End NameSpace