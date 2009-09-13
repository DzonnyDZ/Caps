

Imports Tools, Tools.ExtensionsT, Tools.TypeTools
Imports System.ComponentModel

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

''' <summary>Converts image is of given type to path to that image</summary>
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
                Case "D"c : Return My.Resources.txt_ImageDrawing
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



