Namespace Aggregates
	Partial Class HrUnit
		Inherits GenericBaseAggregate(Of DataAccess.IHrUnit, DataAccess.HrUnit)
		
Public Function GetInstance(hrUnitId As Integer		) As Entities.HrUnit
			Dim retObj As New Entities.HrUnit
			retObj.Id = hrUnitId
			'Check if me.CurrentUser is allowed to this
			Repository.GetEntity(Me.DbName, retObj)
			Return retObj 
		End Function
		Public Function GetAllInstances() As Entities.HrUnitCollection
			Dim retObj As New HrUnitCollection
			'Check if me.CurrentUser is allowed to this
			Repository.GetAll(Me.DbName, retObj)
			Return retObj
		End Function
		Public Function CreateInstance(ByVal o As Entities.HrUnit) As Boolean
			'Validate the object
			'Check if me.CurrentUser is allowed to this
			'Setting default values for the object
			SetDefaultValue(o, "Inactive",0 )
			SetDefaultValue(o, "Guid",Guid.NewGuid() )
			SetDefaultValue(o, "Created",DateTime.Now() )
			SetDefaultValue(o, "Updated",DateTime.Now() )
			SetDefaultValue(o, "SortOrder",0 )
			ValidateCreateEntity(o)
			Return Repository.Create(Me.DbName, o)
		End Function
		Public Function UpdateInstance(ByVal o As Entities.HrUnit) As Boolean
			'Check if me.CurrentUser is allowed to this
			'Validate the object
			Me.ValidateUpdateEntity(o)
			Return Repository.Update(DbName, o)
		End Function
		Public Function DeleteInstance(ByVal o As Entities.HrUnit ) as Boolean
			'Check if me.CurrentUser is allowed to this
			'Validate the object
			Me.ValidateDeleteEntity(o)
			Return Repository.Delete(Me.DbName, o)
		End Function
	End Class
End Namespace
