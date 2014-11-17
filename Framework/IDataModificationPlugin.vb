Public Interface IDataModificationPlugin
    Sub InitPlugin(ByVal params As String)
    Function ExecuteTask(ByVal o As IORDataObject, ByVal modificationType As CommandInfoCommandTypeEnum, ByVal commandOk As Boolean) As Boolean
    Property ExecutionPoint() As PluginExecutionPointEnum
    Property PluginName() As String
End Interface
