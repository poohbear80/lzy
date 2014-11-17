Imports System.IO

Public Class Serializers
    Public Shared Function SerializeObjectToXmlFile(sourceObject As Object, filename As String) As Boolean

        Try
            With New System.Xml.Serialization.XmlSerializer(sourceObject.GetType())
                Dim xmlFile = IO.File.Open(filename, FileMode.Create, FileAccess.ReadWrite)
                .Serialize(xmlFile, sourceObject)
                xmlFile.Dispose()
                Return True
            End With
        Catch ex As Exception
            Throw New Exception("Something went wrong with the serialization process", ex.InnerException)
        End Try
    End Function

    Public Shared Function DeserilalizeObjectFromXmlFile(Of TT)(fileName As String) As TT
        Try
            With New System.Xml.Serialization.XmlSerializer(GetType(TT))
                Dim xmlFile = IO.File.OpenRead(fileName)
                Dim template As TT
                template = DirectCast(.Deserialize(xmlFile), TT)
                xmlFile.Dispose()
                Return template
            End With
        Catch ex As Exception
            Throw New Exception("Something went wrong with the Deserialization process", ex.InnerException)
        End Try

    End Function
End Class
