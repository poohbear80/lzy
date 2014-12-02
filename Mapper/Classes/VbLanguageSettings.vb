Public Class VbLanguageSettings
    Implements ILanguageSettings

    Public Sub New()
        If Not IO.File.Exists(Me.FileName) Then

        End If
    End Sub

    Public Property DataTypes As New Dictionary(Of String, ILanguageSettings.DataType) From {
        {"int", New ILanguageSettings.DataType With {.DbType = "int32", .LanguageType = "Integer"}},
        {"uniqueidentifier", New ILanguageSettings.DataType With {.DbType = "Guid", .LanguageType = "Guid"}},
        {"varchar", New ILanguageSettings.DataType With {.DbType = "string", .LanguageType = "String"}},
        {"nvarchar", New ILanguageSettings.DataType With {.DbType = "string", .LanguageType = "String"}},
        {"char", New ILanguageSettings.DataType With {.DbType = "string", .LanguageType = "String"}},
        {"text", New ILanguageSettings.DataType With {.DbType = "string", .LanguageType = "String"}},
        {"bigint", New ILanguageSettings.DataType With {.DbType = "Int64", .LanguageType = "Int64"}},
        {"bit", New ILanguageSettings.DataType With {.DbType = "boolean", .LanguageType = "Boolean"}},
        {"datetime", New ILanguageSettings.DataType With {.DbType = "datetime", .LanguageType = "DateTime"}},
        {"float", New ILanguageSettings.DataType With {.DbType = "decimal", .LanguageType = "Double"}},
        {"decimal", New ILanguageSettings.DataType With {.DbType = "currency", .LanguageType = "Double"}},
        {"double", New ILanguageSettings.DataType With {.DbType = "double", .LanguageType = "Double"}},
        {"image", New ILanguageSettings.DataType With {.DbType = "binary", .LanguageType = "Byte()"}},
        {"smallint", New ILanguageSettings.DataType With {.DbType = "smallint", .LanguageType = "Int16"}},
        {"tinyint", New ILanguageSettings.DataType With {.DbType = "tinyint", .LanguageType = "Int8"}},
        {"date", New ILanguageSettings.DataType With {.DbType = "date", .LanguageType = "Date"}},
        {"smalldatetime", New ILanguageSettings.DataType With {.DbType = "date", .LanguageType = "DateTime"}}
        } Implements ILanguageSettings.DataTypes


    Public Property DefaultValue As New Dictionary(Of String, String) From {
        {"0", "0"},
        {"1", "1"},
        {"newid", "Guid.NewGuid()"},
        {"getdate", "DateTime.Now()"}
    } Implements ILanguageSettings.DefaultValue

    Public ReadOnly Property FileName As String
        Get
            Return "VB.languagesettings"
        End Get
    End Property
End Class
