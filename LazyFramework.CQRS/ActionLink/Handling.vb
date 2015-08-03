Imports System.Reflection

Namespace ActionLink
    Public Class Handling

        Private Shared ReadOnly PadLock As New Object
        Private Shared _handlers As Dictionary(Of Type, List(Of MethodInfo))


        Private Shared _actionLinkList As Dictionary(Of String, Type)
        Public Shared ReadOnly Property ActionLinkList As Dictionary(Of String, Type)
            Get
                If _actionLinkList Is Nothing Then
                    SyncLock PadLock
                        If _actionLinkList Is Nothing Then
                            Dim temp As New Dictionary(Of String, Type)
                            For Each t In TypeValidation.FindAllClassesOfTypeInApplication(GetType(ActionLinkBase))
                                If t.IsAbstract Then Continue For 'Do not map abstract queries. 

                                Dim c As ActionLinkBase = CType(Activator.CreateInstance(t), ActionLinkBase)
                                temp.Add(c.ActionName, t)
                            Next
                            _actionLinkList = temp
                        End If
                    End SyncLock
                End If

                Return _actionLinkList
            End Get
        End Property
    End Class
End Namespace
