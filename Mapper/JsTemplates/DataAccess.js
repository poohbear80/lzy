result.WriteComment("ReSharper disable once CheckNamespace")
result.StartBlock("Namespace DataAccess");
result.WriteLine("");
result.StartBlock("Friend Class {0}DataAccess", [data.TableName]);
result.WriteLine("Inherits Itas.Core.DataAccessBase.CustomerDb")
result.WriteLine("");
result.EndBlock("End Class");
result.WriteLine("");
result.EndBlock("End Namespace");
