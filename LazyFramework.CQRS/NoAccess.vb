﻿Imports LazyFramework.CQRS.Logging
Imports LazyFramework.Data


Public Class NoAccess
    Inherits BaseLogEvent

    Private ReadOnly _AnAction As IAmAnAction

    Public Sub New(ByVal anAction As IAmAnAction)
        _AnAction = anAction
    End Sub

    Public ReadOnly Property AnAction As IAmAnAction
        Get
            Return _AnAction
        End Get
    End Property
End Class
