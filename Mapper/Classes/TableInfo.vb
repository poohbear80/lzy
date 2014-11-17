Imports System.Runtime.Serialization
Imports Microsoft.SqlServer.Management.Smo

Public Class TableInfo
    Private _project As Project

    Public Sub New()
    End Sub

    Public Sub New(ByVal t As Table, ByVal p As Project)
        _project = p
        Name = t.Name
        Columns = New List(Of ColumnInfo)
        ForeginKeys = New List(Of ForeignKeyInfo)
        For Each c As Column In t.Columns
            Columns.Add(New ColumnInfo(c, p.LanguageSettings))
        Next

        For Each fk As ForeignKey In t.ForeignKeys
            ForeginKeys.Add(New ForeignKeyInfo(fk))
        Next
    End Sub

    Property Name As String
    Property Columns As List(Of ColumnInfo)

    Property ForeginKeys As List(Of ForeignKeyInfo)
    
End Class
