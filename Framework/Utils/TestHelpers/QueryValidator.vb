
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Xml

Namespace Utils.TestHelpers
    Public Class QueryValidator

        Public Sub New()
        End Sub

        Public Shared Sub Validate(Of T)(instance As T)
            Validate(Of T)(New List(Of Type) From {instance.GetType}, instance)
        End Sub

        Public Shared Sub Validate(Of T)()
            Validate(Of T)(Reflection.FindAllClassesOfTypeInApplication(GetType(T)))
        End Sub


        Private Shared Sub Validate(Of T)(allTypes As IEnumerable(Of Type), Optional sentInInstance As T = Nothing)
            Dim dataAccessChecker As DataAccessChecker
            Dim instance As T
            dataAccessChecker = New DataAccessChecker

            Using New ClassFactory.SessionInstance
                ClassFactory.SetTypeInstanceForSession(Of IDataAccess)(dataAccessChecker)
                ClassFactory.SetTypeInstanceForSession(Of ILazyFrameworkConfiguration)(New LazyFrameworkConfiguration)

                For Each implementedType In allTypes
                    If sentInInstance Is Nothing Then
                        instance = CType(Activator.CreateInstance(implementedType), T)
                    Else
                        instance = sentInInstance
                    End If

                    For Each f In GetType(T).GetMethods
                        If f.IsPublic Then
                            CallFunction(f, instance, dataAccessChecker)
                        End If
                    Next
                Next
                ResponseThread.ClearThreadValues()
                ThrowExceptionIfErrorIsFound(dataAccessChecker)
            End Using

        End Sub

        Private Shared Sub ThrowExceptionIfErrorIsFound(ByVal dataAccessChecker As DataAccessChecker)

            Dim s As New StringBuilder
            Dim mustThrow As Boolean = False

            If dataAccessChecker.MissingParameters.Count > 0 Then
                s.AppendLine("*Missing parameters*")
                For Each p In dataAccessChecker.MissingParameters
                    s.AppendLine(String.Format("{0}:{1}", p.FunctionName, p.ParameterName))
                Next
                mustThrow = True
            End If

            If dataAccessChecker.UnusedParameters.Count > 0 Then
                s.AppendLine("*Unused parameters*")
                For Each p In dataAccessChecker.UnusedParameters
                    s.AppendLine(String.Format("{0}:{1}", p.FunctionName, p.ParameterName))
                Next
                mustThrow = True
            End If

            If dataAccessChecker.SqlParseErrors.Count > 0 Then
                s.AppendLine("*SQL Parser error*")
                For Each p In dataAccessChecker.SqlParseErrors
                    s.AppendLine(p)
                Next
                mustThrow = True
            End If

            If mustThrow Then
                Throw New DataaccessValidationException(s.ToString)
            End If

        End Sub

        Private Shared Sub CallFunction(Of T)(ByVal methodInfo As MethodInfo, ByVal instance As T, dataAccess As DataAccessChecker)
            Dim params As New List(Of Object)

            For Each p In methodInfo.GetParameters
                If p.ParameterType.IsValueType Then
                    params.Add(Activator.CreateInstance(p.ParameterType))
                Else
                    Dim val As Object
                    Dim createType As Type = p.ParameterType
                    If p.ParameterType.IsByRef Then
                        Dim typeString As String = p.ParameterType.FullName.Replace("&", "") & "," & p.ParameterType.Assembly.FullName
                        createType = Type.GetType(typeString)
                    End If
                    Try
                        val = Activator.CreateInstance(createType)
                    Catch ex As Exception
                        val = Nothing
                    End Try
                    params.Add(val)
                End If
            Next
            dataAccess.CurrentFunctionName = instance.GetType.Name & "." & methodInfo.Name
            methodInfo.Invoke(instance, params.ToArray)
        End Sub

        Public Class DataAccessChecker
            Implements IDataAccess


            Public Property CurrentFunctionName As String

            Public Property MissingParameters As New List(Of ParameterMissmatchInfo)

            Public Property UnusedParameters As New List(Of ParameterMissmatchInfo)

            Public Property SqlParseErrors() As New List(Of String)

            Public Function CreateCommand(cmd As CommandInfo) As DbCommand Implements IDataAccess.CreateCommand
                Dim retCmd As New SqlCommand
                For Each p In cmd.Parameters
                    retCmd.Parameters.AddWithValue(p.Value.Name, p.Value.Value)
                Next
                retCmd.CommandText = cmd.CommandText
                retCmd.Connection = New SqlConnection
                Return retCmd
            End Function


            Public Function ExecuteDatareader(sourceName As String, command As DbCommand) As DbDataReader Implements IDataAccess.ExecuteDatareader

                Return New DummyReader

                ' ReSharper disable VBWarnings::BC42105
            End Function

            Public Function ExecuteXmlReader(ByVal sourceName As String, ByVal command As DbCommand) As XmlDocument Implements IDataAccess.ExecuteXmlReader
                Throw New NotImplementedException()
            End Function
            ' ReSharper restore VBWarnings::BC42105

            Public Function ExecuteScalar(sourceName As String, command As DbCommand) As Object Implements IDataAccess.ExecuteScalar
                ValidateCommand(command)
                Return 0
            End Function

            Public Function ExecuteScalar(sourceName As String, command As DbCommand, ParamArray params() As Object) As Object Implements IDataAccess.ExecuteScalar
                ValidateCommand(command)
                Return 0
            End Function

            Public Function FillObject(sourceName As String, o As IORDataObject, command As DbCommand, ParamArray params() As Object) As Boolean Implements IDataAccess.FillObject
                ValidateCommand(command)
            End Function


            Public Function FillObject(sourceName As String, o As Queue(Of IORDataObject), command As DbCommand, ParamArray params() As Object) As Boolean Implements IDataAccess.FillObject

            End Function

            Public Function GetDataTable(sourceName As String, sqlQuery As String, commandType As CommandType, ParamArray parameters() As Object) As DataTable Implements IDataAccess.GetDataTable
                Throw New NotImplementedException
            End Function


            Public Function UpdateObject(sourceName As String, o As IORDataObject, cmd As DbCommand) As Boolean Implements IDataAccess.UpdateObject
                ValidateCommand(cmd)
            End Function

            Private Sub ValidateCommand(ByVal command As DbCommand)

                'Add declared parameters in command
                Dim ParamRegexObj As New Regex("Declare.*?(@\w+)", RegexOptions.IgnoreCase)
                Dim ParamsMatchResults As Match = ParamRegexObj.Match(command.CommandText)
                Dim inlineParams As New List(Of String)

                While ParamsMatchResults.Success
                    inlineParams.Add(ParamsMatchResults.Groups(1).Value)
                    ParamsMatchResults = ParamsMatchResults.NextMatch()
                End While


                For Each p As SqlParameter In CType(command, SqlCommand).Parameters
                    If Not Regex.IsMatch(command.CommandText, "@" & p.ParameterName, RegexOptions.IgnoreCase Or RegexOptions.Multiline) Then
                        UnusedParameters.Add(New ParameterMissmatchInfo With {.ParameterName = p.ParameterName, .FunctionName = CurrentFunctionName})
                    End If
                Next

                Dim Found As Boolean = False
                Dim RegexObj As New Regex("@\w+", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
                Dim MatchResults As Match = RegexObj.Match(command.CommandText)
                While MatchResults.Success
                    For Each p As SqlParameter In command.Parameters
                        If String.Compare("@" & p.ParameterName, MatchResults.Value, True) = 0 Then
                            Found = True
                            Exit For
                        End If
                    Next
                    If Not Found Then
                        Found = Not String.IsNullOrEmpty(inlineParams.Find(Function(s) String.Compare(s, MatchResults.Value, True) = 0))
                    End If

                    If Not Found Then
                        MissingParameters.Add(New ParameterMissmatchInfo With {.ParameterName = MatchResults.Value, .FunctionName = CurrentFunctionName})
                    End If
                    Found = False
                    MatchResults = MatchResults.NextMatch()
                End While

                If LazyFramework.ClassFactory.ContainsKey(Of ITestSqlConnectionProvider)() Then
                    command.Connection = ClassFactory.GetTypeInstance(Of ITestSqlConnectionProvider).Connection
                    command.CommandText = "set parseonly on " & command.CommandText
                    Try
                        command.Connection.Open()
                        command.ExecuteReader()
                    Catch ex As Exception
                        SqlParseErrors.Add(CurrentFunctionName & ":" & ex.Message)
                    Finally
                        command.Connection.Close()
                    End Try

                End If

            End Sub

            Public Function ExecuteCommand(sourceName As String, command As DbCommand, ParamArray params() As Object) As Boolean Implements IDataAccess.ExecuteCommand
                Throw New NotImplementedException
            End Function
        End Class
    End Class

    Public Class DummyReader
        Inherits System.Data.Common.DbDataReader

        Public Overrides Sub Close()
            Throw New NotImplementedException
        End Sub

        Public Overrides ReadOnly Property Depth As Integer
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property FieldCount As Integer
            Get
                Return 0
            End Get
        End Property

        Public Overrides Function GetBoolean(ordinal As Integer) As Boolean
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetByte(ordinal As Integer) As Byte
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetBytes(ordinal As Integer, dataOffset As Long, buffer() As Byte, bufferOffset As Integer, length As Integer) As Long
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetChar(ordinal As Integer) As Char
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetChars(ordinal As Integer, dataOffset As Long, buffer() As Char, bufferOffset As Integer, length As Integer) As Long
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetDataTypeName(ordinal As Integer) As String
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetDateTime(ordinal As Integer) As Date
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetDecimal(ordinal As Integer) As Decimal
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetDouble(ordinal As Integer) As Double
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetEnumerator() As IEnumerator
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetFieldType(ordinal As Integer) As Type
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetFloat(ordinal As Integer) As Single
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetGuid(ordinal As Integer) As Guid
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetInt16(ordinal As Integer) As Short
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetInt32(ordinal As Integer) As Integer
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetInt64(ordinal As Integer) As Long
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetName(ordinal As Integer) As String

            Throw New NotImplementedException
        End Function

        Public Overrides Function GetOrdinal(name As String) As Integer
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetSchemaTable() As DataTable

            Throw New NotImplementedException
        End Function

        Public Overrides Function GetString(ordinal As Integer) As String

            Throw New NotImplementedException
        End Function

        Public Overrides Function GetValue(ordinal As Integer) As Object

            Throw New NotImplementedException

        End Function


        Public Overrides Function GetValues(values() As Object) As Integer
            Throw New NotImplementedException
        End Function

        Public Overrides ReadOnly Property HasRows As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property IsClosed As Boolean
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides Function IsDBNull(ordinal As Integer) As Boolean
            Return True
        End Function

        Default Public Overloads Overrides ReadOnly Property Item(ordinal As Integer) As Object
            Get
                Throw New NotImplementedException

            End Get
        End Property

        Default Public Overloads Overrides ReadOnly Property Item(name As String) As Object
            Get

                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides Function NextResult() As Boolean
            Return False
        End Function

        Public Overrides Function Read() As Boolean
            Return False
        End Function

        Public Overrides ReadOnly Property RecordsAffected As Integer
            Get
                Return 0
            End Get
        End Property
    End Class

    Public Class ParameterMissmatchInfo
        Property FunctionName As String
        Property ParameterName As String
    End Class

    <Serializable> Public Class DataaccessValidationException
        Inherits Exception

        Sub New(msg As String)
            MyBase.New(msg)
        End Sub

    End Class


    Public Interface ITestSqlConnectionProvider
        ReadOnly Property Connection As DbConnection
    End Interface

    Public Class TestSqlConnectionProvider
        Implements ITestSqlConnectionProvider
        Private ReadOnly _DbConnection As DbConnection

        Public Sub New(c As DbConnection)
            _DbConnection = c
        End Sub

        Public ReadOnly Property Connection As DbConnection Implements ITestSqlConnectionProvider.Connection
            Get
                Return _DbConnection
            End Get
        End Property
    End Class
End Namespace
