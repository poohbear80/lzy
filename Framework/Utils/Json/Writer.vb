Imports System.IO

Namespace Utils.Json
    Public Class Writer

        Public Shared Formatters As New Dictionary(Of Type, Writer) From {
            {GetType(Integer), Sub(w, val) w.write(val.ToString)},
            {GetType(Long), Sub(w, val) w.write(val.ToString)},
            {GetType(UInteger), Sub(w, val) w.write(val.ToString)},
            {GetType(ULong), Sub(w, val) w.write(val.ToString)},
            {GetType(Double), AddressOf WriteNumber},
            {GetType(Single), AddressOf WriteNumber},
            {GetType(String), AddressOf Writetext},
            {GetType(Decimal), AddressOf WriteNumber},
            {GetType(Date), Sub(w, val) w.write(val.ToString)}
        }

        Public Delegate Sub Writer(writer As StreamWriter, value As Object)

        Public Shared Function ObjectToString(o As Object) As String
            Return ObjectToString(New JSonConfig, o)
            'Dim result = New StreamWriter(New MemoryStream, Text.Encoding.UTF8)
            'ObjectToString(result, o)
            'result.Flush()
            'result.BaseStream.Position = 0
            'Return New StreamReader(result.BaseStream).ReadToEnd
        End Function

        Public Shared Function ObjectToString(config As JSonConfig, o As Object) As String
            Dim result = New StreamWriter(New MemoryStream, Text.Encoding.UTF8)
            ObjectToString(result, o)
            result.Flush()
            result.BaseStream.Position = 0
            Return New StreamReader(result.BaseStream).ReadToEnd
        End Function

        Public Shared Function Config() As JSonConfig
            Return New JSonConfig
        End Function

        Private Shared Sub ObjectToString(result As StreamWriter, o As Object)

            If Formatters.ContainsKey(o.GetType) Then
                Formatters(o.GetType)(result, o)
                Return
            End If

            If TypeOf (o) Is IEnumerable Then
                WriteList(result, o)
            Else
                WriteObject(result, o)
            End If
        End Sub


        Private Shared Sub WriteObject(result As StreamWriter, o As Object)
            Dim first As Boolean = True
            result.Write("{"c)
            For Each member In o.GetType().GetMembers(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance).Where(Function(v) v.MemberType = Reflection.MemberTypes.Field Or v.MemberType = Reflection.MemberTypes.Property)

                If Not first Then
                    result.Write(",")
                End If

                result.Write(Chr(&H22))
                result.Write(member.Name)
                result.Write(Chr(&H22))
                result.Write(":"c)

                Select Case member.MemberType
                    Case Reflection.MemberTypes.Field
                        Dim fld = o.GetType.GetField(member.Name)
                        WriteValue(result, fld.FieldType, fld.GetValue(o))
                    Case Reflection.MemberTypes.Property
                        Dim prop = o.GetType.GetProperty(member.Name)
                        WriteValue(result, prop.PropertyType, prop.GetValue(o))
                End Select


                first = False
            Next
            result.Write("}"c)
        End Sub

        Private Shared Sub WriteList(result As StreamWriter, o As Object)
            Dim first As Boolean = True
            result.Write("[")
            For Each element In CType(o, IEnumerable)
                If Not first Then
                    result.Write(","c)
                End If
                ObjectToString(result, element)
                first = False
            Next
            result.Write("]")
        End Sub

        Private Shared Sub WriteValue(writer As StreamWriter, t As System.Type, value As Object)

            If value Is Nothing Then
                writer.Write("null")
                Return
            End If

            If Formatters.ContainsKey(t) Then
                Formatters(t)(writer, value)
            Else
                If value.GetType.IsValueType Then
                    If value.GetType.GetMembers(System.Reflection.BindingFlags.DeclaredOnly Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance).Count > 0 Then
                        ObjectToString(writer, value)
                    Else
                        writer.Write(value.ToString)
                    End If
                Else
                    ObjectToString(writer, value)
                End If
            End If
        End Sub





#Region "WriteText"
        Private Shared ReadOnly ToEscape As Integer() = {&H22, &H2, &H5C}
        Private Shared Translate As New Dictionary(Of Integer, String) From {
            {&H9, "\t"}, {&HA, "\n"}, {&HC, "\f"}, {&HD, "\r"}}
        Private Shared Sub Writetext(writer As StreamWriter, value As Object)
            writer.Write(Chr(&H22))
            For Each c In value.ToString
                If ToEscape.Contains(Strings.AscW(c)) Then
                    writer.Write("\")
                End If
                If Translate.ContainsKey(AscW(c)) Then
                    writer.Write(Translate(AscW(c)))
                Else
                    writer.Write(c)
                End If
            Next
            writer.Write(Chr(&H22))
        End Sub
#End Region

        Private Shared Sub WriteNumber(w As StreamWriter, val As Object)
            w.Write(val.ToString.Replace(","c, "."))
        End Sub


    End Class

    Public Class JSonConfig

        Public Function ObjectToString(value As Object) As String
            Writer.ObjectToString(Me, value)            
        End Function


        Function FormatDate(format As ToString(Of Date)) As JSonConfig

            Return Me
        End Function
    End Class

    Public Delegate Function ToString(Of T)(value As T) As String


    Public Interface IFormatValue(Of T)
        Function ToString(value As T) As String
        Function ToInstance(value As String) As T
    End Interface

    Public Class DateFormatter
        Implements IFormatValue(Of Date)

        Public Function ToInstance(value As String) As Date Implements IFormatValue(Of Date).ToInstance

        End Function

        Public Function ToString1(value As Date) As String Implements IFormatValue(Of Date).ToString

        End Function
    End Class

End Namespace
