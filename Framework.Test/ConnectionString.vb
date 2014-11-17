Imports NUnit.Framework

<TestFixture> Public Class ConnectionString
    Private _Session As LazyFramework.ClassFactory.SessionInstance

    <SetUp> Public Sub SetUp()
        Runtime.Context.Current = New Runtime.WinThread
        
        _Session = New LazyFramework.ClassFactory.SessionInstance
        Dim serverStore As New ServersCollection

        Dim lazyFrameworkConfiguration As ILazyFrameworkConfiguration = Substitute.For(Of ILazyFrameworkConfiguration)()
        LazyFramework.ClassFactory.SetTypeInstanceForSession(Of LazyFramework.ILazyFrameworkConfiguration)(lazyFrameworkConfiguration)

        lazyFrameworkConfiguration.Servers.Returns(serverStore)

        lazyFrameworkConfiguration.Servers.Add(New ServerConfigElement With {.Name = "server", .ConnectionString = "server=81.175.37.120;Database=[DBName];User ID=sa;Password=InfoTjenester88;"})
        lazyFrameworkConfiguration.Servers.Add(New ServerConfigElement With {.Name = "newServer", .ConnectionString = "server=81.175.37.120;Database=[DBName];User ID=[user];Password=[password];"})
        
    End Sub
    
    <TearDown> Public Sub TearDown()
        _Session.Complete()
        _Session.Dispose()
        _Session = Nothing
    End Sub

    <Test> Public Sub DbAtServerWorksAsBefore()
        Assert.AreEqual("server=81.175.37.120;Database=db;User ID=sa;Password=InfoTjenester88;", LazyFramework.SQLServer.GetConnectionString("db@server"))

    End Sub

    <Test> Public Sub UserNameAndPasswordIsHandledWhenNotAskedForInConnectionString()
        Assert.AreEqual("server=81.175.37.120;Database=db;User ID=sa;Password=InfoTjenester88;", LazyFramework.SQLServer.GetConnectionString("db@server|petter@password"))
    End Sub

    <Test> Public Sub UserNameAndPasswordIsHandled()
        Assert.AreEqual("server=81.175.37.120;Database=db;User ID=petter;Password=password;", LazyFramework.SQLServer.GetConnectionString("db@newServer|petter@password"))
    End Sub


End Class
