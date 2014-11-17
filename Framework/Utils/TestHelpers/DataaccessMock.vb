Namespace Utils.TestHelpers
    Public MustInherit Class DataaccessMock(Of T As {IORDataObject, New})
        Protected Shared BackStore As New Dictionary(Of Integer, T)
        Protected Shared Counter As Integer = 0

        
        Public Delegate Function GetPrimaryKey(o As T) As Integer
        Public Delegate Sub SetPrimaryKey(o As T, id As Integer)

        Dim _spk As SetPrimaryKey
        Private _pk As GetPrimaryKey

        Public Sub New(pk As GetPrimaryKey, spk As SetPrimaryKey)
            _pk = pk
            _spk = spk
        End Sub

        Public Shared Sub AddMany(list As IEnumerable(Of T), pk As GetPrimaryKey, spk As SetPrimaryKey)
            For Each entry In list
                AddEntry(entry, pk, spk)
            Next
        End Sub

        Public Shared Sub AddEntry(o As T, pk As GetPrimaryKey, spk As SetPrimaryKey)
            If pk(o) = 0 Then
                spk(o, NextId)
            Else
                Counter = pk(o) + 1
            End If

            BackStore.Add(pk(o), o)
            Debug.Print(o.GetType.FullName)
        End Sub

        Public Shared Function GetFirst() As T
            Dim ret As New T
            CloneObj(BackStore.Values(0), ret)
            Return ret
        End Function

        Protected Shared ReadOnly Property NextId As Integer
            Get
                Counter += 1
                Debug.WriteLine(GetType(T).Name & " ID:" & Counter)
                Return Counter
            End Get
        End Property

        Public Shared Sub Clear()
            Counter = 0
            BackStore.Clear()
        End Sub


        ''' <summary>
        ''' Copies all values from the 'old' object to the 'new' object
        ''' </summary>
        ''' <param name="old">Contains values to copy from</param>
        ''' <param name="newObj">The object to recive values from 'old'</param>
        ''' <remarks></remarks>
        Protected Shared Sub CloneObj(old As T, newObj As T)

            Dim lOld, lNewobj As IORDataObject
            Dim val As Object

            lOld = CType(old, IORDataObject)
            lNewobj = CType(newObj, IORDataObject)


            For Each s In CType(old, IORDataObject).Fields
                lOld.GetValue(s, val)
                If val IsNot Nothing Then
                    lNewobj.SetValue(s, val)
                End If
            Next

        End Sub

        Protected Function GetFromStore(ByVal o As T) As Boolean
            If BackStore.ContainsKey(_pk(o)) Then
                CloneObj(BackStore(_pk(o)), o)
                o.FillResult = FillResultEnum.DataFound
                Return True
            Else
                o.FillResult = FillResultEnum.NoData
                Return False
            End If
        End Function

        Protected Function CreateInStore(ByVal o As T, setPk As SetPrimaryKey) As Boolean
            setPk(o, NextId)
            BackStore.Add(_pk(o), o)
            o.FillResult = FillResultEnum.UpdateOK
            Return True
        End Function

        Protected Function GetAllFromStore(o As List(Of T)) As Boolean
            o.AddRange(BackStore.Values)
            Return True
        End Function

        Protected Function DeleteInStore(o As T) As Boolean
            If BackStore.ContainsKey(_pk(o)) Then
                BackStore.Remove(_pk(o))

                Return True
            End If

            Return False
        End Function

        Protected Function UpdateInStore(o As T) As Boolean
            If BackStore.ContainsKey(_pk(o)) Then

                CloneObj(o, BackStore(_pk(o)))
                o.FillResult = FillResultEnum.UpdateOK
                Return True
            Else
                Return False
            End If

        End Function


        Protected Shared Function GetManyBySearch(Of TCol As {List(Of T), IORDataObject})(list As TCol, func As Criteria) As Boolean
            Dim ret As Boolean
            Dim o As T
            For Each element As T In BackStore.Values
                If func(element) Then
                    o = New T
                    CloneObj(element, o)
                    list.Add(o)
                    ret = True
                    list.FillResult = FillResultEnum.MultipleLinesFound
                End If
            Next
            Return ret
        End Function


        Protected Shared Function GetFirstBySearch(o As T, func As Criteria) As Boolean
            Dim ret As Boolean
            o.FillResult = FillResultEnum.NoData

            For Each element As T In BackStore.Values
                If func(element) Then
                    CloneObj(element, o)
                    ret = True
                    o.FillResult = FillResultEnum.DataFound
                End If
            Next

            Return ret
        End Function

        Protected Delegate Function Criteria(ByVal o As T) As Boolean

        Protected Function GetManyBySearch(Of T1)(o As T1) As Boolean
            Throw New NotImplementedException
        End Function

    End Class

End Namespace
