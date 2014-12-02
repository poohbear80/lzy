result.WriteComment("ReSharper disable once CheckNamespace")
result.StartBlock("Namespace Aggregates");
result.WriteLine("");
result.StartBlock("Friend Class {0}Aggregate", [data.TableName]);
result.WriteFormatLine("Inherits Itas.Core.BaseAggregateGeneric(Of {0}, {1}DataAccess)", ["DataAccess.I" + data.TableName + "DataAccess", "DataAccess." + data.TableName]);
result.WriteLine("");
result.EndBlock("End Class");
result.WriteLine("");
result.EndBlock("End Namespace");