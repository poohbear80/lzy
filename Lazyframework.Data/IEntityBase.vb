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