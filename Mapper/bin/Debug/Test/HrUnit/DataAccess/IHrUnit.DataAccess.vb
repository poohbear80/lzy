Namespace DataAccess
	Public Interface IHrUnit
		Function GetEntity(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean
		Function GetAll(ByVal sourceName As String, ByRef o As Entities.HrUnitCollection) As Boolean
		Function Create(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean
		Function Update(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean
		Function Delete(ByVal sourceName As String, ByRef o As Entities.HrUnit) As Boolean
	End Interface
End Namespace
