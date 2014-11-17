Imports System.Text.RegularExpressions
Imports LazyFramework.CQRS.EventHandling
Imports LazyFramework.Utils
Imports System.Reflection
Imports LazyFramework.Logging

Public Class Data

    Public Shared Sub Exec(Of T As New)(dbName As ServerConnectionInfo, command As CommandInfo, data As List(Of T))
        Dim listFiller As New ListFiller
        Logger.Log(10, New DataLog("Fill collection -" & command.CommandText))
        ExecReader(Of List(Of T))(dbName, command, New FillStatus(Of List(Of T))(data), CommandBehavior.SingleResult, AddressOf listFiller.FillList, GetType(T))
    End Sub

    Public Shared Sub Exec(Of T As New)(dbName As ServerConnectionInfo, command As CommandInfo, data As FillStatus(Of T))
        ExecReader(dbName, command, data, CommandBehavior.SingleResult Or CommandBehavior.SingleRow, AddressOf ReadOne, GetType(T))
    End Sub

    Public Shared Sub Exec(dbName As ServerConnectionInfo, command As CommandInfo, data As Object)
        Logger.Log(10, New DataLog("Fill object -" & command.CommandText))
        ExecReader(Of Object)(dbName, command, New FillStatus(Of Object)(data), CommandBehavior.SingleResult Or CommandBehavior.SingleRow, AddressOf ReadOne, data.GetType)
    End Sub
    

#Region "Privates"

    Private Shared Sub ExecReader(Of T As New)(ByVal dbName As ServerConnectionInfo, ByVal command As CommandInfo, data As FillStatus(Of T), readerOptions As CommandBehavior, handler As HandleReader(Of T), dataObjectType As Type)
        Logger.Log(100, New DataLog("Executing reader"))
        EventHub.Publish(New ExecutingReaderLogEvent)

        Dim provider = dbName.GetProvider

        Using cmd = provider.CreateCommand(command)
            FillParameters(provider, command, dataObjectType, data.Value, cmd)

            Using conn = provider.CreateConnection(dbName)
                cmd.Connection = conn
                conn.Open()
                Dim filler As FillObject = Nothing
                Dim reader As IDataReader = Nothing

                Using New InlineTimer(dbName.Database & "-" & cmd.CommandText, ResponseThread.Current.Timer.Timings)
                    reader = cmd.ExecuteReader(readerOptions Or CommandBehavior.CloseConnection)
                End Using

                filler = GetFiller(command, reader, dataObjectType)
                handler(filler, reader, data)
            End Using
        End Using
    End Sub

    Private Shared Sub FillParameters(ByVal provider As IDataAccessProvider, ByVal command As CommandInfo, ByVal dataObjectType As Type, ByVal data As Object, ByVal cmd As IDbCommand)
        Dim p As IDbDataParameter

        For Each pi As ParameterInfo In command.Parameters.Values
            p = provider.CreateParameter
            p.DbType = pi.DbType
            p.ParameterName = pi.Name
            If pi.Size <> 0 Then
                p.Size = pi.Size
            End If

            If pi.Value IsNot Nothing Then
                p.Value = pi.Value
            Else
                'Read the value from the object... 
                Logger.Log(1000, New DataLog("Finding value for param:" & pi.Name))
                Dim f = dataObjectType.GetField("_" & pi.Name, BindingFlags.IgnoreCase Or BindingFlags.NonPublic Or BindingFlags.Instance)
                If f Is Nothing Then
                    Logger.Log(1000, New DataLog("Field not found:" & pi.Name))
                    Throw New MissingFieldException("_" & pi.Name)
                Else
                    Dim value As Object = f.GetValue(data)
                    If value IsNot Nothing Then
                        p.Value = value
                    Else
                        If pi.IsNullable Then
                            p.Value = DBNull.Value
                        End If
                    End If
                End If
            End If

            cmd.Parameters.Add(p)
        Next
    End Sub

    Private Shared Sub ReadOne(Of T As New)(filler As FillObject, reader As IDataReader, data As FillStatus(Of T))
        If reader.Read Then
            FillData(reader, filler, data.Value)
            data.FillResult = FillResultEnum.DataFound
            data.Timestamp = Now.Ticks
            If TypeOf (data.Value) Is IEntityBase Then
                CType(data.Value, IEntityBase).Loaded = Now.Ticks
                CType(data.Value, IEntityBase).FillResult = FillResultEnum.DataFound
            End If
        End If
    End Sub

    Friend Shared Sub FillData(ByVal reader As IDataReader, ByVal filler As FillObject, ByVal data As Object)

        filler(reader, data)
    End Sub

    Private Shared ReadOnly PadLock As New Object
    Public Shared ReadOnly Fillers As New Dictionary(Of Integer, DataFiller)
    Private Shared ReadOnly Match As New System.Text.RegularExpressions.Regex("^.*?(?:(?= from(?!.*?(?=from)))|$)", Text.RegularExpressions.RegexOptions.IgnoreCase Or Text.RegularExpressions.RegexOptions.Singleline Or RegexOptions.Compiled) 'Match anything all the way to the last FROM. 

    Private Shared Function GetFiller(ByVal commandInfo As CommandInfo, ByVal dataReader As IDataReader, ByVal t As Type) As FillObject

        Dim key = GetHashCodeForCommand(commandInfo, t)
        If Not Fillers.ContainsKey(key) Then
            SyncLock PadLock
                If Not Fillers.ContainsKey(key) Then
                    Fillers(key) = New DataFiller(dataReader, t)
                End If
            End SyncLock
        End If
        Return AddressOf Fillers(key).FillObject
    End Function

    Private Shared Function GetHashCodeForCommand(ByVal commandInfo As CommandInfo, ByVal t As Type) As Integer
        Dim cmd = Match.Match(commandInfo.CommandText.ToLower).Value.Replace(" ", "")
        Return (cmd & t.ToString).GetHashCode
    End Function

    Friend Delegate Sub FillObject(reader As IDataReader, data As Object)
    Private Delegate Sub HandleReader(Of T As New)(filler As FillObject, reader As IDataReader, data As FillStatus(Of T))
#End Region

End Class
