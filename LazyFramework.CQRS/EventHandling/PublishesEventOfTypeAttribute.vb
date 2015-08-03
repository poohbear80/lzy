Namespace EventHandling
    <AttributeUsage(AttributeTargets.Method)> Public Class PublishesEventOfTypeAttribute
        Inherits Attribute

        Private ReadOnly _Types As Type()
        Public Sub New(ParamArray types As Type())
            _Types = types
        End Sub

        Public ReadOnly Property Types As Type()
            Get
                Return _Types
            End Get
        End Property
    End Class
End Namespace
