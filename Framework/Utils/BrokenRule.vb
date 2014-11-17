
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports LazyFramework.Utils

Public Class ThreadMessage
    Public Sub New(ByVal s As ThreadMessageSeverityEnum, ByVal fName As String, ByVal description As String)
        _FieldName = fName
        _Severity = s
        Messages.Add(Description)
    End Sub

    Public Sub New(ByVal rulNum As Integer)
        RuleNumber = RulNum
        'We must implement some kind of 'rulenumber provider'
    End Sub

    Private _FieldName As String
    Private _Messages As New List(Of String)
    Private _Severity As ThreadMessageSeverityEnum
    Private _RuleNumber As Integer

    Public Property RuleNumber() As Integer
        Get
            Return _RuleNumber
        End Get
        Set(ByVal value As Integer)
            _RuleNumber = value
        End Set
    End Property

    Public ReadOnly Property Messages() As List(Of String)
        Get
            Return _Messages
        End Get
    End Property

    Public Property FieldName() As String
        Get
            Return _FieldName
        End Get
        Set(ByVal value As String)
            _FieldName = value
        End Set
    End Property

    Public Property Severity() As ThreadMessageSeverityEnum
        Get
            Return _Severity
        End Get
        Set(ByVal value As ThreadMessageSeverityEnum)
            _Severity = value
        End Set
    End Property

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If FieldName = CType(obj, ThreadMessage).FieldName Then
            Return True
        End If
        Return False
    End Function
End Class

Public Class ThreadMessageError
    Inherits ThreadMessage

    Public Sub New(ByVal fieldName As String, ByVal description As String)
        MyBase.New(ThreadMessageSeverityEnum.Error, fieldName, description)
    End Sub
End Class

Public Class ThreadMessageWarning
    Inherits ThreadMessage

    Public Sub New(ByVal fieldName As String, ByVal description As String)
        MyBase.New(ThreadMessageSeverityEnum.Warning, fieldName, description)
    End Sub
End Class

Public Class ThreadMessageInfo
    Inherits ThreadMessage

    Public Sub New(ByVal fieldName As String, ByVal description As String)
        MyBase.New(ThreadMessageSeverityEnum.Info, fieldName, description)
    End Sub
End Class


Public Class ThreadMessageCollection
    Inherits List(Of ThreadMessage)

    Public Sub New()
    End Sub

    Public Overloads Sub Add(ByVal item As ThreadMessage)
        If Contains(item) Then
            For Each o As ThreadMessage In Me
                If o.FieldName = item.FieldName Then
                    o.Messages.AddRange(item.Messages)
                End If
            Next
        Else
            MyBase.Add(item)
        End If
    End Sub
End Class

<Flags()> _
Public Enum ThreadMessageSeverityEnum
    [Error] = 1
    Warning = 2
    Info = 4
End Enum

Public Enum ValidationResultEnum As Integer
    Unknown = 0
    OK = 1
    Errors = 2
End Enum

<AttributeUsage(AttributeTargets.Property)> _
Public Class LazyValidators
    Public MustInherit Class LazyValidationAttribute
        Inherits Attribute
        Private _Message As String
        Private _ResourceKey As String
        Private _BrokenRuleNumber As Integer

        Public Property ResourceKey() As String
            Get
                Return _ResourceKey
            End Get
            Set(ByVal value As String)
                _ResourceKey = value
            End Set
        End Property

        Public Overridable Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
            End Set
        End Property

        Public Property BrokenRuleNumber() As Integer
            Get
                Return _BrokenRuleNumber
            End Get
            Set(ByVal value As Integer)
                _BrokenRuleNumber = value
            End Set
        End Property

        Public Sub New(ByVal msg As String)
            _Message = msg
        End Sub

        ''' <summary>
        ''' Validates the specified value against the whatever rule you have specified.
        ''' </summary>
        ''' <param name="value">The value.</param>
        ''' <returns></returns>
        Public MustOverride Function Validate(ByVal value As Object) As Boolean

        ''' <summary>
        ''' Gets the get javascript validator.
        ''' </summary>
        ''' <value>The get javascript validator.</value>
        Public Overridable ReadOnly Property GetJavascriptValidator() As String
            Get
                Return ""
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Validates the length of the string to be between 2 values.
    ''' </summary>
    Public Class LengthValidator
        Inherits LazyValidationAttribute
        Private _MaxLength As Integer = 0
        Private _MinLength As Integer = 0

        Public Property MinLength() As Integer
            Get
                Return _MinLength
            End Get
            Set(ByVal value As Integer)
                _MinLength = value
            End Set
        End Property

        Public Property MaxLength() As Integer
            Get
                Return _MaxLength
            End Get
            Set(ByVal value As Integer)
                _MaxLength = value
            End Set
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="LengthValidator" /> class.
        ''' </summary>
        ''' <param name="msg">The messeage to add to the broken rule.</param>
        Public Sub New(Optional ByVal msg As String = "")
            MyBase.New(msg)
            MaxLength = MaxLength
            MinLength = MinLength
        End Sub


        ''' <summary>
        ''' Validates the specified value against the whatever rule you have specified.
        ''' </summary>
        ''' <param name="value">The value.</param>
        ''' <returns></returns>
        Public Overrides Function Validate(ByVal value As Object) As Boolean
            If TypeOf (value) Is String Then
                If MinLength > 0 Then
                    If CStr(value).Length < MinLength Then Return False
                End If
                If MaxLength > 0 Then
                    If CStr(value).Length > MaxLength Then Return False
                End If
            Else
                Throw New Exception(value.GetType.ToString & " is not valid for length validation")
            End If
            Return True
        End Function

        Public Overrides Property Message() As String
            Get
                Return String.Format(MyBase.Message, MinLength, MaxLength)
            End Get
            Set(ByVal value As String)
                MyBase.Message = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' Validates that the string is not empty
    ''' </summary>
    Public Class NotEmptyStringValidator
        Inherits LazyValidationAttribute

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Validates the specified value against the whatever rule you have specified.
        ''' </summary>
        ''' <param name="value">The value.</param>
        ''' <returns></returns>
        Public Overrides Function Validate(ByVal value As Object) As Boolean
            If CType(value, String) = String.Empty Then
                Return False
            Else
                Return True
            End If
        End Function
    End Class

    ''' <summary>
    ''' Validates the value to be not null.
    ''' will also work agains nullable of properties
    ''' </summary>
    Public Class NotNullValidator
        Inherits LazyValidationAttribute

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Validates the specified value against the whatever rule you have specified.
        ''' </summary>
        ''' <param name="value">The value.</param>
        ''' <returns></returns>
        Public Overrides Function Validate(ByVal value As Object) As Boolean
            If value Is Nothing Then
                Return False
            End If

            If TypeOf (value) Is Int16 Or TypeOf (value) Is Int32 Or TypeOf (value) Is Int64 Then
                If CInt(value) = 0 Then
                    Return False
                Else
                    Return True
                End If
            End If

            If TypeOf (value) Is Nullable(Of Int16) OrElse TypeOf (value) Is Nullable(Of Int32) OrElse TypeOf (value) Is Nullable(Of Int64) Then
                If Not CType(value, Nullable(Of Int64)).HasValue Then
                    Return False
                End If
            End If

            If TypeOf (value) Is Nullable(Of UInt16) OrElse TypeOf (value) Is Nullable(Of UInt32) OrElse TypeOf (value) Is Nullable(Of UInt64) Then
                If Not CType(value, Nullable(Of UInt64)).HasValue Then
                    Return False
                End If
            End If

            If TypeOf (value) Is Nullable(Of Decimal) OrElse TypeOf (value) Is Nullable(Of Double) OrElse TypeOf (value) Is Nullable(Of Single) Then
                If Not CType(value, Nullable(Of Decimal)).HasValue Then
                    Return False
                End If
            End If

            If TypeOf (value) Is Nullable(Of Date) Then
                If Not CType(value, Nullable(Of DateTime)).HasValue Then
                    Return False
                End If
            End If

            Return True
        End Function
    End Class

    ''' <summary>
    ''' Validate the string agains a RegulareExpression
    ''' </summary>
    Public Class RegexpStringValidator
        Inherits LazyValidationAttribute

        Private ReadOnly _Pattern As String

        Public Sub New(Optional ByVal msg As String = "", Optional ByVal pattern As String = "")
            MyBase.New(msg)
            _pattern = pattern
        End Sub

        Public Overrides Function Validate(ByVal value As Object) As Boolean
            If Not Regex.IsMatch(CStr(value), _pattern, RegexOptions.IgnoreCase Or RegexOptions.Singleline) Then
                Return False
            End If
            Return True
        End Function
    End Class


    ''' <summary>
    ''' Validates the object.
    ''' </summary>
    ''' <param name="t">The t.</param>
    ''' <param name="o">The o.</param>
    Public Shared Sub ValidateObject(ByVal t As Type, ByVal o As Object)
        Dim valid As LazyValidationAttribute

        For Each p As PropertyInfo In t.GetProperties
            For Each a As Object In p.GetCustomAttributes(False)
                If TypeOf (a) Is LazyValidationAttribute Then
                    valid = DirectCast(a, LazyValidationAttribute)
                    If Not valid.Validate(p.GetValue(o, Nothing)) Then
                        ResponseThread.Current.Add(New ThreadMessage(ThreadMessageSeverityEnum.Error, p.Name, valid.Message))
                    End If
                End If
            Next
        Next
    End Sub
End Class
