Public Interface ILanguageSettings
    Property DataTypes As Dictionary(Of String, ILanguageSettings.DataType)
    Property DefaultValue As Dictionary(Of String, String)

    Class DataType
        Property DbType As String
        Property LanguageType As String
        Property InsertSize As Boolean
    End Class
End Interface
