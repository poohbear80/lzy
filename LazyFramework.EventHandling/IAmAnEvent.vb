
Public Interface IAmAnEvent
    
    ReadOnly Property RunAsync As Boolean
    Sub ActionComplete()
    Function EndTimeStamp() As Long
    Function Guid() As Guid
    Function TimeStamp() As Long
    Sub HandlerStart()
    Function HandlerStartTimeStamp() As Long
End Interface
