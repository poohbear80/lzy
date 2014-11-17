Namespace CQRS.Command
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=True)> Public Class ExpectsEvent
        Inherits Attribute

        Private ReadOnly _Event As Type

        Public Sub New([event] As Type)
            _Event = [event]
        End Sub

        Public ReadOnly Property [Event] As Type
            Get
                Return _Event
            End Get
        End Property
    End Class
End NameSpace
