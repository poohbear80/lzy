Imports System.Runtime.Serialization

''' <summary>
''' The base class for LazyFramework.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public MustInherit Class LazyBaseClass
    Implements IORDataObject

    Private _Dirty As Boolean ' = False

    ''' <summary>
    ''' Is this object changed?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Dirty() As Boolean
        Get
            Return _Dirty
        End Get
        Set(ByVal value As Boolean)
            _Dirty = value
        End Set
    End Property

    Private _ChangeLog As ChangedValueCollection

    ''' <summary>
    ''' All values changed in the object from when it was loaded from the database is saved here.. 
    ''' Can be used to collect all changed values.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Used by a datamodification plugin to store histrical data for an entity.</remarks>
    Public ReadOnly Property ChangeLog() As ChangedValueCollection
        Get
            If _ChangeLog Is Nothing Then
                _ChangeLog = New ChangedValueCollection
            End If
            Return _ChangeLog
        End Get
    End Property

    ''' <summary>
    ''' Used to set the value
    ''' </summary>
    ''' <param name="name">Then name of then collum in the query. Map this against your local field.</param>
    ''' <param name="value">The value of then named param</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function SetValue(ByVal name As String, ByVal value As Object) As Boolean Implements IORDataObject.SetValue

    ''' <summary>
    ''' Retrives a value from a private field.
    ''' The names asked for is the same name as the parameter in an sql query
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetValue(ByVal name As String, ByRef value As Object) As Boolean Implements IORDataObject.GetValue


    ''' <summary>
    ''' You must overide this function to handle custom values that is collected in the DB, this function is called if InsertValue returns false,
    ''' in the loop in FillObject.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function SetValueExtended(ByVal name As String, ByVal value As Object) As Boolean Implements IORDataObject.SetValueExtended

    End Function

    ''' <summary>
    ''' Override this function, if you are adding extra parameteres to your object
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetValueExtended(ByVal name As String, ByRef value As Object) As Boolean Implements IORDataObject.GetValueExtended
        Return False
    End Function

    Public Overridable Function GetValueExtended(ByVal name As String) As Object Implements IORDataObject.GetValueExtended
        Throw New ValueNameNotFoundException(name)
    End Function


    ''' <summary>
    ''' Check this to see if the dataacces command was successfull
    ''' </summary>
    ''' <remarks></remarks>
    <NonSerialized> _
    Private _FillResult As FillResultEnum

    Public Property FillResult() As FillResultEnum Implements IEntityBase.FillResult
        Get
            Return _FillResult
        End Get
        Set(ByVal value As FillResultEnum)
            _FillResult = value
        End Set
    End Property

    ''' <summary>
    ''' Event fired when the object is filled. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event FillComplete(ByVal sender As Object, ByVal e As FillCompleteEventargs) Implements IORDataObject.FillComplete

    ''' <summary>
    ''' This event fires when the database layer is finished with loading then object with data.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Overridable Sub OnFillComplete(ByVal e As FillCompleteEventargs) Implements IORDataObject.OnFillComplete

    End Sub


    ''' <summary>
    ''' Override this sub to Desrialize your custom values. 
    ''' This sub is called from then special constructor implemented for the iSerializable interface
    ''' </summary>
    ''' <param name="info"></param>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Protected Sub Deserialize(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        TryGet(_ChangeLog, info.GetValue("ChangeLog", GetType(ChangedValueCollection)))
        TryGet(_FillResult, info.GetUInt32("FillResult"))
        TryGet(_Loaded, info.GetInt64("Loaded"))
        TryGet(_Dirty, info.GetBoolean("Dirty"))
    End Sub

    ''' <summary>
    ''' Override this sub to fill the info object with your custom data for serialization
    ''' This sub is called from the GetObjectData sub witch implements the ISerialize interface.
    ''' </summary>
    ''' <param name="info"></param>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Protected Sub Serialize(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        info.AddValue("ChangeLog", ChangeLog)
        info.AddValue("FillResult", FillResult)
        info.AddValue("Dirty", Dirty)
        info.AddValue("Loaded", Loaded)
    End Sub

    ''' <summary>
    ''' Used to retrive a value from the serialsation context.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="param"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Protected Sub TryGet(Of T)(ByRef param As T, ByVal value As Object)
        If Not value Is Nothing Then
            param = CType(value, T)
        End If
    End Sub

    ''' <summary>
    ''' Used by properties to add a value to the cahngedValue collection.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Protected Sub AddChangedValue(ByVal name As String, ByVal value As Object)
        Dim clv As New ChangedValue(name, value)

        For Each c As ChangedValue In ChangeLog
            If c.FieldName = clv.FieldName Then
                c.value = value
                Return
            End If
        Next
        ChangeLog.Add(clv)
    End Sub

    Private _Loaded As Long

    ''' <summary>
    ''' Stores info for when the object is loaded from the databse.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Loaded() As Long Implements IEntityBase.Loaded
        Get
            Return _Loaded
        End Get
        Set(ByVal value As Long)
            _Loaded = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the fields.
    ''' </summary>
    ''' <value>The fields.</value>
    Public Overridable ReadOnly Property Fields() As String() Implements IORDataObject.Fields
        Get
            Return Nothing
        End Get
    End Property
End Class

Public Class EntityBase
    Implements IEntityBase
    
    Public Property FillResult As FillResultEnum Implements IEntityBase.FillResult

    Private _Loaded As Long
    Public Property Loaded As Long Implements IEntityBase.Loaded
        Get
            Return _loaded
        End Get
        Friend Set(value As Long)
            _loaded = value
        End Set
    End Property
End Class
