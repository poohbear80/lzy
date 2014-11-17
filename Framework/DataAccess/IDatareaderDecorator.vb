''' <summary>
''' Wraps the IMultiline/IORdataobject with an IDataReader interface
''' </summary>
''' <remarks>Experimental</remarks>
Friend Class IMultiLineDatareaderDecorator
    Implements IDataReader

    Private ReadOnly dataList As IMultiLine
    Private ReadOnly dataObject As IORDataObject

    Private _TableName As String
    Public Sub New(ByVal d As IMultiLine, ByVal name As String)
        dataList = d
        If TypeOf (d) Is IORDataObject Then
            dataObject = CType(d, IORDataObject)
        Else
            Throw New ArgumentException("Type of d must be IORDataObject")
        End If
        _TableName = name
    End Sub

    Public Sub Close() Implements IDataReader.Close

    End Sub

    Public ReadOnly Property Depth() As Integer Implements IDataReader.Depth
        Get

        End Get
    End Property

    Public Function GetSchemaTable() As DataTable Implements IDataReader.GetSchemaTable
        Return Nothing
    End Function

    Public ReadOnly Property IsClosed() As Boolean Implements IDataReader.IsClosed
        Get
            Return False
        End Get
    End Property

    Public Function NextResult() As Boolean Implements IDataReader.NextResult
        Return dataList.NextRow()
    End Function

    Public Function Read() As Boolean Implements IDataReader.Read
        Return dataList.NextRow
    End Function

    Public ReadOnly Property RecordsAffected() As Integer Implements IDataReader.RecordsAffected
        Get
            Return dataList.Count
        End Get
    End Property

    Public ReadOnly Property FieldCount() As Integer Implements IDataRecord.FieldCount
        Get
            Return dataObject.Fields.Count
        End Get
    End Property

    Public Function GetBoolean(ByVal i As Integer) As Boolean Implements IDataRecord.GetBoolean
        Return CType(GetValue(i), Boolean)
    End Function

    Public Function GetByte(ByVal i As Integer) As Byte Implements IDataRecord.GetByte
        Return CType(GetValue(i), Byte)
    End Function

    Public Function GetBytes(ByVal i As Integer, ByVal fieldOffset As Long, ByVal buffer() As Byte, ByVal bufferoffset As Integer, ByVal length As Integer) As Long Implements IDataRecord.GetBytes

    End Function

    Public Function GetChar(ByVal i As Integer) As Char Implements IDataRecord.GetChar
        Return CType(GetValue(i), Char)
    End Function

    Public Function GetChars(ByVal i As Integer, ByVal fieldoffset As Long, ByVal buffer() As Char, ByVal bufferoffset As Integer, ByVal length As Integer) As Long Implements IDataRecord.GetChars

    End Function

    Public Function GetData(ByVal i As Integer) As IDataReader Implements IDataRecord.GetData
        Return Me
    End Function

    Public Function GetDataTypeName(ByVal i As Integer) As String Implements IDataRecord.GetDataTypeName
        Return Nothing
    End Function

    Public Function GetDateTime(ByVal i As Integer) As Date Implements IDataRecord.GetDateTime
        Return CType(GetValue(i), DateTime)
    End Function

    Public Function GetDecimal(ByVal i As Integer) As Decimal Implements IDataRecord.GetDecimal
        Return CType(GetValue(i), Decimal)
    End Function

    Public Function GetDouble(ByVal i As Integer) As Double Implements IDataRecord.GetDouble
        Return CType(GetValue(i), Double)
    End Function

    Public Function GetFieldType(ByVal i As Integer) As Type Implements IDataRecord.GetFieldType
        Return Nothing
    End Function

    Public Function GetFloat(ByVal i As Integer) As Single Implements IDataRecord.GetFloat
        Return CType(GetValue(i), Single)
    End Function

    Public Function GetGuid(ByVal i As Integer) As Guid Implements IDataRecord.GetGuid
        Return CType(GetValue(i), Guid)
    End Function

    Public Function GetInt16(ByVal i As Integer) As Short Implements IDataRecord.GetInt16
        Return CType(GetValue(i), Int16)
    End Function

    Public Function GetInt32(ByVal i As Integer) As Integer Implements IDataRecord.GetInt32
        Return CType(GetValue(i), Int32)
    End Function

    Public Function GetInt64(ByVal i As Integer) As Long Implements IDataRecord.GetInt64
        Return CType(GetValue(i), Int64)
    End Function

    Public Function GetName(ByVal i As Integer) As String Implements IDataRecord.GetName
        Return dataObject.Fields(i)
    End Function

    Public Function GetOrdinal(ByVal name As String) As Integer Implements IDataRecord.GetOrdinal
        For x As Integer = 0 To dataObject.Fields.Count - 1
            If dataObject.Fields(x).ToLower = name.ToLower Then
                Return x
            End If
        Next
        Throw New NotSupportedException("Field with name " & name & " is not supported. Supported fields:" & Join(dataObject.Fields, ","))
    End Function

    Public Function GetString(ByVal i As Integer) As String Implements IDataRecord.GetString
        Return CType(GetValue(i), String)
    End Function

    Public Function GetValue(ByVal i As Integer) As Object Implements IDataRecord.GetValue
        Dim ret As Object = Nothing
        If dataObject.GetValue(GetName(i), ret) Then
            Return ret
        Else
            Throw New ArgumentOutOfRangeException("Value not found")
        End If
    End Function

    Public Function GetValues(ByVal values() As Object) As Integer Implements IDataRecord.GetValues

    End Function

    Public Function IsDBNull(ByVal i As Integer) As Boolean Implements IDataRecord.IsDBNull
        Dim ret As Object = GetValue(i)

        If TypeOf ret Is Nullable Then
            Return True
        End If
        Return False
    End Function

    Default Public Overloads ReadOnly Property Item(ByVal i As Integer) As Object Implements IDataRecord.Item
        Get
            Return Nothing
        End Get
    End Property

    Default Public Overloads ReadOnly Property Item(ByVal name As String) As Object Implements IDataRecord.Item
        Get
            Return Nothing
        End Get
    End Property

    Private _DisposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not _DisposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        _DisposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
