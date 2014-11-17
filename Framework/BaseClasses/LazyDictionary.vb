
Imports System.Runtime.Serialization

''' <summary>
''' Standard dictionary using generics and the IOrDataobject interface. Any collection wich inherits from this can be filled true the <see cref="IDataAccess.FillObject"></see> FillObjct function.
''' </summary>
''' <typeparam name="TType"></typeparam>
''' <remarks></remarks>
<Serializable(), CLSCompliant(True)> Public MustInherit Class LazyDictionary(Of TType As {IORDataObject, New})
    Inherits Dictionary(Of String, TType)
    Implements IORDataObject
    Implements IMultiLine

    Private _CurrentObj As TType

    Public Sub New()
        MyBase.New()
    End Sub


    ''' <summary>
    ''' Gets the object name collumn.
    ''' Identifies the coloumn to be used as the key in the dictionary.
    ''' </summary>
    ''' <value>The object name collumn.</value>
    MustOverride ReadOnly Property ObjectNameCollumn() As String

    ''' <summary>
    ''' Used to set the value
    ''' </summary>
    ''' <param name="name">Then name of the collum in the query. Map this against your local field.</param>
    ''' <param name="value">The value of the named param</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetValue(ByVal name As String, ByVal value As Object) As Boolean Implements IORDataObject.SetValue
        If _CurrentObj Is Nothing Then
            _CurrentObj = New TType
        End If
        Return _CurrentObj.SetValue(name, value)
    End Function

    ''' <summary>
    ''' Used to set the value
    ''' </summary>
    ''' <param name="name">Then name of the collum in the query. Map this against your local field.</param>
    ''' <param name="value">The value of the named param</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetValueExtended(ByVal name As String, ByVal value As Object) As Boolean Implements IORDataObject.SetValueExtended
        If _CurrentObj Is Nothing Then
            _CurrentObj = New TType
        End If
        Return _CurrentObj.SetValueExtended(name, value)
    End Function

    ''' <summary>
    ''' Retrives a value from a private field.
    ''' The names asked for is the same name as the parameter in an sql query
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetValue(ByVal name As String, ByRef value As Object) As Boolean Implements IORDataObject.GetValue
        Return False
    End Function


    ''' <summary>
    ''' Override this function to give back more values if needed.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetValueExtended(ByVal name As String, ByRef value As Object) As Boolean Implements IORDataObject.GetValueExtended
        Return False
    End Function

    Private _FillResult As FillResultEnum

    ''' <summary>
    ''' Set by the data access layer to tell then caller the result of then query
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

    Public Event FillComplete(ByVal sender As Object, ByVal e As FillCompleteEventargs) Implements IORDataObject.FillComplete

    ''' <summary>
    ''' Called by then data access provider when the query is completed.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Overridable Sub OnFillComplete(ByVal e As FillCompleteEventargs) Implements IORDataObject.OnFillComplete
        RaiseEvent FillComplete(Me, e)
        For Each o As TType In Values
            o.OnFillComplete(e)
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Function NextRow() As Boolean Implements IMultiLine.NextRow
        _CurrentObj.FillResult = FillResultEnum.DataFound
        _CurrentObj.Loaded = Now.Ticks
        Add(CallByName(_CurrentObj, ObjectNameCollumn, CallType.Get).ToString, _CurrentObj)
        _CurrentObj = New TType

        Return True
    End Function

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

    Public Sub SetMode(ByVal mode As IMultiLine.IMultiLineMode) Implements IMultiLine.SetMode

    End Sub

    Public Overloads ReadOnly Property Count() As Integer Implements IMultiLine.Count
        Get
            Return MyBase.Count
        End Get
    End Property

    ''' <summary>
    ''' All available fields in T
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Fields() As String() Implements IORDataObject.Fields
        Get
            Return _CurrentObj.Fields
        End Get
    End Property


    Public Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
    
    Public Overrides Sub GetObjectData(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
        MyBase.GetObjectData(info, context)
    End Sub

End Class
