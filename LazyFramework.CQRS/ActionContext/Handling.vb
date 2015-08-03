Namespace ActionContext
    Public Class Handling
        Private Shared ReadOnly Padlock As New Object
        Private Shared _AllMappings As Dictionary(Of Type, List(Of ActionContext))
        Private Shared ReadOnly Property AllMappings As Dictionary(Of Type, List(Of ActionContext))
            Get
                If _AllMappings Is Nothing Then
                    SyncLock Padlock
                        If _AllMappings Is Nothing Then
                            Dim temp As New Dictionary(Of Type, List(Of ActionContext))
                            For Each ac In AllContexts
                                For Each action In ac.Value.Actions
                                    Dim key As Type = action.GetType
                                    If Not temp.ContainsKey(key) Then
                                        temp.Add(key, New List(Of ActionContext))
                                    End If
                                    temp(key).Add(ac.Value)
                                Next
                            Next
                            _AllMappings = temp
                        End If
                    End SyncLock
                End If
                Return _AllMappings
            End Get
        End Property

        Private Shared ReadOnly Padlock2 As New Object
        Private Shared _AllContexts As Dictionary(Of String, ActionContext)
        Public Shared ReadOnly Property AllContexts() As IDictionary(Of String, ActionContext)
            Get
                If _AllContexts Is Nothing Then
                    SyncLock Padlock2
                        If _AllContexts Is Nothing Then
                            Dim temp As New Dictionary(Of String, ActionContext)
                            Dim ac As ActionContext
                            For Each t In TypeValidation.FindAllClassesOfTypeInApplication(GetType(ActionContext))
                                ac = DirectCast(Activator.CreateInstance(t), ActionContext)
                                temp.Add(ac.GetType.Name, ac)
                            Next
                            _AllContexts = temp
                        End If
                    End SyncLock
                End If
                Return _AllContexts
            End Get
        End Property

        Public Shared Iterator Function GetContextsForAction(action As IActionBase) As IEnumerable(Of ActionContext)
            If AllMappings.ContainsKey(action.GetType) Then
                For Each a In AllMappings(action.GetType)
                    Yield a
                Next
            End If
        End Function


    End Class
End NameSpace