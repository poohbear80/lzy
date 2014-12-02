Imports NUnit.Framework
Imports System.Runtime.Serialization

<testfixture> Public Class MerkeligeGreier

    <Setup> Public Sub Setup()

    End Sub

    <TearDown> Public Sub TearDown()

    End Sub

    <Test> Public Sub Test1()

        Dim inf As New SerializationInfo(GetType(ABC), New System.Runtime.Serialization.FormatterConverter)
        inf.AddValue("I", 2)

        Dim a As ABC
        a = CType(Activator.CreateInstance(GetType(ABC), inf, New StreamingContext), ABC)

        Assert.AreEqual(2, a.I)



    End Sub


    <Test> Public Sub Deser()

        Dim inf As New SerializationInfo(GetType(ABC), New System.Runtime.Serialization.FormatterConverter)
        Dim a As New ABC(2)
        
        a.GetObjectData(inf, New StreamingContext)

        Assert.AreEqual(2, inf.GetInt32("I"))

    End Sub


End Class


Public Class ABC
    Implements ISerializable


    Private _I As Integer

    Public Sub New()
    End Sub

    Public Sub New(a As Integer)
        _I = a
    End Sub

    Public ReadOnly Property I As Integer
        Get
            Return _I
        End Get
    End Property

    Public Sub New(info As SerializationInfo, context As StreamingContext)
        _I = info.GetInt32("I")
    End Sub

    Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
        info.AddValue("I", _I)
    End Sub
End Class
