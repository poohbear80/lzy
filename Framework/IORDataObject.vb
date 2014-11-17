Public Interface IEntityBase
    ''' <summary>
    ''' Filled by the data access layer to tell then caller the result of then query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property FillResult() As FillResultEnum

    ''' <summary>
    ''' When is this entity loaded from then repository
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Loaded() As Long


End Interface


''' <summary>
''' The base interface for talking with the dataaccess layer.
''' </summary>
''' <remarks></remarks>
<CLSCompliant(True)> Public Interface IORDataObject
    Inherits IEntityBase
    
    ''' <summary>
    ''' Used to set the value 
    ''' </summary>
    ''' <param name="name">Then name of then collum in the query. Map this against your local field.</param>
    ''' <param name="value">The value of then named param</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function SetValue(ByVal name As String, ByVal value As Object) As Boolean

    ''' <summary>
    ''' Used to set the value 
    ''' </summary>
    ''' <param name="name">Then name of then collum in the query. Map this against your local field.</param>
    ''' <param name="value">The value of then named param</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function SetValueExtended(ByVal name As String, ByVal value As Object) As Boolean

    ''' <summary>
    ''' Retrives a value from a private field. 
    ''' The names asked for is the same name as the parameter in an sql query
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetValue(ByVal name As String, ByRef value As Object) As Boolean

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetValueExtended(ByVal name As String, ByRef value As Object) As Boolean
    Function GetValueExtended(ByVal name As String) As Object

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Event FillComplete As EventHandler(Of FillCompleteEventargs)

    ''' <summary>
    ''' Called by then data access provider when the query is completed. 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Sub OnFillComplete(ByVal e As FillCompleteEventargs)

    ReadOnly Property Fields() As String()
End Interface
