Imports LazyFramework.CQRS
Imports NUnit.Framework
Imports LazyFramework.CQRS.Transform

Public Class DebugLogger
    Implements CQRS.Monitor.IMonitorWriter


    Public Sub Write(list As IEnumerable(Of Monitor.IMonitorData)) Implements Monitor.IMonitorWriter.Write
        For Each e In list
            Debug.Print(Now().TimeOfDay.ToString & ":" & Newtonsoft.Json.JsonConvert.SerializeObject(e))
        Next
    End Sub

    Public Property IsSuspended As Boolean Implements Monitor.IMonitorWriter.IsSuspended
End Class

<TestFixture> Public Class Query

    <SetUp> Public Sub First()
        LazyFramework.Runtime.Context.Current = New Runtime.WinThread

        LazyFramework.ClassFactory.Clear()
        LazyFramework.ClassFactory.SetTypeInstance(Of IActionSecurity)(New TestSecurity)

        'Debug.Print(Now.Ticks.ToString)

        LazyFramework.CQRS.Monitor.Handling.StartMonitoring()
        Monitor.Logger.Loggers.Add(New DebugLogger)
    End Sub

    '<SetUp> Public Sub SetUp()

    'End Sub

    <TearDown> Public Sub TearDown()
            LazyFramework.CQRS.Monitor.Handling.StopMonitor
    End Sub

    <Test> Public Sub QueryFlowIsCorrect()
        Dim q As New TestQuery With {.Id = 1}

        Dim res As Object

        res = CQRS.Query.Handling.ExecuteQuery(q)
        
        Assert.IsInstanceOf(Of QueryResultDto)(res)

    End Sub


    <Test> Public Sub ListIsConvertedCorrectly()

        Dim q As New TestQuery2 With {.Id = 1, .Startdate = Now}
        Dim res = CQRS.Query.Handling.ExecuteQuery(q)

        Assert.IsInstanceOf(Of QueryResultDto)(CType(res, IEnumerable)(0))

    End Sub

End Class


Public Class ValidateTestQuery
    Inherits CQRS.Validation.ValidateActionBase(Of TestQuery)


End Class

Public Class ValidateTestQuery3
    Inherits CQRS.Validation.ValidateActionBase(Of TestQuery3)

End Class

<Monitor.MonitorMaxTime(0)> Public Class TestQuery
    Inherits CQRS.Query.QueryBase

    Public Id As Integer


End Class


Public Class TestQuery2
    Inherits TestQuery
    Public Startdate As DateTime

End Class

Public Class TestQuery3
    Inherits TestQuery2

End Class

Public Class QueryHandler
    Implements CQRS.Query.IHandleQuery

    Public Shared Function Dummy(q As TestQuery) As QueryResult
        Return New QueryResult With {.Id = 1, .Name = "Espen", .SomeDate = New Date(1986, 7, 24)}
    End Function

    Public Shared Function Dummy2(q As TestQuery2) As List(Of QueryResult)

        Return New List(Of QueryResult) From {New QueryResult With {.Id = 1, .Name = "Espen", .SomeDate = New Date(1986, 7, 24)}}

    End Function

End Class


Public Class QueryResult
    Public Id As Integer
    Public Name As String
    Public SomeDate As DateTime
End Class

Public Class QueryResultDto
    Public Id As Integer
    Public NameAndDate As String

End Class


Public Class TransformFactory
    Inherits TransformerFactoryBase(Of TestQuery, QueryResult)

    Dim trans As New Transformers

    Public Overrides Function GetTransformer(action As TestQuery, ent As QueryResult) As ITransformEntityToDto
        Return trans
    End Function
End Class

Friend Class Transformers
    Inherits TransformerBase(Of QueryResult, QueryResultDto)

    Public Overrides Function TransformToDto(ent As QueryResult) As QueryResultDto

        Dim ret As New QueryResultDto
        ret.Id = ent.Id
        ret.NameAndDate = String.Format("{0} har bursdag på {1}", ent.Name, ent.SomeDate.ToShortDateString)
        Return ret
    End Function
End Class


