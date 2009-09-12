Imports Tools, Tools.ExtensionsT, Tools.TypeTools
Imports System.ComponentModel

''' <summary>Converter thet returns value being converted if tagret type of conversion <see cref="Type.IsAssignableFrom">is assignable from</see> it, null otherwise.</summary>
Public Class SameTypeOrNullConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <paramref name="value"/> returns <paramref name="value"/>; null otherwise</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If targetType.IsAssignableFrom(value.GetType) Then Return value
        Return Nothing
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <paramref name="value"/> returns <paramref name="value"/>; null otherwise</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        If targetType.IsAssignableFrom(value.GetType) Then Return value
        Return Nothing
    End Function
End Class

''' <summary>Converter that adds value to a numeric value</summary>
Public Class PlusConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. <paramref name="value"/> + <paramref name="parameter"/>. If <paramref name="parameter"/> <paramref name="value"/> <see cref="TypeTools.DynamicCast">dynamicly casted</see> to <paramref name="targetType"/> is returned.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property. Result of addition <paramref name="value"/> + <paramref name="parameter"/> (where <paramref name="parameter"/> is firts <see cref="TypeTools.DynamicCast">dynamicly casted</see> to type of <paramref name="value"/>) must be <see cref="TypeTools.DynamicCast"/> dynamicly castable to this type.</param>
    ''' <param name="parameter">Value to add to <paramref name="value"/>. This value must be <see cref="TypeTools.DynamicCast">dynamicly castable</see> to type of <paramref name="value"/>.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="value"/> is null and <paramref name="parameter"/> is not null -or- <paramref name="targetType"/> is null.</exception>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not of supported type (see remarks for list of supporetd types)</exception>
    ''' <exception cref="InvalidCastException">Unable to cast <paramref name="parameter"/> to type of <paramref name="value"/> -or- unable to cast result of arithmetic operation to <paramref name="targetType"/>. See <see cref="TypeTools.DynamicCast"/> for more information.</exception>
    ''' <exception cref="Reflection.AmbiguousMatchException">Cast operators from <paramref name="parameter"/> to type of <paramref name="value"/> or from result of addition to <paramref name="targetType"/> were found, but none is more specific. See <see cref="TypeTools.DynamicCast"/> for more info.</exception>
    ''' <exception cref="OverflowException">Arithmetic operation or type conversion resulted in overflow.</exception>
    ''' <exception cref="Exception">Format exception when attempting to convert string to numeric value</exception>
    ''' <remarks>Supported types are:
    ''' <list type="bullet">
    ''' <item><see cref="Byte"/></item>
    ''' <item><see cref="SByte"/></item>
    ''' <item><see cref="UShort"/></item>
    ''' <item><see cref="Short"/></item>
    ''' <item><see cref="UInteger"/></item>
    ''' <item><see cref="Integer"/></item>
    ''' <item><see cref="ULong"/></item>
    ''' <item><see cref="Long"/></item>
    ''' <item><see cref="Decimal"/></item>
    ''' <item><see cref="Single"/></item>
    ''' <item><see cref="Double"/></item>
    ''' </list>Cross-type conversions are performed via <see cref="TypeTools.DynamicCast"/>. <paramref name="parameter"/> can be of any (even not specifically supported) type that can be <see cref="TypeTools.DynamicCast">dynamicaly casted</see> to type of <paramref name="value"/>. <paramref name="targetType"/> may be of any type result of arithmetic operation can be <see cref="TypeTools.DynamicCast"/> dynamically casted to. Arithmetic operation is performed using Visual Basic + and - operators.</remarks>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim oldC As Globalization.CultureInfo = Nothing
        If culture IsNot Nothing Then
            oldC = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = culture
        End If
        Try
            Return ConvertInternal(value, targetType, parameter, False)
        Finally
            If oldC IsNot Nothing Then System.Threading.Thread.CurrentThread.CurrentCulture = oldC
        End Try
    End Function
    ''' <summary>Performs the convert or convert back operation</summary>
    ''' <param name="value">Value to be converted</param>
    ''' <param name="targetType">Target type of conversion. Result of <paramref name="value"/> + <paramref name="parameter"/> must be <see cref="TypeTools.DynamicCast">dynamicly castable</see> to this type.</param>
    ''' <param name="parameter">Value to add to or subtract from <paramref name="value"/>. This value must be <see cref="TypeTools.DynamicCast">dynamicly castable</see> to type of <paramref name="value"/></param>
    ''' <param name="minus">True to perform subtraction, false to perform addition</param>
    ''' <returns><paramref name="value"/> + or - <paramref name="parameter"/>. If <paramref name="parameter"/> is null <paramref name="value"/> <see cref="TypeTools.DynamicCast">dynamicly casted</see> to <paramref name="targetType"/> is returned.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="value"/> is null and <paramref name="parameter"/> is not null -or- <paramref name="targetType"/> is null.</exception>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not of supported type (see remarks for list of supporetd types)</exception>
    ''' <exception cref="InvalidCastException">Unable to cast <paramref name="parameter"/> to type of <paramref name="value"/> -or- unable to cast result of arithmetic operation to <paramref name="targetType"/>. See <see cref="TypeTools.DynamicCast"/> for more information.</exception>
    ''' <exception cref="Reflection.AmbiguousMatchException">Cast operators from <paramref name="parameter"/> to type of <paramref name="value"/> or from result of addition to <paramref name="targetType"/> were found, but none is more specific. See <see cref="TypeTools.DynamicCast"/> for more info.</exception>
    ''' <exception cref="OverflowException">Arithmetic operation or type conversion resulted in overflow.</exception>
    ''' <exception cref="Exception">Format exception when attempting to convert string to numeric value</exception>
    ''' <remarks>Supported types are:
    ''' <list type="bullet">
    ''' <item><see cref="Byte"/></item>
    ''' <item><see cref="SByte"/></item>
    ''' <item><see cref="UShort"/></item>
    ''' <item><see cref="Short"/></item>
    ''' <item><see cref="UInteger"/></item>
    ''' <item><see cref="Integer"/></item>
    ''' <item><see cref="ULong"/></item>
    ''' <item><see cref="Long"/></item>
    ''' <item><see cref="Decimal"/></item>
    ''' <item><see cref="Single"/></item>
    ''' <item><see cref="Double"/></item>
    ''' </list>Cross-type conversions are performed via <see cref="TypeTools.DynamicCast"/>. <paramref name="parameter"/> can be of any (even not specifically supported) type that can be <see cref="TypeTools.DynamicCast">dynamicaly casted</see> to type of <paramref name="value"/>. <paramref name="targetType"/> may be of any type result of arithmetic operation can be <see cref="TypeTools.DynamicCast"/> dynamically casted to. Arithmetic operation is performed using Visual Basic + and - operators.</remarks>
    Private Function ConvertInternal(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal minus As Boolean) As Object
        If targetType Is Nothing Then Throw New ArgumentNullException("targetType")
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
    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. <paramref name="value"/> - <paramref name="parameter"/>. If <paramref name="parameter"/> <paramref name="value"/> <see cref="TypeTools.DynamicCast">dynamicly casted</see> to <paramref name="targetType"/> is returned.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property. Result of addition <paramref name="value"/> - <paramref name="parameter"/> (where <paramref name="parameter"/> is firts <see cref="TypeTools.DynamicCast">dynamicly casted</see> to type of <paramref name="value"/>) must be <see cref="TypeTools.DynamicCast"/> dynamicly castable to this type.</param>
    ''' <param name="parameter">Value to add to <paramref name="value"/>. This value must be <see cref="TypeTools.DynamicCast">dynamicly castable</see> to type of <paramref name="value"/>.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="value"/> is null and <paramref name="parameter"/> is not null -or- <paramref name="targetType"/> is null.</exception>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not of supported type (see remarks for list of supporetd types)</exception>
    ''' <exception cref="InvalidCastException">Unable to cast <paramref name="parameter"/> to type of <paramref name="value"/> -or- unable to cast result of arithmetic operation to <paramref name="targetType"/>. See <see cref="TypeTools.DynamicCast"/> for more information.</exception>
    ''' <exception cref="Reflection.AmbiguousMatchException">Cast operators from <paramref name="parameter"/> to type of <paramref name="value"/> or from result of addition to <paramref name="targetType"/> were found, but none is more specific. See <see cref="TypeTools.DynamicCast"/> for more info.</exception>
    ''' <exception cref="OverflowException">Arithmetic operation or type conversion resulted in overflow.</exception>
    ''' <exception cref="Exception">Format exception when attempting to convert string to numeric value</exception>
    ''' <remarks>Supported types are:
    ''' <list type="bullet">
    ''' <item><see cref="Byte"/></item>
    ''' <item><see cref="SByte"/></item>
    ''' <item><see cref="UShort"/></item>
    ''' <item><see cref="Short"/></item>
    ''' <item><see cref="UInteger"/></item>
    ''' <item><see cref="Integer"/></item>
    ''' <item><see cref="ULong"/></item>
    ''' <item><see cref="Long"/></item>
    ''' <item><see cref="Decimal"/></item>
    ''' <item><see cref="Single"/></item>
    ''' <item><see cref="Double"/></item>
    ''' </list>Cross-type conversions are performed via <see cref="TypeTools.DynamicCast"/>. <paramref name="parameter"/> can be of any (even not specifically supported) type that can be <see cref="TypeTools.DynamicCast">dynamicaly casted</see> to type of <paramref name="value"/>. <paramref name="targetType"/> may be of any type result of arithmetic operation can be <see cref="TypeTools.DynamicCast"/> dynamically casted to. Arithmetic operation is performed using Visual Basic + and - operators.</remarks>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim oldC As Globalization.CultureInfo = Nothing
        If culture IsNot Nothing Then
            oldC = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = culture
        End If
        Try
            Return ConvertInternal(value, targetType, parameter, True)
        Finally
            If oldC IsNot Nothing Then System.Threading.Thread.CurrentThread.CurrentCulture = oldC
        End Try
    End Function
End Class

''' <summary>Converter that converts null values to false and non-null values to true.</summary>
''' <remarks>Additionally if targetType is <see cref="Visibility"/> it converts null to <see cref="Visibility.Collapsed"/> and non-null to <see cref="Visibility.Visible"/>.
''' <para>This converter is designed as one-way.</para></remarks>
Public Class NullFalseConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>
    ''' If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="Boolean"/> returns true when <paramref name="value"/> is not null; false when <paramref name="value"/> is null.
    ''' If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is not assignable from</see> from <see cref="Boolean"/> but it <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="Visibility"/> returns <see cref="Visibility.Visible"/> when <paramref name="parameter"/> is not null; <see cref="Visibility.Collapsed"/> when <paramref name="value"/> is null.
    ''' </returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property. Must <see cref="Type.IsAssignableFrom">be assignable from</see> either <see cref="Boolean"/> or <see cref="Visibility"/>.</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException"><paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> neither <see cref="Boolean"/> nor <see cref="Visibility"/>.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If targetType.IsAssignableFrom(GetType(Boolean)) Then
            Return value IsNot Nothing
        ElseIf targetType.IsAssignableFrom(GetType(Visibility)) Then
            Return If(value Is Nothing, Visibility.Collapsed, Visibility.Visible)
        Else
            Throw New NotSupportedException(My.Resources.ex_ConvertsOnlyToBool.f(Me.GetType.Name))
        End If
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If <paramref name="value"/> is <see cref="Boolean"/> false or <paramref name="value"/> is <see cref="Visibility.Collapsed"/> returns null. Otherwise an exception is thrown.</returns>
    ''' <param name="value">The value that is produced by the binding target.</param>
    ''' <param name="targetType">ignored</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is neither false nor <see cref="Visibility.Collapsed"/> - this converter is not designed to be used with <see cref="ConvertBack"/></exception>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If TypeOf value Is Boolean AndAlso DirectCast(value, Boolean) = False Then Return Nothing
        Throw New NotSupportedException(My.Resources.ex_CanConvertBackOnlyFromFalse.f(Me.GetType.Name))
    End Function
End Class

''' <summary>Converter that gets file name part of path</summary>
''' <remarks>This converter is one-way.</remarks>
Public Class FileNameConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>File name part of path given in <paramref name="parameter"/>. Uses <see cref="IO.Path.GetFileName"/>. When <paramref name="value"/> is null returns null.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">Ignored. Returns null. or <see cref="String"/></param>
    ''' <param name="parameter">ignored.</param>
    ''' <param name="culture">ignored.</param>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither <see cref="String"/> nor null</exception>
    ''' <remarks>Uses <see cref="IO.Path.Combine"/></remarks>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Return IO.Path.GetFileName(value.ToString)
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>Never returns, always throws <see cref="NotSupportedException"/>.</returns>
    ''' <param name="value">ignored</param>
    ''' <param name="targetType">ignored</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException">Always thrown</exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function


End Class


''' <summary>Converts relative path to absolute</summary>
''' <remarks>This converter is one way</remarks>
Public Class RelativePathConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. Absolute path made from relative path given in <paramref name="value"/>. Relative path root to make it absolute is either <paramref name="parameter"/> is specified or <see cref="Environment.CurrentDirectory"/>. If <paramref name="value"/> <see cref="IO.Path.IsPathRooted">is absolute path</see>, <paramref name="value"/> is vreturned without any change. if <paramref name="value"/> is null, returns null.</returns>
    ''' <param name="value">The value produced by the binding source. It must be valid relative or absolute path (<see cref="String"/>). Additionaly it can be 1-dimensional array of <see cref="String"/> with at least 1 item. Items of such aray are converter using <see cref="IO.Path.Combine"/> left to right.</param>
    ''' <param name="targetType">Ignored. Always returns <see cref="String"/> or null.</param>
    ''' <param name="parameter">String path to prepend to <paramref name="value"/>. If null <see cref="Environment.CurrentDirectory"/> is used instead.</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="TypeMismatchException">Either <paramref name="value"/> or <paramref name="parameter"/> is neither <see cref="String"/> nor null nor one dimensional array of <see cref="String"/> with at least one item.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If TypeOf parameter Is String() AndAlso DirectCast(parameter, String()).Length > 0 Then
            Dim str = DirectCast(parameter, String())(0)
            For i As Integer = 0 To DirectCast(parameter, String()).Length - 1
                str = IO.Path.Combine(str, DirectCast(parameter, String())(i))
            Next i
            parameter = str
        End If
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        If parameter IsNot Nothing AndAlso Not TypeOf parameter Is String Then Throw New TypeMismatchException("parameter", parameter, GetType(String))
        Dim path = If(DirectCast(parameter, String), Environment.CurrentDirectory)
        If (IO.Path.IsPathRooted(value.ToString)) Then Return value
        Return IO.Path.Combine(path, value)
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>Never returns, always throws <see cref="NotSupportedException"/>.</returns>
    ''' <param name="value">ignored</param>
    ''' <param name="targetType">ignored</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException">Always thrown</exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.[GetType].Name))
    End Function
End Class

''' <summary>Converts value of type <see cref="Int32"/> to <see cref="Color"/></summary>
Public Class IntColorConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. Either <see cref="Color"/>, <see cref="SolidColorBrush"/> or <see cref="System.Drawing.Color"/> depending on <paramref name="targetType"/>. The color of retuned objects if made from <paramref name="value"/> as if it represents ARGB color code.</returns>
    ''' <param name="value">The value produced by the binding source. It must be built-in numeric type representing color ARGB code, <see cref="Color"/>, <see cref="SolidColorBrush"/> or <see cref="System.Drawing.Color"/>.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <remarks>Return type of this method depends on <paramref name="targetType"/>:
    ''' <para>If it <see cref="Type.IsAssignableFrom">is assignable form</see> <see cref="System.Drawing.Color"/> (but not from <see cref="Brush"/> or <see cref="Color"/>), <see cref="System.Drawing.Color"/> is retuned.</para>
    ''' <para>If it <see cref="Type.IsAssignableFrom">is assignable form</see> <see cref="Brush"/> (but not from <see cref="Color"/>), <see cref="SolidColorBrush"/> is returned.</para>
    ''' <para>If it <see cref="Type.IsAssignableFrom">is assignable form</see> <see cref="Color"/>, <see cref="Color"/> is returned.</para>
    ''' <para>If it <see cref="Type.IsAssignableFrom">is assignable form</see> neither from <see cref="Color"/>, <see cref="Brush"/> not <see cref="System.Drawing.Color"/> an exception is thrown.</para></remarks>
    ''' <exception cref="OverflowException">Numeric type conversion from <paramref name="value"/> to <see cref="Integer"/> failed.</exception>
    ''' <exception cref="ArgumentException"><paramref name="value"/> is neither <see cref="Integer"/>, <see cref="SByte"/>, <see cref="Byte"/>, <see cref="UShort"/>, <see cref="Short"/>, <see cref="UInt32"/>, <see cref="ULong"/>, <see cref="Long"/>, <see cref="Decimal"/>, <see cref="Single"/>, <see cref="Double"/>, <see cref="Color"/>, <see cref="System.Drawing.Color"/> nor <see cref="SolidColorBrush"/></exception>
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
        ElseIf TypeOf value Is SolidColorBrush Then : val = DirectCast(value, SolidColorBrush).Color.ToArgb
        Else : Throw New ArgumentException(My.Resources.ex_ConvertOnlyFromNumericAndColors)
        End If
        If (targetType.IsAssignableFrom(GetType(System.Drawing.Color)) OrElse targetType.IsAssignableFrom(GetType(System.Drawing.Color?))) AndAlso _
                Not targetType.IsAssignableFrom(GetType(Brush)) AndAlso _
                Not (targetType.IsAssignableFrom(GetType(Color)) OrElse targetType.IsAssignableFrom(GetType(Color?))) Then _
                    Return System.Drawing.Color.FromArgb(val)
        If targetType.IsAssignableFrom(GetType(Brush)) AndAlso Not (targetType.IsAssignableFrom(GetType(Color)) OrElse targetType.IsAssignableFrom(GetType(Color?))) Then _
            Return New SolidColorBrush(System.Drawing.Color.FromArgb(val).ToColor)
        Return System.Drawing.Color.FromArgb(val).ToColor
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>Converterd value. Type depends on <paramref name="targetType"/>. Typically is returned integer representing ARGB value of color given in <paramref name="value"/>.</returns>
    ''' <param name="value">>A converted value. This must be either null, <see cref="Color"/>, <see cref="SolidColorBrush"/> or <see cref="System.Drawing.Color"/>.</param>
    ''' <param name="targetType">The type to convert to. It must be either <see cref="Color"/>, <see cref="SolidColorBrush"/>, <see cref="System.Drawing.Color"/> or type <see cref="Integer"/> is <see cref="TypeTools.DynamicCast">danamicly castable</see> to.</param>
    ''' <param name="parameter">The converter parameter to use.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither <see cref="Color"/>, <see cref="SolidColorBrush"/> nor <see cref="System.Drawing.Color"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="targetType"/> is null</exception>
    ''' <exception cref="InvalidCastException">Failed to convert <see cref="Integer"/> to <paramref name="targetType"/> and <paramref name="targetType"/> is neither <see cref="Color"/>, <see cref="Brush"/> nor <see cref="System.Drawing.Color"/>.</exception>
    ''' <exception cref="Reflection.AmbiguousMatchException">Cast operators from <see cref="Integer"/> to <paramref name="targetType"/> were found, but no one is most specific and <paramref name="targetType"/> is neither <see cref="Color"/>, <see cref="Brush"/> nor <see cref="System.Drawing.Color"/>.</exception>
    ''' <exception cref="OverflowException">Conversion of type <see cref="Integer"/> to <paramref name="targetType"/> failed due to overwlow exception and <paramref name="targetType"/> is neither <see cref="Color"/>, <see cref="Brush"/> nor <see cref="System.Drawing.Color"/>.</exception>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        Dim val As Color
        If TypeOf value Is Color Then : value = DirectCast(value, Color)
        ElseIf TypeOf value Is SolidColorBrush Then : value = DirectCast(value, SolidColorBrush).Color
        ElseIf TypeOf value Is System.Drawing.Color Then : value = DirectCast(value, System.Drawing.Color).ToColor
        Else : Throw New TypeMismatchException("value", value, GetType(Color), My.Resources.ex_CamConvertBackOnlyFromColors.f(Me.GetType.Name, GetType(Color).FullName, GetType(System.Drawing.Color).FullName))
        End If
        If targetType.IsAssignableFrom(GetType(Color)) OrElse targetType.IsAssignableFrom(GetType(Color?)) Then : Return val
        ElseIf targetType.Equals(GetType(System.Drawing.Color)) OrElse targetType.Equals(GetType(System.Drawing.Color?)) Then : Return val.ToColor
        ElseIf targetType.Equals(GetType(Brush)) OrElse targetType.IsSubclassOf(GetType(Brush)) Then : Return New SolidColorBrush(val)
        Else : Return TypeTools.DynamicCast(val.ToArgb, targetType)
        End If
    End Function

End Class

''' <summary>Converter that test if value being converter equals to parameter</summary>
''' <remarks>This converter is intended as is one-way.</remarks>
Public Class CompareConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If <paramref name="value"/> is null or <paramref name="parameter"/> is null, returns null. Otherwise returns boolean value indicating if <paramref name="value"/> equals to <paramref name="parameter"/> using <see cref="System.Object.Equals"/>.</returns>
    ''' <param name="value">The value produced by the binding source. Thsi value will be compared for equality with <paramref name="parameter"/>.</param>
    ''' <param name="targetType">Ignored. Always returns null or <see cref="Boolean"/></param>
    ''' <param name="parameter">Value to compare <paramref name="value"/> with.</param>
    ''' <param name="culture">Ignored.</param>
    ''' <remarks>No type conversion is performed on <paramref name="value"/> and <paramref name="parameter"/> arguments.
    ''' Simply <c><paramref name="value"/>.<see cref="System.Object.Equals">Equals</see>(<paramref name="parameter"/>)</c> is called.</remarks>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return parameter Is Nothing
        If parameter Is Nothing Then Return value Is Nothing
        Return value.Equals(parameter)
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>If <paramref name="value"/> is true returns <paramref name="parameter"/>; otherwise throws an exception</returns>
    ''' <param name="value">Value to be converted</param>
    ''' <param name="targetType">ignored</param>
    ''' <param name="parameter">Value this converter compares values to</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not true</exception>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If TypeOf value Is Boolean AndAlso DirectCast(value, Boolean) Then Return parameter
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

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">The converter parameter to use. If string "!" converter negates <paramref name="value"/> first.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
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

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value that is produced by the binding target.</param>
    ''' <param name="targetType">The type to convert to.</param>
    ''' <param name="parameter">The converter parameter to use. If string "!" converter negates return value.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
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
''' <remarks>This is one-wy converter</remarks>
Public Class StringFormatConverter
    Implements IValueConverter

    ''' <summary>Formats a value.</summary>
    ''' <returns>A converted value.
    ''' If <paramref name="parameter"/> is null and <paramref name="value"/> is null, an empty string is returned.
    ''' If <paramref name="value"/> is not null but <paramref name="parameter"/> is null, <paramref name="value"/>.<see cref="System.Object.ToString">ToString</see> is returned. (If <paramref name="value"/> is <see cref="IFormattable"/> <see cref="IFormattable.ToString"/> with format argument null is used instead.)
    ''' Othewise result of <see cref="System.String.Format"/> with <paramref name="culture"/>, <paramref name="parameter"/> as format and <paramref name="value"/> as the only argument is returned.
    ''' </returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">The converter parameter to use.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <exception cref="TypeMismatchException"><paramref name="parameter"/> is neither null nor <see cref="String"/>.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If parameter Is Nothing AndAlso value Is Nothing Then Return ""
        If parameter Is Nothing Then Return If(TypeOf value Is IFormattable, DirectCast(value, IFormattable).ToString(Nothing, culture), value.ToString)
        If Not TypeOf parameter Is String Then Throw New TypeMismatchException("parameter", parameter, GetType(String))
        Return String.Format(culture, DirectCast(parameter, String), value)
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>Never returns, always throws <see cref="NotSupportedException"/>.</returns>
    ''' <param name="value">ignored</param>
    ''' <param name="targetType">ignored</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException">Always thrown</exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class

''' <summary>Comnverter of type <see cref="Visibility"/> that converts is to oposite value</summary>
''' <remarks>Both, <see cref="IValueConverter.Convert"/> and <see cref="IValueConverter.ConvertBack"/> functions are implemented by the same <see cref="NotVisibilityConverter.Convert"/> function</remarks>
Public Class NotVisibilityConverter
    Implements IValueConverter

    ''' <summary>Converts a value</summary>
    ''' <param name="value">A value to be converted. It must be either <see cref="Visibility"/> or <see cref="Boolean"/></param>
    ''' <param name="targetType">Type to return. It must be assignable from <see cref="Visibility"/> or <see cref="Boolean"/>.</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <returns>
    ''' If <paramref name="value"/> is <see cref="Boolean"/> it's treated as <see cref="Visibility.Collapsed"/> (false) or <see cref="Visibility.Visible"/> (true).
    ''' <paramref name="value"/> is converted to output: <see cref="Visibility.Visible"/> to <see cref="Visibility.Collapsed"/>; <see cref="Visibility.Collapsed"/> to <see cref="Visibility.Visible"/>; any other value is left unchanged.
    ''' If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="Boolean"/> but not from <see cref="Visibility"/>, <see cref="Boolean"/> (true for <see cref="Visibility.Visible"/>, false for <see cref="Visibility.Collapsed"/>); otherwise <see cref="Visibility"/> is returned.
    ''' </returns>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither <see cref="Visibility"/> nor <see cref="Boolean"/></exception>
    ''' <exception cref="ArgumentException"><paramref name="targetType"/> is neither <see cref="Visibility"/> nor <see cref="Boolean"/> or <paramref name="targetType"/> is <see cref="Boolean"/> and <paramref name="value"/> is neither <see cref="Boolean"/>, <see cref="Visibility.Visible"/> nor <see cref="Visibility.Collapsed"/>.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert, System.Windows.Data.IValueConverter.ConvertBack
        If TypeOf value Is Boolean Then value = If(DirectCast(value, Boolean), Visibility.Visible, Visibility.Collapsed)
        If Not TypeOf value Is Visibility Then Throw New TypeMismatchException("value", value, GetType(Visibility))
        Dim ret As Visibility
        Select Case DirectCast(value, Visibility)
            Case Visibility.Collapsed : ret = Visibility.Visible
            Case Visibility.Visible : ret = Visibility.Collapsed
            Case Else
                If Not targetType.IsAssignableFrom(GetType(Visibility)) Then Throw New ArgumentException(My.Resources.ex_IsNotAssignableFrom.f("targetType", GetType(Visibility).Name))
                Return value
        End Select
        If targetType.IsAssignableFrom(GetType(Boolean)) AndAlso Not targetType.IsAssignableFrom(GetType(Visibility)) Then
            Return If(ret = Visibility.Visible, True, False)
        ElseIf targetType.IsAssignableFrom(GetType(Visibility)) Then
            Return ret
        Else
            Throw New ArgumentNullException(My.Resources.ex_IsAssignableFromNeither, "targetType".f(GetType(Visibility).Name, GetType(Boolean).Name))
        End If
    End Function

End Class


''' <summary>Delegate that represents <see cref="IValueConverter.Convert"/> and <see cref="IValueConverter.ConvertBack"/> functions</summary>
''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
''' <param name="value">The value produced by the binding source.</param>
''' <param name="targetType">The type of the binding target property.</param>
''' <param name="parameter">The converter parameter to use.</param>
''' <param name="culture">The culture to use in the converter.</param>
Public Delegate Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object

''' <summary>Converter that performs conversion by calling any function</summary>
''' <remarks>
''' This is general purpose implementation of <see cref="IValueConverter"/> which's goal is to simplify creation of WPF converters by not needing to have class for each simple convertor. Using this class it is possible to just implement 2 (or one forn one-way) functions for each convertors.
''' <para>Alternativly convertor can be delegate-powerd. But delegates cannot be AFAIK set in XAML.</para>
''' </remarks>
Public Class FunctionConverter
    Implements IValueConverter
    ''' <summary>Contains value of the <see cref="Type"/> property</summary>
    Private _Type As Type
    ''' <summary>Contains value of the <see cref="[Function]"/> property</summary>
    Private _Function As String
    ''' <summary>Contains value of the <see cref="FunctionBack"/> property</summary>
    Private _FunctionBack As String
    ''' <summary>Contains value of the <see cref="[Delegate]"/> property</summary>
    Private _Delegate As Convert
    ''' <summary>Contains value of the <see cref="DelegateBack"/> property</summary>
    Private _DelegateBack As Convert

    ''' <summary>Gets or sets type function to be called is defined in.</summary>
    ''' <value>Type <see cref="[Function]"/> and <see cref="FunctionBack"/> is defined in.</value>
    ''' <remarks>Ignored for particular conversion direction if <see cref="[Delegate]"/> or <see cref="DelegateBack"/> is set</remarks>
    Public Property Type() As Type
        Get
            Return _Type
        End Get
        Set(ByVal value As Type)
            _Type = value
            _Delegate = Nothing
        End Set
    End Property
    ''' <summary>Name of public statis function in type <see cref="Type"/> to be called when <see cref="Convert"/> is called.</summary>
    ''' <value>Function called when <see cref="Convert"/> is called. Ignored when <see cref="[Delegate]"/> is set.</value>
    ''' <remarks>The function must have same signature as the <see cref="Caps.Console.Convert"/> delegate.
    ''' <para>Setting this property cause <see cref="[Delegate]"/> to be set to null.</para></remarks>
    Public Property [Function]() As String
        Get
            Return _Function
        End Get
        Set(ByVal value As String)
            _Function = value
            _Delegate = Nothing
        End Set
    End Property
    ''' <summary>Name of public statis function in type <see cref="Type"/> to be called when <see cref="ConvertBack"/> is called.</summary>
    ''' <value>Function called when <see cref="ConvertBack"/> is called. Ignored when <see cref="[DelegateBack]"/> is set.</value>
    ''' <remarks>The function must have same signature as the <see cref="Caps.Console.Convert"/> delegate.
    ''' <para>Setting this property cause <see cref="DelegateBack"/> to be set to null.</para></remarks>
    Public Property [FunctionBack]() As String
        Get
            Return _FunctionBack
        End Get
        Set(ByVal value As String)
            _FunctionBack = value
            _DelegateBack = Nothing
        End Set
    End Property
    ''' <summary>Gets or sets value of the <see cref="Type"/> property using type <see cref="Type.AssemblyQualifiedName">assembly qualified name</see>.</summary>
    ''' <returns><see cref="Type"/>.<see cref="Type.AssemblyQualifiedName">AssemblyQualifiedName</see></returns>
    ''' <value>Sets value of the <see cref="Type"/> property by requesting type using <see cref="Type.[GetType]"/>.</value>
    ''' <exception cref="Reflection.TargetInvocationException">A class initializer is invoked and throws an exception.</exception>
    ''' <exception cref="ArgumentException">Value being set is a pointer, passed by reference, or is a generic class with a <see cref="System.Void" /> as its type parameter.</exception>
    ''' <exception cref="TypeLoadException">Value being set is invalid.  -or- Value being set is an empty string.  -or- Value being set represents an array of <see cref="System.TypedReference" />.</exception>
    ''' <exception cref="IO.FileNotFoundException">The assembly or one of its dependencies was not found.</exception>
    ''' <exception cref="IO.FileLoadException">The assembly or one of its dependencies was found, but could not be loaded.</exception>
    ''' <exception cref="BadImageFormatException">The assembly or one of its dependencies is not valid. -or- Version 2.0 or later of the common language runtime is currently loaded, and the assembly was compiled with a later version.</exception>
    Public Property TypeName() As String
        Get
            If Type Is Nothing Then Return Nothing
            Return Type.AssemblyQualifiedName
        End Get
        Set(ByVal value As String)
            If value Is Nothing Then Type = Nothing Else Type = Type.GetType(value)
        End Set
    End Property
    ''' <summary>Delegate to be called when <see cref="Convert"/> is called</summary>
    ''' <value>Delegate called when <see cref="Convert"/> is called</value>
    ''' <remarks>Setting this property causes <see cref="[Function]"/> to be set to null.</remarks>
    Public Property [Delegate]() As Convert
        Get
            Return _Delegate
        End Get
        Set(ByVal value As Convert)
            _Delegate = value
            _Function = Nothing
        End Set
    End Property
    ''' <summary>Delegate to be called when <see cref="ConvertBack"/> is called</summary>
    ''' <value>Delegate called when <see cref="ConvertBack"/> is called</value>
    ''' <remarks>Setting this property causes <see cref="[FunctionBack]"/> to be set to null.</remarks>
    Public Property [DelegateBack]() As Convert
        Get
            Return _DelegateBack
        End Get
        Set(ByVal value As Convert)
            _DelegateBack = value
            _FunctionBack = Nothing
        End Set
    End Property
    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">The converter parameter to use.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <remarks>If <see cref="[Delegate]"/> is not null, this function calls <see cref="[Delegate]"/>; otherwise function attempts to call the <see cref="[Function]"/> function in type <see cref="Type"/>.</remarks>
    ''' <exception cref="InvalidOperationException">Both <see cref="[Delegate]"/> and <see cref="[Function]"/> are null. -or- <see cref="[Delegate]"/> is null and <see cref="Type"/> is null.</exception> 
    ''' <exception cref="Reflection.AmbiguousMatchException">More than one method is found with the name <see cref="[Function]"/> and matching the specified binding constraints (<see cref="Caps.Console.Convert"/> delegate signature).</exception>
    ''' <exception cref="ArgumentException">Parameters of this method do not match the signature of the method <see cref="[Function]"/>.</exception>
    ''' <exception cref="Reflection.TargetInvocationException">The invoked method throws an exception.</exception>
    ''' <exception cref="MethodAccessException">The caller does not have permission to execute the method.</exception>
    ''' <exception cref="InvalidOperationException">The type that declares the method is an open generic type.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If [Delegate] IsNot Nothing Then
            Return [Delegate].Invoke(value, targetType, parameter, culture)
        ElseIf [Function] Is Nothing Then
            Throw New InvalidOperationException(My.Resources.ex_FunctionForForwardConversionHaveNotBeenSet)
        ElseIf Type Is Nothing Then
            Throw New InvalidOperationException(My.Resources.ex_IsNull.f("Type"))
        Else
            Dim f = Type.GetMethod([Function], Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static, Nothing, _
                       New Type() {GetType(Object), GetType(Type), GetType(Object), GetType(System.Globalization.CultureInfo)}, New System.Reflection.ParameterModifier() {})
            Return f.Invoke(Nothing, New Object() {value, targetType, parameter, culture})
        End If
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value that is produced by the binding target.</param>
    ''' <param name="targetType">The type to convert to.</param>
    ''' <param name="parameter">The converter parameter to use.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <remarks>If <see cref="[DelegateBack]"/> is not null, this function calls <see cref="[DelegateBack]"/>; otherwise function attempts to call the <see cref="[FunctionBack]"/> function in type <see cref="Type"/>.</remarks>
    ''' <exception cref="InvalidOperationException">Both <see cref="[DelegateBack]"/> and <see cref="[FunctionBack]"/> are null. -or- <see cref="[DelegateBack]"/> is null and <see cref="Type"/> is null.</exception> 
    ''' <exception cref="System.Reflection.AmbiguousMatchException">More than one method is found with the name <see cref="[FunctionBack]"/> and matching the specified binding constraints (<see cref="Caps.Console.Convert"/> delegate signature).</exception>
    ''' <exception cref="ArgumentException">Parameters of this method do not match the signature of the method <see cref="[FunctionBack]"/>.</exception>
    ''' <exception cref="Reflection.TargetInvocationException">The invoked method throws an exception.</exception>
    ''' <exception cref="MethodAccessException">The caller does not have permission to execute the method.</exception>
    ''' <exception cref="InvalidOperationException">The type that declares the method is an open generic type.</exception>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If DelegateBack IsNot Nothing Then
            Return DelegateBack.Invoke(value, targetType, parameter, culture)
        ElseIf FunctionBack Is Nothing Then
            Throw New InvalidOperationException(My.Resources.ex_FunctionForBackwardConversionHasNotBeenSet)
        ElseIf Type Is Nothing Then
            Throw New InvalidOperationException(My.Resources.ex_IsNull.f("Type"))
        Else
            Dim f = Type.GetMethod([FunctionBack], Reflection.BindingFlags.Public Or Reflection.BindingFlags.Static, Nothing, _
                                   New Type() {GetType(Object), GetType(Type), GetType(Object), GetType(System.Globalization.CultureInfo)}, New System.Reflection.ParameterModifier() {})
            Return f.Invoke(Nothing, New Object() {value, targetType, parameter, culture})
        End If
    End Function
End Class