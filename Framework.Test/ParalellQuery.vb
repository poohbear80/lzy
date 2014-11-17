Imports LazyFramework.CQRS
Imports NUnit.Framework
Imports LazyFramework.Runtime
Imports LazyFramework.CQRS.Query

<TestFixture> Public Class ParalellQuery

    Public Class QuerySecurity
        Implements IActionSecurity



        Public Function UserCanRunThisAction(c As IAmAnAction) As Boolean Implements IActionSecurity.UserCanRunThisAction
            Return True
        End Function


        Public Function EntityIsAvailableForUser(user As Security.Principal.IPrincipal, action As IAmAnAction, entity As Object) As Boolean Implements IActionSecurity.EntityIsAvailableForUser
            Return True
        End Function
    End Class

    Dim s As LazyFramework.ClassFactory.SessionInstance

    <SetUp> Public Sub SetUp()
        Context.Current = New WinThread

        s = New LazyFramework.ClassFactory.SessionInstance
        LazyFramework.ClassFactory.Clear()
        LazyFramework.ClassFactory.SetTypeInstance(Of IDataAccessProvider)(New SqlServer2)

        Dim lazyFrameworkConfiguration As LazyFrameworkConfiguration = New LazyFrameworkConfiguration
        LazyFramework.ClassFactory.SetTypeInstanceForSession(Of ILazyFrameworkConfiguration)(lazyFrameworkConfiguration)
        LazyFramework.ClassFactory.SetTypeInstanceForSession(Of IConnectionInfoBuilder)(New SqlConnectionInfoBuilder)

        lazyFrameworkConfiguration.LogLevel = 100
        lazyFrameworkConfiguration.EnableTiming = True

        LazyFramework.ClassFactory.SetTypeInstanceForSession(Of IActionSecurity)(New QuerySecurity)
    End Sub

    <TearDown> Public Sub TearDown()
        s.Complete()
        s.Dispose()
        s = Nothing
    End Sub


    <Test> Public Sub ExecuteMultiQuery()

        Dim res As Person
        Dim q As New GetPersonInfoQuery
        q.Id = 1

        res = CType(Handling.ExecuteQuery(q), Person)

        Assert.AreEqual(1, res.Id)
        Assert.AreEqual("Petter Ekrann", res.Name)
        Assert.AreEqual(1, res.Email.Count)

    End Sub


    <Test, Ignore> Public Sub SpeedTest()
        Dim tinfo As New Utils.TimingInfos
        Dim res As Person
        Dim q As New GetPersonInfoQuery
        q.Id = 1

        res = CType(Handling.ExecuteQuery(q), Person)

        Using New Utils.InlineTimer("Paralell", tinfo)
            For x = 0 To 100
                res = CType(Handling.ExecuteQuery(q), Person)
            Next
        End Using
        Using New Utils.InlineTimer("Seq", tinfo)
            For x = 0 To 100
                Dim t As New GetPersonInfoQueryHandler
                t.Query = q
                res = t.Result
                't.FillEmail()
                t.FillPerson()
                't.FillAddress()
                't.FillAddress2()
                't.FillAddress3()
                't.FillAddress4()
                't.FillAddress5()
            Next
        End Using
        
        Debug.Print(tinfo("Paralell").List.Sum().ToString)
        Debug.Print(tinfo("Seq").List.Sum().ToString)

    End Sub

    Public Class Person
        Public Property Id As Integer
        Public Property Name As String

        Public Property Email As IEnumerable(Of EMail)
        Public Property Addresss As IEnumerable(Of Addresss)
        Public Property Addresss2 As IEnumerable(Of Addresss)
        Public Property Addresss3 As IEnumerable(Of Addresss)
        Public Property Addresss4 As IEnumerable(Of Addresss)
        Public Property Addresss5 As IEnumerable(Of Addresss)
    End Class

    Public Class EMail
        Public Property Id As Integer
        Public Property UnitId As Integer
        Public Property Adress As String
    End Class
    Public Class Addresss
        Public Property Id As Integer
        Public Property UnitId As Integer
        Public Property Address As String
    End Class



    Public Class GetPersonInfoQuery
        Inherits QueryBase

        Public Property Id As Integer

        Public Overrides ReadOnly Property Name As String
            Get
                Return "GetPersonInfo"
            End Get
        End Property
    End Class


    Public Class GetPersonInfoQueryHandler
        Inherits ParalellQueryBase(Of GetPersonInfoQuery, Person)

        Public ConnectionInfo As New ServerConnectionInfo With {.Address = "13-testsql", .Database = "hr", .UserName = "sa", .Password = "supermann"}

        Public Sub FillPerson()
            Dim cmd As New CommandInfo
            cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
            cmd.CommandText = "Select * from HrUnit where Id = @Id "
            Data.Exec(ConnectionInfo, cmd, Result)
        End Sub

        Public Sub FillEmail()
            Dim list As New List(Of EMail)
            Dim cmd As New CommandInfo
            cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
            cmd.CommandText = "Select * from HrElectronicAddress where unitId = @Id "
            Data.Exec(ConnectionInfo, cmd, list)
            Result.Email = list
        End Sub

        Public Sub FillAddress()
            Dim list As New List(Of Addresss)
            Dim cmd As New CommandInfo
            cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
            cmd.CommandText = "Select * from HrAddress where unitId = @Id "
            Data.Exec(ConnectionInfo, cmd, list)
            Result.Addresss = list
        End Sub

        'Public Sub FillAddress2()
        '    Dim list As New List(Of Addresss)
        '    Dim cmd As New CommandInfo
        '    cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
        '    cmd.CommandText = "Select * from HrAddress where unitId = @Id "
        '    Data.Exec(ConnectionInfo, cmd, list)
        '    Result.Addresss2 = list
        'End Sub
        'Public Sub FillAddress3()
        '    Dim list As New List(Of Addresss)
        '    Dim cmd As New CommandInfo
        '    cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
        '    cmd.CommandText = "Select * from HrAddress where unitId = @Id "
        '    Data.Exec(ConnectionInfo, cmd, list)
        '    Result.Addresss3 = list
        'End Sub
        'Public Sub FillAddress4()
        '    Dim list As New List(Of Addresss)
        '    Dim cmd As New CommandInfo
        '    cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
        '    cmd.CommandText = "Select * from HrAddress where unitId = @Id "
        '    Data.Exec(ConnectionInfo, cmd, list)
        '    Result.Addresss4 = list
        'End Sub
        'Public Sub FillAddress5()
        '    Dim list As New List(Of Addresss)
        '    Dim cmd As New CommandInfo
        '    cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
        '    cmd.CommandText = "Select * from HrAddress where unitId = @Id "
        '    Data.Exec(ConnectionInfo, cmd, list)
        '    Result.Addresss5 = list
        'End Sub
        'Public Sub FillAddress6()
        '    Dim list As New List(Of Addresss)
        '    Dim cmd As New CommandInfo
        '    cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
        '    cmd.CommandText = "Select * from HrAddress where unitId = @Id "
        '    Data.Exec(ConnectionInfo, cmd, list)
        '    Result.Addresss5 = list
        'End Sub
        'Public Sub FillAddress7()
        '    Dim list As New List(Of Addresss)
        '    Dim cmd As New CommandInfo
        '    cmd.Parameters.Add("Id", DbType.Int32, Query.Id)
        '    cmd.CommandText = "Select * from HrAddress where unitId = @Id "
        '    Data.Exec(ConnectionInfo, cmd, list)
        '    Result.Addresss5 = list
        'End Sub

    End Class


End Class
