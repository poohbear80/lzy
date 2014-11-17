Public MustInherit Class DataModificationPluginBase
    Implements IDataModificationPlugin

    ''' <summary>
    ''' Executes the task.
    ''' </summary>
    ''' <param name="o">The instance.</param>
    ''' <param name="modificationType">Type of the modification.</param>
    ''' <param name="commandOk">if set to <c>true</c> [command ok].</param>
    ''' <returns></returns>
    Public MustOverride Function ExecuteTask(ByVal o As IORDataObject, ByVal modificationType As CommandInfoCommandTypeEnum, ByVal commandOk As Boolean) As Boolean Implements IDataModificationPlugin.ExecuteTask

    Protected PassedParemeters As New List(Of String)

    ''' <summary>
    ''' Inits the plugin.
    ''' </summary>
    ''' <param name="params">The params.</param>
    Public Overridable Sub InitPlugin(ByVal params As String) Implements IDataModificationPlugin.InitPlugin
        PassedParemeters.AddRange(params.Split(","c))
    End Sub

    Private _Ep As PluginExecutionPointEnum

    ''' <summary>
    ''' Gets or sets the execution point.
    ''' </summary>
    ''' <value>The execution point.</value>
    Public Property ExecutionPoint() As PluginExecutionPointEnum Implements IDataModificationPlugin.ExecutionPoint
        Get
            Return _ep
        End Get
        Set(ByVal value As PluginExecutionPointEnum)
            _ep = value
        End Set
    End Property

    Private _Name As String

    ''' <summary>
    ''' Gets or sets the name of the plugin.
    ''' </summary>
    ''' <value>The name of the plugin.</value>
    Public Property PluginName() As String Implements IDataModificationPlugin.PluginName
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
End Class
