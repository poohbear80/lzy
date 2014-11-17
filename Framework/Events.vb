
''' <summary>
''' Fired when a object is filled with data
''' </summary>
''' <remarks></remarks>
Public Class FillCompleteEventargs
    Inherits EventArgs

    Private ReadOnly _Command As CommandInfo

    Public Sub New(command As CommandInfo)
        _Command = command
        _SpName = command.CommandText
    End Sub

    Private _SpName As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CmdName() As String
        Get
            Return _SpName
        End Get
        Set(ByVal value As String)
            _SpName = value
        End Set
    End Property

    Public ReadOnly Property Command As CommandInfo
        Get
            Return _Command
        End Get
    End Property
End Class
