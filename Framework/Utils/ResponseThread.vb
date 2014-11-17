Imports System.Web
Imports System.Threading
Imports System.ServiceModel

Namespace Utils

    Public Class ResponseThread
        Private ReadOnly _AllMessages As New ThreadMessageCollection

        Public Sub New()
            _Logger = New Logging.Logger
            If LazyFrameworkConfiguration.Current IsNot Nothing Then
                Timer.DoLog = LazyFrameworkConfiguration.Current.EnableTiming
                Logging.Logger.LogLevel = LazyFrameworkConfiguration.Current.LogLevel
            Else
                Timer.DoLog = False
                Logging.Logger.LogLevel = 0
            End If
        End Sub

        Public ReadOnly Property HasErrors() As Boolean
            Get
                Return Errors.Count > 0
            End Get
        End Property

        Public ReadOnly Property HasWarnings() As Boolean
            Get
                Return Warings.Count > 0
            End Get
        End Property

        Public ReadOnly Property Info() As IList(Of ThreadMessage)
            Get
                Return _AllMessages.FindAll(Function(e) (e IsNot Nothing) AndAlso e.Severity = ThreadMessageSeverityEnum.Info)
            End Get
        End Property

        Public ReadOnly Property Warings() As IList(Of ThreadMessage)
            Get
                Return _AllMessages.FindAll(Function(e) (e IsNot Nothing) AndAlso e.Severity = ThreadMessageSeverityEnum.Warning)
            End Get
        End Property

        Public ReadOnly Property Errors() As IList(Of ThreadMessage)
            Get
                Return _AllMessages.FindAll(Function(e) (e IsNot Nothing) AndAlso e.Severity = ThreadMessageSeverityEnum.Error)
            End Get
        End Property

        Public ReadOnly Property AllMessages() As IEnumerable(Of ThreadMessage)
            Get
                Return _AllMessages
            End Get
        End Property

        Public Sub Add(ByVal br As ThreadMessage)
            _AllMessages.Add(br)
        End Sub


        Public ReadOnly Property Timer As Timing
            Get                
                If Not ThreadHasKey(Timerforthread) Then
                    ThreadStore.Add(Timerforthread, New Timing)
                End If
                Return CType(ThreadStore(Timerforthread), Timing)
            End Get
        End Property

        Private _Logger As Logging.Logger
        Public ReadOnly Property Logger As Logging.Logger
            Get
                Return _Logger
            End Get
        End Property


        Public Sub Clear()
            _AllMessages.Clear()
            ThreadStore.Remove(Timerforthread)
            Querys = New QueryInfo
        End Sub

        Public Sub ClearMessages()
            _AllMessages.Clear()
        End Sub

        Private Const ResponseThreadSlot As String = "01F7441D-FCA0-48A5-AE29-CBD6E5F27C9C"
        Private Shared PadLock As New Object
        Private Const Timerforthread As String = "TimerForThread"

        Public Shared Property Current() As ResponseThread
            Get
                If Not ThreadHasKey(ResponseThreadSlot) Then
                    SyncLock PadLock
                        If Not ThreadHasKey(ResponseThreadSlot) Then
                            SetThreadValue(ResponseThreadSlot, New ResponseThread)
                        End If
                    End SyncLock
                End If
                Return GetThreadValue(Of ResponseThread)(ResponseThreadSlot)
            End Get
            Set(value As ResponseThread)
                SetThreadValue(ResponseThreadSlot, value)
            End Set
        End Property

        Public Property Querys As New QueryInfo
        
        Private Const MyStoreName As String = "FC2ED2E8-47BA-4374-80C4-CD51ADE709E5"

        Public Shared Function GetThreadValue(Of TT)(ByVal name As String) As TT
            If Not ThreadStore.ContainsKey(name) Then Throw New KeyNotFoundException

            Return CType(ThreadStore(name), TT)

        End Function
        Public Shared Sub SetThreadValue(ByVal name As String, ByVal value As Object)
            ThreadStore(name) = value
        End Sub
        Public Shared Sub ClearThreadValues()
            ThreadStore.Clear()
        End Sub
        Public Shared Function ThreadHasKey(name As String) As Boolean
            Return ThreadStore.ContainsKey(name)
        End Function

        Private Shared Function ThreadStore() As Dictionary(Of String, Object)

            Return Runtime.Context.Current.Storage

            'If HttpContext.Current IsNot Nothing Then
            '    If HttpContext.Current.Items(MyStoreName) Is Nothing Then
            '        HttpContext.Current.Items(MyStoreName) = New Dictionary(Of String, Object)
            '    End If
            '    Return CType(HttpContext.Current.Items(MyStoreName), Dictionary(Of String, Object))
            'ElseIf OperationContext.Current IsNot Nothing Then
            '    If OperationContext.Current.Extensions.Find(Of ServiceBag)() Is Nothing Then
            '        OperationContext.Current.Extensions.Add(New ServiceBag)
            '    End If
            '    Return OperationContext.Current.Extensions.Find(Of ServiceBag).AllItems
            'Else
            '    If TestHelpers.TestContext.Current IsNot Nothing Then
            '        If Not TestHelpers.TestContext.Current.Items.ContainsKey(MyStoreName) Then
            '            TestHelpers.TestContext.Current.Items.Add(MyStoreName, New Dictionary(Of String, Object))
            '        End If
            '        Return CType(TestHelpers.TestContext.Current.Items(MyStoreName), Dictionary(Of String, Object))
            '    Else
            '        Dim ldss As LocalDataStoreSlot = Thread.GetNamedDataSlot(MyStoreName)
            '        If Thread.GetData(ldss) Is Nothing Then
            '            Thread.SetData(ldss, New Dictionary(Of String, Object))
            '        End If
            '        Return CType(Thread.GetData(ldss), Dictionary(Of String, Object))
            '    End If
            'End If
        End Function
        
        Public Overrides Function ToString() As String
            Dim ret As String = ""
            For Each s In AllMessages

                ret += s.FieldName & ":"
                For Each i In s.Messages
                    ret += i & vbCrLf
                Next
            Next
            Return ret
        End Function

        Public Class QueryInfo
            Property Count As Integer = 0

            Property LogDetails As Boolean = False

            ''' <summary>
            ''' Gives u the number of ms that the system has used to retrvie data from the DB and fill object with that data.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            ReadOnly Property Time As Integer
                Get
                    Return CInt(CDbl(Ticks / Stopwatch.Frequency) * 1000)
                End Get
            End Property

            ReadOnly Property Frq As Long
                Get
                    Return Stopwatch.Frequency
                End Get
            End Property

            Public Ticks As Long = 0
            Public Max As CommandInfo

            Sub Add(ByVal dbName As String, ByVal c As CommandInfo)
                If dbName Is Nothing Then Return
                Add(c)
                If Not Servers.ContainsKey(dbName) Then
                    Servers(dbName) = New QueryInfo
                End If
                Servers(dbName).Add(c)
            End Sub

            Private Sub Add(ByVal c As CommandInfo)
                Count += 1
                Ticks += c.CommandDuration
                If Max Is Nothing Then
                    Max = c
                Else
                    If Max.CommandDuration < c.CommandDuration Then
                        Max = c
                    End If
                End If
                If LogDetails Then
                    If Not _commands.ContainsKey(c.CommandText) Then
                        _commands.Add(c.CommandText, c)
                    Else
                        _commands(c.CommandText).CommandDuration += c.CommandDuration
                        _commands(c.CommandText).Count += 1
                    End If
                End If

            End Sub

            Public Property Servers As New Dictionary(Of String, QueryInfo)

            Private _commands As New Dictionary(Of String, CommandInfo)
            Public ReadOnly Property Commands As Dictionary(Of String, CommandInfo)
                Get
                    Return _commands
                End Get
            End Property

        End Class
    End Class
End Namespace


