Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Lazyframework.DependencyWalker

Module Module1

    Sub Main(params() As String)

        Dim settings As New Settings
        Try
            LoadPrams(settings, params)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            For Each m In map
                Console.WriteLine(m.Key)
            Next
        End Try


        Dim res As New DependencyTree
        res.ProjectName = settings.StartDll
        res.Location = settings.BasePath
        res.RootNode.FileName = settings.StartDll
        res.Comment = settings.Comment

        Dim rootAssembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(settings.BasePath + settings.StartDll)

        res.RootNode.Crc = System.Convert.ToBase64String(System.Security.Cryptography.MD5.Create().ComputeHash(IO.File.OpenRead(settings.BasePath + settings.StartDll)))

        ParseAssembly(rootAssembly, settings, res.RootNode)

        FindUniqueReferances(res, res.RootNode)

        res.AllReferences.Sort()


        FindNoneReferencedFiles(res, settings)

        Dim outputStream As System.IO.StreamWriter

        If settings.OutputPath = String.Empty Then
            outputStream = New IO.StreamWriter(Console.OpenStandardOutput) 'CType(Console.Out, IO.StreamWriter)
        Else
            outputStream = System.IO.File.CreateText(settings.OutputPath + rootAssembly.GetName.Name + ".dep")
        End If


        Lazyframework.Utils.Json.Writer.WriteObject(outputStream, res)


        outputStream.Close()
        Console.WriteLine("FÆRDIG")

    End Sub

    Private Sub FindNoneReferencedFiles(res As DependencyTree, settings As Settings)
        For Each f In settings.FolderInfo.GetFiles("*.dll")
            If Not res.AllFiles.Contains(f.Name) Then
                res.NoneReferenced.Add(f.Name)
                res.AllFiles.Add(f.Name)
            End If
        Next
    End Sub

    Private Sub FindUniqueReferances(res As DependencyTree, rootNode As NodeInfo)

        If Not res.AllReferences.Contains(rootNode.Name + " - " + rootNode.Crc) Then
            res.AllReferences.Add(rootNode.Name + " - " + rootNode.Crc)
            If rootNode.FileName IsNot Nothing Then
                res.AllFiles.Add(rootNode.FileName)
            End If
        End If

        For Each d In rootNode.Dependecies
            FindUniqueReferances(res, d)
        Next

    End Sub

    Public Sub ParseAssembly(a As Assembly, settings As Settings, n As NodeInfo, Optional level As Integer = 0)

        n.Name = a.FullName
        Try
            n.AssemblyGuid = CType(a.GetCustomAttributesData.First(Function(e) e.AttributeType = GetType(GuidAttribute)).ConstructorArguments(0).Value, String)
        Catch ex As Exception
            n.AssemblyGuid = Guid.Empty.ToString
        End Try


        If level > settings.MaxLevel Then Return

        For Each referencedAssName In a.GetReferencedAssemblies
            Dim newNode = New NodeInfo
            Dim FileName As String = referencedAssName.Name & ".dll"

            If settings.FolderInfo.GetFiles(FileName).Count = 0 Then
                If Not settings.IncludeSystemAssemblies Then Continue For
                newNode.Name = referencedAssName.FullName
                newNode.IsSystemAssembly = True
            Else
                Dim referencedAss = System.Reflection.Assembly.ReflectionOnlyLoadFrom(settings.BasePath + FileName)
                newNode.FileName = FileName
                newNode.Crc = System.Convert.ToBase64String(System.Security.Cryptography.MD5.Create().ComputeHash(IO.File.OpenRead(settings.BasePath + newNode.FileName)))
                ParseAssembly(referencedAss, settings, newNode, level + 1)
            End If
            n.Dependecies.Add(newNode)
        Next

    End Sub


    Private map As New Dictionary(Of String, MethodInfo)

    Public Sub LoadPrams(s As Settings, params As String())


        For Each method In GetType(SwitchHandler).GetMethods
            If [Delegate].CreateDelegate(GetType(SetValue), method, False) IsNot Nothing Then
                map(method.Name.ToLower) = method
            End If
        Next

        For Each svalue In params
            If svalue = String.Empty Then Continue For

            Dim values = svalue.Split("="c)
            Dim name As String = values(0).ToLower
            Dim Value As String
            If values.Length > 1 Then
                Value = values(1)
            Else
                Value = String.Empty
            End If


            If Not map.ContainsKey(name) Then
                Try
                    CallByName(s, name, CallType.Set, Value)
                Catch ex As Exception
                    Throw New Exception("Switch not supported " & name)
                End Try
            Else
                map(name).Invoke(Nothing, {s, Value})
            End If


        Next

    End Sub


End Module


Public Delegate Sub SetValue(o As Settings, value As String)

Public Class Settings
    Public BasePath As String
    Public OutputPath As String
    Public MaxLevel As Integer = 1
    Public StartDll As String
    Public FolderInfo As System.IO.DirectoryInfo
    Public IncludeSystemAssemblies As Boolean = False
    Public Comment As String
End Class


Public Class SwitchHandler
    Public Shared Sub Test(o As Settings, value As String)
    End Sub

    Public Shared Sub MaxLevel(o As Settings, value As String)
        o.MaxLevel = CInt(value)
    End Sub

    Public Shared Sub StartDll(o As Settings, value As String)
        o.StartDll = value
    End Sub

    Public Shared Sub BasePath(o As Settings, value As String)
        o.BasePath = value
        o.FolderInfo = New IO.DirectoryInfo(value)
    End Sub

    Public Shared Sub Output(o As Settings, value As String)

        o.OutputPath = value

    End Sub

    Public Shared Sub ISA(o As Settings, value As String)
        o.IncludeSystemAssemblies = True
    End Sub



End Class




Public Class DependencyTree
    Public ProjectName As String
    Public Location As String
    Public AllReferences As New List(Of String)
    Public AllFiles As New List(Of String)
    Public NoneReferenced As New List(Of String)
    Public RootNode As New NodeInfo
    Public Comment As String

End Class

Public Class NodeInfo
    Public Name As String
    Public Version As String
    Public AssemblyGuid As String
    Public FileName As String
    Public Crc As String
    Public IsSystemAssembly As Boolean

    Public Dependecies As New List(Of NodeInfo)

End Class