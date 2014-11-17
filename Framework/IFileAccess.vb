''' <summary>
''' For now this is a readonly interface
''' </summary>
''' <remarks></remarks>
Public Interface IFileAccess
    Function GetData(ByVal File As String, ByVal Command As CommandInfo, ByVal o As IORDataObject, ByVal ParamArray Parameters As Object()) As Boolean
End Interface
