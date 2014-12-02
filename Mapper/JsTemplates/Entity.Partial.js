result.StartBlock("Namespace Entities");
result.StartBlock("Partial Class {0}", [data.Table.Name]);

result.StartBlock("Public sub New()");
_.each(data.Table.Columns, function (c, i) {
    if (c.DefaultValue) result.WriteFormatLine('{0} = {1}', [c.Name, c.DefaultValueCode]);
});
result.EndBlock("End Sub");


//Properies
_.each(data.Table.Columns, function (o) {
    result.WriteLine("Public Property {0} As {1}{2}", [o.Name, o.NetRuntimeType, o.Nullable && o.IsValueType ? '?' : '']);
});
result.WriteLine("");

result.EndBlock("End Class");
result.WriteLine("");
result.EndBlock("End Namespace");
result.WriteLine("");
