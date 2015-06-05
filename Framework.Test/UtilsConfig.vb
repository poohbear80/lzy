Imports NUnit.Framework

<TestFixture> Public Class UtilsConfig

    <Test> Public Sub LoadConfigFromFile()

        Dim conf = Utils.Config.Reader.Load(Of SystemConfig)

        Assert.AreEqual("ABC", conf.StringValue)

    End Sub

    Public Class SystemConfig
        Public StringValue As String
        Public IntValue As Integer
        Public DoubleValue As Double
    End Class

End Class
