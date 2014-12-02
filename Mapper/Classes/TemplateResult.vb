Public Class TemplateResult
    Private ReadOnly _stream As System.IO.TextWriter
    Private ReadOnly _lang As String

    Public Sub New(stream As System.IO.TextWriter, lang As String)
        _stream = stream
        _lang = lang
    End Sub

    Private _tabs As Integer = 0
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PushTab()
        _tabs += 1
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PopTab()
        _tabs -= 1
        If _tabs < 0 Then Throw New ArgumentOutOfRangeException("To many pops")
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <remarks></remarks>
    Public Sub WriteComment(s As String)
        If _lang.ToLower = "vb" Then
            WriteLine("'" & s)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <remarks></remarks>
    Public Sub Comment(s As String)
        WriteComment(s)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="type"></param>
    ''' <param name="defaultValue"></param>
    ''' <remarks></remarks>
    Public Sub WriteProp(name As String, type As String, defaultValue As String)

        Dim pName As String = "_" & name(0).ToString.ToLower & Mid(name, 2)

        WriteFormatLine("Private {0} as {1} {2}  ", pName, type, If(Not String.IsNullOrEmpty(defaultValue), "=" & defaultValue, ""))
        WriteFormatLine("Public Property {0} as {1}", name, type)
        PushTab()
        WriteLine("Get")
        PushTab()
        WriteFormatLine("Return {0}", pName)
        PopTab()
        WriteLine("End Get")
        PopTab()
        WriteFormatLine("Set(value as {0})", type)
        PushTab()
        WriteFormatLine("{0} = value", pName)
        PopTab()
        WriteLine("End Get")
        PopTab()
        WriteFormatLine("End Property")
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="type"></param>
    ''' <param name="defaultValue"></param>
    ''' <param name="modifier"></param>
    ''' <remarks></remarks>
    Public Sub WriteReadOnlyProp(name As String, type As String, defaultValue As String, modifier As String)
        Dim pName As String = "_" & name

        WriteFormatLine("Private Readonly {0} as {1} {2}  ", pName, type, If(Not String.IsNullOrEmpty(defaultValue), "=" & defaultValue, ""))
        WriteFormatLine("Public {2} Readonly Property {0} as {1}", name, type, modifier)
        PushTab()
        WriteLine("Get")
        PushTab()
        WriteFormatLine("Return {0}", pName)
        PopTab()
        WriteLine("End Get")
        PopTab()
        WriteLine("End Property")
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <remarks></remarks>
    Public Sub StartBlock(s As String)
        WriteLine(s)
        PushTab()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub StartBlock(s As String, ParamArray value As String())
        WriteFormatLine(s, value)
        PushTab()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <remarks></remarks>
    Public Sub EndBlock(s As String)
        PopTab()
        WriteLine(s)
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="count"></param>
    ''' <remarks></remarks>
    Public Sub WriteTab(count As Integer)
        For x = 0 To count - 1
            Write(vbTab)
        Next
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub WriteTabs()
        WriteTab(_tabs)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub WriteTab()
        WriteTab(1)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub WriteFormatLine(s As String, ParamArray value As String())
        WriteTab(_tabs)
        _stream.WriteLine(String.Format(s, value))
        _stream.Flush()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub WriteFormat(s As String, ParamArray value As String())
        _stream.Write(String.Format(s, value))
        _stream.Flush()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <remarks></remarks>
    Public Sub WriteLine(s As String)
        WriteTab(_tabs)
        _stream.WriteLine(s)
        _stream.Flush()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub WriteLine(s As String, ParamArray value As String())
        WriteTab(_tabs)
        _stream.WriteLine(String.Format(s, value))
        _stream.Flush()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub Write(s As String, ParamArray value As String())
        _stream.Write(String.Format(s, value))
        _stream.Flush()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <remarks></remarks>
    Public Sub Write(s As String)
        _stream.Write(s)
        _stream.Flush()
    End Sub
End Class
