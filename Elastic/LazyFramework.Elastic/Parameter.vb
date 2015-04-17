Public Class Parameter
    Implements IDbDataParameter
    
    Public Property DbType As DbType Implements IDataParameter.DbType

    Public Property Direction As ParameterDirection Implements IDataParameter.Direction

    Public ReadOnly Property IsNullable As Boolean Implements IDataParameter.IsNullable
        Get

        End Get
    End Property

    Public Property ParameterName As String Implements IDataParameter.ParameterName

    Public Property SourceColumn As String Implements IDataParameter.SourceColumn

    Public Property SourceVersion As DataRowVersion Implements IDataParameter.SourceVersion

    Public Property Value As Object Implements IDataParameter.Value

    Public Property Precision As Byte Implements IDbDataParameter.Precision

    Public Property Scale As Byte Implements IDbDataParameter.Scale

    Public Property Size As Integer Implements IDbDataParameter.Size
End Class