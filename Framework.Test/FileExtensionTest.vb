Imports Newtonsoft.Json
Imports NUnit.Framework
Imports LazyFramework.Utils

<TestFixture> Public Class FileExtensionTest

    <SetUp> Public Sub Setup()

    End Sub


    <Test> Public Sub SettingDataDoesNotThrowExecption()
        Dim test As New MyInfo

        FileExtension.SetData("TekstFile.txt", test, New JsonSerializer)

    End Sub

    <Test> Public Sub GetData()
        Dim test As New MyInfo
        test.A = "Some string"
        test.B = 10
        test.C = 21.34

        FileExtension.SetData("TekstFile.txt", test, New JsonSerializer)

        Dim retrive = FileExtension.LoadData(Of MyInfo)("TekstFile.txt", New JsonSerializer)

        Assert.AreEqual(test.A, retrive.A)
        Assert.AreEqual(test.B, retrive.B)
        Assert.AreEqual(test.C, retrive.C)

    End Sub


End Class


Public Class JsonSerializer
    Implements FileExtension.IDoSerializiation

    Public Function DeSerialize(Of T)(content As String) As T Implements FileExtension.IDoSerializiation.DeSerialize
        Return JsonConvert.DeserializeObject(Of T)(content)
    End Function

    Public Function Serialize(data As Object) As String Implements FileExtension.IDoSerializiation.Serialize
        Return JsonConvert.SerializeObject(data)
    End Function

End Class

Public Class MyInfo
    Public A As String
    Public B As Integer
    Public C As Double
End Class
