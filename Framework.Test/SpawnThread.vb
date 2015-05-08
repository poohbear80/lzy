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
                                                                           Assert.AreEqual("Tast2", Context.Current.Storage("Test"))
                                                                       End Using
                                                                   End Sub)

                                         Assert.AreEqual("Tast", Context.Current.Storage("Test"))

                                     End Using
                                 End Sub)


        



    End Sub


    <Test> Public Sub UsingClassFactoryInMultipleSpawns()
        Dim winThread = New Runtime.WinThread

        Dim values = {1}
        Dim values2 = {6}

        LazyFramework.Runtime.Context.Current = winThread
        Context.Current.Storage("Test") = "Tast"
        Dim store = winThread.Storage

        LazyFramework.ClassFactory.SetTypeInstance(Of ISomeThing)(New SomeThing)
        
        Assert.IsInstanceOf(Of SomeThing)(LazyFramework.ClassFactory.GetTypeInstance(Of ISomeThing))

        values.AsParallel.ForAll(Sub(i)
                                     Using New Runtime.SpawnThreadContext(Nothing, store, True)
                                         Assert.IsInstanceOf(Of ISomeThing)(LazyFramework.ClassFactory.GetTypeInstance(Of ISomeThing))
                                         values2.AsParallel.ForAll(Sub(i2)
                                                                       Using New Runtime.SpawnThreadContext(Nothing, Context.Current.Storage, True)
                                                                           Assert.IsInstanceOf(Of SomeThing)(LazyFramework.ClassFactory.GetTypeInstance(Of ISomeThing))
                                                                       End Using
                                                                   End Sub)


                                     End Using
                                 End Sub)

    End Sub

    Public Interface ISomeThing

    End Interface

    Public Class SomeThing
        Implements ISomeThing

    End Class

End Class