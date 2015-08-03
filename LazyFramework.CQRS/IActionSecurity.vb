Imports System.Security.Principal


    Public Interface IActionSecurity
        Function UserCanRunThisAction(user As IPrincipal, action As IActionBase) As Boolean
        Function UserCanRunThisAction(user As IPrincipal, action As IActionBase, entity As Object) As Boolean
        Function EntityIsAvailableForUser(user As IPrincipal, ByVal action As IAmAnAction, ByVal entity As Object) As Boolean
        Function GetActionList(user As IPrincipal, action As IActionBase, entity As Object) As List(Of IActionDescriptor)

    End Interface

