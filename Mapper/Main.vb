Imports Microsoft.SqlServer.Management.Common
Imports Microsoft.SqlServer.Management.Smo
Imports System.Xml.Xsl
Imports System.Xml

Module Main
    'TODO: 
    'Generere kode
    'Cache databaser slik at vi ikke må hente dette hver gang
    'Fyll på ...

    Sub Main(args() As String)
        
        If args.Length = 1 AndAlso args(0) = "?" Then
            Console.WriteLine("USAGE: lzy Tablename\DatabaseConnectionInfoName\ProjectName")
            Console.WriteLine("No params will generate default settings the 1. time.")
            Console.WriteLine("Will loop trough all .tablesettings file other times.")
            Return
        End If

        Dim settings As Settings
        
        If Not IO.File.Exists("Settings.xml") Then
            settings = Tools.GenerateSettings()
        Else
            settings = Serializers.DeserilalizeObjectFromXmlFile(Of Settings)("Settings.xml")
        End If

        Dim allTableSettings As IEnumerable(Of TableSettings) = LoadTableSettings(settings, args)

        For Each ts In allTableSettings
            If ts.DatabaseInfoName = "TempName" Then
                Console.WriteLine("..." & ts.TableName & " har blitt opprettet. KONFIGURER!")
            Else
                Console.WriteLine("...Genererer kode for " & ts.TableName)
                GenerateCode(settings, ts)
            End If

        Next

    End Sub

    Private Function OpenInNotepad(ByVal fileName As String) As Process

        Return Process.Start("notepad.exe", fileName)
    End Function

    Private Sub GenerateCode(ByVal settings As Settings, ByVal tableSettings As TableSettings)
        Try
            LoadTable(settings, tableSettings)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        Dim OutputPath As String = ""


        Using context As New Noesis.Javascript.JavascriptContext()
            context.SetParameter("console", Console.Out)
            context.Run(My.Resources.JS.Underscore)
            context.Run(My.Resources.JS.Utils)

            For Each t In tableSettings.Project.TemplateSettings.TemplateFiles

                OutputPath = tableSettings.Project.Path + t.OutputPath.Replace("{TABLENAME}", tableSettings.TableName)

                'Sjekke om folderen finnes....
                FileIO.FileSystem.CreateDirectory(OutputPath)

                OutputPath += "\" & t.FileName.Replace("{TABLENAME}", tableSettings.TableName)
                If FileIO.FileSystem.FileExists(OutputPath) AndAlso Not t.Overwrite Then
                    Continue For
                End If

                'Her må vi ha inn logikk for om vi skal generere med JS eller xslt
                If FileIO.FileSystem.FileExists(t.JsFilePath) Then

                    Dim template As String = New System.IO.StreamReader(t.JsFilePath).ReadToEnd()
                    If FileIO.FileSystem.FileExists(OutputPath) Then
                        Dim readHead = FileIO.FileSystem.OpenTextFileReader(OutputPath)
                        Dim line As String

                        line = readHead.ReadLine()
                        While Not readHead.EndOfStream
                            If line.StartsWith("'LZY:DNO") Then
                                readHead.Close()
                                Continue For
                            End If
                            line = readHead.ReadLine
                        End While

                        readHead.Close()
                        FileIO.FileSystem.DeleteFile(OutputPath)
                    End If

                    Using s As New System.IO.StreamWriter(OutputPath, False, Text.Encoding.UTF8)
                        context.SetParameter("result", New TemplateResult(s, tableSettings.Project.Language))
                        context.SetParameter("data", tableSettings)
                        context.SetParameter("cols", tableSettings.Table.Columns.FindAll(Function(e) Not tableSettings.ExcludeCols.Contains(e.Name)))
                        context.Run("var options = " & tableSettings.Options)
                        Try
                            context.Run(template)
                        Catch ex As Noesis.Javascript.JavascriptException
                            Dim bc = Console.BackgroundColor
                            Dim fc = Console.ForegroundColor

                            Console.BackgroundColor = ConsoleColor.Red
                            Console.ForegroundColor = ConsoleColor.Yellow

                            Console.WriteLine(t.JsFilePath)
                            Console.WriteLine("Error in template:" & ex.Message)
                            Console.WriteLine("On Line:" & ex.Line)
                            Console.WriteLine("Col start:" & ex.StartColumn & " ColEnd:" & ex.EndColumn)

                            Console.BackgroundColor = bc
                            Console.ForegroundColor = fc

                        End Try
                        s.Flush()
                    End Using
                Else
                    Console.WriteLine("Template file not found:" & t.JsFilePath)
                End If
            Next
        End Using


        Return

        'Dim document = New XmlDocument()
        'document.LoadXml(GetXml(Of TableInfo)(tableSettings.Table))
        'Dim navigator = document.CreateNavigator

        'Dim outputPath As String = GetOutputPath(settings, tableSettings)


        'For Each t In settings.TemplateSettings.TemplateFiles
        '    Dim outputstream As IO.StreamWriter = IO.File.CreateText(outputPath & "")
        '    Dim transformer As XslCompiledTransform ' transform document
        '    transformer = New XslCompiledTransform()
        '    transformer.Load(t.XsltFilePath)
        '    'transformer.Transform(navigator, Nothing, outputstream)
        '    'outputstream.Flush()
        '    'outputstream.Close()
        'Next
    End Sub

    Private Function GetXml(Of T)(o As T) As String
        Dim xml As String

        Dim s As New IO.MemoryStream
        With s
            With (New Serialization.XmlSerializer(GetType(T)))
                .Serialize(s, o)
            End With
            .Flush()
            .Position = 0

            With New IO.StreamReader(s)
                xml = .ReadToEnd
            End With
            .Close()
        End With
        Return xml
    End Function

    Private Function LoadTemplate(ByVal settings As Settings, ByVal tableSettings As TableSettings) As Object
        'settings.
    End Function

    Private Sub LoadTable(ByVal settings As Settings, ByVal tableSettings As TableSettings)

        If tableSettings.Table IsNot Nothing AndAlso Not tableSettings.Reload Then Return

        Dim dbInfo As DatabaseConnectionInfo = settings.GetDatabaseConnectionInfo(tableSettings.DatabaseInfoName)

        If dbInfo Is Nothing Then
            dbInfo = New DatabaseConnectionInfo
            dbInfo.Name = tableSettings.DatabaseInfoName
            FillObjectFromInput(dbInfo)
            settings.Databases.Add(dbInfo)
            Serializers.SerializeObjectToXmlFile(settings, "Settings.xml")
        End If

        Try
            Dim server = ConnectToSever(dbInfo)
            Dim currentDb As Database = Nothing

            For Each db As Database In server.Databases
                If String.Compare(db.Name, settings.GetDatabaseConnectionInfo(tableSettings.DatabaseInfoName).DbName, True) = 0 Then
                    currentDb = db
                    Exit For
                End If
            Next

            If currentDb Is Nothing Then
                Throw New ApplicationException("Database Not found!")
            End If

            Dim t = currentDb.Tables(tableSettings.TableName)

            If t IsNot Nothing Then
                tableSettings.Table = New TableInfo(t, tableSettings.Project)
                Serializers.SerializeObjectToXmlFile(tableSettings, tableSettings.FileNameWithExtension)
            Else
                Throw New ApplicationException(tableSettings.TableName & " does not exists")
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine("Unable to load table. Using prior data.")
        End Try

    End Sub

    Private Function ConnectToSever(dbinfo As DatabaseConnectionInfo) As Server
        Dim con As New ServerConnection(dbinfo.ServerName, dbinfo.Username, dbinfo.Password)
        con.LoginSecure = False
        Return New Server(con)
    End Function

    Private Function LoadTableSettings(settings As Settings, ByVal cmdArgs As String()) As IEnumerable(Of TableSettings)

        Dim ret As New List(Of TableSettings)
        Dim arguments As New List(Of CommandArg)

        If cmdArgs.Length = 0 Then
            Dim d As New System.IO.DirectoryInfo(".")
            For Each s In d.GetFiles("*.tablesettings")
                arguments.Add(New CommandArg(s.Name.Replace(".tablesettings", "")))
            Next
        Else
            For Each s In cmdArgs
                arguments.Add(New CommandArg(s))
            Next
        End If
        Dim ts As TableSettings
        For Each s In arguments
            If Not IO.File.Exists(s.TabelName & ".tablesettings") Then
                ts = GenerateDefaultTableSetting(s, settings)
                ret.Add(ts)
            Else
                ts = Serializers.DeserilalizeObjectFromXmlFile(Of TableSettings)(s.TabelName & ".tablesettings")
                ret.Add(ts)
            End If
            ts.GlobalSettings = settings
        Next

        Return ret
    End Function

    Private Function GenerateDefaultTableSetting(ByVal options As CommandArg, ByVal settings As Settings) As TableSettings
        Dim ts As New TableSettings

        ts.TableName = options.TabelName
        ts.ProjectName = options.ProjectName
        ts.DatabaseInfoName = options.DbProfileName

        FillObjectFromInput(Of TableSettings)(ts)

        If settings.Projects.Find(Function(e As Project) e.Name = ts.ProjectName) Is Nothing Then
            Tools.AddProject(settings, ts.ProjectName)
        End If
        If settings.Databases.Find(Function(e) e.Name = ts.DatabaseInfoName) Is Nothing Then
            Tools.AddDb(settings, ts.DatabaseInfoName)
        End If

        Serializers.SerializeObjectToXmlFile(settings, "Settings.xml")

        Serializers.SerializeObjectToXmlFile(ts, ts.FileNameWithExtension)
        Return ts
    End Function

    Private Function CreateObjectFromInput(Of T As New)() As T
        Dim props = GetType(T).GetProperties
        Dim ret As New T
        Console.WriteLine("Create new entry of type: " & GetType(T).Name)
        For Each p In props
            If p.CanWrite AndAlso (p.PropertyType.IsValueType Or p.PropertyType.Name = "String") Then
                Console.Write(p.Name & ":")
                Dim val = Console.ReadLine()
                p.SetValue(ret, val)
            End If
        Next
        Return ret
    End Function

    Public Sub FillObjectFromInput(Of T)(ByRef o As T)
        Dim props = GetType(T).GetProperties
        Console.WriteLine("Update entry of type: " & GetType(T).Name)
        Dim left As Integer
        Dim top As Integer

        For Each p In props
            If p.CanWrite AndAlso (p.PropertyType.IsValueType Or p.PropertyType.Name = "String") Then
                Console.Write(p.Name & ":")
                left = Console.CursorLeft
                top = Console.CursorTop
                Dim defaultVal = p.GetValue(o)

                Console.Write(defaultVal)
                Console.SetCursorPosition(left, top)

                Dim val = Console.ReadLine()
                If String.IsNullOrEmpty(val) Then
                    val = CType(defaultVal, String)
                End If

                Select Case p.PropertyType.Name
                    Case "Boolean"
                        p.SetValue(o, CBool(val))
                    Case Else
                        p.SetValue(o, val)
                End Select

            End If
        Next
    End Sub
End Module
