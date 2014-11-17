Imports NUnit.Framework

<testfixture> Public Class ResponseThread

    <SetUp> Public Sub SetUp()
        Utils.ResponseThread.Current.Clear()
    End Sub
    
    <Test> Public Sub ValueIsPreserverdInThreadStore()
        LazyFramework.Utils.ResponseThread.SetThreadValue("Test", 10)

        Assert.AreEqual(10, Utils.ResponseThread.GetThreadValue(Of Integer)("Test"))

    End Sub
    <Test> Public Sub ThreadIsCleardWhenClearIsCalled()

        Utils.ResponseThread.SetThreadValue("Test", 10)
        Utils.ResponseThread.ClearThreadValues()
        Assert.Throws(Of KeyNotFoundException)(Sub() Utils.ResponseThread.GetThreadValue(Of Integer)("Test"))
        
    End Sub

    <Test> Public Sub AddmessageToCurrent()
        Utils.ResponseThread.Current.Add(New ThreadMessage(10))
        Assert.AreEqual(1, Utils.ResponseThread.Current.AllMessages.Count)
    End Sub

    <Test> Public Sub AddMessageToErrorCollection()
        Utils.ResponseThread.Current.Add(New ThreadMessage(ThreadMessageSeverityEnum.Error, "Test", "Test"))
        Assert.AreEqual(1, Utils.ResponseThread.Current.AllMessages.Count)
        Assert.IsTrue(Utils.ResponseThread.Current.HasErrors)
    End Sub
End Class
