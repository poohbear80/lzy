Imports System.Security.Principal
Imports System.Threading

Namespace Runtime
    Public Class SpawnThreadContext
        Implements IContext, IDisposable



        Private _User As IPrincipal
        Private ReadOnly _Storage As ProxyStorage(Of String, Object)
        Private _Cm As Boolean

        Private myName As String

        Public Sub New(ByVal user As IPrincipal, ByVal storage As Dictionary(Of String, Object), ByVal cm As Boolean)
            _User = user
            _Storage = New ProxyStorage(Of String, Object)(storage)
            _Cm = cm
            Context.AddOverrideContext(Me)
        End Sub

        Public Sub ContextSet() Implements IContext.ContextSet

        End Sub

    

        Public Function Storage() As Dictionary(Of String, Object) Implements IContext.Storage
            Return _Storage
        End Function

        Public Property ChickenMode As Boolean Implements IContext.ChickenMode
            Get
                Return _Cm
            End Get
            Set(value As Boolean)
                _Cm = value
            End Set
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Context.RemoveContext()
                End If
                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region



        Public Shared Function WrapAndFire(toFire As Action) As ThreadStart
            Dim user = Runtime.Context.Current.CurrentUser  'Have to copy this from outside of the loop
            Dim s = Runtime.Context.Current.Storage
            Dim cm = Runtime.Context.Current.ChickenMode

            Return Sub()
                       Using New Runtime.SpawnThreadContext(user, s, cm)
                           toFire()
                       End Using
                   End Sub

        End Function

        Public Property CurrentUser As IPrincipal Implements IContext.CurrentUser
            Get
                Return _User
            End Get
            Set(value As IPrincipal)
                _User = value
            End Set
        End Property
    End Class
End Namespace