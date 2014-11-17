Imports Microsoft.SqlServer.Management
Imports Microsoft.SqlServer.Management.Smo

Public Class ColumnInfo
    Private Property Ls As ILanguageSettings

    Public Sub New()

    End Sub

    Public Sub New(ByVal c As Column, ls As ILanguageSettings)
        Me.Ls = ls
        Name = c.Name
        NameToUpper = c.Name.ToUpper
        Nullable = c.Nullable
        DataType = c.DataType.ToString

        PartOfPrimaryKey = c.InPrimaryKey

        Identity = c.Identity
        Length = c.DataType.MaximumLength
        IsForeignKey = c.IsForeignKey
        ParamName = c.Name(0).ToString.ToLower & Mid(c.Name, 2)
        DefaultValue = If(c.DefaultConstraint Is Nothing, "", c.DefaultConstraint.Text)
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

    Property NetRuntimeType As String
        Get
            Try
                Return Ls.DataTypes(Me.DataType.ToLower).LanguageType
            Catch ex As Exception
                Return Me.DataType & " ble ikke funnet i LanguageSettings"
            End Try
        End Get
        Set(value As String)

        End Set
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

    Property DefaultValueCode As String
        Get
            Try
                If String.IsNullOrEmpty(Me.DefaultValue) Then Return ""
                Dim val = Me.DefaultValue.Replace("(", "").Replace(")", "")
                Return Ls.DefaultValue(val)
            Catch ex As Exception
                Return Me.DefaultValue & " ble ikke funnet i LanguageSettings"
            End Try
        End Get
        Set(value As String)

        End Set
    End Property

    Property DbType As String
        Get
            Try
                Return Ls.DataTypes(DataType).DbType
            Catch ex As Exception
                Return "NOT IMPLEMENTED:" & DataType
            End Try
        End Get
        Set(value As String)

        End Set
    End Property





End Class
