Public Class TemplateSettings
    Public Property TemplateFiles As New List(Of TemplateFile)
End Class

Public Class TemplateFile

    Property FileName As String = ""
    Property OutputPath As String = ""
    Property Overwrite As Boolean = False
    Property JsFilePath As String = ""

End Class
