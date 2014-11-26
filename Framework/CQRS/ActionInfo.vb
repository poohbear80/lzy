Imports System.Security.Principal
Imports LazyFramework.CQRS.Command

Namespace CQRS
    Public Class ActionInfo

        Private Shared _actionsForType As Dictionary(Of Type, List(Of Type))
        Private Shared ReadOnly Pl As New Object

        Private Shared ReadOnly Property AllActions As Dictionary(Of Type, List(Of Type))
            Get
                If _actionsForType Is Nothing Then
                    SyncLock Pl
                        If _actionsForType Is Nothing Then
                            Dim temp As New Dictionary(Of Type, List(Of Type))
                            For Each el In TypeValidation.FindAllClassesOfTypeInApplication(GetType(IActionBase))
                                If el.IsAbstract Then Continue For

                                Dim basetype = el.BaseType
                                'Have to find the common basetype
                                While basetype IsNot Nothing AndAlso Not basetype.IsGenericType
                                    basetype = basetype.BaseType
                                End While

                                If basetype IsNot Nothing Then
                                    Dim getGenericArguments As Type = basetype.GetGenericArguments(0)
                                    If Not temp.ContainsKey(getGenericArguments) Then
                                        temp.Add(getGenericArguments, New List(Of Type))
                                    End If
                                    temp(getGenericArguments).Add(el)
                                End If
                            Next
                            _actionsForType = temp
                        End If
                    End SyncLock
                End If
                Return _actionsForType
            End Get
        End Property


        Public Shared Function GetAvailableActionsForType(user As IPrincipal, entityType As Type) As List(Of IActionBase)
            Dim ret As New List(Of IActionBase)
            If AllActions.ContainsKey(entityType) Then
                For Each t In _actionsForType(entityType)
                    Dim createInstance As IActionBase = CType(Activator.CreateInstance(t), IActionBase)
                    If createInstance.IsAvailable(user) Then
                        ret.Add(createInstance)
                    End If
                Next
            End If
            Return ret
        End Function

        Public Shared Function GetAvailableActionsForEntity(user As IPrincipal, entity As Object) As List(Of IActionBase)
            Dim ret As New List(Of IActionBase)
            If AllActions.ContainsKey(entity.GetType) Then
                For Each t In _actionsForType(entity.GetType)
                    Dim createInstance As IActionBase = CType(Activator.CreateInstance(t), IActionBase)
                    If createInstance.IsAvailable(user, entity) Then
                        If TypeOf (createInstance) Is CommandBase Then
                            CType(createInstance, CommandBase).SetInnerEntity(entity)
                        End If
                        If Not ActionSecurity.Current.UserCanRunThisAction(user, createInstance, If(TypeOf (entity) Is IProvideSecurityContext, DirectCast(entity, IProvideSecurityContext).Context, entity)) Then
                            Continue For
                        End If
                        ret.Add(createInstance)
                    End If
                Next
            End If
            Return ret
        End Function

    End Class
End Namespace
