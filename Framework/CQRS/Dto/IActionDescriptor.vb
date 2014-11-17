Namespace CQRS
    Public Interface IActionDescriptor
        Property Name As String
        Property Type As String

    End Interface


    Public Class ActionDescriptor
        Implements IActionDescriptor
        
        Public Property Name As String Implements IActionDescriptor.Name
        Public Property Type As String Implements IActionDescriptor.Type

    End Class

End NameSpace
