//
result.StartBlock("Namespace DataAccess",[]);
result.StartBlock("Interface I{0}",[data.TableName]);

result.WriteFormatLine("Sub GetEntity(ByVal sourceName As String, ByRef o As Entities.{0}) ", [data.TableName]);
result.WriteFormatLine("Sub GetAll(ByVal sourceName As String, ByRef o As Entities.{0}Collection) ", [data.TableName]);
result.WriteFormatLine("Sub Create(ByVal sourceName As String, ByRef o As Entities.{0})", [data.TableName]);
result.WriteFormatLine("Sub Update(ByVal sourceName As String, ByRef o As Entities.{0})", [data.TableName]);
result.WriteFormatLine("Sub Delete(ByVal sourceName As String, ByRef o As Entities.{0})", [data.TableName]);

result.EndBlock("End Interface");
result.EndBlock("End Namespace");

