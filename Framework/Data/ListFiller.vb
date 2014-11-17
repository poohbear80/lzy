Friend Class ListFiller
    
    Public Sub FillList(Of T As {New, IList})(filler As Data.FillObject, reader As IDataReader, data As FillStatus(Of T))
        'Data her er en ienumerable(of T)
        Dim counter As Integer = 0
        Dim listObjectType As Type = Nothing

        Dim type = data.Value.GetType
        While type IsNot Nothing
            If type.IsGenericType Then
                listObjectType = type.GetGenericArguments(0)
                Exit While
            End If
            type = type.BaseType
        End While

        If listObjectType Is Nothing Then
            Throw New NotSupportedException("List must inherit from System.Collection.Generic.List<type>")
        End If
        
        While reader.Read
            counter += 1
            Dim toFill = Activator.CreateInstance(listObjectType)
            LazyFramework.Data.FillData(reader, filler, toFill)
            data.Value.Add(toFill)
        End While


        Select Case counter
            Case 0
                data.FillResult = FillResultEnum.NoData
            Case 1
                data.FillResult = FillResultEnum.DataFound
            Case Else
                data.FillResult = FillResultEnum.MultipleLinesFound
        End Select

    End Sub
End Class
