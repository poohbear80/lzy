result.WriteLine("");
result.StartBlock("Namespace Entities");
result.WriteLine("");
result.StartBlock("Public Class {0}", [data.Table.Name]);

result.WriteLine("");
result.EndBlock("End Class");
result.WriteLine("");
result.EndBlock("End Namespace");
