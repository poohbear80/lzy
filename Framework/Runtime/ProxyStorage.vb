Namespace Runtime
    Public Class ProxyStorage(Of TKey, TValue)
        Inherits Dictionary(Of TKey, TValue)

        Private ReadOnly _Origin As Dictionary(Of TKey, TValue)

        Public Sub New(origin As Dictionary(Of TKey, TValue))
            _Origin = origin
        End Sub

        Public Overloads Function ContainsKey(key As TKey) As Boolean
            If MyBase.ContainsKey(key) Then
                Return True
            Else
                Return _Origin.ContainsKey(key)
            End If
        End Function

        Default Public Overloads Property Item(key As TKey) As TValue
            Get
                If MyBase.ContainsKey(key) Then
                    Return MyBase.Item(key)
                Else
                    Return _Origin(key)
                End If
            End Get
            Set(value As TValue)
                MyBase.Item(key) = value
            End Set
        End Property


    End Class
End NameSpace