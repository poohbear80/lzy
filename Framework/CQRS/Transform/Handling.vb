Imports LazyFramework.CQRS.Dto

Namespace CQRS.Transform
    Public Class Handling

        Public Shared Function TransformResult(ByVal action As IAmAnAction, ByVal result As Object) As Object
            Dim transformerFactory As ITransformerFactory = EntityTransformerProvider.GetFactory(action)

            'Hmmmm skal vi ha logikk her som sjekker om det er noe factory, og hvis det ikke er det bare returnere det den fikk inn. 
            'Egentlig er det jo bare commands som trenger dette. Queries bør jo gjøre dette selv.. Kanskje. 

            If TypeOf result Is IList Then
                Dim ret As New List(Of Object)
                Dim res As Object
                For Each e In CType(result, IList)
                    res = TransformAndAddAction(action, transformerFactory, e)
                    If res IsNot Nothing Then
                        ret.Add(res)
                    End If
                Next
                Return ret
            Else
                Return TransformAndAddAction(action, transformerFactory, result)
            End If
        End Function

        Private Shared Function TransformAndAddAction(ByVal action As IAmAnAction, ByVal transformerFactory As ITransformerFactory, e As Object) As Object
            Dim securityContext As Object

            If TypeOf (e) Is IProvideSecurityContext Then
                securityContext = DirectCast(e, IProvideSecurityContext).Context
            Else
                securityContext = e
            End If

            If Not ActionSecurity.Current.EntityIsAvailableForUser(action.User, action, securityContext) Then Return Nothing
            
            Dim transformer = transformerFactory.GetTransformer(action, e)
            If transformer Is Nothing Then Return Nothing

            Dim transformEntity As Object = transformer.TransformEntity(e)
            If transformEntity Is Nothing Then Return Nothing

            If TypeOf (transformEntity) Is ISupportActionList Then
                CType(transformEntity, ISupportActionList).Actions.AddRange(ActionSecurity.Current.GetActionList(action.User, action, e))
            End If
            If TypeOf transformEntity Is ActionContext.ActionContext Then

            End If

            Return transformEntity
        End Function
    End Class
End Namespace
