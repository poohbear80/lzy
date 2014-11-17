Namespace Utils
    Public NotInheritable Class Helpers
        Private Sub New()
        End Sub


        Public Shared Function CloneObject(Of T As {LazyBaseClass, New})(org As T) As T
            Dim ret As New T
            Dim val As Object = Nothing

            For Each f In ret.Fields

                If org.GetValue(f, val) Then
                    ret.SetValue(f, val)
                    If TypeOf val Is DBNull Then
                        val = ""
                    End If
                    ret.ChangeLog.Add(New ChangedValue(f, val))
                End If
            Next
            Return ret
        End Function


    End Class
End Namespace
