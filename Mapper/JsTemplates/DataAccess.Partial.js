//Her kommer det en beskrivelse av js variabler som man skal ha tilgang til.

var identity = _.filter(data.Table.Columns, function (e) { return e.Identity; });
var primaryKey = _.filter(data.Table.Columns, function (e) { return e.PartOfPrimaryKey; });
var notIdentity = _.filter(data.Table.Columns, function (e) { return !e.Identity; });
var params = _.map(primaryKey, function (c) { return Utils.toParamName(c.Name) + " as " + c.NetRuntimeType; });

result.WriteLine("Imports LazyFramework");
result.WriteLine("' ReSharper disable once CheckNamespace");
result.StartBlock("Namespace DataAccess");
result.WriteLine("");

result.StartBlock("Partial Class {0}DataAccess", [data.TableName]);
result.WriteFormatLine("Implements I{0}DataAccess", [data.TableName]);
result.WriteLine("");

//GetEntity
if (identity.length) {
    result.StartBlock("Public Sub GetOne( ByRef o As Entities.{0}{1}) Implements I{0}DataAccess.GetOne", [data.TableName, "," + params.join(",")]);
    result.WriteLine("Dim dbc As New CommandInfo");
    result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read");

    _.each(primaryKey, function (e, i) {
        result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
    });

    result.WriteFormatLine('dbc.CommandText = "select * from {0} where {1}"', [data.TableName, _.map(primaryKey, function (o) { return o.Name + "=@" + o.Name }).join(" and ")]);
    result.WriteLine("Data.Store.Exec(of Entities.{0} )(SourceName, dbc, o)", [data.TableName]);
    result.EndBlock("End Sub");
    result.WriteLine("");


    //Update
    result.StartBlock("Public Sub Update( ByRef o As Entities.{0}) Implements I{0}DataAccess.Update", [data.TableName]);
    result.WriteLine("Dim dbc As New CommandInfo");
    result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Update");

    _.each(data.Table.Columns, function (e, i) {
        result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
    });

    result.WriteFormatLine('dbc.CommandText = "SET NOCOUNT ON Update {0} SET {1} where {2} "', [
        data.TableName,
        _.map(notIdentity, function (o) { return '[' + o.Name + '] = @' + o.Name }).join(','),
        _.map(primaryKey, function (o) { return o.Name + "=@" + o.Name }).join(" and ")]);

    result.WriteLine("Data.Store.Exec(of Entities.{0} )(SourceName, dbc, o)", [data.TableName]);
    result.EndBlock("End Sub");
    result.WriteLine("");

    //Delete
    result.StartBlock("Public Sub Delete( ByRef o As Entities.{0}) Implements I{0}DataAccess.Delete", [data.TableName]);
    result.WriteLine("Dim dbc As New CommandInfo");
    result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Delete");
    _.each(primaryKey, function (e, i) {
        result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
    });
    result.WriteFormatLine('dbc.CommandText = "Delete from {0} where {1}"', [
        data.TableName,
        _.map(primaryKey, function (o) { return o.Name + "=@" + o.Name }).join(" and ")
    ]);

    result.WriteLine("Data.Store.Exec(of Entities.{0} )(SourceName, dbc, o)", [data.TableName]);
    result.EndBlock("End Sub");


}


//GetAll

result.StartBlock("Public Sub GetAll( ByRef o As Entities.{0}Collection) Implements I{0}DataAccess.GetAll", [data.TableName]);
result.WriteLine("Dim dbc As New CommandInfo");
result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read");
result.WriteFormatLine('dbc.CommandText = "select * from {0}"', [data.TableName]);
result.WriteLine("Data.Store.Exec(of Entities.{0} )(SourceName, dbc, o)", [data.TableName]);
result.EndBlock("End Sub");
result.WriteLine("");


//Create

result.StartBlock("Public Sub Create( ByRef o As Entities.{0})  Implements I{0}DataAccess.Create", [data.TableName]);
result.WriteLine("Dim dbc As New CommandInfo");
result.WriteLine("dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Create");

_.each(notIdentity, function (e, i) {
    result.WriteFormatLine('dbc.Parameters.Add("{0}", DbType.{1},o.{0})', [e.Name, e.DbType]);
});


if (identity.length) {
    result.WriteFormatLine('dbc.CommandText = "SET NOCOUNT ON insert into {0}({1}) values({2}); declare @InsertedID int; set @InsertedID =  SCOPE_IDENTITY() ; Select * from {0} where {3} "',
        [data.TableName,
        _.map(notIdentity, function (o) { return '[' + o.Name + ']' }).join(','),
        _.map(notIdentity, function (o) { return ' @' + o.Name }).join(','),
        _.map(primaryKey, function (o) { return '[' + o.Name + ']=@' + (o.Identity ? 'InsertedId' : o.Name) }).join(' and ')]);
} else {
    result.WriteFormatLine('dbc.CommandText = "SET NOCOUNT ON insert into {0}({1}) values({2});"',
    [data.TableName,
    _.map(notIdentity, function (o) { return "[" + o.Name + "]" }).join(","),
    _.map(notIdentity, function (o) { return " @" + o.Name }).join(",")]);
}
result.WriteLine("Data.Store.Exec(of Entities.{0} )(SourceName, dbc, o)", [data.TableName]);
result.EndBlock("End Sub");
result.WriteLine("");

result.WriteLine("");
result.EndBlock("End Class");
result.WriteLine("");
result.EndBlock("End Namespace");
result.WriteLine("");
