Imports System.Security.Principal
Imports System.Threading

Namespace Runtime
    ''' <summary>
    ''' To be used in windowsapplications. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WinThread
        Implements IContext



        Private ReadOnly PadLock As New Object
        Public Function Storage() As IDictionary(Of String, Object) Implements IContext.Storage
            Dim ldss As LocalDataStoreSlot = Thread.GetNamedDataSlot(Constants.StoreName)
            If Thread.GetData(ldss) Is Nothing Then
                SyncLock PadLock
                    If Thread.GetData(ldss) Is Nothing Then
                        Thread.SetData(ldss, New Dictionary(Of String, Object))
                    End If
                End SyncLock
            End If
            Return CType(Thread.GetData(ldss), Dictionary(Of String, Object))
        End Function

        Public Sub ContextSet() Implements IContext.ContextSet

        End Sub

        Public Property ChickenMode As Boolean = False Implements IContext.ChickenMode

        Public Property CurrentUser As IPrincipal Implements IContext.CurrentUser
            Get
                Return System.Threading.Thread.CurrentPrincipal
            End Get
            Set(value As IPrincipal)
                System.Threading.Thread.CurrentPrincipal = value
            End Set
        End Property
    End Class
End Namespace
