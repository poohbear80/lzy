//Her kommer det en beskrivelse av js variabler som man skal ha tilgang til.

var identity = _.filter(data.Table.Columns, function (e) { return e.Identity; });
var primaryKey = _.filter(data.Table.Columns, function (e) { return e.PartOfPrimaryKey; });
var notIdentity = _.filter(data.Table.Columns, function (e) { return !e.Identity; });

result.StartBlock("Namespace DataAccess");

result.StartBlock("Partial Class {0}DataAccess", [data.TableName]);
result.WriteFormatLine("Implements I{0}", [data.TableName]);
//GetEntity
result.StartBlock("Public Sub GetEntity(ByVal sourceName As String, ByRef o As Entities.{0})  Implements I{0}.GetEntity", [data.TableName]);
result.WriteLine("Dim dbc As New LazyFramework.CommandInfo");
result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read")

_.each(primaryKey, function (e, i) {
    result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
});

result.WriteFormatLine('dbc.CommandText = "select * from {0} where {1}"', [data.TableName, _.map(primaryKey, function (o) { return o.Name + "=@" + o.Name }).join(" and ")]);
result.WriteLine("LazyFramework.DataAccessFactory.FillObject(SourceName, CType(o, IORDataObject), dbc)");
result.EndBlock("End Sub");

//GetAll

result.StartBlock("Public Sub GetAll(ByVal sourceName As String, ByRef o As Entities.{0}Collection) Implements I{0}.GetAll", [data.TableName]);
result.WriteLine("Dim dbc As New LazyFramework.CommandInfo");
result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read");
result.WriteFormatLine('dbc.CommandText = "select * from {0}"', [data.TableName]);
result.WriteLine("LazyFramework.DataAccessFactory.FillObject(SourceName, CType(o, IORDataObject), dbc)");
result.EndBlock("End Sub");


//Create

result.StartBlock("Public Sub Create(ByVal sourceName As String, ByRef o As Entities.{0})  Implements I{0}.Create", [data.TableName]);
result.WriteLine("Dim dbc As New LazyFramework.CommandInfo");
result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Create");

_.each(notIdentity, function (e, i) {
    result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
});

result.WriteFormatLine('dbc.CommandText = "SET NOCOUNT ON insert into {0}({1}) values({2}); declare @InsertedID int; set @InsertedID =  SCOPE_IDENTITY() ; Select * from {0} where {3} "',
	[data.TableName,
    _.map(notIdentity, function (o) { return '[' + o.Name + ']'; }).join(','),
    _.map(notIdentity, function (o) { return ' @' + o.Name; }).join(','),
	_.map(primaryKey, function (o) { return '[' + o.Name + ']=@' + (o.Identity ? 'InsertedId' : o.Name); }).join(' and ')]);

result.WriteLine("LazyFramework.DataAccessFactory.UpdateObject(SourceName, CType(o, IORDataObject), dbc)");
result.EndBlock("End Sub");

//Update
result.StartBlock("Public Sub Update(ByVal sourceName As String, ByRef o As Entities.{0}) Implements I{0}.Update", [data.TableName]);
result.WriteLine("Dim dbc As New LazyFramework.CommandInfo");
result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Update");

_.each(data.Table.Columns, function (e, i) {
    result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
});

result.WriteFormatLine('dbc.CommandText = "SET NOCOUNT ON Update {0} SET {1} where {2} "', [
    data.TableName,
    _.map(notIdentity, function (o) { return '[' + o.Name + '] = @' + o.Name }).join(','),
    _.map(primaryKey, function (o) { return o.Name + "=@" + o.Name; }).join(" and ")]);

result.WriteLine("LazyFramework.DataAccessFactory.UpdateObject(SourceName, CType(o, IORDataObject), dbc)");
result.EndBlock("End Sub");

//Delete
result.StartBlock("Public Sub Delete(ByVal sourceName As String, ByRef o As Entities.{0}) Implements I{0}.Delete", [data.TableName]);
result.WriteLine("Dim dbc As New LazyFramework.CommandInfo");
result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Delete");
_.each(primaryKey, function (e, i) {
    result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
});
result.WriteFormatLine('dbc.CommandText = "Delete from {0} where {1}"', [
    data.TableName,
    _.map(primaryKey, function (o) { return o.Name + "=@" + o.Name; }).join(" and ")
]);

result.WriteLine("LazyFramework.DataAccessFactory.UpdateObject(SourceName, CType(o, IORDataObject), dbc)");
result.EndBlock("End Sub");
result.EndBlock("End Class");

result.EndBlock("End Namespace");
