Public Class Settings
    Property Databases As New List(Of DatabaseConnectionInfo)
    Property Projects As New List(Of Project)

    Public Function GetDatabaseConnectionInfo(ByVal databaseInfoName As String) As DatabaseConnectionInfo
        Return Databases.Find(Function(db As DatabaseConnectionInfo) String.Compare(databaseInfoName, db.Name, True) = 0)
    End Function

End Class

Public Class DatabaseConnectionInfo
    <Xml.Serialization.XmlAttribute> Property Name As String = ""
    Public Property DbName() As String
    Property ServerName As String
    Property Username As String
    Property Password As String
End Class

Public Class Project
    <Xml.Serialization.XmlAttribute> Property Name As String
    Property Path As String
    Property Language As String = "VB"
    Property CodeGenerator As String = "JS"
    Property TemplateName As String = "Default"

    Private _languageSettings As ILanguageSettings = Nothing
    ReadOnly Property LanguageSettings As ILanguageSettings
        Get
            If _languageSettings Is Nothing Then
                _languageSettings = LanguageSettingsFactory.GetLanguageSettings(Language)
            End If
            Return _languageSettings
        End Get
    End Property

    Private _templateSettings As TemplateSettings
    ReadOnly Property TemplateSettings As TemplateSettings
        Get
            Dim fileName As String = TemplateName & ".templatesettings"
            If _templateSettings Is Nothing Then
                If Not FileIO.FileSystem.FileExists(fileName) Then
                    Tools.CreateTemplate(TemplateName)
                End If

                _templateSettings = Serializers.DeserilalizeObjectFromXmlFile(Of TemplateSettings)(fileName)
            End If
            Return _templateSettings
        End Get
    End Property
End Class
