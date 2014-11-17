Imports Microsoft.SqlServer.Management.Smo

Namespace Classes
    Public Class TypeMapper

        Private Shared ReadOnly Dtypes As New Dictionary(Of String, DbType) From {
                    {"varchar", DbType.String},
                    {"nvarchar", DbType.String},
                    {"char", DbType.String}
                }

        Public Shared Function ConvertDataType(ByVal sqlType As DataType) As String
            If dtypes.ContainsKey(sqlType.ToString) Then
                Return dtypes(sqlType.ToString).ToString
            End If
            
            Return sqlType.ToString

        End Function

    End Class
End Namespace
