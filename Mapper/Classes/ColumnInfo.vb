Imports Microsoft.SqlServer.Management
Imports Microsoft.SqlServer.Management.Smo

Public Class ColumnInfo
    Private ReadOnly _Column As Column
    Private Property Ls As ILanguageSettings

    Public Sub New()

    End Sub

    Public Sub New(ByVal column As Column, ls As ILanguageSettings)
        _Column = Column
        Me.Ls = ls
        Name = Column.Name
        NameToUpper = Column.Name.ToUpper
        Nullable = Column.Nullable
        DataType = Column.DataType.ToString

        PartOfPrimaryKey = Column.InPrimaryKey

        Identity = Column.Identity
        Length = Column.DataType.MaximumLength
        IsForeignKey = Column.IsForeignKey
        ParamName = Column.Name(0).ToString.ToLower & Mid(Column.Name, 2)
        DefaultValue = If(Column.DefaultConstraint Is Nothing, "", Column.DefaultConstraint.Text)
    End Sub
    
    Property PartOfPrimaryKey As Boolean
    Property IsForeignKey As Boolean
    Property Identity As Boolean
    Property Name As String
    Property NameToUpper As String
    Property DataType As String
    Property Nullable As Boolean
    Property Length As Integer
    Property ParamName As String

    ReadOnly Property NetRuntimeType As String
        Get
            Try
                Return Ls.DataTypes(Me.DataType.ToLower).LanguageType
            Catch ex As Exception
                Return Me.DataType & " ble ikke funnet i LanguageSettings"
            End Try
        End Get
    End Property

    ReadOnly Property IsValueType As Boolean
        Get
            If NetRuntimeType.ToLower = "string" OrElse Me.DbType.ToLower = "binary" Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Property DefaultValue As String

    ReadOnly Property DefaultValueCode As String
        Get
            Try
                If String.IsNullOrEmpty(Me.DefaultValue) Then Return ""
                For Each i In Ls.DefaultValues
                    If i.Match.IsMatch(DefaultValue) AndAlso String.Compare(i.DbType.ToString, DataType, True) = 0 Then
                        Return i.Match.Replace(DefaultValue, i.Replace)
                    End If
                Next
            Catch ex As Exception
                Return Me.DefaultValue & " ble ikke funnet i LanguageSettings"
            End Try

            Return Me.DefaultValue & " ble ikke funnet i LanguageSettings"
        End Get
    End Property

    ReadOnly Property DbType As String
        Get
            Try
                Return Ls.DataTypes(DataType).DbType
            Catch ex As Exception
                Return "NOT IMPLEMENTED:" & DataType
            End Try
        End Get
        
    End Property





End Class
