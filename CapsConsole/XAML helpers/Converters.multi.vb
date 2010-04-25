''' <summary>Implemenst <see cref="IMultiValueConverter"/> which ORs given boolean values</summary>
Public Class OrBooleanConverter
    Implements IMultiValueConverter

    ''' <summary>Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
    ''' <returns>A converted value. OR-ed <see cref="Boolean"/> values form <paramref name="values"/> argument. <c>False</c> when <paramref name="values"/> is null or empty. Null and <see cref="DependencyProperty.UnsetValue"/> values in <paramref name="values"/> are ignored.</returns>
    ''' <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion. Array must contain values <see cref="DynamicCast">dynamicly castable</see> to <see cref="Boolean"/>.</param>
    ''' <param name="targetType">The type of the binding target property. It must <see cref="Type.IsAssignableFrom">be assignable from</see> <see cref="Boolean"/>.</param>
    ''' <param name="parameter">ignored.</param>
    ''' <param name="culture">ignored.</param>
    ''' <exception cref="ArgumentException"><paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is not assignable from</see> <see cref="Boolean"/>.</exception>
    ''' <exception cref="InvalidCastException">Value in <paramref name="values"/> is not <see cref="DynamicCast">dynamically castable</see> to <see cref="Boolean"/>.</exception>
    Public Function Convert(ByVal values() As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IMultiValueConverter.Convert
        If Not targetType.IsAssignableFrom(GetType(Boolean)) Then Throw New ArgumentException("{0} can convert only to {1}".f([GetType].Name, GetType(Boolean).Name))
        If values Is Nothing OrElse values.Length = 0 Then Return False
        Dim ret As Boolean = False
        For Each item In values
            If item Is Nothing OrElse item Is DependencyProperty.UnsetValue Then Continue For
            ret = ret OrElse DynamicCast(Of Boolean)(item)
            If ret Then Return ret
        Next
        Return ret
    End Function

    ''' <summary>Throws a <see cref="NotSupportedException"/></summary>
    ''' <returns>This function never returns. It always throws <see cref="NotSupportedException"/></returns>
    ''' <param name="value">ignored.</param>
    ''' <param name="targetTypes">ignored.</param>
    ''' <param name="parameter">ignored.</param>
    ''' <param name="culture">ignored.</param>
    ''' <exception cref="NotSupportedException">This method always throws <see cref="NotSupportedException"/>, because <see cref="OrBooleanConverter"/> doesnot support backward conversion.</exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetTypes() As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements System.Windows.Data.IMultiValueConverter.ConvertBack
        Throw New NotSupportedException("{0} cannot convert back.".f([GetType].Name))
    End Function
End Class


''' <summary>Implements <see cref="IMultiValueConverter"/> which ANDs given boolean values</summary>
Public Class AndBooleanConverter
    Implements IMultiValueConverter

    ''' <summary>Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
    ''' <returns>A converted value. AND-ed <see cref="Boolean"/> values form <paramref name="values"/> argument. <c>False</c> when <paramref name="values"/> is null or empty. Null and <see cref="DependencyProperty.UnsetValue"/> values in <paramref name="values"/> are ignored.</returns>
    ''' <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion. Array must contain values <see cref="DynamicCast">dynamicly castable</see> to <see cref="Boolean"/>.</param>
    ''' <param name="targetType">The type of the binding target property. It must <see cref="Type.IsAssignableFrom">be assignable from</see> <see cref="Boolean"/>.</param>
    ''' <param name="parameter">ignored.</param>
    ''' <param name="culture">ignored.</param>
    ''' <exception cref="ArgumentException"><paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is not assignable from</see> <see cref="Boolean"/>.</exception>
    ''' <exception cref="InvalidCastException">Value in <paramref name="values"/> is not <see cref="DynamicCast">dynamically castable</see> to <see cref="Boolean"/>.</exception>
    Public Function Convert(ByVal values() As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IMultiValueConverter.Convert
        If Not targetType.IsAssignableFrom(GetType(Boolean)) Then Throw New ArgumentException("{0} can convert only to {1}".f([GetType].Name, GetType(Boolean).Name))
        If values Is Nothing OrElse values.Length = 0 Then Return False
        Dim ret As Boolean = True
        For Each item In values
            If item Is Nothing OrElse item Is DependencyProperty.UnsetValue Then Continue For
            ret = ret AndAlso DynamicCast(Of Boolean)(item)
            If Not ret Then Return ret
        Next
        Return ret
    End Function

    ''' <summary>Throws a <see cref="NotSupportedException"/></summary>
    ''' <returns>This function never returns. It always throws <see cref="NotSupportedException"/></returns>
    ''' <param name="value">ignored.</param>
    ''' <param name="targetTypes">ignored.</param>
    ''' <param name="parameter">ignored.</param>
    ''' <param name="culture">ignored.</param>
    ''' <exception cref="NotSupportedException">This method always throws <see cref="NotSupportedException"/>, because <see cref="AndBooleanConverter"/> doesnot support backward conversion.</exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetTypes() As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements System.Windows.Data.IMultiValueConverter.ConvertBack
        Throw New NotSupportedException("{0} cannot convert back.".f([GetType].Name))
    End Function
End Class