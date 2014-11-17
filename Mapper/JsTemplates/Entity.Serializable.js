result.StartBlock("Imports LazyFramework");
result.StartBlock("Namespace Entities");
result.StartBlock("<Serializable> Partial Class {0}", [data.Table.Name]);
result.WriteLine("Implements ISerializable");

result.StartBlock("Public Sub New(info As SerializationInfo, context As StreamingContext)");
result.EndBlock("End Sub");

result.StartBlock("Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData");

result.EndBlock("End Sub");

result.EndBlock("End Class");
result.EndBlock("End Namespace");