var primaryKey = _.filter(data.Table.Columns, function (e) { return e.PartOfPrimaryKey; });
var params = _.map(primaryKey, function (c) { return Utils.toParamName(c.Name) + " as " + c.NetRuntimeType; });


result.WriteComment("ReSharper disable once CheckNamespace")
result.StartBlock("Namespace DataAccess", []);
result.StartBlock("Public Interface I{0}DataAccess", [data.TableName]);


if (identity.length) {
    result.WriteFormatLine("Sub GetOne( ByRef o As Entities.{0}{1}) ", [data.TableName, "," + params.join(",")]);
    result.WriteFormatLine("Sub Update( ByRef o As Entities.{0}) ", [data.TableName]);
    result.WriteFormatLine("Sub Delete( ByRef o As Entities.{0}) ", [data.TableName]);
}
result.WriteFormatLine("Sub GetAll( ByRef o As Entities.{0}Collection)", [data.TableName]);
result.WriteFormatLine("Sub Create( ByRef o As Entities.{0}) ", [data.TableName]);

result.EndBlock("End Interface");
result.EndBlock("End Namespace");
