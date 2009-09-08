Imports Tools, Tools.ExtensionsT
Public Class Converters
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If targetType.IsAssignableFrom(value.GetType) Then Return value
        Return Nothing
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        If targetType.IsAssignableFrom(value.GetType) Then Return value
        Return Nothing
    End Function

End Class

Public Class PlusConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return ConvertInternal(value, targetType, parameter, False)
    End Function

    Private Function ConvertInternal(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal minus As Boolean) As Object
        If parameter Is Nothing Then Return Tools.DynamicCast(value, targetType)
        Dim ret As Object
        If value Is Nothing Then Throw New ArgumentNullException("value")
        If TypeOf value Is Byte Then
            If minus Then ret = DirectCast(value, Byte) - TypeTools.DynamicCast(Of Byte)(parameter) Else ret = DirectCast(value, Byte) + TypeTools.DynamicCast(Of Byte)(parameter)
        ElseIf TypeOf value Is SByte Then
            If minus Then ret = DirectCast(value, SByte) - TypeTools.DynamicCast(Of SByte)(parameter) Else ret = DirectCast(value, SByte) + TypeTools.DynamicCast(Of SByte)(parameter)
        ElseIf TypeOf value Is UShort Then
            If minus Then ret = DirectCast(value, UShort) - TypeTools.DynamicCast(Of UShort)(parameter) Else ret = DirectCast(value, UShort) + TypeTools.DynamicCast(Of UShort)(parameter)
        ElseIf TypeOf value Is Short Then
            If minus Then ret = DirectCast(value, Short) - TypeTools.DynamicCast(Of Short)(parameter) Else ret = DirectCast(value, Short) + TypeTools.DynamicCast(Of Short)(parameter)
        ElseIf TypeOf value Is UInteger Then
            If minus Then ret = DirectCast(value, UInteger) - TypeTools.DynamicCast(Of UInteger)(parameter) Else ret = DirectCast(value, UInteger) + TypeTools.DynamicCast(Of UInteger)(parameter)
        ElseIf TypeOf value Is Integer Then
            If minus Then ret = DirectCast(value, Integer) - TypeTools.DynamicCast(Of Integer)(parameter) Else ret = DirectCast(value, Integer) + TypeTools.DynamicCast(Of Integer)(parameter)
        ElseIf TypeOf value Is ULong Then
            If minus Then ret = DirectCast(value, ULong) - TypeTools.DynamicCast(Of ULong)(parameter) Else ret = DirectCast(value, ULong) + TypeTools.DynamicCast(Of ULong)(parameter)
        ElseIf TypeOf value Is Long Then
            If minus Then ret = DirectCast(value, Long) - TypeTools.DynamicCast(Of Long)(parameter) Else ret = DirectCast(value, Long) + TypeTools.DynamicCast(Of Long)(parameter)
        ElseIf TypeOf value Is Decimal Then
            If minus Then ret = DirectCast(value, Decimal) - TypeTools.DynamicCast(Of Decimal)(parameter) Else ret = DirectCast(value, Decimal) + TypeTools.DynamicCast(Of Decimal)(parameter)
        ElseIf TypeOf value Is Single Then
            If minus Then ret = DirectCast(value, Single) - TypeTools.DynamicCast(Of Single)(parameter) Else ret = DirectCast(value, Single) + TypeTools.DynamicCast(Of Single)(parameter)
        ElseIf TypeOf value Is Double Then
            If minus Then ret = DirectCast(value, Double) - TypeTools.DynamicCast(Of Double)(parameter) Else ret = DirectCast(value, Double) + TypeTools.DynamicCast(Of Double)(parameter)
        Else
            Throw New NotSupportedException(My.Resources.ex_UnsupportedDataType.f(value.GetType.Name))
        End If
        Return TypeTools.DynamicCast(ret, targetType)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return ConvertInternal(value, targetType, parameter, True)
    End Function
End Class

''' <summary>Converter that converts null values to false and non-null values to true.</summary>
''' <remarks>Additionally if targetType is <see cref="Visibility"/> it converts null to <see cref="Visibility.Collapsed"/> and non-null to <see cref="Visibility.Visible"/>.</remarks>
Public Class NullFalseConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If targetType.IsAssignableFrom(GetType(Boolean)) Then
            Return value IsNot Nothing
        ElseIf targetType.IsAssignableFrom(GetType(Visibility)) Then
            Return If(value Is Nothing, Visibility.Collapsed, Visibility.Visible)
        Else
            Throw New NotSupportedException(My.Resources.ex_ConvertsOnlyToBool.f(Me.GetType.Name))
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If TypeOf value Is Boolean AndAlso DirectCast(value, Boolean) = False Then Return Nothing
        Throw New NotSupportedException(My.Resources.ex_CanConvertBackOnlyFromFalse.f(Me.GetType.Name))
    End Function
End Class

Public Class FileNameConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        Return IO.Path.GetFileName(value.ToString)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class

Public Class RelativePathConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Dim path = If(parameter, Environment.CurrentDirectory).ToString
        If (IO.Path.IsPathRooted(value.ToString)) Then Return value
        Return IO.Path.Combine(path, value)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.[GetType].Name))
    End Function
End Class
''' <summary>Converts relative path of caps image to absolute path to image of given size</summary>
Public Class CapImageConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Dim folders$()
        If parameter Is Nothing Then
            folders = New String() {"original", "256_256", "64_64"}
        ElseIf TypeOf parameter Is Integer AndAlso DirectCast(parameter, Integer) = 64 Then
            folders = New String() {"64_64", "256_256", "original"}
        ElseIf TypeOf parameter Is Integer AndAlso DirectCast(parameter, Integer) = 256 Then
            folders = New String() {"256_256", "64_64", "original"}
        Else
            Throw New ArgumentException(My.Resources.ex_CapImageConverterParameter, "parameter")
        End If
        For Each folder In folders
            Dim path = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, folder), value)
            If IO.File.Exists(path) Then Return path
        Next
        Return IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "original"), value)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Return IO.Path.GetFileName(value)
    End Function
End Class

Public Class MyImageIDConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, parameter.ToString), String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", value) & ".png")
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Integer.Parse(IO.Path.GetFileNameWithoutExtension(value), System.Globalization.CultureInfo.InvariantCulture)
    End Function
End Class

''' <summary>Converts picture type code to description</summary>
Public Class PictureTypeConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If TypeOf value Is String Then
            If DirectCast(value, String) = "" Then Throw New ArgumentException(My.Resources.ex_ImgTypeCodeEmpty, "value")
            value = DirectCast(value, String)(0)
        End If
        If TypeOf value Is Char? AndAlso DirectCast(value, Char?).HasValue Then value = DirectCast(value, Char?).Value
        If (TypeOf value Is Char? AndAlso Not DirectCast(value, Char?).HasValue) OrElse value Is Nothing Then Return Nothing
        If TypeOf value Is Char Then
            Select Case DirectCast(value, Char)
                Case "G"c : Return My.Resources.txt_ImageGeometry
                Case "L"c : Return My.Resources.txt_ImageLogo
                Case "O"c : Return My.Resources.txt_ImageDrawing
                Case "P"c : Return My.Resources.txt_ImagePhoto
                Case Else : Throw New ArgumentException(My.Resources.ex_UnknownImageType.f(value), "value")
            End Select
        End If
        Throw New TypeMismatchException("value", value, GetType(Char), My.Resources.ex_ExpectedValueOfType1Or2.f(GetType(PictureTypeConverter), GetType(Char).Name, GetType(String).Name))
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class

''' <summary>Converts value of type <see cref="Int32"/> to <see cref="Color"/></summary>
Public Class IntColorConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">The converter parameter to use.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        Dim val As Int32
        If TypeOf value Is Int32 Then : val = DirectCast(value, Int32)
        ElseIf TypeOf value Is Int16 Then : val = DirectCast(value, Int16)
        ElseIf TypeOf value Is Int64 Then : val = DirectCast(value, Int64)
        ElseIf TypeOf value Is UInt32 Then : val = DirectCast(value, UInt32)
        ElseIf TypeOf value Is UInt16 Then : val = DirectCast(value, UInt16)
        ElseIf TypeOf value Is UInt64 Then : val = DirectCast(value, UInt64)
        ElseIf TypeOf value Is Byte Then : val = DirectCast(value, Byte)
        ElseIf TypeOf value Is SByte Then : val = DirectCast(value, SByte)
        ElseIf TypeOf value Is Decimal Then : val = DirectCast(value, Decimal)
        ElseIf TypeOf value Is Double Then : val = DirectCast(value, Double)
        ElseIf TypeOf value Is Single Then : val = DirectCast(value, Single)
        ElseIf TypeOf value Is System.Drawing.Color Then : val = DirectCast(value, System.Drawing.Color).ToArgb
        ElseIf TypeOf value Is Color Then : val = DirectCast(value, Color).ToArgb
        Else : Throw New ArgumentNullException(My.Resources.ex_ConvertOnlyFromNumericAndColors)
        End If
        If targetType.Equals(GetType(System.Drawing.Color)) Then Return System.Drawing.Color.FromArgb(val)
        Return System.Drawing.Color.FromArgb(val).ToColor
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value that is produced by the binding target.</param>
    ''' <param name="targetType">The type to convert to.</param>
    ''' <param name="parameter">The converter parameter to use.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        Dim val As Color
        If TypeOf value Is Color Then : value = DirectCast(val, Color)
        ElseIf TypeOf value Is System.Drawing.Color Then : value = DirectCast(value, System.Drawing.Color).ToColor
        Else : Throw New TypeMismatchException("value", value, GetType(Color), My.Resources.ex_CamConvertBackOnlyFromColors.f(Me.GetType.Name, GetType(Color).FullName, GetType(System.Drawing.Color).FullName))
        End If
        If targetType.Equals(GetType(Color)) Then : Return val
        ElseIf targetType.Equals(GetType(System.Drawing.Color)) Then : Return val.ToColor
        Else : Return val.ToArgb
        End If
    End Function

End Class
''' <summary>Converter that test if value being converter equals to parameter</summary>
Public Class CompareConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return parameter Is Nothing
        If parameter Is Nothing Then Return value Is Nothing
        Return value.Equals(parameter)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class