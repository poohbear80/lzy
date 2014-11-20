Imports System.IO

Namespace Utils.Json
    Public Class Reader

        Public Shared Function StringToObject(Of T As New)(input As String) As T
            Dim mem As New MemoryStream
            Dim write As New StreamWriter(mem, Text.Encoding.UTF8)
            write.Write(input)
            write.Flush()
            write.BaseStream.Position = 0
            Return StringToObject(Of T)(New StreamReader(mem, System.Text.Encoding.UTF8))
        End Function

        'Here we can add another function that accepts stream as parameter


        Public Shared Function StringToObject(Of T As New)(input As StreamReader) As T
            Dim builder As New ObjectBuilder(Of T)

            Dim parser As New JsonParser(input)
            parser.Parse(builder)

            Return builder.Result

        End Function


        Private Class JsonParser
            Implements ITextParser

            Private ReadOnly _StreamReader As StreamReader

            Public Sub New(ByVal streamReader As StreamReader)
                _StreamReader = streamReader
            End Sub


            Public Sub Parse(builder As Builder) Implements ITextParser.Parse
                Dim currByte As Char
                While Not _StreamReader.EndOfStream
                    currByte = ChrW(_StreamReader.Read())
                    For Each ta In builder.TokenAcceptors
                        If ta.AcceptsToken(currByte) Then
                            For Each b In builder.SubBuilders

                            Next
                        End If
                    Next
                End While
            End Sub
        End Class

        Private Interface ITextParser

            Sub Parse(builder As Builder)

        End Interface

        Public MustInherit Class Builder
            Public TokenAcceptors As New List(Of ITokenAcceptor)
            Public MustOverride Iterator Function SubBuilders() As IEnumerable(Of Builder)
            Public Result As Object
            Protected TextRead As String = ""
            Public Sub AddText(c As Char)
                TextRead += c
            End Sub
        End Class

        Friend Class ValueBuilder
            Inherits Builder

            Public Sub New()

            End Sub

            Public Overrides Function SubBuilders() As IEnumerable(Of Builder)

            End Function
        End Class

        Friend Class AttributeBuilder
            Inherits Builder
            Public Sub New()
                TokenAcceptors.Add(New AttributeQulifierTokenAcceptor)
            End Sub

            Public Overrides Iterator Function SubBuilders() As IEnumerable(Of Builder)

            End Function
        End Class

        Friend Class ObjectBuilder(Of T As New)
            Inherits Builder
            Public Sub New()
                TokenAcceptors.Add(New ObjectTokenAcceptor With {.Builder = Me})
                TokenAcceptors.Add(New WhiteSpaceTokenAcceptor With {.Builder = Me})

                Result = New T
            End Sub
            Public Shadows Result As T

            Public Overrides Iterator Function SubBuilders() As IEnumerable(Of Builder)
                Yield New AttributeBuilder
            End Function
        End Class


        Public Interface ITokenAcceptor
            Function AcceptsToken(t As Char) As Boolean
            Property Builder As Builder
        End Interface

        Public MustInherit Class TokenAcceptor
            Implements ITokenAcceptor
            Public MustOverride Function AcceptsToken(t As Char) As Boolean Implements ITokenAcceptor.AcceptsToken
            Public Property Builder As Builder Implements ITokenAcceptor.Builder
        End Class


        Public Class ObjectTokenAcceptor
            Inherits TokenAcceptor
            Private Start As Boolean = True

            Public Overrides Function AcceptsToken(t As Char) As Boolean
                If t = "{" And Start Then
                    Start = False
                    Return True
                End If
                If t = "}" And Not Start Then
                    Return True
                End If
            End Function
        End Class
        Public Class AttributeQulifierTokenAcceptor
            Inherits TokenAcceptor
            Public Overrides Function AcceptsToken(t As Char) As Boolean
                Return t = """"
            End Function
        End Class
        Public Class TextQulifierTokenAcceptor
            Inherits TokenAcceptor

            Public Overrides Function AcceptsToken(t As Char) As Boolean
                Return t = """"
            End Function
        End Class
        Public Class AttributeNameTokenAcceptor
            Inherits TokenAcceptor


            ''' <summary>
            ''' [a-z][A-Z]
            ''' </summary>
            ''' <param name="t"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Overrides Function AcceptsToken(t As Char) As Boolean
                Dim w As Integer = AscW(t)
                If w > 64 AndAlso w < 91 Then
                    Return True
                End If
                If w > 96 AndAlso w < 124 Then
                    Return True
                End If
            End Function
        End Class
        Public Class WhiteSpaceTokenAcceptor
            Inherits TokenAcceptor

            Public Overrides Function AcceptsToken(t As Char) As Boolean
                If AscW(t) <= 32 Then
                    Return True
                End If
            End Function
        End Class


    End Class



End Namespace