Namespace DataAccess
	Partial Class HrUnit
		Implements IHrUnit
		Public Function GetEntity(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean Implements IHrUnit.GetEntity
			Dim dbc As New LazyFramework.CommandInfo
			dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
			dbc.Parameters.Add("Id", DbType.int32,o.Id)
			dbc.CommandText = "select * from HrUnit where Id=@Id"
			Return LazyFramework.DataAccessFactory.FillObject(SourceName, CType(o, IORDataObject), dbc)
		End Function
		Public Function GetAll(ByVal sourceName As String, ByRef o As Entities.HrUnitCollection) As Boolean Implements IHrUnit.GetAll
			Dim dbc As New LazyFramework.CommandInfo
			dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Read
			dbc.CommandText = "select * from HrUnit"
			Return LazyFramework.DataAccessFactory.FillObject(SourceName, CType(o, IORDataObject), dbc)
		End Function
		Public Function Create(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean Implements IHrUnit.Create
			Dim dbc As New LazyFramework.CommandInfo
			dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Create
			dbc.CommandText = "SET NOCOUNT ON insert into HrUnit([Id],[ParentId],[UnitTypeId],[Number],[Name],[Description],[Inactive],[Guid],[Created],[Updated],[SortOrder]) values( @Id, @ParentId, @UnitTypeId, @Number, @Name, @Description, @Inactive, @Guid, @Created, @Updated, @SortOrder); declare @InsertedID int; set @InsertedID =  SCOPE_IDENTITY() ; Select * from HrUnit where Id =  @InsertedID "
			Return LazyFramework.DataAccessFactory.UpdateObject(SourceName, CType(o, IORDataObject), dbc)
		End Function
		Public Function Update(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean Implements IHrUnit.Update
			Dim dbc As New LazyFramework.CommandInfo
			dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Update
			dbc.Parameters.Add("Id", DbType.int32, 4, False)
			dbc.CommandText = "SET NOCOUNT ON Update HrUnit SET [Id] = @Id,[ParentId] = @ParentId,[UnitTypeId] = @UnitTypeId,[Number] = @Number,[Name] = @Name,[Description] = @Description,[Inactive] = @Inactive,[Guid] = @Guid,[Created] = @Created,[Updated] = @Updated,[SortOrder] = @SortOrder where Id=@Id "
			Return LazyFramework.DataAccessFactory.UpdateObject(SourceName, CType(o, IORDataObject), dbc)
		End Function
		Public Function Delete(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean Implements IHrUnit.Delete
			Dim dbc As New LazyFramework.CommandInfo
			dbc.TypeOfCommand = CommandInfoCommandTypeEnum.Delete
			dbc.CommandText = "Delete from HrUnit where Id=@Id"
			Return LazyFramework.DataAccessFactory.UpdateObject(SourceName, CType(o, IORDataObject), dbc)
		End Function
	End Class
End Namespace
