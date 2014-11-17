Imports System.Runtime.Serialization

Public Class Tools
    Public Shared Sub CreateTemplate(ByVal templateName As String)

        Dim jsPath As String = "js\" & templateName & "\"
        Dim xslPath As String = "xslt\" & templateName & "\"
        Dim t As New TemplateSettings

        If IO.File.Exists(templateName & ".templatesettings") Then
            Return
        End If

        IO.Directory.CreateDirectory(jsPath)
        IO.Directory.CreateDirectory(xslPath)

        t.TemplateFiles.Add(New TemplateFile With {.FileName = "{TABLENAME}.vb", .OutputPath = "\{TABLENAME}", .Overwrite = False, .XsltFilePath = xslPath & "Entity.xslt", .JsFilePath = jsPath & "Entity.js"})
        CreateFile(xslPath & "Entity.xslt", My.Resources.XSLTDefault.Entity)
        CreateFile(jsPath & "Entity.js", My.Resources.JsDefault.Entity)


        t.TemplateFiles.Add(New TemplateFile With {.FileName = "{TABLENAME}.Partial.vb", .OutputPath = "\{TABLENAME}", .Overwrite = True, .XsltFilePath = xslPath & "Entity.Partial.xslt", .JsFilePath = jsPath & "Entity.Partial.js"})
        CreateFile(xslPath & "Entity.Partial.xslt", My.Resources.XSLTDefault.EntityPartial)
        CreateFile(jsPath & "Entity.Partial.js", My.Resources.JsDefault.Entity_Partial)

        t.TemplateFiles.Add(New TemplateFile With {.FileName = "{TABLENAME}Collection.vb", .OutputPath = "\{TABLENAME}", .Overwrite = False, .XsltFilePath = xslPath & "Entity.Collection.xslt", .JsFilePath = jsPath & "Entity.Collection.js"})
        CreateFile(xslPath & "Entity.Collection.xslt", My.Resources.XSLTDefault.Standard)
        CreateFile(jsPath & "Entity.Collection.js", My.Resources.JsDefault.Entity_Collection)

        t.TemplateFiles.Add(New TemplateFile With {.FileName = "{TABLENAME}Aggregate.vb", .OutputPath = "\{TABLENAME}", .Overwrite = False, .XsltFilePath = xslPath & "Aggregate.xslt", .JsFilePath = jsPath & "Aggregate.js"})
        CreateFile(xslPath & "Aggregate.xslt", My.Resources.XSLTDefault.Standard)
        CreateFile(jsPath & "Aggregate.js", My.Resources.JsDefault.Aggregate)

        t.TemplateFiles.Add(New TemplateFile With {.FileName = "{TABLENAME}Aggregate.Partial.vb", .OutputPath = "\{TABLENAME}", .Overwrite = True, .XsltFilePath = xslPath & "Aggregate.Partial.xslt", .JsFilePath = jsPath & "Aggregate.Partial.js"})
        CreateFile(xslPath & "Aggregate.Partial.xslt", My.Resources.XSLTDefault.Standard)
        CreateFile(jsPath & "Aggregate.Partial.js", My.Resources.JsDefault.Aggregate_Partial)

        t.TemplateFiles.Add(New TemplateFile With {.FileName = "{TABLENAME}DataAccess.vb", .OutputPath = "\{TABLENAME}\DataAccess", .Overwrite = False, .XsltFilePath = xslPath & "DataAccess.xslt", .JsFilePath = jsPath & "DataAccess.js"})
        CreateFile(xslPath & "DataAccess.xslt", My.Resources.XSLTDefault.Standard)
        CreateFile(jsPath & "DataAccess.js", My.Resources.JsDefault.DataAccess)

        t.TemplateFiles.Add(New TemplateFile With {.FileName = "{TABLENAME}DataAccess.Partial.vb", .OutputPath = "\{TABLENAME}\DataAccess", .Overwrite = True, .XsltFilePath = xslPath & "DataAccess.Partial.xslt", .JsFilePath = jsPath & "DataAccess.Partial.js"})
        CreateFile(xslPath & "DataAccess.Partial.xslt", My.Resources.XSLTDefault.Standard)
        CreateFile(jsPath & "DataAccess.Partial.js", My.Resources.JsDefault.DataAccess_Partial)

        t.TemplateFiles.Add(New TemplateFile With {.FileName = "I{TABLENAME}.vb", .OutputPath = "\{TABLENAME}\DataAccess", .Overwrite = True, .XsltFilePath = xslPath & "IDataAccess.xslt", .JsFilePath = jsPath & "IDataAccess.js"})
        CreateFile(xslPath & "IDataAccess.xslt", My.Resources.XSLTDefault.Standard)
        CreateFile(jsPath & "IDataAccess.js", My.Resources.JsDefault.IDataAccess)

        Serializers.SerializeObjectToXmlFile(t, templateName & ".templatesettings")
    End Sub

    Public Shared Sub CreateFile(ByVal entityXsl As String, ByVal entity As String)
        Dim path As String = entityXsl

        If Not IO.File.Exists(path) Then
            Dim w = System.IO.File.CreateText(path)
            w.WriteLine(entity)
            w.Flush()
            w.Close()
        End If
    End Sub

    Public Shared Sub AddDb(ByVal s As Settings, ByVal dbName As String)
        Dim db As New DatabaseConnectionInfo
        db.Name = dbName
        FillObjectFromInput(Of DatabaseConnectionInfo)(db)
        s.Databases.Add(db)

    End Sub

    Public Shared Sub AddProject(ByVal s As Settings, ByVal projectName As String)
        Dim p As New Project
        p.Name = projectName
        FillObjectFromInput(Of Project)(p)
        s.Projects.Add(p)
        Tools.CreateTemplate(p.TemplateName)

    End Sub

    Public Shared Function GenerateSettings(Optional dbName As String = "YourDbName", Optional projectName As String = "Backend") As Settings
        Dim s As New Settings

        Serializers.SerializeObjectToXmlFile(s, "Settings.xml")
        Tools.CreateTemplate("Default")

        Return s
    End Function
End Class


<Serializable> Public Class Template
    Implements ISerializable
    
    Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

    End Sub
End Class
