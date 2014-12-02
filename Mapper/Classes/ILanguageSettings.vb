Imports System.Text.RegularExpressions

Public Interface ILanguageSettings
    Property DataTypes As Dictionary(Of String, ILanguageSettings.DataType)
    Property DefaultValues As List(Of DefaultValue)

    Class DataType
        Property DbType As String
        Property LanguageType As String
        Property InsertSize As Boolean
    End Class
End Interface

Public Class DefaultValue
    Public DbType As String
    Public Match As Regex
    Public Replace As String
End Class
