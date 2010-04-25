Imports Tools.TypeTools, Tools.ReflectionT
Imports System.ComponentModel, System.Linq
Imports System.Security
Imports System.Numerics
Imports System.Net

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
''' <remarks>Additionally if targetType is <see cref="Windows.Visibility"/> it converts null to <see cref="Windows.Visibility.Collapsed"/> and non-null to <see cref="Windows.Visibility.Visible"/>.
''' <para>This converter is designed as one-way.</para></remarks>
Public Class NullFalseConverter
    Implements IValueConverter
    ''' <summary>Converts a value.</summary>
    ''' <returns>
    ''' If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="Boolean"/> returns true when <paramref name="value"/> is not null; false when <paramref name="value"/> is null.
    ''' If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is not assignable from</see> from <see cref="Boolean"/> but it <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="Windows.Visibility"/> returns <see cref="Windows.Visibility.Visible"/> when <paramref name="parameter"/> is not null; <see cref="Windows.Visibility.Collapsed"/> when <paramref name="value"/> is null.
    ''' </returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property. Must <see cref="Type.IsAssignableFrom">be assignable from</see> either <see cref="Boolean"/> or <see cref="Windows.Visibility"/>.</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException"><paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> neither <see cref="Boolean"/> nor <see cref="Windows.Visibility"/>.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If targetType.IsAssignableFrom(GetType(Boolean)) Then
            Return value IsNot Nothing
        ElseIf targetType.IsAssignableFrom(GetType(Windows.Visibility)) Then
            Return If(value Is Nothing, Windows.Visibility.Collapsed, Windows.Visibility.Visible)
        Else
            Throw New NotSupportedException(My.Resources.ex_ConvertsOnlyToBool.f(Me.GetType.Name))
        End If
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If <paramref name="value"/> is <see cref="Boolean"/> false or <paramref name="value"/> is <see cref="Windows.Visibility.Collapsed"/> returns null. Otherwise an exception is thrown.</returns>
    ''' <param name="value">The value that is produced by the binding target.</param>
    ''' <param name="targetType">ignored</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is neither false nor <see cref="Windows.Visibility.Collapsed"/> - this converter is not designed to be used with <see cref="ConvertBack"/></exception>
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
        If TypeOf value Is Color Then : val = DirectCast(value, Color)
        ElseIf TypeOf value Is SolidColorBrush Then : val = DirectCast(value, SolidColorBrush).Color
        ElseIf TypeOf value Is System.Drawing.Color Then : val = DirectCast(value, System.Drawing.Color).ToColor
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



''' <summary>Converter tha converts <see cref="Boolean"/> to <see cref="Windows.Visibility"/></summary>
Public Class BooleanVisibilityConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value produced by the binding source.</param>
    ''' <param name="targetType">The type of the binding target property.</param>
    ''' <param name="parameter">The converter parameter to use. If string "!" converter negates <paramref name="value"/> first.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not <see cref="Boolean"/> or <paramref name="targetType"/> is not <see cref="Windows.Visibility"/> or one of its base types.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If TypeOf value Is Boolean AndAlso targetType.IsAssignableFrom(GetType(Windows.Visibility)) Then
            If If(TypeOf parameter Is String AndAlso DirectCast(parameter, String) = "!", Not DirectCast(value, Boolean), DirectCast(value, Boolean)) Then
                Return Windows.Visibility.Visible
            Else
                Return Windows.Visibility.Collapsed
            End If
        End If
        Throw New NotSupportedException(My.Resources.ex_ConverterCanConvertOnlyFromTo.f(Me.GetType.Name, GetType(Boolean).Name, GetType(Windows.Visibility).Name))
    End Function

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    ''' <param name="value">The value that is produced by the binding target.</param>
    ''' <param name="targetType">The type to convert to.</param>
    ''' <param name="parameter">The converter parameter to use. If string "!" converter negates return value.</param>
    ''' <param name="culture">The culture to use in the converter.</param>
    ''' <exception cref="ArgumentException"><paramref name="value"/> is  neither <see cref="Windows.Visibility.Collapsed"/> nor <see cref="Windows.Visibility.Visible"/></exception> 
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not <see cref="Windows.Visibility"/> or <paramref name="targetType"/> is not <see cref="Boolean"/> or one of its base types.</exception>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim ret As Boolean
        If TypeOf value Is Windows.Visibility AndAlso targetType.IsAssignableFrom(GetType(Boolean)) Then
            Select Case DirectCast(value, Windows.Visibility)
                Case Windows.Visibility.Collapsed : ret = False
                Case Windows.Visibility.Visible : ret = True
                Case Else : Throw New ArgumentException(My.Resources.ex_ConverterCannotConvertValueBack.f(Me.GetType, GetType(Windows.Visibility).Name, value))
            End Select
        Else
            Throw New NotSupportedException(My.Resources.ex_ConverterCanConvertBackOnlyFromTo.f(Me.GetType.Name, GetType(Windows.Visibility).Name, GetType(Boolean).Name))
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

''' <summary>Comnverter of type <see cref="Windows.Visibility"/> that converts is to oposite value</summary>
''' <remarks>Both, <see cref="IValueConverter.Convert"/> and <see cref="IValueConverter.ConvertBack"/> functions are implemented by the same <see cref="NotVisibilityConverter.Convert"/> function</remarks>
Public Class NotVisibilityConverter
    Implements IValueConverter

    ''' <summary>Converts a value</summary>
    ''' <param name="value">A value to be converted. It must be either <see cref="Windows.Visibility"/> or <see cref="Boolean"/></param>
    ''' <param name="targetType">Type to return. It must be assignable from <see cref="Windows.Visibility"/> or <see cref="Boolean"/>.</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="culture">ignored</param>
    ''' <returns>
    ''' If <paramref name="value"/> is <see cref="Boolean"/> it's treated as <see cref="Windows.Visibility.Collapsed"/> (false) or <see cref="Windows.Visibility.Visible"/> (true).
    ''' <paramref name="value"/> is converted to output: <see cref="Windows.Visibility.Visible"/> to <see cref="Windows.Visibility.Collapsed"/>; <see cref="Windows.Visibility.Collapsed"/> to <see cref="Windows.Visibility.Visible"/>; any other value is left unchanged.
    ''' If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="Boolean"/> but not from <see cref="Windows.Visibility"/>, <see cref="Boolean"/> (true for <see cref="Windows.Visibility.Visible"/>, false for <see cref="Windows.Visibility.Collapsed"/>); otherwise <see cref="Windows.Visibility"/> is returned.
    ''' </returns>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither <see cref="Windows.Visibility"/> nor <see cref="Boolean"/></exception>
    ''' <exception cref="ArgumentException"><paramref name="targetType"/> is neither <see cref="Windows.Visibility"/> nor <see cref="Boolean"/> or <paramref name="targetType"/> is <see cref="Boolean"/> and <paramref name="value"/> is neither <see cref="Boolean"/>, <see cref="Windows.Visibility.Visible"/> nor <see cref="Windows.Visibility.Collapsed"/>.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert, System.Windows.Data.IValueConverter.ConvertBack
        If TypeOf value Is Boolean Then value = If(DirectCast(value, Boolean), Windows.Visibility.Visible, Windows.Visibility.Collapsed)
        If Not TypeOf value Is Windows.Visibility Then Throw New TypeMismatchException("value", value, GetType(Windows.Visibility))
        Dim ret As Windows.Visibility
        Select Case DirectCast(value, Windows.Visibility)
            Case Windows.Visibility.Collapsed : ret = Windows.Visibility.Visible
            Case Windows.Visibility.Visible : ret = Windows.Visibility.Collapsed
            Case Else
                If Not targetType.IsAssignableFrom(GetType(Windows.Visibility)) Then Throw New ArgumentException(My.Resources.ex_IsNotAssignableFrom.f("targetType", GetType(Windows.Visibility).Name))
                Return value
        End Select
        If targetType.IsAssignableFrom(GetType(Boolean)) AndAlso Not targetType.IsAssignableFrom(GetType(Windows.Visibility)) Then
            Return If(ret = Windows.Visibility.Visible, True, False)
        ElseIf targetType.IsAssignableFrom(GetType(Windows.Visibility)) Then
            Return ret
        Else
            Throw New ArgumentNullException(My.Resources.ex_IsAssignableFromNeither, "targetType".f(GetType(Windows.Visibility).Name, GetType(Boolean).Name))
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

''' <summary>Converter that converts color to color negative to given color</summary>
''' <remarks>This converter does not change alpha cannel of color.
''' <para>This converter accepts and can return types <see cref="Color"/>, <see cref="System.Drawing.Color"/>, <see cref="SolidColorBrush"/> and <see cref="Integer"/> (ARGB value).</para></remarks>
Public Class NotColorConverter
    Implements IValueConverter
    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. Null when <paramref name="value"/> is null; othervice solor negative to given color. Depending on <paramref name="targetType"/> value of following type is returned (in order of precedence, first type for which <paramref name="targetType"/>.<see cref="Type.IsAssignableFrom">IsAssignableFrom</see> returns true is used): <see cref="Color"/>, <see cref="System.Drawing.Color"/>, <see cref="SolidColorBrush"/>, <see cref="Integer"/>.</returns>
    ''' <param name="value">The value that is produced by the binding target. It shall be <see cref="Color"/>, <see cref="System.Drawing.Color"/>, <see cref="SolidColorBrush"/> or <see cref="Integer"/>.</param>
    ''' <param name="targetType">The type to convert to. Shall be one of following type or their base types: <see cref="Color"/>, <see cref="System.Drawing.Color"/>, <see cref="SolidColorBrush"/>, <see cref="Integer"/></param>
    ''' <param name="parameter">Ignored.</param>
    ''' <param name="culture">Ignored.</param>
    ''' <exception cref="ArgumentException"><paramref name="targetType"/> is assignable neither from <see cref="Color"/> nor from <see cref="System.Drawing.Color"/> nor from <see cref="SolidColorBrush"/> nor from <see cref="Integer"/></exception>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither <see cref="Color"/> nor <see cref="System.Drawing.Color"/> nor <see cref="SolidColorBrush"/> nor <see cref="Integer"/></exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert, System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        Dim col As System.Drawing.Color
        If TypeOf value Is System.Drawing.Color Then
            col = value
        ElseIf TypeOf value Is Color Then
            col = DirectCast(value, Color).ToColor
        ElseIf TypeOf value Is SolidColorBrush Then
            col = DirectCast(value, SolidColorBrush).Color.ToColor
        ElseIf TypeOf value Is Integer Then
            col = System.Drawing.Color.FromArgb(value)
        Else
            Throw New TypeMismatchException("value", value, GetType(Color), "NotColorConverter accepts values of type {0}, {1} and {2}".f(GetType(Color).FullName, GetType(System.Drawing.Color).FullName, GetType(Int32).FullName))
        End If
        col = System.Drawing.Color.FromArgb(col.A, Not col.R, Not col.G, Not col.B)
        If targetType Is Nothing OrElse targetType.IsAssignableFrom(GetType(Color)) Then
            Return col.ToColor
        ElseIf targetType.IsAssignableFrom(GetType(System.Drawing.Color)) Then
            Return col
        ElseIf targetType.IsAssignableFrom(GetType(SolidColorBrush)) Then
            Return New SolidColorBrush(col.ToColor)
        ElseIf targetType.IsAssignableFrom(GetType(Integer)) Then
            Return col.ToArgb
        Else
            Throw New ArgumentException("NotColorConverter can convert only to {0}, {1} and {2}".f(GetType(Color).FullName, GetType(System.Drawing.Color).FullName, GetType(Int32).FullName))
        End If
    End Function


End Class

''' <summary>Converter to convert <see cref="Boolean"/> values to <see cref="Char"/>. It also supports null values instead of booleand and <see cref="String"/> isntead of <see cref="Char"/>.</summary>
Public Class BooleanToCharConverter
    Implements IValueConverter
    ''' <summary>Default value used for parameter of <see cref="BooleanToCharConverter"/> when null is supplied</summary>
    ''' <remarks><see cref="BooleanToCharConverter"/> parameter consists of 3 comma-separated strings each containing characters representing one nullable boolean value: 1st - True (checked), 2nd - False (unchecked), 3rd - unknow/null (intermediate).
    ''' If you are sure any value is never used, you cann ommit (leave empty) appropriate part of parameter. First character in each group is considered default.
    ''' There is no way to escape comma to pecify it as one of characters.</remarks>
    Public Const DefaultParameter = "☑☒☓✓✔✕✖✗✘◉⌧1+,☐❍❏❐❑❒□▢◯0-,▣◎●■•⌼⌻?"
    ''' <summary>Converts value of type <see cref="Boolean"/> or <see cref="Nullable(Of Boolean)"/>[<see cref="Boolean"/>] to <see cref="Char"/> or <see cref="String"/>.</summary>
    ''' <param name="value">Value to be converted. It shall be <see cref="Boolean"/> value or null</param>
    ''' <param name="targetType">Type to convert value to. It shall be <see cref="Char"/>, <see cref="String"/> or <see cref="Nullable(Of Char)"/>[<see cref="Char"/>] or type <see cref="Type.IsAssignableFrom">assignable</see> from that.</param>
    ''' <param name="parameter">Either null or string containing 3 comma-separated strings defining characters used fot true, false and unknown values. See <see cref="DefaultParameter"/> for details.</param>
    ''' <param name="culture">Ignored</param>
    ''' <returns>One of default characters (first character of group) specified in <paramref name="parameter"/> or <see cref="DefaultParameter"/> depending on <see cref="Boolean"/> value <paramref name="value"/>. Return type depends on requested <paramref name="targetType"/>.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="parameter"/> is neither null nor <see cref="String"/>. -or- <paramref name="value"/> is neither <see cref="Boolean"/> nor null nor <see cref="Nullable(Of Boolean)"/>[<see cref="Boolean"/>].</exception>
    ''' <exception cref="ArgumentException"><paramref name="parameter"/> does not contain at least one character for group requested by <paramref name="value"/> or does not contain the group at ll.</exception>
    ''' <exception cref="NotSupportedException"><paramref name="targetType"/> is neither <see cref="Char"/> nor <see cref="String"/> nor <see cref="Nullable(Of Char)"/>[<see cref="Char"/>] nor <see cref="Type.IsAssignableFrom">is assignable</see> froma ny of them.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim par As String
        If parameter Is Nothing Then
            par = DefaultParameter
        ElseIf TypeOf parameter Is String Then
            par = parameter
        Else
            Throw New TypeMismatchException("parameter", parameter, GetType(String))
        End If
        Dim val As Boolean?
        If TypeOf value Is Boolean Then
            val = DirectCast(value, Boolean)
        ElseIf value Is Nothing Then
            val = New Boolean?
        Else
            Throw New TypeMismatchException("value", value, GetType(Boolean?))
        End If
        Dim groups = par.Split(",")
        Dim ret As Char
        If Not val.HasValue Then
            If groups.Length < 3 Then Throw New ArgumentException("Converter parameter does not contain definition for intermediate value.", "parameter")
            If String.IsNullOrEmpty(groups(2)) Then Throw New ArgumentException("Converter parameter for intermediate value is empty.", "parameter")
            ret = groups(2)(0)
        ElseIf val.Value Then
            If groups.Length < 1 Then Throw New ArgumentException("Converter parameter does not contain definition for checked value.", "parameter")
            If String.IsNullOrEmpty(groups(0)) Then Throw New ArgumentException("Converter parameter for checked value is empty.", "parameter")
            ret = groups(0)(0)
        Else
            If groups.Length < 2 Then Throw New ArgumentException("Converter parameter does not contain definition for unchecked value.", "parameter")
            If String.IsNullOrEmpty(groups(1)) Then Throw New ArgumentException("Converter parameter for unchecked value is empty.", "parameter")
            ret = groups(1)(0)
        End If
        If targetType.IsAssignableFrom(GetType(Char)) Then
            Return ret
        ElseIf targetType.IsAssignableFrom(GetType(String)) Then
            Return New String(ret, 1)
        ElseIf targetType.IsAssignableFrom(GetType(Char?)) Then
            Return CType(ret, Char?)
        Else
            Throw New NotSupportedException("{0} supports only target types {0} and {1} and {2} of {1}".f(Me.GetType.Name, GetType(Char).Name, GetType(String).Name, GetType(Nullable(Of )).Name))
        End If
    End Function
    ''' <summary>Converts value of type <see cref="Char"/> or <see cref="String"/> bact to type <see cref="Boolean"/>.</summary>
    ''' <param name="value">Value to be converted. It shall be null, <see cref="Char"/> or <see cref="String"/>. In case <paramref name="value"/> is <see cref="String"/> it shall be non-empty and only firts character is taken from it.</param>
    ''' <param name="targetType">Requested return type. It shall be <see cref="Boolean"/> or <see cref="Nullable(Of Bollean)"/>[<see cref="Boolean"/>] or type assignable from it. When <paramref name="targetType"/> is <see cref="Boolean"/> this method still can return null if character for intermediate state is passed to <paramref name="value"/>.</param>
    ''' <param name="parameter">Either null or string containing 3 comma-separated strings defining characters used fot true, false and unknown values. See <see cref="DefaultParameter"/> for details.</param>
    ''' <param name="culture">Ignored</param>
    ''' <returns>True, False or null depending on which of comma-separated groups in <paramref name="parameter"/> or <see cref="DefaultParameter"/> <paramref name="value"/> (or first character from <paramref name="value"/> when it is <see cref="String"/>) falls into.</returns>
    ''' <exception cref="NotSupportedException"><paramref name="targetType"/> is neither <see cref="Boolean"/> nor <see cref="Nullable(Of Boolean)"/>[<see cref="Boolean"/>] nor type <see cref="Type.IsAssignableFrom">assignable</see> from it.</exception>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither null nor <see cref="Char"/> nor <see cref="String"/>. -or- <paramref name="parameter"/> is neither null nor <see cref="String"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="value"/> none of characters specified in <paramref name="parameter"/> (or <see cref="DefaultParameter"/> in case <paramref name="parameter"/> is null) except comas.</exception>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim val As String
        If Not targetType.IsAssignableFrom(GetType(Boolean)) AndAlso Not targetType.IsAssignableFrom(GetType(Boolean?)) Then _
          Throw New NotSupportedException("{0} can convert back obly too {1} and {2} of {1}".f(Me.GetType.Name, GetType(Boolean).Name, GetType(Nullable(Of )).Name))
        If value Is Nothing OrElse (TypeOf value Is String AndAlso DirectCast(value, String) = "") Then
            Return New Boolean?
        ElseIf TypeOf value Is Char Then
            val = DirectCast(value, Char)
        ElseIf TypeOf value Is String Then
            val = value
        Else
            Throw New TypeMismatchException("{0} can convert back only from {1} and {2}".f(Me.GetType.Name, GetType(Char).Name, GetType(String).Name), value)
        End If
        Dim par As String
        If parameter Is Nothing Then
            par = DefaultParameter
        ElseIf TypeOf parameter Is String Then
            par = parameter
        Else
            Throw New TypeMismatchException("parameter", parameter, GetType(String))
        End If
        Dim parts = par.Split(",")
        If parts.Length >= 1 AndAlso parts(0).Contains(val(0)) Then
            Return True
        ElseIf parts.Length >= 2 AndAlso parts(1).Contains(val(0)) Then
            Return False
        ElseIf parts.Length >= 3 AndAlso parts(2).Contains(val(0)) Then
            Return New Boolean?
        Else
            Throw New ArgumentException("Character '{0}' does not represent known boolean value".f(val(0)), "value")
        End If
    End Function
End Class

''' <summary>Converter that converts <see cref="IEnumerable"/> to comma-seperated list (or another seperator can be chosen)</summary>
''' <remarks>This converter is designed as one-way, howver <see cref="IValueConverter.ConvertBack"/> is implemented.</remarks>
Public Class ConcatConverter
    Implements IValueConverter
    ''' <summary>Gets or sets name of property of item in collection to caoncat value got from. If null (default) entire object is used.</summary>
    Public Property PropertyName As String
    ''' <summary>Converts value</summary>
    ''' <param name="value">Value to be converted. Shall be null or <see cref="IEnumerable"/></param>
    ''' <param name="targetType">Ignored. This method always returns null or <see cref="String"/></param>
    ''' <param name="parameter">Any objects which string representation will be used as item seperator. If null ', ' is used.</param>
    ''' <param name="culture">Ignored</param>
    ''' <returns>Stirng representations of objects obrained form <see cref="IEnumerable"/> <paramref name="value"/> concatenated to string using <paramref name="parameter"/> (or ', ' if parameter is null).</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither null nor <see cref="IEnumerable"/>.</exception>
    ''' <exception cref="MissingMemberException">Property name is specified in <paramref name="parameter"/>, but there is no such property or field on item in <paramref name="value"/> collection.</exception>
    ''' <exception cref="Reflection.AmbiguousMatchException">Property name is specified in <paramref name="parameter"/> and the property is overloaded</exception>
    ''' <exception cref="Reflection.TargetParameterCountException">Property name is specified in <paramref name="parameter"/> and property is indexed</exception>
    ''' <exception cref="MethodAccessException">Property name is specified in <paramref name="parameter"/> and property getter is not public</exception>
    ''' <exception cref="Reflection.TargetInvocationException">Property name is specified in <paramref name="parameter"/> and an error occurred while retrieving the property value. The <see cref="System.Exception.InnerException"/> property indicates the reason for the error.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return ""
        If Not TypeOf value Is IEnumerable Then Throw New TypeMismatchException("value", value, GetType(IEnumerable))
        Dim sb As New Text.StringBuilder
        Dim separator$
        separator = If(parameter, ", ").ToString
        For Each item As Object In DirectCast(value, IEnumerable)
            If sb.Length <> 0 Then sb.Append(separator)
            If item Is Nothing Then
                'DoNothing
            ElseIf PropertyName = "" Then
                sb.Append(item.ToString)
            Else
                Dim prp = item.GetType.GetProperty(PropertyName, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
                Dim field = If(prp Is Nothing, item.GetType.GetField(PropertyName, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance), Nothing)
                If prp Is Nothing AndAlso field Is Nothing Then Throw New MissingMemberException(item.GetType.FullName, PropertyName)
                Dim pvalue = prp.GetValue(item, Nothing)
                If pvalue IsNot Nothing Then sb.Append(pvalue.ToString) 'Else DoNothing
            End If
        Next
        Return sb.ToString
    End Function
    ''' <summary>Converts value back from concateenated list to string array</summary>
    ''' <param name="value">Value to be converted. It shall be <see cref="String"/> or null.</param>
    ''' <param name="targetType">Ignored. This method always returns <see cref="String()"/> array</param>
    ''' <param name="parameter">Any objects which string representation will be used as item seperator. If null ', ' is used.</param>
    ''' <param name="culture">Ignored.</param>
    ''' <returns><paramref name="value"/> <see cref="String.Split">splitted</see> using <paramref name="parameter"/> (', ' if <paramref name="parameter"/> is null)</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither null nor <see cref="String"/></exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing OrElse (TypeOf value Is String AndAlso DirectCast(value, String) = "") Then Return New String() {}
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Return DirectCast(value, String).Split(If(parameter, ", ").ToString)
    End Function
End Class

''' <summary>Implements <see cref="IValueConverter"/> that converts URI (path) to cached <see cref="BitmapImage"/></summary>
''' <remarks>Use this converter to avoid images to be locked by <see cref="Image"/> control.</remarks>
Public Class CachedImageConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value - <see cref="BitmapImage"/></returns>
    ''' <param name="value">The value produced by the binding source. It must be either <see cref="String"/>, <see cref="Uri"/> or <see cref="Tools.IOt.Path"/>.</param>
    ''' <param name="targetType">The type of the binding target property. It must <see cref="Type.IsAssignableFrom">be assignable from</see> <see cref="BitmapImage"/>.</param>
    ''' <param name="parameter">Ignored</param>
    ''' <param name="culture">Ignored</param>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If Not targetType.IsAssignableFrom(GetType(BitmapImage)) Then Throw New NotSupportedException(My.Resources.err_CanConvertOnlyTo.f(Me.GetType.Name, GetType(BitmapImage).Name))
        If TypeOf value Is Uri OrElse TypeOf value Is Tools.IOt.Path Then value = value.ToString
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        If DirectCast(value, String) = "" Then Return Nothing
        Try
            Dim img As New BitmapImage
            img.BeginInit()
            img.CacheOption = BitmapCacheOption.OnLoad
            img.UriSource = New Uri(DirectCast(value, String))
            img.EndInit()
            Return img
        Catch : End Try
        Return Nothing
    End Function

    ''' <summary>Converts a value back.</summary>
    ''' <returns>A converted value - URI of <see cref="BitmapSource"/> passed in value <paramref name="value"/>. Type depends on <paramref name="targetType"/>.</returns>
    ''' <param name="value">The value that is produced by the binding target. It must be <see cref="BitmapSource"/>.</param>
    ''' <param name="targetType">The type to convert to. It must <see cref="Type.IsAssignableFrom">be assignable from</see> either <see cref="String"/>, <see cref="Uri"/> or <see cref="Tools.IOt.Path"/>. </param>
    ''' <param name="parameter">Ignored</param>
    ''' <param name="culture">Ignored</param>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        If Not TypeOf value Is BitmapImage Then Throw New TypeMismatchException("value", value, GetType(BitmapImage))
        If DirectCast(value, BitmapImage).UriSource Is Nothing Then Return Nothing
        If targetType.IsAssignableFrom(GetType(String)) Then
            Return DirectCast(value, BitmapImage).UriSource.ToString
        ElseIf targetType.IsAssignableFrom(GetType(Uri)) Then
            Return DirectCast(value, BitmapImage).UriSource
        ElseIf targetType.IsAssignableFrom(GetType(Tools.IOt.Path)) Then
            Return New Tools.IOt.Path(DirectCast(value, BitmapImage).UriSource.ToString)
        Else
            Throw New NotSupportedException(My.Resources.err_CanConvertBackOnlyTo.f(Me.GetType.Name, GetType(String).Name))
        End If
    End Function
End Class

''' <summary>Converter used to test if enumerated value is one of given values</summary>
''' <remarks>For normal enumerations checks exact value, for flags enumerations tests flass in addition.</remarks>
Public Class EnumInConverter
    Implements IValueConverter

    ''' <summary>Converts a value. </summary>
    ''' <returns>A converted value. True when <paramref name="value"/> is one of values given in <paramref name="parameter"/>; false otherwise. When <paramref name="targetType"/> is <see cref="Windows.Visibility"/> returns either <see cref="Windows.Visibility.Visible"/> or <see cref="Windows.Visibility.Hidden"/>.</returns>
    ''' <param name="value">The value produced by the binding source. Value must be of type <see cref="[Enum]"/> or null.</param>
    ''' <param name="targetType">The type of the binding target property. It must be <see cref="Boolean"/>, <see cref="Windows.Visibility"/> or <see cref="Nullable(Of T)"/> of that type.</param>
    ''' <param name="parameter">The converter parameter to use. String. Comma-seperated list of value to test <paramref name="value"/> for.</param>
    ''' <param name="culture">The culture to use in the converter. Ignored.</param>
    ''' <exception cref="ArgumentException"><paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> neither from <see cref="Boolean"/>, <see cref="Windows.Visibility"/> nor from <see cref="Nullable(Of T)"/> of <see cref="Boolean"/> or <see cref="Windows.Visibility"/>.</exception>
    ''' <exception cref="ArgumentNullException"><paramref name="parameter"/> is null</exception>
    ''' <exception cref="TypeMismatchException"><paramref name="parameter"/> is not <see cref="String"/></exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If Not targetType.IsAssignableFrom(GetType(Boolean)) AndAlso Not targetType.IsAssignableFrom(GetType(Windows.Visibility)) AndAlso
            Not targetType.IsAssignableFrom(GetType(Boolean?)) AndAlso Not targetType.IsAssignableFrom(GetType(Windows.Visibility?)) Then _
            Throw New ArgumentException("{0} can convert only to {1} or {2}".f([GetType].Name, GetType(Boolean), GetType(Windows.Visibility)))
        Dim ret As Boolean
        If parameter Is Nothing Then Throw New ArgumentNullException("parameter")
        If Not TypeOf parameter Is String Then Throw New TypeMismatchException("parameter", parameter, GetType(String))
        Dim Parts = DirectCast(parameter, String).Split(",", StringSplitOptions.RemoveEmptyEntries)
        If value Is Nothing Then
            ret = False
        ElseIf Not TypeOf value Is [Enum] Then
            Throw New TypeMismatchException("value", value, GetType([Enum]))
        Else
            ret = False
            For Each name In Parts
                Dim enmVal As [Enum]
                Try
                    enmVal = [Enum].Parse(value.GetType, name.Trim, True)
                Catch ex As ArgumentException
                    Continue For
                End Try
                If (DirectCast(value, [Enum]).IsFlags AndAlso DirectCast(value, [Enum]).HasFlag(enmVal)) OrElse
                    value.Equals(enmVal) Then
                    ret = True
                    Exit For
                End If
            Next
        End If
        If targetType.IsAssignableFrom(GetType(Boolean)) OrElse targetType.IsAssignableFrom(GetType(Boolean?)) Then
            Return ret
        Else 'Visibility
            Return If(ret, Windows.Visibility.Visible, Windows.Visibility.Collapsed)
        End If
    End Function

    ''' <summary>Throws a <see cref="NotSupportedException"/></summary>
    ''' <returns>Never returns. This function always throws a <see cref="NotSupportedException"/>.</returns>
    ''' <param name="value">Ignored.</param>
    ''' <param name="targetType">Ignored.</param>
    ''' <param name="parameter">Ignored.</param>
    ''' <param name="culture">Ignored.</param>
    ''' <exception cref="NotSupportedException">This function always throws a <see cref="NotSupportedException"/>, because <see cref="EnumInConverter"/> does not support backward conversion.</exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException("{0} cannot convert back".f([GetType].Name))
    End Function
End Class


''' <summary><see cref="IValueConverter"/> which performs conversion between <see cref="String"/> and <see cref="SecureString"/></summary>
''' <remarks>Converting <see cref="SecureString"/> to plain <see cref="String"/> causes string data to be stored plain in memory which can be security risk.</remarks>
''' <seelaso cref="String"/><seelaso cref="SecureString"/>
Public Class SecureStringConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value.
    ''' When <paramref name="value"/> is <see cref="String"/> it's converted to <see cref="SecureString"/> (unless <paramref name="targetType"/> is <see cref="String"/>).
    ''' When <paramref name="value"/> is <see cref="SecureString"/> it's converted to <see cref="String"/> (unless <paramref name="targetType"/> is <see cref="SecureString"/>).
    ''' Returns null whan <paramref name="value"/> is null.
    ''' </returns>
    ''' <param name="value">The value produced by the binding source (value to be converted). It must be either <see cref="String"/> or <see cref="SecureString"/>.</param>
    ''' <param name="targetType">The type of the binding target property. The type must <see cref="Type.IsAssignableFrom">be assignable form</see> either <see cref="String"/> or <see cref="SecureString"/>.</param>
    ''' <param name="parameter">The converter parameter to use. Ignored.</param>
    ''' <param name="culture">The culture to use in the converter. Ignored.</param>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is neither <see cref="String"/>, <see cref="SecureString"/> or null. -or- <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable</see> neither from <see cref="String"/> nor from <see cref="SecureString"/>.</exception>
    ''' <remarks>This method implements both - <see cref="IValueConverter.Convert"/> and <see cref="IValueConverter.ConvertBack"/>.</remarks>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert, IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        If TypeOf value Is String Then
            If targetType.IsAssignableFrom(GetType(SecureString)) Then
                Dim ret As New SecureString
                For Each ch In DirectCast(value, String)
                    ret.AppendChar(ch)
                Next
                Return ret
            ElseIf targetType.IsAssignableFrom(GetType(String)) Then
                Return value
            Else
                Throw New NotSupportedException("{0} can convert only to {1} and {2}".f([GetType].Name, GetType(String).Name, GetType(SecureString).Name))
            End If
        ElseIf TypeOf value Is SecureString Then
            If targetType.IsAssignableFrom(GetType(String)) Then
                Return DirectCast(value, SecureString).ToString
            ElseIf targetType.IsAssignableFrom(GetType(SecureString)) Then
                Return value
            Else
                Throw New NotSupportedException("{0} can convert only to {1} and {2}".f([GetType].Name, GetType(String).Name, GetType(SecureString).Name))
            End If
        Else
            Throw New NotSupportedException("{0} can convert only from {1} and {2}".f([GetType].Name, GetType(String).Name, GetType(SecureString).Name))
        End If
    End Function
End Class


''' <summary>Converter that test if value being converted relates to parameter</summary>
''' <remarks>This converter is intended as is one-way.</remarks>
Public Class CompareConverterEx
    Implements IValueConverter
    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value. If <paramref name="value"/> is null or <paramref name="parameter"/> is null, returns null. Otherwise returns boolean value indicating if <paramref name="value"/> equals to <paramref name="parameter"/> using <see cref="System.Object.Equals"/>.</returns>
    ''' <param name="value">The value produced by the binding source. Thsi value will be compared for equality with <paramref name="parameter"/>.</param>
    ''' <param name="targetType">Ignored. Always returns null or <see cref="Boolean"/></param>
    ''' <param name="parameter">Value to compare <paramref name="value"/> with. If parametr is <see cref="String"/> extended comparison is performed. Othervise parameter is tested for equality with <paramref name="value"/>.</param>
    ''' <param name="culture">Culture to convert string values to numbers/dates etc. Ignored when <paramref name="parameter"/> is not string.</param>
    ''' <remarks>Fully supported types are:
    ''' <list type="bullet">
    ''' <item>Null</item>
    ''' <item><see cref="Char"/></item>
    ''' <item><see cref="String"/></item>
    ''' <item><see cref="Boolean"/></item>
    ''' <item><see cref="SByte"/></item>
    ''' <item><see cref="Byte"/></item>
    ''' <item><see cref="Short"/></item>
    ''' <item><see cref="UShort"/></item>
    ''' <item><see cref="Integer"/></item>
    ''' <item><see cref="UInteger"/></item>
    ''' <item><see cref="Long"/></item>
    ''' <item><see cref="ULong"/></item>
    ''' <item><see cref="BigInteger"/></item>
    ''' <item><see cref="Single"/></item>
    ''' <item><see cref="Double"/></item>
    ''' <item><see cref="Decimal"/></item>
    ''' <item><see cref="DateTime"/></item>
    ''' <item><see cref="DateTimeOffset"/></item>
    ''' <item><see cref="TimeSpan"/></item>
    ''' <item><see cref="TimeSpanFormattable"/></item>
    ''' <item><see cref="IntPtr"/></item>
    ''' <item><see cref="UIntPtr"/></item>
    ''' <item><see cref="[Enum]"/></item>
    ''' </list></remarks>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If culture Is Nothing Then culture = System.Globalization.CultureInfo.CurrentCulture
        If value Is Nothing AndAlso Not TypeOf parameter Is String Then Return parameter Is Nothing
        If parameter Is Nothing Then Return value Is Nothing
        If TypeOf parameter Is String Then
            Dim parstr As String = parameter
            Dim compVal As String
            Dim op As String
            If parstr.StartsWith(">=") Then
                op = ">=" : compVal = parstr.Substring(2)
            ElseIf parstr.StartsWith("<=") Then
                op = "<=" : compVal = parstr.Substring(2)
            ElseIf parstr.StartsWith("==") Then
                op = "=" : compVal = parstr.Substring(2)
            ElseIf parstr.StartsWith("=") Then
                op = "=" : compVal = parstr.Substring(1)
            ElseIf parstr.StartsWith("<>") OrElse parstr.StartsWith("!=") Then
                op = "<>" : compVal = parstr.Substring(2)
            ElseIf parstr.StartsWith("<") Then
                op = "<" : compVal = parstr.Substring(1)
            ElseIf parstr.StartsWith(">") Then
                op = ">" : compVal = parstr.Substring(1)
            Else
                op = "="
                compVal = parstr
                GoTo SkipQuotes
            End If
            Dim isString As Boolean = False
            If compVal.StartsWith("""") AndAlso compVal.EndsWith("""") AndAlso compVal <> """" Then
                isString = True
                compVal = compVal.Substring(1, compVal.Length - 2)
            End If
SkipQuotes:
            If value Is Nothing Then
                Dim isNull = (Not isString AndAlso (compVal.ToLowerInvariant = "null" OrElse compVal.ToLowerInvariant = "nothing")) OrElse compVal = ""
                If op = "=" Then Return isNull
                If op = "<>" Then Return Not isNull
                Return False
            ElseIf TypeOf value Is String Then
                Dim ivalue$ = value
                If Not isString AndAlso (compVal.ToLowerInvariant = "null" OrElse compVal.ToLowerInvariant = "nothing") Then
                    If op = "=" Then Return ivalue = ""
                    If op = "<>" Then Return ivalue <> ""
                End If
                Dim res = StringComparer.Create(culture, False).Compare(ivalue, compVal)
                Return (res < 0 AndAlso (op = "<" OrElse op = "<=" OrElse op = "<>")) OrElse
                       (res = 0 AndAlso (op = "=" OrElse op = "<=" OrElse op = ">=")) OrElse
                       (res > 0 AndAlso (op = ">" OrElse op = ">=" OrElse op = "<>"))
            Else
                Dim res As Integer
                If TypeOf value Is [Enum] Then
                    Dim enumType = value.GetType
                    value = DirectCast(value, [Enum]).GetValue
                    Dim enVal As [Enum]
                    Try
                        enVal = [Enum].Parse(enumType, compVal, True)
                    Catch ex As Exception
                        Return False
                    End Try
                    compVal = enVal.GetValue.ToString(culture)
                End If
                If TypeOf value Is Integer Then
                    Dim ival As Integer
                    If Integer.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, Integer).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is UInteger Then
                    Dim ival As UInteger
                    If UInteger.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, UInteger).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Short Then
                    Dim ival As Short
                    If Short.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, Short).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is UShort Then
                    Dim ival As UShort
                    If UShort.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, UShort).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Byte Then
                    Dim ival As Byte
                    If Byte.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, Byte).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is SByte Then
                    Dim ival As SByte
                    If SByte.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, SByte).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is UShort Then
                    Dim ival As ULong
                    If ULong.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, ULong).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Long Then
                    Dim ival As Long
                    If Long.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, Long).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Single Then
                    Dim ival As Single
                    If Single.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, Single).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Double Then
                    Dim ival As Double
                    If Double.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, Double).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Decimal Then
                    Dim ival As Decimal
                    If Decimal.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, Decimal).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is DateTime Then
                    Dim ival As DateTime
                    If DateTime.TryParse(compVal, culture, Globalization.DateTimeStyles.AllowInnerWhite, ival) Then res = DirectCast(value, DateTime).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is DateTimeOffset Then
                    Dim ival As DateTimeOffset
                    If DateTimeOffset.TryParse(compVal, culture, Globalization.DateTimeStyles.AllowWhiteSpaces, ival) Then res = DirectCast(value, DateTimeOffset).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is TimeSpanFormattable Then
                    Dim ival As TimeSpanFormattable
                    If TimeSpanFormattable.TryParse(compVal, culture, ival) Then res = DirectCast(value, TimeSpanFormattable).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is TimeSpan Then
                    Dim ival As TimeSpan
                    If TimeSpan.TryParse(compVal, culture, ival) Then res = DirectCast(value, TimeSpan).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Char Then
                    Dim ival As Char
                    If Char.TryParse(compVal, ival) Then res = DirectCast(value, Char).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is Boolean Then
                    Dim ival As Boolean
                    If Boolean.TryParse(compVal, ival) Then res = DirectCast(value, Boolean).CompareTo(ival) Else Return False
                ElseIf TypeOf value Is IntPtr Then
                    Dim ival As Long
                    If Long.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, IntPtr).ToInt64.CompareTo(ival) Else Return False
                ElseIf TypeOf value Is UIntPtr Then
                    Dim ival As ULong
                    If ULong.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, UIntPtr).ToUInt64.CompareTo(ival) Else Return False
                ElseIf TypeOf value Is BigInteger Then
                    Dim ival As BigInteger = BigInteger.Zero
                    If BigInteger.TryParse(compVal, Globalization.NumberStyles.Any, culture, ival) Then res = DirectCast(value, BigInteger).CompareTo(ival) Else Return False
                ElseIf op = "=" Then
                    Return value.Equals(compVal)
                ElseIf op = "<>" Then
                    Return Not value.Equals(compVal)
                Else
                    Return False
                End If
                Return (res < 0 AndAlso (op = "<" OrElse op = "<=" OrElse op = "<>")) OrElse
                     (res = 0 AndAlso (op = "=" OrElse op = "<=" OrElse op = ">=")) OrElse
                     (res > 0 AndAlso (op = ">" OrElse op = ">=" OrElse op = "<>"))
            End If
        End If
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