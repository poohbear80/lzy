Imports System.Data.Common
Imports System.Xml

''' <summary>
''' The dataaccess interface
''' </summary>
''' <remarks></remarks>
Public Interface IDataAccess
    
    

    ''' <summary>
    ''' Fills the object.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="o">The IOrDataobject to fill</param>
    ''' <param name="Command">The command, as a </param>
    ''' <param name="params">The params.</param>
    ''' <returns></returns>
    Function FillObject(ByVal sourceName As String, ByVal o As IORDataObject, ByVal command As DbCommand, ByVal ParamArray params() As Object) As Boolean


    ''' <summary>
    ''' Fills the object.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="o">The o.</param>
    ''' <param name="Command">The command.</param>
    ''' <param name="params">The params.</param>
    ''' <returns></returns>
    Function FillObject(ByVal sourceName As String, ByVal o As Queue(Of IORDataObject), ByVal command As DbCommand, ByVal ParamArray params() As Object) As Boolean
    
    

    ''' <summary>
    ''' Updates the object.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="o">The o.</param>
    ''' <param name="cmd">The CMD.</param>
    ''' <returns></returns>
    Function UpdateObject(ByVal sourceName As String, ByVal o As IORDataObject, ByVal cmd As DbCommand) As Boolean

    

    ''' <summary>
    ''' Executes the command.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="Command">The command.</param>
    ''' <param name="params">The params.</param>
    ''' <returns></returns>
    Function ExecuteCommand(ByVal sourceName As String, ByVal command As DbCommand, ByVal ParamArray params() As Object) As Boolean


    ''' <summary>
    ''' Gets the data table.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="sqlQuery">The SQL query.</param>
    ''' <param name="commandType">Type of the command.</param>
    ''' <param name="parameters">The parameters.</param>
    ''' <returns></returns>
    Function GetDataTable(ByVal sourceName As String, ByVal sqlQuery As String, ByVal commandType As CommandType, ByVal ParamArray parameters As Object()) As DataTable


    ''' <summary>
    ''' Creates the command.
    ''' </summary>
    ''' <param name="cmd">The CMD.</param>
    ''' <returns></returns>
    Function CreateCommand(ByVal cmd As CommandInfo) As DbCommand

    ''' <summary>
    ''' Executes the scalar.
    ''' </summary>
    ''' <param name="sourceName">Name of the source.</param>
    ''' <param name="Command">The command.</param>
    ''' <param name="params">The params.</param>
    ''' <returns></returns>
    Function ExecuteScalar(ByVal sourceName As String, ByVal command As DbCommand, ByVal ParamArray params() As Object) As Object

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sourceName"></param>
    ''' <param name="command"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExecuteScalar(ByVal sourceName As String, ByVal command As DbCommand) As Object

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sourceName"></param>
    ''' <param name="command"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExecuteDatareader(sourceName As String, command As DbCommand) As DbDataReader

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sourceName"></param>
    ''' <param name="command"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExecuteXmlReader(sourceName As String, command As DbCommand) As XmlDocument

End Interface

