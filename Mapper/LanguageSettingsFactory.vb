Public Class LanguageSettingsFactory
    Public Shared Function GetLanguageSettings(language As String) As ILanguageSettings
        Select Case language.ToUpper
            Case "VB"
                Return New VbLanguageSettings()
            Case "CS"
                Return New CsLanguageSettings
            Case Else
                Throw New NotImplementedException("ugyldig languagesettings")
        End Select
    End Function
End Class
