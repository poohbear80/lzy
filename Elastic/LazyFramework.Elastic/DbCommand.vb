Public Class DbCommand
    Implements IDbCommand

    Public Sub Cancel() Implements IDbCommand.Cancel

    End Sub

    Public Property CommandText As String Implements IDbCommand.CommandText

    Public Property CommandTimeout As Integer Implements IDbCommand.CommandTimeout

    Public Property CommandType As CommandType Implements IDbCommand.CommandType

    Public Property Connection As IDbConnection Implements IDbCommand.Connection

    Public Function CreateParameter() As IDbDataParameter Implements IDbCommand.CreateParameter

    End Function

    Public Function ExecuteNonQuery() As Integer Implements IDbCommand.ExecuteNonQuery

    End Function

    Public Function ExecuteReader() As IDataReader Implements IDbCommand.ExecuteReader

    End Function

    Public Function ExecuteReader(behavior As CommandBehavior) As IDataReader Implements IDbCommand.ExecuteReader

    End Function

    Public Function ExecuteScalar() As Object Implements IDbCommand.ExecuteScalar

    End Function

    Public ReadOnly Property Parameters As IDataParameterCollection Implements IDbCommand.Parameters
        Get

        End Get
    End Property

    Public Sub Prepare() Implements IDbCommand.Prepare

    End Sub

    Public Property Transaction As IDbTransaction Implements IDbCommand.Transaction

    Public Property UpdatedRowSource As UpdateRowSource Implements IDbCommand.UpdatedRowSource

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class