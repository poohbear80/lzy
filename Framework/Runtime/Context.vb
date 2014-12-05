Imports System.Threading

Namespace Runtime
    Public Class Context

        Private Shared _current As IContext
        Public Shared Property Current As IContext
            Get
                If OverrideContext IsNot Nothing Then
                    Return OverrideContext
                Else
                    Return _current
                End If
            End Get
            Set(value As IContext)
                _current = value
            End Set
        End Property

        <ThreadStatic> Public Shared OverrideContext As IContext

    End Class

End Namespace
