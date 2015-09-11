Public Class EntityBase
    Implements IEntityBase
    
    Public Property FillResult As FillResultEnum Implements IEntityBase.FillResult

    Private _Loaded As Long
    Public Property Loaded As Long Implements IEntityBase.Loaded
        Get
            Return _loaded
        End Get
        Friend Set(value As Long)
            _loaded = value
        End Set
    End Property
End Class