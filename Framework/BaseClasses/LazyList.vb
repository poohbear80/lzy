Imports System.Text.RegularExpressions

''' <summary>
''' A base list collection using generics and implementing the IOrDataobject interface.
''' Also hold the abilities for paging and sorting.
''' </summary>
''' <typeparam name="T"></typeparam>
''' <remarks></remarks>
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class LazyList(Of T As {IORDataObject, New})
    Inherits List(Of T)
    Implements IORDataObject
    Implements IMultiLine



    Protected SortPropName As String = ""
    Protected SortAsc As Integer = 1
    Private _PageNo As Integer
    Private _PageSize As Integer
    Private _PageCache As List(Of T)
    Private _Count As Integer = 0

    Private _CurrentObj As T

    ''' <summary>
    ''' Gets or sets the current obj.
    ''' </summary>
    ''' <value>The current obj.</value>
    Protected Property CurrentObj() As T
        Get
            If _CurrentObj Is Nothing Then
                _CurrentObj = New T
            End If
            Return _CurrentObj
        End Get
        Set(ByVal value As T)
            _CurrentObj = value
        End Set
    End Property


    ''' <summary>
    ''' All available fields in T
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Fields() As String() Implements IORDataObject.Fields
        Get
            Return CurrentObj.Fields
        End Get
    End Property

    ''' <summary>
    ''' Resets the pager.
    ''' </summary>
    Public Sub ResetPager()
        _Count = 0
        _PageCache = Nothing
    End Sub

    ''' <summary>
    ''' Then size of the page for the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PageSize() As Integer
        Get
            Return _PageSize
        End Get
        Set(ByVal value As Integer)
            _PageSize = value
            SetStopAndStartIndex()
        End Set
    End Property

    ''' <summary>
    ''' Then current page of the collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PageNo() As Integer
        Get
            Return _PageNo
        End Get
        Set(ByVal value As Integer)
            _PageNo = value
            SetStopAndStartIndex()
        End Set
    End Property

    ''' <summary>
    ''' Used to set the value
    ''' </summary>
    ''' <param name="name">The name of the collum in the query. Map this against your local field.</param>
    ''' <param name="value">The value of the named param</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function SetValue(ByVal name As String, ByVal value As Object) As Boolean Implements IORDataObject.SetValue
        Return CurrentObj.SetValue(name, value)
    End Function

    ''' <summary>
    ''' Used to set the value
    ''' </summary>
    ''' <param name="name">The name of then collum in the query. Map this against your local field.</param>
    ''' <param name="value">The value of the named param</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function SetValueExtended(ByVal name As String, ByVal value As Object) As Boolean Implements IORDataObject.SetValueExtended
        Return CurrentObj.SetValueExtended(name, value)
    End Function

    Private _FillResult As FillResultEnum

    ''' <summary>
    ''' Filled by then data access layer to tell then caller the result of then query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FillResult() As FillResultEnum Implements IEntityBase.FillResult
        Get
            Return _FillResult
        End Get
        Set(ByVal value As FillResultEnum)
            _FillResult = value
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Event FillComplete(ByVal sender As Object, ByVal e As FillCompleteEventargs) Implements IORDataObject.FillComplete

    ''' <summary>
    ''' Called by the data access provider when the query is completed.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Overridable Sub onFillComplete(ByVal e As FillCompleteEventargs) Implements IORDataObject.OnFillComplete
        RaiseEvent FillComplete(Me, e)
        For Each o As T In Me
            o.OnFillComplete(e)
        Next
    End Sub


    ''' <summary>
    ''' Sorts the collection by property with then given name.
    ''' </summary>
    ''' <param name="propName">Name of the prop.</param>
    ''' <param name="asc">if set to <c>true</c> [asc].</param>
    Public Overridable Sub SortByProperty(ByVal propName As String, ByVal asc As Boolean)
        SortPropName = propName
        If asc Then
            SortAsc = 1
        Else
            SortAsc = -1
        End If
        Sort(AddressOf CompareObject)
    End Sub

    ''' <summary>
    ''' Sorting of the objects is done here...
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Overridable Function CompareObject(ByVal a As T, ByVal b As T) As Integer
        Dim Val1 As Object = Nothing
        Dim Val2 As Object = Nothing

        If Not a.GetValue(SortPropName, Val1) Then
            Throw New NotSupportedException("Could not find property named:" & SortPropName)
        End If
        If Not b.GetValue(SortPropName, Val2) Then
            Throw New NotSupportedException("Could not find property named:" & SortPropName)
        End If

        Return CompareValues(Val1, Val2, SortAsc)
    End Function


    Public Sub FilterByProperty(ByVal propName As String, ByVal value As Object, ByRef newList As LazyList(Of T))
        Dim val As Object = Nothing

        If String.IsNullOrEmpty(propName) OrElse value Is Nothing OrElse newList Is Nothing Then
            Return
        End If

        For Each o As IORDataObject In Me
            If o.GetValue(propName, val) Then
                If CompareValues(val, value, SortAsc) = 0 Then
                    newList.Add(CType(o, T))
                End If
            Else
                Throw New NotSupportedException("Property name:" & propName & " does not exist")
            End If
        Next
    End Sub

    Public Sub FilterByPropertyRegExp(ByVal propName As String, ByVal pattern As String, ByRef newList As LazyList(Of T))
        Dim r As New Regex(pattern)
        Dim val As String = ""

        For Each o As IORDataObject In Me
            If o.GetValue(propName, CObj(val)) Then
                If r.IsMatch(val) Then
                    newList.Add(CType(o, T))
                End If
            Else
                Throw New NotSupportedException("Property name:" & propName & " does not exist")
            End If
        Next
    End Sub


    Private _StartIndex As Integer = 0
    Private _StopIndex As Integer = 0

    Private Sub SetStopAndStartIndex()
        _StartIndex = _PageNo * _PageSize
        _StopIndex = (_PageNo * _PageSize) + _PageSize
    End Sub

    ''' <summary>
    ''' The possibility to iterate over pages in the collection.
    ''' </summary>
    ''' <param name="o"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>Unknown</remarks>
    Public Overridable Function PageCollection(ByVal o As T) As Boolean
        _Count += 1
        If _Count >= _StartIndex AndAlso _Count < _StopIndex Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Get a specific page from the collection
    ''' </summary>
    ''' <param name="page">Number of the page you want</param>
    ''' <returns>System.Collections.Generic.List(Of T)</returns>
    ''' <remarks></remarks>
    Public Function GetPage(ByVal page As Integer) As List(Of T)
        PageNo = page
        Return GetPage()
    End Function

    ''' <summary>
    ''' Get a specific page from the collection and set the pagesize.
    ''' </summary>
    ''' <param name="size">Size of page</param>
    ''' <param name="page">Page number to return</param>
    ''' <returns>System.Collections.Generic.List(Of T)</returns>
    ''' <remarks></remarks>
    Public Function GetPage(ByVal size As Integer, ByVal page As Integer) As List(Of T)
        PageSize = size
        Return GetPage(page)
    End Function

    ''' <summary>
    ''' Get the current page from the collection
    ''' </summary>
    ''' <returns>System.Collections.Generic.List(Of T)</returns>
    ''' <remarks></remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="GetPage")>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="PageSize")>
    Public Function GetPage() As List(Of T)
        If _PageCache Is Nothing Then
            If PageSize = 0 Then
                Throw New ApplicationException("You must set the PageSize property before calling then GetPage function.")
            End If
            _PageCache = FindAll(AddressOf PageCollection)
        End If
        Return _PageCache
    End Function


    ''' <summary>
    ''' get a queue from the collection
    ''' </summary>
    ''' <returns>System.Collections.Generic.Queue(Of T)</returns>
    ''' <remarks></remarks>
    Public Function GetQueue() As Queue(Of T)
        Dim r As New Queue(Of T)
        For Each o As T In Me
            r.Enqueue(o)
        Next
        Return r
    End Function

    ''' <summary>
    ''' Get a stack from the collection
    ''' </summary>
    ''' <returns>System.Collections.Generic.Stack(Of T)</returns>
    ''' <remarks></remarks>
    Public Function GetStack() As Stack(Of T)
        Dim r As New Stack(Of T)
        Dim x As Integer

        For x = Count - 1 To 0 Step -1
            r.Push(Me(x))
        Next
        Return r

    End Function


    Private _CurrentIndex As Integer = 0
    ''' <summary>
    ''' Tells the collection ready the next object for filling.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Function NextRow() As Boolean Implements IMultiLine.NextRow
        If _ListMode = IMultiLine.IMultiLineMode.Write Then
            CurrentObj.Loaded = Now.Ticks
            CurrentObj.FillResult = FillResultEnum.DataFound
            Add(CurrentObj)
            CurrentObj = New T
        Else
            If _CurrentIndex < Count Then
                CurrentObj = Me(_CurrentIndex)
                _CurrentIndex += 1
            Else
                Return False
            End If
        End If

        Return True

    End Function


    ''' <summary>
    ''' Part of the IOrDataobject interface
    ''' </summary>
    ''' <param name="name">Name of the value to get</param>
    ''' <param name="value">Then value</param>
    ''' <returns>boolean</returns>
    ''' <remarks>Not implemented in the baseclass</remarks>
    Public Overridable Function GetValue(ByVal name As String, ByRef value As Object) As Boolean Implements IORDataObject.GetValue
        Return CurrentObj.GetValue(name, value)
    End Function

    ''' <summary>
    ''' Part of the IOrDataobject interface
    ''' </summary>
    ''' <param name="name">Name of the value to get</param>
    ''' <param name="value">Then value</param>
    ''' <returns>boolean</returns>
    ''' <remarks>Not implemented in the baseclass</remarks>
    Public Overridable Function GetValueExtended(ByVal name As String, ByRef value As Object) As Boolean Implements IORDataObject.GetValueExtended
        Return CurrentObj.GetValueExtended(name, value)
    End Function

    ''' <summary>
    ''' Implements the operator +.
    ''' </summary>
    ''' <param name="col1">The col1.</param>
    ''' <param name="col2">The col2.</param>
    ''' <returns>The result of the operator.
    ''' Col1 is filled with the objects in Col2</returns>
    Public Shared Operator +(ByVal col1 As LazyList(Of T), ByVal col2 As LazyList(Of T)) As LazyList(Of T)
        For Each o As T In col2
            col1.Add(o)
        Next
        Return col1
    End Operator

    Private _Loaded As Long = Now.Ticks

    Public Property Loaded() As Long Implements IEntityBase.Loaded
        Get
            Return _Loaded
        End Get
        Friend Set(ByVal value As Long)
            _Loaded = value
        End Set
    End Property


    Public Function GetValueExtended1(ByVal name As String) As Object Implements IORDataObject.GetValueExtended
        Throw New ValueNameNotFoundException(name & "is not a part of this class " & Me.GetType.ToString)
    End Function

    Private _ListMode As IMultiLine.IMultiLineMode = IMultiLine.IMultiLineMode.Write
    Public Sub SetMode(ByVal mode As IMultiLine.IMultiLineMode) Implements IMultiLine.SetMode
        _ListMode = mode
    End Sub

    Public Overloads ReadOnly Property Count() As Integer Implements IMultiLine.Count
        Get
            Return MyBase.Count
        End Get
    End Property
End Class
