Namespace Validation
    Public Class ValidationException
        Inherits Exception

        Private ReadOnly _Inner As New Dictionary(Of String, Exception)
        Public ReadOnly Property ExceptionList As Dictionary(Of String, Exception)
            Get
                Return _Inner
            End Get
        End Property
    End Class
End NameSpace
