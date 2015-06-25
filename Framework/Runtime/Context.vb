Imports System.Threading

Namespace Runtime
    Public Class Context

        Private Shared _current As IContext
        Public Shared Property Current As IContext
            Get
                If ctxStore.ContainsKey(Thread.CurrentThread.ManagedThreadId) Then
                    Return ctxStore(Thread.CurrentThread.ManagedThreadId)
                Else
                    Return _current
                End If
            End Get
            Set(value As IContext)
                _current = value
            End Set
        End Property
        
        Private Shared PadLock As New Object
        Private shared _ctxStore As Dictionary(Of Integer, IContext)
        Private Shared ReadOnly Property CtxStore As  Dictionary(Of Integer, IContext)
        Get
            If _ctxStore Is Nothing Then
                SyncLock PadLock
                    If _ctxStore Is Nothing Then
                        _ctxStore = New Dictionary(Of Integer, IContext)
                    End If
                End SyncLock
            End If
            Return _ctxStore
        End Get
        End Property


        Public Shared Sub AddOverrideContext(ctx As IContext)
            ctxStore(Thread.CurrentThread.ManagedThreadId) = ctx
        End Sub

        Public Shared Sub RemoveContext()
            If ctxStore.ContainsKey(Thread.CurrentThread.ManagedThreadId) Then
                ctxStore.Remove(Thread.CurrentThread.ManagedThreadId)
            End If
        End Sub


    End Class

End Namespace
