Imports System.IO
Imports System.Reflection

Public Class Reflection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FindAllClassesOfTypeInApplication(ByVal t As Type, Optional ByVal skipSystem As Boolean = True, Optional ByVal forceLoad As Boolean = True) As List(Of Type)

        Return AllTypes.FindAll(Function(type) t.IsAssignableFrom(type))

    End Function

    Private Shared _allTypes As List(Of Type)

    Private Shared ReadOnly _PadLock As New Object
    Private Shared ReadOnly Property AllTypes() As List(Of Type)
        Get
            Dim a As Assembly
            Dim f As FileInfo
            Dim assembly As Assembly
            Dim type As Type
            Dim tle As Exception
            Dim allFiles As New List(Of String)

            If _allTypes Is Nothing Then
                SyncLock _PadLock
                    If _allTypes Is Nothing Then
                        Dim allTypesTemp = New List(Of Type)
                        Dim loaded As New List(Of Assembly)
                        Dim toIgnore As New System.Text.RegularExpressions.Regex(TypeValidation.IgnoreAssemblies, Text.RegularExpressions.RegexOptions.Compiled Or Text.RegularExpressions.RegexOptions.IgnoreCase)

                        For Each a In AppDomain.CurrentDomain.GetAssemblies()
                            allFiles.Add(a.FullName)
                            If Not a.GlobalAssemblyCache AndAlso Not a.IsDynamic Then
                                loaded.Add(a)
                            End If
                        Next


                        Try
                            Dim fileInfos As FileInfo() = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll", SearchOption.AllDirectories)

                            For Each f In fileInfos

                                If Not String.IsNullOrWhiteSpace(TypeValidation.IgnoreAssemblies) AndAlso toIgnore.IsMatch(f.FullName) Then Continue For

                                Dim dllIsLoaded As Boolean = False

                                For Each a In loaded
                                    If a.ManifestModule.ScopeName.ToLower = f.Name.ToLower Then
                                        dllIsLoaded = True
                                        Exit For
                                    End If
                                Next

                                If Not dllIsLoaded Then
                                    Try
                                        loaded.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(f.FullName)))
                                    Catch ex As Exception
                                        'Gidder ikke gj�re noe... ;)
                                        Continue For
                                    End Try
                                End If
                            Next
                        Catch ex As Exception
                            Throw New ApplicationException("DLL Loading", ex)
                        End Try


                        For Each assembly In loaded
                            Try
                                Dim getTypes As Type()
                                If Not String.IsNullOrWhiteSpace(TypeValidation.IgnoreAssemblies) AndAlso toIgnore.IsMatch(assembly.FullName) Then Continue For
                                getTypes = assembly.GetTypes
                                For Each type In getTypes
                                    Try
                                        If type.IsClass AndAlso Not type.IsAbstract Then
                                            allTypesTemp.Add(type)
                                        End If
                                    Catch ex As Exception
                                        Throw New ApplicationException("Add type" & type.FullName, ex)
                                    End Try
                                Next
                            Catch ex As ReflectionTypeLoadException
                                Trace.Write(ex.GetType.Name)
                                Dim s As String
                                s = assembly.Location
                                For Each tle In ex.LoaderExceptions
                                    s += tle.Message & vbCrLf
                                Next
                                Throw New ApplicationException(s)
                            Catch ex As Exception
                                Throw
                            End Try
                        Next





                        _allTypes = allTypesTemp

                    End If

                End SyncLock
            End If
            Return _allTypes
        End Get
    End Property
End Class