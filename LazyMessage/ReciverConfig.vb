Public MustInherit Class ReciverConfig
    Public MustOverride Function CreateConnection() As IReciveConnection

    Private _reciveCallback As Action(Of String)
    Private _connection As IReciveConnection

    Public ReadOnly Property Connection As IReciveConnection
        Get
            If _connection Is Nothing Then
                _connection = CreateConnection()
            End If
            Return _connection
        End Get
    End Property

    
End Class