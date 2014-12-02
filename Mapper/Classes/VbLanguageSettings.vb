Imports System.Text.RegularExpressions

Public Class VbLanguageSettings
    Implements ILanguageSettings

    Public Sub New()
        If Not IO.File.Exists(Me.FileName) Then

        End If
    End Sub

    Public Property DataTypes As New Dictionary(Of String, ILanguageSettings.DataType) From {
        {"int", New ILanguageSettings.DataType With {.DbType = "Int32", .LanguageType = "Integer"}},
        {"uniqueidentifier", New ILanguageSettings.DataType With {.DbType = "Guid", .LanguageType = "Guid"}},
        {"varchar", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "String"}},
        {"nvarchar", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "String"}},
        {"char", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "String"}},
        {"text", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "String"}},
        {"bigint", New ILanguageSettings.DataType With {.DbType = "Int64", .LanguageType = "Int64"}},
        {"bit", New ILanguageSettings.DataType With {.DbType = "Boolean", .LanguageType = "Boolean"}},
        {"datetime", New ILanguageSettings.DataType With {.DbType = "Datetime", .LanguageType = "DateTime"}},
        {"float", New ILanguageSettings.DataType With {.DbType = "Decimal", .LanguageType = "Decimal"}},
        {"decimal", New ILanguageSettings.DataType With {.DbType = "Decimal", .LanguageType = "Decimal"}},
        {"double", New ILanguageSettings.DataType With {.DbType = "Double", .LanguageType = "Double"}},
        {"image", New ILanguageSettings.DataType With {.DbType = "Binary", .LanguageType = "Byte()"}},
        {"smallint", New ILanguageSettings.DataType With {.DbType = "Smallint", .LanguageType = "Int16"}},
        {"tinyint", New ILanguageSettings.DataType With {.DbType = "Tinyint", .LanguageType = "Int8"}},
        {"date", New ILanguageSettings.DataType With {.DbType = "Date", .LanguageType = "Date"}},
        {"smalldatetime", New ILanguageSettings.DataType With {.DbType = "date", .LanguageType = "DateTime"}}
        } Implements ILanguageSettings.DataTypes


    Public Property DefaultValue As New List(Of DefaultValue) From {
        New DefaultValue With {.Match = New Regex(".*?(0).*"), .Replace = "false", .DbType = "bit"},
        New DefaultValue With {.Match = New Regex(".*?(1).*"), .Replace = "true", .DbType = "bit"},
        New DefaultValue With {.Match = New Regex(".*?(\d+).*"), .Replace = "${1}", .DbType = "int"},
        New DefaultValue With {.Match = New Regex(".*?(\d+).*"), .Replace = "${1}", .DbType = "int"},
        New DefaultValue With {.Match = New Regex(".*?([\d\.]+).*"), .Replace = "${1}D", .DbType = "decimal"},
        New DefaultValue With {.Match = New Regex("newid"), .Replace = "Guid.NewGuid()", .DbType = "guid"},
        New DefaultValue With {.Match = New Regex("getdate"), .Replace = "DateTime.Now()", .DbType = "datetime"}
    } Implements ILanguageSettings.DefaultValues

    Public ReadOnly Property FileName As String
        Get
            Return "VB.languagesettings"
        End Get
    End Property
End Class
