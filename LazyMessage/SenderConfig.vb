Public MustInherit Class SenderConfig
    Public MustOverride Function CreateConnection() As ISenderConnection

    Public MustOverride ReadOnly Property SystemName As String

End Class