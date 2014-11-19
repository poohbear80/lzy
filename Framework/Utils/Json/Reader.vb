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

            Dim nextChar As Integer = input.Read()
            While nextChar > 0

                If builder.AcceptChar(ChrW(nextChar)) Then

                End If

                nextChar = input.Read
                If builder.Complete Then
                    Exit While
                End If
            End While


            If builder.Complete Then
                Return builder.Result
            End If

            Throw New MissingTokenException()

        End Function

        Public MustInherit Class Builder(Of T)
            Public Result As T
            Public MustOverride Function AcceptChar(c As Char) As Boolean
            Public MustOverride ReadOnly Property Complete As Boolean

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
            Inherits Builder(Of T)
            Private state As Integer
            Private _complete As Boolean = False

            Private blanks As New WhiteSpaceTokenAcceptor


            Public Sub New()
                Result = New T
            End Sub

            Public Overrides Function AcceptChar(c As Char) As Boolean
                Select Case state
                    Case 0
                        If blanks.AcceptsToken(c) Then
                            Return True
                        Else
                            If c = "{" Then
                                state = 1
                                Return True
                            End If
                            Throw New MissingTokenException("{")
                        End If
                    Case 1

                    Case 2
                        If blanks.AcceptsToken(c) Then
                            Return True
                        End If
                        If c = "}" Then
                            _complete = True
                            Return True
                        End If
                        Throw New MissingTokenException("{")
                End Select
            End Function

            Public Overrides ReadOnly Property Complete As Boolean
                Get
                    Return _complete
                End Get
            End Property
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

    Public Class MissingTokenException
        Inherits Exception

        Private ReadOnly _S As String

        Public Sub New(ByVal s As String)
            MyBase.New(s)
            _S = s
        End Sub

        Public ReadOnly Property Token As String
            Get
                Return _S
            End Get
        End Property
    End Class
End Namespace