Imports NUnit.Framework

<TestFixture> Public Class Helpers

    <SetUp> Public Sub Setup()

    End Sub

    <TearDown> Public Sub TearDown()

    End Sub

    <Test> Public Sub CloneIOrDataObject()
        Dim org As New ToClone

        org.Id = "Audun"
        org.Name = "Petter"

        Dim clone = Utils.Helpers.CloneObject(Of ToClone)(org)

        Assert.AreEqual(org.Id, clone.Id)
        Assert.AreEqual(org.Name, clone.Name)
        Assert.AreEqual(2, clone.ChangeLog.Count)

    End Sub


    Public Class ToClone
        Inherits LazyBaseClass

        Public Overrides ReadOnly Property Fields As String()
            Get
                Return {"Id", "Name"}
            End Get
        End Property

        Property Id As String
        Property Name As String

        Public Overrides Function GetValue(name As String, ByRef value As Object) As Boolean

            Select Case name.ToLower
                Case "id"
                    value = Id
                Case "name"
                    value = Me.Name
                Case Else
                    Return False
            End Select

            Return True
        End Function

        Public Overrides Function SetValue(name As String, value As Object) As Boolean

            Select Case name.ToLower
                Case "id"
                    Id = CType(value, String)
                Case "name"
                    Me.Name = CType(value, String)
                Case Else
                    Return False
            End Select
            Return True
        End Function
    End Class

End Class
