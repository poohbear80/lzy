Module CompareFunctions
    Public Function CompareValues(ByVal val1 As Object, ByVal val2 As Object, ByVal sortAsc As Integer) As Integer
        If TypeOf val1 Is DBNull And TypeOf Val2 Is DBNull Then Return 0

        'If TypeOf Val1 Is Date? Then
        '    If CType(Val1, Date?).HasValue Then
        '        Val1 = CType(Val1, Date?).Value
        '    Else
        '        Val1 = Nothing
        '    End If
        'End If

        'If TypeOf Val2 Is Date? Then
        '    If CType(Val2, Date?).HasValue Then
        '        Val2 = CType(Val2, Date?).Value
        '    Else
        '        Val2 = Nothing
        '    End If
        'End If


        If TypeOf val1 Is String OrElse TypeOf Val2 Is String Then
            If TypeOf val1 Is DBNull Then val1 = ""
            If TypeOf Val2 Is DBNull Then Val2 = ""

            Return StrComp(CStr(val1), CStr(Val2), CompareMethod.Text) * sortAsc
        End If

        If (TypeOf val1 Is Integer Or TypeOf val1 Is Long) OrElse (TypeOf Val2 Is Integer Or TypeOf Val2 Is Long) Then
            If TypeOf val1 Is DBNull Then val1 = 0
            If TypeOf Val2 Is DBNull Then Val2 = 0

            Return IntComp(CInt(val1), CInt(Val2)) * sortAsc
        End If

        If TypeOf val1 Is Date OrElse TypeOf Val2 Is Date Then
            If TypeOf val1 Is DBNull Then val1 = New Date
            If TypeOf Val2 Is DBNull Then Val2 = New Date

            Return IntComp(CDate(val1).Ticks, CDate(Val2).Ticks) * sortAsc
        End If

        If TypeOf val1 Is Double OrElse TypeOf Val2 Is Double Then
            If TypeOf val1 Is DBNull Then val1 = 0
            If TypeOf Val2 Is DBNull Then Val2 = 0

            Return DblComp(CDbl(val1), CDbl(Val2)) * sortAsc
        End If


        Throw New NotSupportedException("Sorting on properties of type:" & val1.GetType.ToString)
    End Function

    Private Function IntComp(ByVal i1 As Long, ByVal i2 As Long) As Integer
        Select Case i1 - i2
            Case Is > 0
                Return 1
            Case Is < 0
                Return -1
            Case Is = 0
                Return 0
        End Select
    End Function

    Private Function DblComp(ByVal d1 As Double, ByVal d2 As Double) As Integer
        Select Case d1 - d2
            Case Is > 0
                Return 1
            Case Is < 0
                Return -1
            Case Is = 0
                Return 0
        End Select
    End Function
End Module
