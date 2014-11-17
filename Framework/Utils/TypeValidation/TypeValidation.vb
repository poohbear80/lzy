Imports System.Reflection
Imports System.IO

Public Class TypeValidation

    Public Interface ICheckTypeProvider
        Function GetTypes() As List(Of String)
        ReadOnly Property Name As String
        Property Result As Boolean
        ReadOnly Property MissingClasses As List(Of String)

    End Interface

    Public MustInherit Class CheckTypeProviderBaseClass
        Implements ICheckTypeProvider


        Public MustOverride Function GetTypes() As List(Of String) Implements ICheckTypeProvider.GetTypes
        Public MustOverride ReadOnly Property Name As String Implements ICheckTypeProvider.Name
        Private _result As Boolean
        Public Property Result As Boolean Implements ICheckTypeProvider.Result
            Get
                Return _result
            End Get
            Set(ByVal value As Boolean)
                _result = value
            End Set
        End Property

        Private ReadOnly _missingClasses As New List(Of String)
        Public ReadOnly Property MissingClasses As List(Of String) Implements ICheckTypeProvider.MissingClasses
            Get
                Return _missingClasses
            End Get

        End Property
    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="class"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateClass(ByVal [class] As String) As Boolean
        If Type.GetType([class]) Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FindAllClassesOfTypeInApplication(ByVal t As Type, Optional ByVal skipSystem As Boolean = True, Optional ByVal forceLoad As Boolean = True) As List(Of Type)

        Return AllTypes.FindAll(Function(type) t.IsAssignableFrom(type))

    End Function
    
    Private Shared ReadOnly Locker As New Object
    Private Shared _allTypes As List(Of Type)

    Public Shared IgnoreAssemblies As String = ""


    Private Shared ReadOnly Property AllTypes() As List(Of Type)
        Get
            Dim a As Assembly
            Dim f As FileInfo
            Dim assembly As Assembly
            Dim type As Type
            Dim tle As Exception
            Dim allFiles As New List(Of String)

            If _allTypes Is Nothing Then
                SyncLock Locker
                    If _allTypes Is Nothing Then
                        Dim allTypesTemp = New List(Of Type)
                        Dim loaded As New List(Of Assembly)
                        Dim toIgnore As New System.Text.RegularExpressions.Regex(IgnoreAssemblies, Text.RegularExpressions.RegexOptions.Compiled Or Text.RegularExpressions.RegexOptions.IgnoreCase)

                        For Each a In AppDomain.CurrentDomain.GetAssemblies()
                            allFiles.Add(a.FullName)
                            If Not a.GlobalAssemblyCache AndAlso Not a.IsDynamic Then
                                loaded.Add(a)
                            End If
                        Next


                        Try
                            Dim fileInfos As FileInfo() = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll", SearchOption.AllDirectories)

                            For Each f In fileInfos

                                If Not String.IsNullOrWhiteSpace(IgnoreAssemblies) AndAlso toIgnore.IsMatch(f.FullName) Then Continue For

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
                                        'Gidder ikke gjøre noe... ;)
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
                                If Not String.IsNullOrWhiteSpace(IgnoreAssemblies) AndAlso toIgnore.IsMatch(assembly.FullName) Then Continue For
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateApplicationWeakReferances() As List(Of ICheckTypeProvider)
        Dim validator As ICheckTypeProvider
        Dim ret As New List(Of ICheckTypeProvider)

        For Each t In FindAllClassesOfTypeInApplication(GetType(ICheckTypeProvider))
            Try
                validator = CType(Activator.CreateInstance(t), ICheckTypeProvider)
                ret.Add(validator)
                validator.Result = True
                For Each s In validator.GetTypes
                    If Not ValidateClass(s) Then
                        validator.Result = False
                        validator.MissingClasses.Add(s)
                    End If
                Next
            Catch ex As Exception
                Throw New MissingMethodException("Klarte ikke å opprette validator av type:" & t.FullName & "Error:" & ex.Message)
            End Try

        Next
        Return ret
    End Function

End Class
