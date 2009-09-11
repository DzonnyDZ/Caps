Imports Tools, Tools.ExtensionsT, Tools.TypeTools
Imports System.ComponentModel

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
        If targetType.IsAssignableFrom(GetType(Brush)) Then Return New SolidColorBrush(System.Drawing.Color.FromArgb(val).ToColor)
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


''' <summary>Converter that combines two <see cref="IValueConverter">IValueConverters</see> - passes output of in ner converter to outer converter</summary>
Public Class NestedConverter
    Implements IValueConverter
    ''' <summary>Defines possible values used by the <see cref="ParameterRule"/> property to determine to which converter(s) pass converter parameter</summary>
    Public Enum ParameterRules
        ''' <summary>Converter parameter is passed to both, <see cref="Inner">Inner</see> and <see cref="Outer">Outer</see> converters</summary>
        Both
        ''' <summary>Converter parameter is passed to <see cref="Inner">Inner</see> converter only</summary>
        InnerOnly
        ''' <summary>Converter parameter is passed to <see cref="Outer">Outer</see> converter only</summary>
        OuterOnly
    End Enum
    ''' <summary>Contains value of the <see cref="ParameterRule"/> property</summary>
    Private _ParameterRule As ParameterRules = ParameterRules.Both
    ''' <summary>Contains value of the <see cref="Outer"/> property</summary>
    Private _Outer As IValueConverter
    ''' <summary>Contains value of the <see cref="Inner"/> property</summary>
    Private _Inner As IValueConverter
    ''' <summary>Contains value of the <see cref="IntermediateTargetType"/> property</summary>
    Private _IntermediateTargetType As Type = GetType(Object)
    ''' <summary>Contains value of the <see cref="IntermediateTargetTypeBack"/> property</summary>
    Private _IntermediateTargetTypeBack As Type = GetType(Object)
    ''' <summary>Contains value of the <see cref="InnerParam"/> property</summary>
    Private _InnerParam As Object
    ''' <summary>Conrains value of the <see cref="OuterParam"/> property</summary>
    Private _OuterParam As Object
    ''' <summary>CTor form inner and outer converters</summary>
    ''' <param name="Inner">Inner converter. This converter is called first and its output is passed to <paramref name="Outer"/> converter.</param>
    ''' <param name="Outer">Outer converter. This converter converts value returned from <paramref name="Inner"/> converter to output.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Inner"/> is null or <paramref name="Outer"/> is null</exception>
    Public Sub New(ByVal Inner As IValueConverter, ByVal Outer As IValueConverter)
        If Inner Is Nothing Then Throw New ArgumentNullException("Inner")
        If Outer Is Nothing Then Throw New ArgumentNullException("Outer")
        _Inner = Inner
        _Outer = Outer
    End Sub
    ''' <summary>Parameter-less CTor to be used by XAML</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Sub New()
    End Sub
    ''' <summary>CTor form inner and outer converters and intermediate target types</summary>
    ''' <param name="Inner">Inner converter. This converter is called first and its output is passed to <paramref name="Outer"/> converter.</param>
    ''' <param name="Outer">Outer converter. This converter converts value returned from <paramref name="Inner"/> converter to output.</param>
    ''' <param name="IntermediateTargetType">Target type of <paramref name="Inner"/> converter when <see cref="Convert"/> is called.</param>
    ''' <param name="ItntermediateTargetTypeBack">Target type of <paramref name="Outer"/> converter when <see cref="ConvertBack"/> is called. If null <see cref="System.Object"/> is assumed.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Inner"/>, <paramref name="Outer"/> or <paramref name="IntermediateTargetType"/> is null</exception>
    Public Sub New(ByVal Inner As IValueConverter, ByVal Outer As IValueConverter, ByVal IntermediateTargetType As Type, Optional ByVal ItntermediateTargetTypeBack As Type = Nothing)
        Me.New(Inner, Outer)
        If IntermediateTargetType Is Nothing Then Throw New ArgumentNullException("IntermediateTargetType")
        _IntermediateTargetType = IntermediateTargetType
        If ItntermediateTargetTypeBack IsNot Nothing Then _IntermediateTargetTypeBack = IntermediateTargetTypeBack
    End Sub
    ''' <summary>Inner converter</summary>
    ''' <remarks>
    ''' <para>When calling <see cref="Convert">Convert</see> this converter's <see cref="IValueConverter.Convert">Convert</see> is called firts and its result is passed to <see cref="IValueConverter.Convert">Convert</see> of <see cref="Outer">Outer</see> converter. When <see cref="Convert">Convert</see> is called, target type of this converter is <see cref="IntermediateTargetType">IntermediateTargetType</see>.</para>
    ''' <para>When calling <see cref="ConvertBack">ConvertBack</see> this converter is called with value returned by <see cref="Outer">Outer</see> converter. Tagret type is target type passed to <see cref="ConvertBack">ConvertBack</see>.</para>
    ''' </remarks>
    ''' <exception cref="ArgumentNullException">Value being set is null</exception>
    Public Property Inner() As IValueConverter
        <DebuggerStepThrough()> Get
            Return _Inner
        End Get
        Set(ByVal value As IValueConverter)
            If value Is Nothing Then Throw New ArgumentNullException("value")
            _Inner = value
        End Set
    End Property
    ''' <summary>Outer converter</summary>
    ''' <remarks>
    ''' <para>When calling <see cref="Convert">Convert</see> this converter is called to convert value returned by <see cref="Inner">Inner</see> converter to tagret type passed to <see cref="Convert">Convert</see>.</para>
    ''' <para>When calling <see cref="ConvertBack">ConvertBack</see> this converter is called to first with target type <see cref="IntermediateTargetTypeBack">IntermediateTargetTypeBack</see>. Return value of this converter's <see cref="IValueConverter.Convert">Convert</see> is passed to <see cref="IValueConverter.Convert">Convert</see> of <see cref="Inner">Inner</see> converter.</para>
    ''' </remarks>
    ''' <exception cref="ArgumentNullException">Value being set is null</exception>
    Public Property Outer() As IValueConverter
        <DebuggerStepThrough()> Get
            Return _Outer
        End Get
        Set(ByVal value As IValueConverter)
            If value Is Nothing Then Throw New ArgumentNullException("value")
            _Outer = value
        End Set
    End Property
    ''' <summary>Target type of <see cref="Inner">Inner</see> converter when <see cref="Convert"/> is called</summary>
    ''' <value>Default value is <see cref="System.Object"/></value>
    ''' <remarks>This property can be set only via constructor</remarks>
    ''' <exception cref="ArgumentNullException">Value being set is null</exception>
    <DefaultValue(GetType(Object))> _
    Public Property IntermediateTargetTypeBack() As Type
        <DebuggerStepThrough()> Get
            Return _IntermediateTargetTypeBack
        End Get
        Set(ByVal value As Type)
            If value Is Nothing Then Throw New ArgumentNullException("value")
            _IntermediateTargetTypeBack = value
        End Set
    End Property
    ''' <summary>Target type of <see cref="Outer">Outer</see> converter when <see cref="ConvertBack"/> is called</summary>
    ''' <value>Default value is <see cref="System.Object"/></value>
    ''' <remarks>This property can be set only via constructor</remarks>
    ''' <exception cref="ArgumentNullException">Value being set is null</exception>
    <DefaultValue(GetType(Object))> _
    Public Property IntermediateTargetType() As Type
        <DebuggerStepThrough()> Get
            Return _IntermediateTargetType
        End Get
        Set(ByVal value As Type)
            If value Is Nothing Then Throw New ArgumentNullException("value")
            _IntermediateTargetType = value
        End Set
    End Property
    ''' <summary>Gets or sets value to be passed to converter parameter of <see cref="Inner">Inner</see> converter instead of value passed to <see cref="Convert">Convert</see> or <see cref="ConvertBack">ConvertBack.</see></summary>
    ''' <value>Value to be passed to converter parameter of <see cref="IValueConverter.Convert">Convert</see> or <see cref="IValueConverter.ConvertBack">ConvertBack</see> of <see cref="Inner">Inner</see> converter. Null to obey <see cref="ParameterRule"/>.</value>
    ''' <returns>Value being passed to converter parameter of <see cref="IValueConverter.Convert">Convert</see> and <see cref="IValueConverter.ConvertBack">ConvertBack</see> of <see cref="Inner">Inner</see> converter. Null when <see cref="ParameterRule"/> shall determine value passed to the methods.</returns>
    Public Property InnerParam() As Object
        <DebuggerStepThrough()> Get
            Return _InnerParam
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Object)
            _InnerParam = value
        End Set
    End Property
    ''' <summary>Gets or sets value to be passed to converter parameter of <see cref="Outer">Outer</see> converter instead of value passed to <see cref="Convert">Convert</see> or <see cref="ConvertBack">ConvertBack.</see></summary>
    ''' <value>Value to be passed to converter parameter of <see cref="IValueConverter.Convert">Convert</see> or <see cref="IValueConverter.ConvertBack">ConvertBack</see> of <see cref="Outer">Outer</see> converter. Null to obey <see cref="ParameterRule"/>.</value>
    ''' <returns>Value being passed to converter parameter of <see cref="IValueConverter.Convert">Convert</see> and <see cref="IValueConverter.ConvertBack">ConvertBack</see> of <see cref="Outer">Outer</see> converter. Null when <see cref="ParameterRule"/> shall determine value passed to the methods.</returns>
    Public Property OuterParam() As Object
        <DebuggerStepThrough()> Get
            Return _OuterParam
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Object)
            _OuterParam = value
        End Set
    End Property
    ''' <summary>Gets or sets rule used to determine which converter(s) converter parameter is passed to when <see cref="Convert"/> or <see cref="ConvertBack"/> is called</summary>
    ''' <value>Rule to be used by <see cref="Convert"/> and <see cref="ConvertBack"/> to determine which converter(s) pass converter parameter to. Default value is <see cref="ParameterRules.Both"/></value>
    ''' <returns>RUle currently used by <see cref="Convert"/> and <see cref="ConvertBack"/> to determine which converter(s) pass converter parameter to.</returns>
    ''' <remarks>Converter which is exluded by the rule from receiving converter parameter to its <see cref="IValueConverter.Convert">Convert</see> or <see cref="IValueConverter.ConvertBack">ConvertBack</see> receives null insted.
    ''' <para>This rule can be overriden by setting converter parameter value directly using <see cref="InnerParam"/> and <see cref="OuterParam"/> properties.</para></remarks>
    ''' <exception cref="InvalidEnumArgumentException">Value being set is not one of <see cref="ParameterRules"/> values.</exception>
    Public Property ParameterRule() As ParameterRules
        <DebuggerStepThrough()> Get
            Return _ParameterRule
        End Get
        Set(ByVal value As ParameterRules)
            If Not value.IsDefined Then Throw New InvalidEnumArgumentException("value", value, value.GetType)
            _ParameterRule = value
        End Set
    End Property
    ''' <summary>Converts a value using <see cref="IValueConverter.Convert"/> of <see cref="Inner">Inner</see> converter first and then of <see cref="Outer">Outer</see> one.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">The converter parameter to use. <see cref="ParameterRule"/> determines which converter(s) receive this value.</param>
    ''' <param name="culture">The culture to use in converters.</param>
    ''' <exception cref="InvalidOperationException"><see cref="Inner"/> or <see cref="Outer"/> is null</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If Inner Is Nothing Then Throw New InvalidOperationException(My.Resources.ex_ValueMustBeSetPriorCalling.f("Inner", "Convert"))
        If Outer Is Nothing Then Throw New InvalidOperationException(My.Resources.ex_ValueMustBeSetPriorCalling.f("Outer", "Convert"))
        Return Outer.Convert(Inner.Convert(value, IntermediateTargetType, If(InnerParam IsNot Nothing, InnerParam, If(ParameterRule <> ParameterRules.OuterOnly, parameter, Nothing)), culture), targetType, If(OuterParam IsNot Nothing, OuterParam, If(ParameterRule <> ParameterRules.InnerOnly, parameter, Nothing)), culture)
    End Function

    ''' <summary>Converts a value using <see cref="IValueConverter.ConvertBack"/> of <see cref="Outer">Outer</see> converter first and then of <see cref="Inner">Inner</see> one.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value that is produced by the binding target.</param>
    ''' <param name="targetType">The type to convert to.</param>
    ''' <param name="parameter">The converter parameter to use. <see cref="ParameterRule"/> determines which converter(s) receive this value.</param>
    ''' <param name="culture">The culture to use in converters.</param>
    ''' <exception cref="InvalidOperationException"><see cref="Inner"/> or <see cref="Outer"/> is null</exception>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If Inner Is Nothing Then Throw New InvalidOperationException(My.Resources.ex_ValueMustBeSetPriorCalling.f("Inner", "ConvertBack"))
        If Outer Is Nothing Then Throw New InvalidOperationException(My.Resources.ex_ValueMustBeSetPriorCalling.f("Outer", "ConvertBack"))
        Return Inner.ConvertBack(Outer.Convert(value, IntermediateTargetTypeBack, If(OuterParam IsNot Nothing, OuterParam, If(ParameterRule <> ParameterRules.InnerOnly, parameter, Nothing)), culture), targetType, If(InnerParam IsNot Nothing, InnerParam, If(ParameterRule <> ParameterRules.OuterOnly, parameter, Nothing)), culture)
    End Function
End Class


''' <summary>Converter tha converts <see cref="Boolean"/> to <see cref="Visibility"/></summary>
Public Class BooleanVisibilityConverter
    Implements IValueConverter

    ''' <summary>                    Converts a value.                 </summary>
    ''' <returns>                    A converted value. If the method returns null, the valid null value is used.                </returns>
    ''' <param name="value">                    The value produced by the binding source.                </param>
    ''' <param name="targetType">                    The type of the binding target property.                </param>
    ''' <param name="parameter">                    The converter parameter to use. If string "!" converter negates <paramref name="value"/> first.               </param>
    ''' <param name="culture">                    The culture to use in the converter.                </param>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not <see cref="Boolean"/> or <paramref name="targetType"/> is not <see cref="Visibility"/> or one of its base types.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If TypeOf value Is Boolean AndAlso targetType.IsAssignableFrom(GetType(Visibility)) Then
            If If(TypeOf parameter Is String AndAlso DirectCast(parameter, String) = "!", Not DirectCast(value, Boolean), DirectCast(value, Boolean)) Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End If
        Throw New NotSupportedException(My.Resources.ex_ConverterCanConvertOnlyFromTo.f(Me.GetType.Name, GetType(Boolean).Name, GetType(Visibility).Name))
    End Function

    ''' <summary>                    Converts a value.                 </summary>
    ''' <returns>                    A converted value. If the method returns null, the valid null value is used.                </returns>
    ''' <param name="value">                    The value that is produced by the binding target.                </param>
    ''' <param name="targetType">                    The type to convert to.                </param>
    ''' <param name="parameter">                    The converter parameter to use. If string "!" converter negates return value.                </param>
    ''' <param name="culture">                    The culture to use in the converter.                </param>
    ''' <exception cref="ArgumentException"><paramref name="value"/> is  neither <see cref="Visibility.Collapsed"/> nor <see cref="Visibility.Visible"/></exception> 
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not <see cref="Visibility"/> or <paramref name="targetType"/> is not <see cref="Boolean"/> or one of its base types.</exception>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim ret As Boolean
        If TypeOf value Is Visibility AndAlso targetType.IsAssignableFrom(GetType(Boolean)) Then
            Select Case DirectCast(value, Visibility)
                Case Visibility.Collapsed : ret = False
                Case Visibility.Visible : ret = True
                Case Else : Throw New ArgumentException(My.Resources.ex_ConverterCannotConvertValueBack.f(Me.GetType, GetType(Visibility).Name, value))
            End Select
        Else
            Throw New NotSupportedException(My.Resources.ex_ConverterCanConvertBackOnlyFromTo.f(Me.GetType.Name, GetType(Visibility).Name, GetType(Boolean).Name))
        End If
        Return If(TypeOf parameter Is String AndAlso DirectCast(parameter, String) = "!", Not ret, ret)
    End Function
End Class

''' <summary>Converter that passes value to <see cref="String.Format"/></summary>
Public Class StringFormatConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If parameter Is Nothing AndAlso value Is Nothing Then Return ""
        If parameter Is Nothing Then Return value.ToString
        If Not TypeOf parameter Is String Then Throw New TypeMismatchException("parameter", parameter, GetType(String))
        Return String.Format(culture, DirectCast(parameter, String), value)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class
''' <summary>Comnverter of type <see cref="Visibility"/> that converts is to oposite value</summary>
Public Class NotVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert, System.Windows.Data.IValueConverter.ConvertBack
        Select Case DirectCast(value, Visibility)
            Case Visibility.Collapsed : Return Visibility.Visible
            Case Visibility.Visible : Return Visibility.Collapsed
            Case Else : Return value
        End Select
    End Function

End Class