Imports System.Data.Common
Imports System.Data.SqlClient
Imports LazyFramework.Utils
Imports System.Xml

Public Class SQLServer
    Implements IDataAccess

#Region "iDataAccess"

    ''' <summary>
    ''' Fills the object.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="o">The IOrDataobject to fill</param>
    ''' <param name="Command">The command, as a</param>
    ''' <param name="params">The params.</param>
    ''' <returns></returns>
    Public Function FillObject(ByVal sourceName As String, ByVal o As IORDataObject, ByVal command As DbCommand, ByVal ParamArray params() As Object) As Boolean Implements IDataAccess.FillObject
        Using cnn As New SqlConnection(GetConnectionString(sourceName))
            Using command
                command.Connection = cnn
                command.Connection.Open()
                If params.Length = 0 Then
                    ExecuteCommandAndFillObject(CType(command, SqlCommand), o, o)
                Else
                    ExecuteCommandAndFillObject(CType(command, SqlCommand), o, params)
                End If

            End Using
        End Using
        Return True
    End Function

    ''' <summary>
    ''' Fills the object.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="objectQueue">The object queue.</param>
    ''' <param name="Command">The command.</param>
    ''' <param name="params">The params.</param>
    ''' <returns></returns>
    Public Function FillObject(ByVal sourceName As String, ByVal objectQueue As Queue(Of IORDataObject), ByVal command As DbCommand, ByVal ParamArray params() As Object) As Boolean Implements IDataAccess.FillObject
        Dim r As SqlDataReader
        Dim o As IORDataObject

        Using c As New SqlConnection(GetConnectionString(sourceName))
            Using command
                command.Connection = c
                command.Connection.Open()
                o = objectQueue.Dequeue
                If params.Length = 0 Then
                    r = ExecReader(CType(command, SqlCommand), o, o)
                Else
                    r = ExecReader(CType(command, SqlCommand), o, params)
                End If

                DoFill(r, o)
                While objectQueue.Count <> 0 And r.NextResult
                    o = objectQueue.Dequeue
                    DoFill(r, o)
                End While
                r.Close()
            End Using
        End Using
        Return True
    End Function
    
    Public Function UpdateObject(ByVal sourceName As String, ByVal o As IORDataObject, ByVal command As DbCommand) As Boolean Implements IDataAccess.UpdateObject
        Dim ret As Boolean = True
        Using c As New SqlConnection(GetConnectionString(sourceName))
            Using command
                c.Open()
                command.Connection = c
                ret = ExecuteCommandAndFillObject(CType(command, SqlCommand), o, o)
            End Using
        End Using
        Return ret
    End Function

    Public Function ExecuteCommand(ByVal sourceName As String, ByVal command As DbCommand, ByVal ParamArray params() As Object) As Boolean Implements IDataAccess.ExecuteCommand
        If command.CommandType = CommandType.StoredProcedure Then
            ExecuteSp(sourceName, command.CommandText, params)
        Else
            ExecuteText(sourceName, CType(command, SqlCommand), params)
        End If
        Return True
    End Function

    
    ''' <summary>
    ''' Gets the data table.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="sqlQuery">The SQL query.</param>
    ''' <param name="commandType">Type of the command.</param>
    ''' <param name="parameters">The parameters.</param>
    ''' <returns></returns>
    Public Function GetDataTable(ByVal sourceName As String, ByVal sqlQuery As String, ByVal commandType As CommandType, ByVal ParamArray parameters As Object()) As DataTable Implements IDataAccess.GetDataTable
        Dim dt As New DataTable
        Using cmd As New SqlCommand
            cmd.CommandType = commandType
            cmd.CommandText = sqlQuery
            cmd.Connection = New SqlConnection(GetConnectionString(sourceName))
            cmd.Connection.Open()
            If commandType = commandType.StoredProcedure Then
                FillParameters(cmd, parameters)
            End If
            dt.Load(cmd.ExecuteReader())
            cmd.Connection.Close()
        End Using

        Return dt
    End Function

    Public Function CreateCommand(ByVal cmd As CommandInfo) As DbCommand Implements IDataAccess.CreateCommand
        Dim lResult As DbCommand = Nothing
        Dim p As SqlParameter

        If cmd Is Nothing Then
            Return lResult
        End If

        lResult = New SqlCommand
        lResult.CommandText = cmd.CommandText
        lResult.CommandType = cmd.CommandType

        For Each pi As ParameterInfo In cmd.Parameters.Values
            p = New SqlParameter
            p.DbType = pi.DbType
            p.ParameterName = pi.Name
            p.IsNullable = pi.IsNullable
            If pi.Size <> 0 Then
                p.Size = pi.Size
            End If
            If pi.Value IsNot Nothing Then
                p.Value = pi.Value
            End If
            lResult.Parameters.Add(p)
        Next

        Return lResult
    End Function

#End Region

#Region "Helpers"

    Private Function ExecuteCommandAndFillObject(ByVal cmd As SqlCommand, ByVal o As IORDataObject, ByVal ParamArray params As Object()) As Boolean
        Try
            Dim r As SqlDataReader
            r = ExecReader(cmd, o, params)
            If r.HasRows Then
                DoFill(r, o)
            End If
            r.Close()
            Return True
        Catch ex As Exception
            ResponseThread.Current.Add(New ThreadMessage(ThreadMessageSeverityEnum.Error, "Database", ex.Message))
            Throw
        End Try
    End Function

#End Region

#Region "Privates"


    ''' <summary>
    ''' Executes a reader an returns then result in the reader
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <param name="o"></param>
    ''' <param name="Params"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecReader(ByVal cmd As SqlCommand, ByVal o As IORDataObject, ByVal ParamArray params() As Object) As SqlDataReader
        Try
            If cmd.CommandType = CommandType.StoredProcedure Or cmd.Parameters.Count > 0 Then
                If params.Length <> 0 Then
                    FillParameters(cmd, params)
                Else
                    If TypeOf (o) Is IORDataObject Then
                        FillParameters(cmd, o)
                    End If
                End If
            End If

            Return cmd.ExecuteReader

        Catch ex As Exception
            cmd.Connection.Close()
            cmd.Dispose()
            ResponseThread.Current.Add(New ThreadMessage(ThreadMessageSeverityEnum.Error, "DATABASE:", ex.Message))
            Throw
        End Try
    End Function


    ''' <summary>
    ''' INternal sub for filling the object's
    ''' </summary>
    ''' <param name="r">The r.</param>
    ''' <param name="o">The o.</param>
    Private Sub DoFill(ByVal r As SqlDataReader, ByVal o As IORDataObject)
        If TypeOf (o) Is IMultiLine Then
            If r.HasRows Then
                While r.Read
                    FillLoop(r, o)
                    CType(o, IMultiLine).NextRow()
                End While
            Else
                o.FillResult = FillResultEnum.NoData
                'ResponseThread.GetCurrent.Add(New BrokenRule(BrokenRuleSeverityEnum.Error, "Not found", "No object matched this query"))
            End If
        Else
            If r.Read Then
                FillLoop(r, o)
            Else
                o.FillResult = FillResultEnum.NoData
                ResponseThread.Current.Add(New ThreadMessage(ThreadMessageSeverityEnum.Warning, "Not found", "No object matched this query"))
            End If
        End If
        o.Loaded = Now.Ticks
    End Sub

    ''' <summary>
    ''' Loop til fill data
    ''' </summary>
    ''' <param name="r">The r.</param>
    ''' <param name="o">The o.</param>
    Private Shared Sub FillLoop(ByVal r As SqlDataReader, ByVal o As IORDataObject)
        'Her er logikken for å fylle denne.. Den er tatt vekk fra baseobjectet slik at den skal bli enda mer overførbar til andre dataprovidere.. 
        Dim name As String, value As Object

        For x As Integer = 0 To r.FieldCount - 1
            name = r.GetName(x)
            value = r.GetValue(x)

            If Not IsDBNull(value) Then
                If Not o.SetValue(name, value) Then
                    o.SetValueExtended(name, value)
                End If
            End If
        Next
        o.FillResult = FillResultEnum.DataFound
    End Sub


    ''' <summary>
    ''' Retrives the connectionstring from the config file. 
    ''' </summary>
    ''' <param name="dbName" ></param>
    ''' <returns></returns>
    ''' <remarks>use the syntax dbName@servername to get the right connectionstring. The string [DBName] in 
    ''' the connectionstring is replaced by the text infront of the @
    ''' </remarks>
    Public Shared Function GetConnectionString(ByVal dbName As String) As String
        Static conStrCache As SortedList(Of String, String)
        Static nameCacheSync As New Object
        Dim conStr As String = ""


        If String.IsNullOrEmpty(dbName) Then
            Throw New ApplicationException("dbName is blank or nothing")
        End If

        If conStrCache Is Nothing Then conStrCache = New SortedList(Of String, String)

        SyncLock nameCacheSync
            If Not conStrCache.ContainsKey(dbName) Then
                Dim userNamePassword As String() = Nothing
                Dim dbNameLocal As String() = Nothing

                If dbName.Contains("@") Or dbName.Contains("|") Then
                    If dbName.Contains("|") Then
                        userNamePassword = dbName.Split("|"c)(1).Split("@"c)
                        dbNameLocal = dbName.Split("|"c)(0).Split("@"c)
                    Else
                        dbNameLocal = dbName.Split("@"c)
                    End If
                    If Not LazyFrameworkConfiguration.Current.Servers(dbNameLocal(1)) Is Nothing Then
                        conStr = LazyFrameworkConfiguration.Current.Servers(dbNameLocal(1)).ConnectionString
                    Else
                        Throw New ServerNotSpecifiedException("The server " & dbNameLocal(1) & " is not specified in the LazyFrameworkConfiguration section")
                    End If
                    conStr = conStr.Replace("[DBName]", dbNameLocal(0))
                    If Not userNamePassword Is Nothing Then
                        conStr = conStr.Replace("[user]", userNamePassword(0))
                        conStr = conStr.Replace("[password]", userNamePassword(1))
                    End If
                End If
                conStrCache.Add(dbName, conStr)
            End If
        End SyncLock
        Return conStrCache(dbName)
    End Function


    Private Shared ReadOnly SyncLockObject As New Object

    Private Shared Function GetCachedCommand(ByVal cmd As SqlCommand) As SqlCommand
        Static cmdCollection As SortedList(Of String, SqlCommand)
        'Første gangen vi er inne må vi initiere collection
        '**************************************************
        If cmdCollection Is Nothing Then cmdCollection = New SortedList(Of String, SqlCommand)
        '**************************************************
        Dim cmdRet As SqlCommand

        SyncLock SyncLockObject
            If Not cmdCollection.ContainsKey(cmd.CommandText) Then
                cmdRet = cmd.Clone
                SqlCommandBuilder.DeriveParameters(cmdRet)
                cmdCollection.Add(cmd.CommandText, cmdRet)
            Else
                cmdRet = cmdCollection(cmd.CommandText)
            End If
        End SyncLock

        Return cmdRet
    End Function

    ''' <summary>
    ''' Fills a command object with parameters. The parameters is cached in a collection to improve performance.
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <remarks>Det er ikke lov for en parameter å tilhøre flere kommandoer så den må klones..</remarks>
    Public Shared Sub FillParameters(ByVal cmd As SqlCommand, ByVal ParamArray parameters As Object())
        Dim o As IORDataObject
        Dim tempValue As Object
        Dim paramName As String
        Dim sqlCommand As SqlCommand
        Dim sqlParameterTemp As SqlParameter

        If cmd.Parameters.Count = 0 AndAlso cmd.CommandType = CommandType.StoredProcedure Then
            sqlCommand = GetCachedCommand(cmd)
            For x As Integer = 0 To sqlCommand.Parameters.Count - 1
                sqlParameterTemp = DirectCast(DirectCast(sqlCommand.Parameters(x), ICloneable).Clone, SqlParameter)
                'Første parameter er UT parameter så den skal vi ikke sette...
                cmd.Parameters.Add(sqlParameterTemp)
            Next
        End If

        If cmd.Parameters.Count > 0 Then
            If parameters.Length = 1 AndAlso TypeOf (parameters(0)) Is IORDataObject Then
                o = CType(parameters(0), IORDataObject)

                For x As Integer = 0 To cmd.Parameters.Count - 1
                    sqlParameterTemp = cmd.Parameters(x)
                    If sqlParameterTemp.Direction = ParameterDirection.Input Then

                        If sqlParameterTemp.Value Is Nothing Then 'the value for this parameter is not set. Ask the object for the parameter..
                            tempValue = DBNull.Value
                            paramName = sqlParameterTemp.ParameterName.Replace("@"c, "")
                            If o.GetValue(paramName, tempValue) Then
                                sqlParameterTemp.Value = tempValue
                            Else
                                If o.GetValueExtended(paramName, tempValue) Then
                                    sqlParameterTemp.Value = tempValue
                                Else
                                    Throw New ArgumentException("Object and database is not compatible. This can cause loss of data.", sqlParameterTemp.ParameterName)
                                End If
                            End If
                        End If


                    End If
                Next
            Else
                If Not parameters.Length = 0 Then
                    Dim paramPos As Integer = -1

                    For x As Integer = 0 To cmd.Parameters.Count - 1
                        sqlParameterTemp = cmd.Parameters(x)
                        If sqlParameterTemp.Direction = ParameterDirection.Input Then
                            paramPos += 1
                            If parameters.Length >= paramPos Then
                                sqlParameterTemp.Value = parameters(paramPos)
                            End If
                        End If
                    Next

                End If
            End If
        End If
    End Sub


    ''' <summary>
    ''' Executes an sp. The data in the result parameter is returned.
    ''' </summary>
    ''' <param name="databaseName"></param>
    ''' <param name="spName"></param>
    ''' <param name="parameters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ExecuteSp(ByVal databaseName As String, ByVal spName As String, ByVal ParamArray parameters As Object()) As Object
        Using c As New SqlConnection(GetConnectionString(databaseName))
            Using cmd As New SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = spName
                cmd.Connection = c
                cmd.Connection.Open()
                FillParameters(cmd, parameters)

                Dim o As Object
                o = cmd.ExecuteScalar()
                If o Is Nothing Then
                    If cmd.Parameters(0).Value Is DBNull.Value Then
                        o = Nothing
                    Else
                        o = cmd.Parameters(0).Value
                    End If
                End If
                Return o
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Execute a sql text against the databse
    ''' </summary>
    ''' <param name="databaseName"></param>
    ''' <param name="text"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function ExecuteText(ByVal databaseName As String, ByVal text As String, ByVal ParamArray parameters As Object()) As Object
        Using c As New SqlConnection(GetConnectionString(databaseName))
            Using cmd As New SqlCommand
                Dim o As Object
                cmd.CommandType = CommandType.Text
                cmd.CommandText = text
                cmd.Connection = c
                cmd.Connection.Open()
                FillParameters(cmd, parameters)
                o = cmd.ExecuteScalar()
                Return o
            End Using
        End Using
    End Function

    Shared Function ExecuteText(ByVal databaseName As String, ByVal command As SqlCommand, ByVal ParamArray parameters As Object()) As Object
        Dim o As Object
        Using c As New SqlConnection(GetConnectionString(databaseName))
            command.Connection = c
            command.Connection.Open()
            If parameters IsNot Nothing AndAlso parameters.Length <> 0 Then
                FillParameters(command, parameters)
            End If
            o = command.ExecuteScalar()
        End Using
        Return o
    End Function

#End Region

    Public Function ExecuteScalar(ByVal sourceName As String, ByVal command As DbCommand, ByVal ParamArray params() As Object) As Object Implements IDataAccess.ExecuteScalar
        Dim ret As Object
        Using cnn As New SqlConnection(GetConnectionString(sourceName))
            Using command
                command.Connection = cnn
                command.Connection.Open()
                FillParameters(CType(command, SqlCommand), params)
                ret = command.ExecuteScalar()
            End Using
        End Using
        Return ret
    End Function

    Public Function BulkCopy(ByVal sourceName As String, ByVal tableName As String, ByVal data As IMultiLine) As Boolean
        Using con = New SqlConnection(GetConnectionString(sourceName))
            con.Open()
            data.SetMode(IMultiLine.IMultiLineMode.Read)
            Using bc As New SqlBulkCopy(con)
                bc.DestinationTableName = tableName
                bc.WriteToServer(New IMultiLineDatareaderDecorator(data, tableName))
            End Using
        End Using
    End Function

    Public Function ExecuteScalar(sourceName As String, command As DbCommand) As Object Implements IDataAccess.ExecuteScalar
        Dim ret As Object
        Using cnn As New SqlConnection(GetConnectionString(sourceName))
            Using command
                command.Connection = cnn
                command.Connection.Open()
                ret = command.ExecuteScalar()
            End Using
        End Using

        Return ret
    End Function

    Public Function ExecuteDatareader(ByVal sourceName As String, ByVal command As DbCommand) As DbDataReader Implements IDataAccess.ExecuteDatareader
        Dim ret As DbDataReader
        command.Connection = New SqlConnection(GetConnectionString(sourceName))
        command.Connection.Open()
        ret = command.ExecuteReader()
        Return ret
    End Function

    Public Function ExecuteXmlReader(ByVal sourceName As String, ByVal command As DbCommand) As XmlDocument Implements IDataAccess.ExecuteXmlReader
        Dim sqlcommand = CType(command, SqlCommand)
        Dim xmlDoc As New XmlDocument
        Using cnn As New SqlConnection(GetConnectionString(sourceName))
            Using command
                command.Connection = cnn
                command.Connection.Open()
                Using ret = sqlcommand.ExecuteXmlReader
                    xmlDoc.Load(ret)
                End Using
            End Using
        End Using
        Return xmlDoc
    End Function
End Class
