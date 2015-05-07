Imports LazyFramework.Runtime
Imports NUnit.Framework

<TestFixture> Public Class SpawnThread


    <Test> Public Sub ContextIsPreservedInMultipleSpawns()
        Dim winThread = New Runtime.WinThread

        Dim values = {1}
        Dim values2 = {6}

        LazyFramework.Runtime.Context.Current = winThread
        Context.Current.Storage("Test") = "Tast"

        Dim store = winThread.Storage

        Assert.IsTrue(Context.Current.Storage.ContainsKey("Test"))
        values.AsParallel.ForAll(Sub(i)
                                     Using New Runtime.SpawnThreadContext(Nothing, store, True)
                                         Assert.IsInstanceOf(Of SpawnThreadContext)(Context.Current)
                                         Assert.IsInstanceOf(Of ProxyStorage(Of String, Object))(Context.Current.Storage)
                                         Assert.IsTrue(Context.Current.Storage.ContainsKey("Test"))
                                         Assert.AreEqual("Tast", Context.Current.Storage("Test"))


                                         values2.AsParallel.ForAll(Sub(i2)
                                                                       Using New Runtime.SpawnThreadContext(Nothing, Context.Current.Storage, True)
                                                                           Context.Current.Storage("Test") = "Tast2"
                                                                           Assert.IsInstanceOf(Of SpawnThreadContext)(Context.Current)
                                                                           Assert.IsInstanceOf(Of ProxyStorage(Of String, Object))(Context.Current.Storage)
                                                                           Assert.AreEqual("Tast", Context.Current.Storage("Test2"))
                                                                       End Using
                                                                   End Sub)


                                     End Using
                                 End Sub)


        



    End Sub


End Class