Imports System.Reflection

Public Class DataFiller
    Private ReadOnly _Fields As New Dictionary(Of Integer, Reflection.FieldInfo)

    Public Sub New(ByVal dataReader As IDataReader, ByVal t As Type)
        'Her kunne vi laget noe lureri for å gjøre dette med emitting av il, men det lar vi være enn så lenge. 

        Dim n As String
        For x = 0 To dataReader.FieldCount - 1
            n = dataReader.GetName(x)
            Dim f As FieldInfo
            f = t.GetField("_" & n, BindingFlags.IgnoreCase Or BindingFlags.NonPublic Or BindingFlags.Instance)
            If f Is Nothing Then
                f = t.GetField(n, BindingFlags.IgnoreCase Or BindingFlags.Instance Or BindingFlags.Public)
            End If
            If f IsNot Nothing Then
                _Fields.Add(x, f)
            End If
        Next
    End Sub

    Public Sub FillObject(reader As IDataReader, data As Object)
        
        For Each f In _Fields
            Dim tempValue = reader.GetValue(f.Key)
            If TypeOf (tempValue) Is DBNull Then
                f.Value.SetValue(data, Nothing)
            Else
                f.Value.SetValue(data, tempValue)
            End If
        Next



    End Sub
End Class
