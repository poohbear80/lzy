Imports NUnit.Framework
Imports LazyFramework.Utils.TestHelpers
Imports System.Data.SqlClient

<TestFixture> Public Class QueryValidator
    <Test> Public Sub ValidateOk()
        Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New ValidateOk)
    End Sub

    <Test> Public Sub ValidateOkForUpdate()
        Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New ValidateOkForUpdate)
    End Sub

    <Test> Public Sub MissmatchCaseDoesNotThrow()
        Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New MissMatchCaseParametersOK)
    End Sub


    <Test> Public Sub MissingParameters()
        Assert.Throws(Of DataaccessValidationException)(Sub() Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New MissingParameter))
    End Sub

    <Test> Public Sub ToManyParameters()
        Assert.Throws(Of DataaccessValidationException)(Sub() Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New ToManyParameters))
    End Sub

    <Test> Public Sub MissingParametersForUpdate()

        Assert.Throws(Of DataaccessValidationException)(Sub() Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New MissingParameterForUpdate))
    End Sub

    <Test> Public Sub ParametersIsDeclaredInSqlDoesNotThrow()
        Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New ParametersDeclaredInCommandDoesNotFail)
    End Sub

    <Test> Public Sub ByrefParamWithParameterlessConstructorIsCreated()
        Dim s = Substitute.For(Of IWithByrefParam)()
        s.ToBeCalled(Arg.Any(Of SteveO)).ReturnsForAnyArgs(Function(params)
                                                               Assert.IsNotNull(params(0))
                                                               Return True
                                                           End Function)
        Utils.TestHelpers.QueryValidator.Validate(Of IWithByrefParam)(s)



    End Sub

    '<Test> Public Sub SendToSqlServerForVerification()
    '    Using New LazyFramework.ClassFactory.SessionInstance
    '        Dim sqlConnection As SqlConnection = New SqlClient.SqlConnection("server=11-testsql;Database=hr;User ID=sa;Password=supermann;")
    '       LazyFramework.ClassFactory.SetTypeInstanceForSession(Of Utils.TestHelpers.ITestSqlConnectionProvider)(New TestSqlConnectionProvider(sqlConnection))
    '        Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New ValidateSql)
    '        Assert.Throws(Of DataaccessValidationException)(Sub() Utils.TestHelpers.QueryValidator.Validate(Of ISomeDatalayerInterface)(New ValidateSqlExpectError))
    '    End Using

    'End Sub


End Class

Public Class MissingParameterForUpdate
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled
        Dim dbc As New LazyFramework.CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Update
        dbc.Parameters.Add("Id", DbType.Int32)

        dbc.CommandText = "select * from AboGroupMapping where [Id] = @id @id2"
        Return LazyFramework.DataAccessFactory.UpdateObject(dbName, CType(o, IORDataObject), dbc)
    End Function
End Class
Public Class MissMatchCaseParametersOK
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled
        Dim dbc As New CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
        dbc.Parameters.Add("Id", DbType.Int32)

        dbc.CommandText = "select * from AboGroupMapping where [Id] = @id"
        Return DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)
    End Function
End Class
Public Class ParametersDeclaredInCommandDoesNotFail
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled
        Dim dbc As New CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
        dbc.Parameters.Add("Id", DbType.Int32)

        dbc.CommandText = "DECLARE @Id2 as int; select * from AboGroupMapping where [Id] = @id and @id2"
        Return DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)
    End Function
End Class

Public Class ToManyParameters
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled
        Dim dbc As New LazyFramework.CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
        dbc.Parameters.Add("Id", DbType.Int32)
        dbc.Parameters.Add("Id2", DbType.Int32)
        dbc.CommandText = "select * from AboGroupMapping where [Id] = @Id"
        Return LazyFramework.DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)
    End Function
End Class

Public Class MissingParameter
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled
        Dim dbc As New LazyFramework.CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
        dbc.Parameters.Add("Id", DbType.Int32)
        dbc.CommandText = "select * from AboGroupMapping where [Id] = @Id and Value2 = @Id2"
        Return LazyFramework.DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)
    End Function
End Class

Public Class ValidateOk
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled

        Dim dbc As New LazyFramework.CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
        dbc.Parameters.Add("Id", DbType.Int32)
        dbc.CommandText = "select * from AboGroupMapping where [Id] = @Id "
        Return LazyFramework.DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)

    End Function
End Class

Public Class ValidateSql
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled

        Dim dbc As New LazyFramework.CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
        dbc.Parameters.Add("Id", DbType.Int32)
        dbc.CommandText = "select * from AboGroupMapping where [Id] = @Id"
        Return LazyFramework.DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)

    End Function
End Class

Public Class ValidateSqlExpectError
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled

        Dim dbc As New LazyFramework.CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
        dbc.Parameters.Add("Id", DbType.Int32)
        dbc.CommandText = "select * from AboGroupMapping where [Id] = @Id ans SomeCol is null"
        Return LazyFramework.DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)

    End Function
End Class


Public Class ValidateOkForUpdate
    Implements ISomeDatalayerInterface

    Public Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean Implements ISomeDatalayerInterface.ToBeCalled

        Dim dbc As New LazyFramework.CommandInfo
        dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Update
        dbc.Parameters.Add("Id", DbType.Int32)
        dbc.CommandText = "select * from AboGroupMapping where [Id] = @Id "
        Return LazyFramework.DataAccessFactory.FillObject(dbName, CType(o, IORDataObject), dbc)

    End Function
End Class



Public Class SteveO
    Inherits LazyFramework.LazyBaseClass

    Public Overrides Function GetValue(name As String, ByRef value As Object) As Boolean
        Return False
    End Function

    Public Overrides Function SetValue(name As String, value As Object) As Boolean
        Return False
    End Function
End Class


Public Interface ISomeDatalayerInterface
    Function ToBeCalled(ByVal dbName As String, ByVal o As SteveO, ByVal pram1 As Integer) As Boolean
End Interface

Public Interface IWithByrefParam
    Function ToBeCalled(ByRef a As SteveO) As Boolean
End Interface

