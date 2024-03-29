﻿Imports System.Data.SqlClient
Imports System.Reflection


    Public Class DataFiller

        Private ReadOnly _fields As New List(Of FieldInfoDecorator)

        Private Class FieldInfoDecorator
            Private ReadOnly _col As Integer
            Private ReadOnly _fieldInfo As FieldInfo

            Public Sub New(reader As Object, col As Integer, fieldInfo As FieldInfo)
                _col = col
                _fieldInfo = fieldInfo
                If GetType(System.IO.Stream).IsAssignableFrom(_fieldInfo.FieldType) Then
                    If TypeOf (reader) Is SqlDataReader Then
                        _getValue = AddressOf GetStream
                    Else
                        Throw New NotSupportedException("Streams is only supported for SQLDataReader.")
                    End If
                Else
                    _getValue = AddressOf GetPrimitiveType
                End If
            End Sub

            Private ReadOnly _getValue As GetValueDelegate

            Private Function GetPrimitiveType(o As IDataReader) As Object
                Return o.GetValue(_col)
            End Function

            Private Function GetStream(o As IDataReader) As System.IO.Stream
                Return CType(o, SqlDataReader).GetStream(_col)
            End Function

            Private Delegate Function GetValueDelegate(o As IDataReader) As Object

            Public Sub SetValueToObject(reader As IDataReader, o As Object)
                Dim tempValue = _getValue(reader)

                If TypeOf (tempValue) Is DBNull Then
                    _fieldInfo.SetValue(o, Nothing)
                Else
                    _fieldInfo.SetValue(o, tempValue)
                End If
            End Sub

        End Class

        Public Sub New(ByVal dataReader As IDataReader, ByVal t As Type)
            'Her kunne vi laget noe lureri for å gjøre dette med emitting av il, men det lar vi være enn så lenge. 

            Dim n As String
            For x = 0 To dataReader.FieldCount - 1
                n = dataReader.GetName(x)
                Dim fieldInfo As FieldInfo = Nothing
                Dim currType As Type
                currType = t

                While fieldInfo Is Nothing AndAlso currType IsNot Nothing
                    fieldInfo = currType.GetField("_" & n, BindingFlags.IgnoreCase Or BindingFlags.NonPublic Or BindingFlags.Instance)
                    If fieldInfo Is Nothing Then
                        fieldInfo = currType.GetField(n, BindingFlags.IgnoreCase Or BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.FlattenHierarchy)
                    End If
                    If fieldInfo IsNot Nothing Then
                        _fields.Add(New FieldInfoDecorator(dataReader, x, fieldInfo))
                    End If
                    currType = currType.BaseType
                End While
            Next
        End Sub

        Public Sub FillObject(reader As IDataReader, data As Object)
            For Each fieldInfo In _fields
                fieldInfo.SetValueToObject(reader, data)
            Next
        End Sub
    End Class
