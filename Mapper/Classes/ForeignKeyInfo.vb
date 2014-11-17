Imports Microsoft.SqlServer.Management.Smo

Public Class ForeignKeyInfo
    Public Sub New()
    End Sub

    Public Sub New(ByVal foreignKey As ForeignKey)
        
        Name = foreignKey.Name
        TableName = foreignKey.ReferencedTable
        ColumnName = foreignKey.Columns(0).Name

        'SeriousHack:......
        DataType = "int"

    End Sub

    Public Property ColumnName As String
    Public Property TableName As String
    Public Property DataType As String
    Public Property Name As String


End Class
