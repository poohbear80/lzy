var primaryKey = _.filter(data.Table.Columns, function (e) { return e.PartOfPrimaryKey; });

var notIdentity = _.filter(data.Table.Columns, function (e) { return !e.Identity; });
var Identity = _.map(primaryKey, function (c) { return Utils.toParamName(c.Name) });
var params = _.map(primaryKey, function (c) { return Utils.toParamName(c.Name) + " as " + c.NetRuntimeType; });


result.Comment("ReSharper disable once CheckNamespace");
result.StartBlock("Namespace Aggregates");
result.WriteLine("");
result.StartBlock("Partial Class {0}Aggregate", [data.TableName]);

//GetInstance
if (params.length) {
    result.WriteLine("");
    result.WriteFormatLine("Public Function GetOne({0}) as Entities.{1}", [params.join(","), data.TableName]);
    result.WriteLine("");

    result.WriteFormatLine("Dim retObj As New Entities.{0}", [data.TableName]);

    result.WriteFormatLine("Repository.GetOne(retObj,{0})", [Identity.join(",")]);
    result.WriteLine("Return retObj ");
    result.WriteLine("");
    result.EndBlock("End Function");

    //UpdateInstance
    result.StartBlock("");
    result.StartBlock("Public Sub UpdateInstance(ByRef o As Entities.{0}) ", [data.TableName]);
    result.WriteLine("");
    result.WriteLine("Repository.Update( o)");

    result.WriteLine("");
    result.EndBlock("End Sub");

    //Delete Instance
    result.StartBlock("");
    result.StartBlock("Public Sub DeleteInstance(ByVal o As Entities.{0} ) ", [data.TableName]);
    result.WriteLine("");

    result.WriteLine("Repository.Delete( o)");

    result.WriteLine("");
    result.EndBlock("End Sub");
    result.WriteLine("");

}


//GetAllInstances
result.StartBlock("");
result.StartBlock("Public Function GetAllInstances() As Entities.{0}Collection", [data.TableName]);
result.WriteLine("");
result.WriteFormatLine("Dim retObj As New Entities.{0}Collection", [data.TableName]);
result.WriteLine("Repository.GetAll( retObj)");
result.WriteLine("Return retObj");
result.WriteLine("");
result.EndBlock("End Function");

//CreateInstance
result.StartBlock("");
result.StartBlock("Public Sub CreateInstance(ByRef o As Entities.{0})", [data.TableName]);
result.WriteLine("");

result.WriteLine("Repository.Create(o)");
result.WriteLine("");
result.EndBlock("End Sub");



result.EndBlock("End Class");
result.WriteLine("");
result.EndBlock("End Namespace");
