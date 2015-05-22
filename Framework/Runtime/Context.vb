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
        
        Private Shared ctxStore As New Dictionary(Of Integer, IContext)
        Public Shared Sub AddOverrideContext(ctx As IContext)
            ctxStore(Thread.CurrentThread.ManagedThreadId) = ctx
        End Sub

        Public Shared Sub RemoveContext()
            ctxStore.Remove(Thread.CurrentThread.ManagedThreadId)
        End Sub





    End Class

End Namespace
