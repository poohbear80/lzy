Imports System.Xml.Serialization

Public Class TableSettings

    Property DatabaseInfoName As String
    Property TableName As String
    Property ProjectName As String
    Property Table As TableInfo
    Property Options As String = "{}"
    Property RootNamespace As String
    <XmlArray(IsNullable:=True)> Property ExcludeCols As New List(Of String)
    Property Reload As Boolean = True

    <Xml.Serialization.XmlIgnore> Property GlobalSettings As Settings

    ReadOnly Property FileNameWithExtension As String
        Get
            Return TableName & ".tablesettings"
        End Get
    End Property
    ReadOnly Property Project As Project
        Get
            Return GlobalSettings.Projects.Find(Function(e) String.Compare(e.Name, ProjectName, True) = 0)
        End Get
    End Property
    
End Class
