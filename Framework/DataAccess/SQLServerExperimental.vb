Imports System.Xml

Public Class SqlServerExperimental
    Implements IDataAccess

    'Private Shared ReadOnly InMapperList As New Dictionary(Of UInt32, IInMapper)
    'Private Shared ReadOnly OutMapperList As New Dictionary(Of UInt32, IOutMapper)

    Public Function CreateCommand(cmd As CommandInfo) As DbCommand Implements IDataAccess.CreateCommand
        Dim ret As DbCommand = Nothing
        Dim p As SqlParameter

        If cmd Is Nothing Then
            Return ret
        End If

        ret = New SqlCommand
        ret.CommandText = cmd.CommandText
        ret.CommandType = cmd.CommandType

        For Each pi As ParameterInfo In cmd.Parameters.Values
            p = New SqlParameter(pi.Name, pi.DbType)
            p.IsNullable = pi.IsNullable
            If pi.Size <> 0 Then
                p.Size = pi.Size
            End If
            If pi.Value IsNot Nothing Then
                p.Value = pi.Value
            End If
            ret.Parameters.Add(p)
        Next
        Return ret
    End Function

    

    Public Function ExecuteCommand(sourceName As String, command As DbCommand, ParamArray params() As Object) As Boolean Implements IDataAccess.ExecuteCommand

    End Function

    Public Function ExecuteScalar(sourceName As String, command As DbCommand, ParamArray params() As Object) As Object Implements IDataAccess.ExecuteScalar
        Throw New NotImplementedException
    End Function

    

    Public Function FillObject(sourceName As String, o As IORDataObject, command As DbCommand, ParamArray params() As Object) As Boolean Implements IDataAccess.FillObject

    End Function

    
    Public Function FillObject(sourceName As String, o As Queue(Of IORDataObject), command As DbCommand, ParamArray params() As Object) As Boolean Implements IDataAccess.FillObject

    End Function

    Public Function GetDataTable(sourceName As String, sqlQuery As String, commandType As CommandType, ParamArray parameters() As Object) As DataTable Implements IDataAccess.GetDataTable

        Throw New NotImplementedException
    End Function


    Public Function UpdateObject(sourceName As String, o As IORDataObject, cmd As DbCommand) As Boolean Implements IDataAccess.UpdateObject

    End Function

    Public Function ExecuteScalar(sourceName As String, command As System.Data.Common.DbCommand) As Object Implements IDataAccess.ExecuteScalar
        Throw New NotImplementedException
    End Function

    Public Function ExecuteDatareader(ByVal sourceName As String, ByVal command As DbCommand) As DbDataReader Implements IDataAccess.ExecuteDatareader
        Throw New NotImplementedException()
    End Function

    Public Function ExecuteXmlReader(ByVal sourceName As String, ByVal command As DbCommand) As XmlDocument Implements IDataAccess.ExecuteXmlReader
        Throw New NotImplementedException()
    End Function
End Class
