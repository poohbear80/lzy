Public Enum FillResultEnum As Integer
    ''' <summary>
    ''' Set when query returns no rows
    ''' </summary>
    ''' <remarks></remarks>
    NoData = 0
    ''' <summary>
    ''' Set when only 1 line is returned from then query
    ''' </summary>
    ''' <remarks></remarks>
    DataFound = 1
    ''' <summary>
    ''' Set when more than 1 line is returned from query
    ''' </summary>
    ''' <remarks></remarks>
    MultipleLinesFound = 2
    ''' <summary>
    ''' Set when an dataupdate query returned no errors
    ''' </summary>
    ''' <remarks></remarks>
    UpdateOk = 4
End Enum
