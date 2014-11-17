Namespace Entities
	Partial Class HrUnit
		Inherits LazyBaseclass
		Private Readonly _fields as String() ={"Id","ParentId","UnitTypeId","Number","Name","Description","Inactive","Guid","Created","Updated","SortOrder"}  
		Public Overrides Readonly Property Fields as String()
			Get
				Return _fields
			End Get
		End Property
		Private _id as Integer
		Private _parentId as Integer?
		Private _unitTypeId as Integer
		Private _number as string
		Private _name as string
		Private _description as string
		Private _inactive as Boolean
		Private _guid as GUID
		Private _created as DateTime
		Private _updated as DateTime
		Private _sortOrder as Integer
		Public Property Id As Integer
			Get
				Return _id
			End Get
			Set(value as Integer)
				if _id <> value Then
					Dirty = True
					AddChangedValue("Id",value)
					_id = value
				End If
			End Set
		End Property
		Public Property ParentId As Integer?
			Get
				Return _parentId
			End Get
			Set(value as Integer?)
				If _parentId Is Nothing AndAlso value Is Nothing Then Return
				If _parentId <> value OrElse (_parentId <> value) Is Nothing Then
					Dirty = True
					AddChangedValue("ParentId",value)
					_parentId = value
				End If
			End Set
		End Property
		Public Property UnitTypeId As Integer
			Get
				Return _unitTypeId
			End Get
			Set(value as Integer)
				if _unitTypeId <> value Then
					Dirty = True
					AddChangedValue("UnitTypeId",value)
					_unitTypeId = value
				End If
			End Set
		End Property
		Public Property Number As string
			Get
				Return _number
			End Get
			Set(value as string)
				if _number <> value Then
					Dirty = True
					AddChangedValue("Number",value)
					_number = value
				End If
			End Set
		End Property
		Public Property Name As string
			Get
				Return _name
			End Get
			Set(value as string)
				if _name <> value Then
					Dirty = True
					AddChangedValue("Name",value)
					_name = value
				End If
			End Set
		End Property
		Public Property Description As string
			Get
				Return _description
			End Get
			Set(value as string)
				if _description <> value Then
					Dirty = True
					AddChangedValue("Description",value)
					_description = value
				End If
			End Set
		End Property
		Public Property Inactive As Boolean
			Get
				Return _inactive
			End Get
			Set(value as Boolean)
				if _inactive <> value Then
					Dirty = True
					AddChangedValue("Inactive",value)
					_inactive = value
				End If
			End Set
		End Property
		Public Property Guid As GUID
			Get
				Return _guid
			End Get
			Set(value as GUID)
				if _guid <> value Then
					Dirty = True
					AddChangedValue("Guid",value)
					_guid = value
				End If
			End Set
		End Property
		Public Property Created As DateTime
			Get
				Return _created
			End Get
			Set(value as DateTime)
				if _created <> value Then
					Dirty = True
					AddChangedValue("Created",value)
					_created = value
				End If
			End Set
		End Property
		Public Property Updated As DateTime
			Get
				Return _updated
			End Get
			Set(value as DateTime)
				if _updated <> value Then
					Dirty = True
					AddChangedValue("Updated",value)
					_updated = value
				End If
			End Set
		End Property
		Public Property SortOrder As Integer
			Get
				Return _sortOrder
			End Get
			Set(value as Integer)
				if _sortOrder <> value Then
					Dirty = True
					AddChangedValue("SortOrder",value)
					_sortOrder = value
				End If
			End Set
		End Property
		Public Overrides Function GetValue(ByVal fieldName As String, ByRef value As Object) As Boolean
			Select Case fieldName.ToUpper
				Case "ID"
					value = _id
				
				Case "PARENTID"
					If _parentId IsNot Nothing AndAlso _parentId.HasValue Then
						value = _parentId.Value
					Else
						value = DBNull.Value
					End If
				
				Case "UNITTYPEID"
					value = _unitTypeId
				
				Case "NUMBER"
					If _number IsNot nothing then
						value = _number
					Else
					value = DbNull.value
					End If
				
				Case "NAME"
					If _name IsNot nothing then
						value = _name
					Else
					value = DbNull.value
					End If
				
				Case "DESCRIPTION"
					If _description IsNot nothing then
						value = _description
					Else
					value = DbNull.value
					End If
				
				Case "INACTIVE"
					value = _inactive
				
				Case "GUID"
					value = _guid
				
				Case "CREATED"
					value = _created
				
				Case "UPDATED"
					value = _updated
				
				Case "SORTORDER"
					value = _sortOrder
				
				Case Else
					Return False
				End Select
				Return True
			End Function
			Public Overrides Function SetValue(ByVal fieldName As String, ByVal value As Object) As Boolean
				If Not value Is Nothing And Not IsDBNull(value) Then
					Select Case fieldName.ToUpper
						Case "ID"
							_id = Ctype(value,Integer)
						
						Case "PARENTID"
							_parentId = Ctype(value,Integer)
						
						Case "UNITTYPEID"
							_unitTypeId = Ctype(value,Integer)
						
						Case "NUMBER"
							_number = Ctype(value,string)
						
						Case "NAME"
							_name = Ctype(value,string)
						
						Case "DESCRIPTION"
							_description = Ctype(value,string)
						
						Case "INACTIVE"
							_inactive = Ctype(value,Boolean)
						
						Case "GUID"
							_guid = Ctype(value,GUID)
						
						Case "CREATED"
							_created = Ctype(value,DateTime)
						
						Case "UPDATED"
							_updated = Ctype(value,DateTime)
						
						Case "SORTORDER"
							_sortOrder = Ctype(value,Integer)
						
						Case Else
							Return False
						
					End Select
					Return True
				End If
				Return True
			End Function
		End Class
	End Namespace
