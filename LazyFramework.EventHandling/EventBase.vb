Public MustInherit Class EventBase
    Implements IAmAnEvent


    Private _guid As Guid
    Private _endTimestamp As Long
    Private _TimeStamp As Long

    Public Sub New()
        _guid = System.Guid.NewGuid()
        _TimeStamp = Now.Ticks
    End Sub

    Public Overridable ReadOnly Property RunAsync As Boolean Implements IAmAnEvent.RunAsync
        Get
            Return False
        End Get
    End Property

    
    
    Public Sub ActionComplete() Implements IAmAnEvent.ActionComplete
        _endTimestamp = Now.Ticks
    End Sub

    Public Function EndTimeStamp() As Long Implements IAmAnEvent.EndTimeStamp
        Return _endTimestamp
    End Function

    Public Function Guid() As Guid Implements IAmAnEvent.Guid
        Return _guid
    End Function

    Public Function TimeStamp() As Long Implements IAmAnEvent.TimeStamp
        Return _TimeStamp
    End Function
        
    Private _hsts As Long
    Public Sub HandlerStart() Implements IAmAnEvent.HandlerStart
        _hsts = Now.Ticks
    End Sub
    Public Function HandlerStartTimeStamp() As Long Implements IAmAnEvent.HandlerStartTimeStamp
        Return _hsts
    End Function
End Class

