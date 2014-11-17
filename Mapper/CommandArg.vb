Friend Class CommandArg
    Public Sub New(cmdArg As String)
        Dim args = cmdArg.Split("\"c)

        For x = 0 To args.Length - 1
            Select Case x
                Case 0
                    TabelName = args(x)
                Case 1
                    DbProfileName = args(x)
                Case 2
                    ProjectName = args(x)
            End Select
        Next
    End Sub
    Public Property TabelName As String
    Public Property DbProfileName As String
    Public Property ProjectName As String
End Class
