Namespace Validation
    Public MustInherit Class ValidateActionBase(Of TAction As IAmAnAction)
        Implements IValidateAction

        Friend Sub InternalValidate(action As IAmAnAction) Implements IValidateAction.InternalValidate
            '    ValidatAction(CType(action, TAction))
            Dim val As New ValidationException

            Dim [getType] As Type = Me.GetType

            For Each p In [getType].GetMethods(system.Reflection.BindingFlags.DeclaredOnly Or system.Reflection.BindingFlags.Public Or system.Reflection.BindingFlags.Instance)
                Try
                    If p.GetParameters.Count = 1 AndAlso p.GetParameters(0).ParameterType.IsAssignableFrom(action.GetType) Then
                        p.Invoke(Me, {action})
                    End If
                Catch ex As Exception
                    val.ExceptionList.Add(p.Name, ex.InnerException)
                End Try

            Next

            If val.ExceptionList.Count > 0 Then
                Throw val
            End If

        End Sub

        '        Public MustOverride Sub ValidatAction(action As TAction)

    End Class
End NameSpace
