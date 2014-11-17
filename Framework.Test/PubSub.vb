Imports LazyFramework.CQRS.EventHandling
Imports NUnit.Framework

<TestFixture> Public Class PubSub

    <Test> Public Sub AllHandlersIsFound()
        Assert.Greater(Eventhub.AllHandlers.Count, 0)
    End Sub


    <Test> Public Sub PublishedEventIsRecived()
        
        EventPublisher.PublishEvent()

        Assert.IsNotNull(ClassThatHandlesEvent.Hmm)
        Assert.AreEqual(ClassThatHandlesEvent.Hmm.Name, "Petter")
        Assert.AreEqual(2, ClassThatHandlesEvent.Counter)

    End Sub

    <Test> Public Sub FindSourcesForEvent()
        
        Assert.Greater(EventHub.Publishers(Of PersonGotNewName).Count, 0)
        Assert.AreEqual(EventHub.Publishers(Of PersonGotNewName)(0).Name, "PublishEvent")

    End Sub

End Class

Public Class EventPublisher
    Implements IPublishEvent

    <PublishesEventOfType(GetType(PersonGotNewName))> Public Shared Sub PublishEvent()

        EventHub.Publish(New PersonGotNewName With {.Name = "Petter"})

    End Sub
End Class


Public Class PersonGotNewName
    Inherits EventBase

    Public Property Name() As String

End Class

Public Class AnotherHandler
    Implements IHandleEvent

    Public Shared Sub jsdklfj(p As PersonGotNewName)

    End Sub

End Class


Public Class ClassThatHandlesEvent
    Implements IHandleEvent

    Public Shared Hmm As PersonGotNewName
    Public Shared Counter As Integer = 0

    Public Shared Sub Handlejhgkjhgjhghjg(param As PersonGotNewName)
        Hmm = param
        Counter += 1
    End Sub

    Public Shared Sub HandlesToo(param As PersonGotNewName)
        Hmm = param
        Counter += 1
    End Sub


End Class
