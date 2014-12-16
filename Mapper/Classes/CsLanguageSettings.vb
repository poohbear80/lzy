Imports System.Text.RegularExpressions

Public Class CsLanguageSettings
    Implements ILanguageSettings

    Public Sub New()
        If Not IO.File.Exists(Me.FileName) Then

        End If
    End Sub

    Public Property DataTypes As New Dictionary(Of String, ILanguageSettings.DataType) From {
        {"int", New ILanguageSettings.DataType With {.DbType = "Int32", .LanguageType = "int"}},
        {"uniqueidentifier", New ILanguageSettings.DataType With {.DbType = "Guid", .LanguageType = "Guid"}},
        {"varchar", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "string"}},
        {"nvarchar", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "string"}},
        {"char", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "string"}},
        {"text", New ILanguageSettings.DataType With {.DbType = "String", .LanguageType = "string"}},
        {"bigint", New ILanguageSettings.DataType With {.DbType = "Int64", .LanguageType = "int64"}},
        {"bit", New ILanguageSettings.DataType With {.DbType = "Boolean", .LanguageType = "bool"}},
        {"datetime", New ILanguageSettings.DataType With {.DbType = "DateTime", .LanguageType = "DateTime"}},
        {"float", New ILanguageSettings.DataType With {.DbType = "Decimal", .LanguageType = "double"}},
        {"decimal", New ILanguageSettings.DataType With {.DbType = "Currency", .LanguageType = "double"}},
        {"double", New ILanguageSettings.DataType With {.DbType = "Double", .LanguageType = "double"}},
        {"image", New ILanguageSettings.DataType With {.DbType = "Binary", .LanguageType = "Byte[]"}},
        {"smallint", New ILanguageSettings.DataType With {.DbType = "Smallint", .LanguageType = "int16"}},
        {"tinyint", New ILanguageSettings.DataType With {.DbType = "Tinyint", .LanguageType = "int8"}},
        {"date", New ILanguageSettings.DataType With {.DbType = "Date", .LanguageType = "date"}},
        {"smalldatetime", New ILanguageSettings.DataType With {.DbType = "Date", .LanguageType = "DateTime"}}
        } Implements ILanguageSettings.DataTypes


    Public Property DefaultValues As New List(Of DefaultValue) From {
        New DefaultValue With {.Match = New Regex(".*?(\d+).*"), .Replace = "${1}", .DbType = "boolean"},
        New DefaultValue With {.Match = New Regex("newid"), .Replace = "Guid.NewGuid()", .DbType = "guid"},
        New DefaultValue With {.Match = New Regex("getdate"), .Replace = "DateTime.Now()", .DbType = "DateTime"}
    } Implements ILanguageSettings.DefaultValues

    Public ReadOnly Property FileName As String
        Get
            Return "CS.languagesettings"
        End Get
    End Property
End Class
