Imports System.Threading

Namespace Monitor
    <AttributeUsage(AttributeTargets.Class)> Public Class MonitorMaxTimeAttribute
        Inherits Attribute

        Public MaxTimeInMs As Integer


        Public Sub New(maxTimeInMs As Integer)
            Me.MaxTimeInMs = maxTimeInMs
        End Sub

    End Class

    Public Interface IMonitorData
        Property Name As String
        Property StartTime As Long
        Property EndTime As Long
        ReadOnly Property Took() As Long
        Property User As String
    End Interface



    Public Class Handling

        Private Shared _monitorQueue As New Queue(Of IMonitorData)

        Public Shared Function GetQueueCount() As Integer
            Return _monitorQueue.Count
        End Function

        Private Shared ReadOnly PadLock As New Object

        Public Shared Function GetMonitorItems() As IEnumerable(Of IMonitorData)
            Dim temp As Queue(Of IMonitorData)
            SyncLock PadLock
                temp = _monitorQueue
                _monitorQueue = New Queue(Of IMonitorData)
            End SyncLock
            Return temp.ToArray
        End Function

        Public Shared Sub AddToQueue(m As IMonitorData)
            If _monitorQueue.Count > 1000 Then
                _monitorQueue.Dequeue()
            End If
            _monitorQueue.Enqueue(m)
        End Sub

        Private Shared QWThread As Threading.Thread
        Private Shared Watcher As QueueWatcher

        Public Shared Sub StartMonitoring()
            Watcher = New QueueWatcher
            QWThread = New Threading.Thread(AddressOf Watcher.Run)
            QWThread.Start()
        End Sub

        Public Shared Sub StopMonitor()
            Watcher.Abort()
            QWThread.Join()
        End Sub
    End Class

    Public Class QueueWatcher
        Private Const StartSleepLength As Integer = 100

        Private continueRun As Boolean = True
        Private sleepLength As Integer = StartSleepLength

        Public Sub Abort()
            continueRun = False
        End Sub

        Public Sub Run()
            While continueRun
                Try
                    If Handling.GetQueueCount <> 0 Then
                        Logger.Log(Handling.GetMonitorItems)
                        sleepLength = StartSleepLength
                    Else
                        If sleepLength < StartSleepLength * 100 Then
                            sleepLength = sleepLength + StartSleepLength
                        End If
                    End If
                    Threading.Thread.Sleep(sleepLength)
                Catch ex As ThreadAbortException
                    continueRun = False
                End Try
            End While
            Logger.Log(Handling.GetMonitorItems)
        End Sub
    End Class

    Public Class Logger
        Public Shared Sub Log(m As IEnumerable(Of IMonitorData))
            For Each w In Loggers
                Try
                    If Not w.IsSuspended Then
                        w.Write(m)
                    End If
                Catch ex As Exception
                    w.IsSuspended = True
                End Try
            Next
        End Sub

        Public Shared ReadOnly Loggers As New List(Of IMonitorWriter)

    End Class

    Public Interface IMonitorWriter
        Sub Write(list As IEnumerable(Of IMonitorData))
        Property IsSuspended As Boolean
    End Interface
End Namespace