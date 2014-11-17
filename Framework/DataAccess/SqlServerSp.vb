' ''' <summary>
' ''' A class for typesafing of sp's in the database
' ''' </summary>
' ''' <typeparam name="TGeneric"></typeparam>
' ''' <remarks></remarks>
'    <Obsolete()>
'    <Serializable> Public MustInherit Class SqlServerSp(Of TGeneric As ItasParamClass)
'    Private ReadOnly _cmd As New SqlCommand
'    Private _dtResult As DataTable
'    Private _retVal As Integer
'    Public MustOverride ReadOnly Property SpName() As String
'    Public MustOverride ReadOnly Property Params() As TGeneric

'    ''' <summary>
'    ''' 
'    ''' </summary>
'    ''' <value></value>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Public Property RetVal() As Integer
'        Get
'            Return _retVal
'        End Get
'        Set(ByVal value As Integer)
'            _retVal = value
'        End Set
'    End Property

'    ''' <summary>
'    ''' 
'    ''' </summary>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Public Function Execute() As Boolean
'        Using _cmd
'            _cmd.CommandText = SpName
'            _cmd.CommandType = CommandType.StoredProcedure
'            _cmd.Connection = New SqlConnection(SQLServer.GetConnectionString(DbName))
'            _cmd.Connection.Open()
'            Params.GetParams(_cmd)

'            _dtResult = New DataTable()
'            _dtResult.Load(_cmd.ExecuteReader)
'            _retVal = CInt(_cmd.Parameters(0).Value)
'            _cmd.Connection.Close()
'        End Using
'    End Function

'    ''' <summary>
'    ''' 
'    ''' </summary>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Public Function ExecuteNonQuery() As Boolean
'        Using _cmd
'            _cmd.CommandText = SpName
'            _cmd.CommandType = CommandType.StoredProcedure
'            _cmd.Connection = New SqlConnection(SQLServer.GetConnectionString(DbName))
'            _cmd.Connection.Open()
'            Params.GetParams(_cmd)
'            _cmd.ExecuteNonQuery()
'            _cmd.Connection.Close()
'        End Using
'    End Function

'    ''' <summary>
'    ''' 
'    ''' </summary>
'    ''' <value></value>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Public ReadOnly Property ResultSet() As DataTable
'        Get
'            Return _dtResult
'        End Get
'    End Property

'    Private _dbName As String

'    ''' <summary>
'    ''' 
'    ''' </summary>
'    ''' <value></value>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Public Property DbName() As String
'        Get
'            Return _dbName
'        End Get
'        Set(ByVal value As String)
'            _dbName = value
'        End Set
'    End Property

'    ''' <summary>
'    ''' 
'    ''' </summary>
'    ''' <remarks></remarks>
'        Public MustInherit Class ItasParamClass
'        Public MustOverride Sub GetParams(ByVal cmd As SqlCommand)
'    End Class
'End Class
