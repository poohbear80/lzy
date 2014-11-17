Imports NUnit.Framework

<TestFixture> Public Class MultiThread
    Private _Session As LazyFramework.ClassFactory.SessionInstance

    Public Interface IA
        Function B() As String
    End Interface

    Public Class User
        Public Function GetA() As String
            Return LazyFramework.ClassFactory.GetTypeInstance(Of IA, CA).B()
        End Function
    End Class

    Public Class CA
        Implements IA
        Public Function B() As String Implements IA.B
            Return "CA"
        End Function
    End Class


    <SetUp> Public Sub SetUp()
        Runtime.Context.Current = New Runtime.WinThread
        _Session = New LazyFramework.ClassFactory.SessionInstance
    End Sub

    <TearDown> Public Sub TearDown()
        _Session.Complete()
    End Sub


    <Test>
    Public Sub Test2()
        Dim t As New User
        Assert.AreEqual("CA", t.GetA)
        
    End Sub


    <Test>
    Public Sub Test1()
        Dim instance As IA = Substitute.For(Of IA)()
        instance.B.Returns("MOCK")
        LazyFramework.ClassFactory.SetTypeInstanceForSession(Of IA)(instance)
        Dim t As New User
        Assert.AreEqual("MOCK", t.GetA)

    End Sub


End Class
