Imports LazyFramework.CQRS.EventHandling

''' <summary>
''' A base class for aggregated functionallity. 
''' </summary>
Public MustInherit Class LazyBaseAggregate
    
    ''' <summary>
    ''' Gets or sets the name of the db.
    ''' </summary>
    ''' <value>The name of the db.</value>
    <Obsolete> Public MustOverride Property DbName() As String


    ''' <summary>
    ''' Used to set a default value for an entity in this aggregate class.
    ''' The value is set if and only if the PropName is not found in the ChangeLog of the entity.
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <param name="propName"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    <Obsolete> Protected Shared Sub SetDefaultValue(ByVal instance As LazyBaseClass, ByVal propName As String, ByVal value As Object)
        If instance Is Nothing Then Return

        'Is the value named 'PropName' changed by the system. If it's set we do not set the default value.
        If instance.ChangeLog IsNot Nothing Then
            For Each cv As ChangedValue In instance.ChangeLog
                If cv.FieldName = propName Then Return
            Next
        End If

        instance.SetValue(propName, value)
    End Sub
End Class


