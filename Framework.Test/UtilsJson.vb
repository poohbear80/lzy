
Imports LazyFramework.Utils.Json
Imports NUnit.Framework

<TestFixture> Public Class UtilsJson

    <SetUp> Public Sub Setup()

    End Sub

    <Test> Public Sub IntegerIsWritten()

        Assert.AreEqual("{""ToTest"":1}", Writer.ObjectToString(New With {.ToTest = 1}))

    End Sub


    <Test(Description:="Testing encoding of strings"),
    TestCase("StandardText"),
    TestCase("ÆØÅ{"),
    TestCase("Some€"),
    TestCase("with " & Chr(&H22) & " "),
    TestCase("\\\///" & Chr(9)),
    TestCase("--" & Chr(&HC), Description:="Formfeed"),
    TestCase("--" & vbCrLf),
    TestCase(Nothing)
    > Public Sub TextIsWritten(input As String)

        Dim value = New With {.ToTest = input, .test = input}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(value), Writer.ObjectToString(value))

    End Sub


    <Test,
    TestCase(1),
    TestCase(123467854684),
    TestCase(-1)> Public Sub IntegersIsWritten(input As Long)
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))
    End Sub

    <Test,
    TestCase(1),
    TestCase(1234),
    TestCase(Nothing)> Public Sub NullableIntegers(input As Integer?)

        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))

    End Sub

    <Test,
    TestCase(1.123),
    TestCase(684.7853),
    TestCase(-1.45644)> Public Sub DoubleIsWritten(input As Double)
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))
    End Sub

    <Test,
    TestCase(1.123),
    TestCase(684.7853),
    TestCase(-1.45644)> Public Sub DoubleIsWritten(input As Decimal)
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))
    End Sub

    <Test> Public Sub MultilevelObjects()
        Dim o = New With {.ToTest = "", .Child = New With {.Name = "Test", .Year = 12}}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(o), Writer.ObjectToString(o))
    End Sub


    <Test> Public Sub MultilevelObjectsWithStructure()
        Dim o = New With {.ToTest = "", .Child = New Test With {.Name = "jklj", .Year = 12}}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(o), Writer.ObjectToString(o))
    End Sub

    <Test> Public Sub IntegerArray()
        Dim toWrite As Integer() = {1, 2, 3}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))

    End Sub

    <Test> Public Sub StringArray()
        Dim toWrite As String() = {"abc", "æøå", ""}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))

    End Sub
    <Test> Public Sub ObjectArray()
        Dim toWrite As Object() = {New With {.Name = "sd"}, New With {.Age = 42}}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))

    End Sub
End Class


<TestFixture> Public Class TestParser
    <Test> Public Sub ParseSimpleObject()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)("{""Navn"":""Petter""}")
        Assert.AreEqual("Petter", p.Navn)
        'Assert.AreEqual(43, p.Alder)
    End Sub

    <Test> Public Sub ParseTextWithEscapeObject()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)("{""Navn"":""Petter\nGjermund\\     ""}")
        Assert.AreEqual("Petter" & vbCrLf & "Gjermund\     ", p.Navn)
        'Assert.AreEqual(43, p.Alder)
    End Sub
End Class

Public Structure Test
    Public Name As String
    Public Year As Integer
End Structure


Public Class Person
    Public Navn As String
    Public Alder As Integer
    Public Barn As List(Of Person)
End Class

