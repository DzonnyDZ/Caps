

Imports Tools, Tools.ExtensionsT, Tools.TypeTools
Imports System.ComponentModel

''' <summary>Converts relative path of caps image to absolute path to image of given size</summary>
Public Class CapImageConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        'If Not TypeOf value Is String AndAlso TypeOf value Is IConvertible Then _
        '    value = DirectCast(value, IConvertible).ToString(culture)
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


''' <summary>Gets random top X items from caps table</summary>
Public Class TopRandomConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If TypeOf value Is IQueryable AndAlso TypeOf value Is System.Data.Linq.ITable AndAlso TypeOf DirectCast(value, System.Data.Linq.ITable).Context Is CapsDataDataContext Then
            Dim context As CapsDataDataContext = DirectCast(value, System.Data.Linq.ITable).Context
            Dim list = DirectCast(value, IQueryable)
            Dim count As Integer
            Dim oldc = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture
            Try
                count = TypeTools.DynamicCast(Of Integer)(parameter)
            Finally
                System.Threading.Thread.CurrentThread.CurrentCulture = oldc
            End Try
            Return From item In list Order By context.NewID Take count
        Else : Throw New NotSupportedException(My.Resources.ex_CanConvertOnlyFromValuesImplementingAndHaving.f(Me.GetType.Name, GetType(IQueryable).Name, GetType(System.Data.Linq.ITable).Name, "Context", GetType(CapsDataDataContext).Name))
        End If
    End Function

    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class

''' <summary>gets caps associated with certain object</summary>
Public Class GetCapsOfConverter
    Implements IValueConverter
    ''' <summary>CTor</summary>
    Public Sub New()
    End Sub
    ''' <summary>CTor with data context</summary>
    ''' <param name="Context">Data context to use</param>
    Public Sub New(ByVal Context As CapsDataDataContext)
        Me.Context = Context
    End Sub
    ''' <summary>Contains value of the <see cref="Context"/> property</summary>
    Private _Context As CapsDataDataContext
    ''' <summary>Gtes or sets data context used for querying for caps</summary>
    ''' <exception cref="ArgumentNullException">Value is being set to null</exception>
    Public Property Context() As CapsDataDataContext
        Get
            Return _Context
        End Get
        Set(ByVal value As CapsDataDataContext)
            If value Is Nothing Then Throw New ArgumentNullException("value")
            _Context = value
        End Set
    End Property
    ''' <summary>Performs conversion from object to gets caps associated with it</summary>
    ''' <param name="value">Objects to get caps for. It must be either <see cref="Material"/>, <see cref="CapType"/>, <see cref="Shape"/>, <see cref="MainType"/>, <see cref="Product"/>, <see cref="Company"/>, <see cref="ProductType"/>, <see cref="Storage"/> or <see cref="Target"/> or null.</param>
    ''' <param name="targetType">Ignored. Always returns <see cref="IEnumerable(Of Cap)"/>[<see cref="Cap"/>].</param>
    ''' <param name="parameter">Maximal count of items to be returned. Value must be <see cref="TypeTools.DynamicCast">dynamicly castable</see> to <see cref="Integer"/>.</param>
    ''' <param name="culture">Ignored.</param>
    ''' <returns><see cref="IEnumerable(Of Cap)"/>[<see cref="Cap"/>] containing maximally <paramref name="parameter"/> random items associated by <see cref="Object"/>. Null when <paramref name="value"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is not of supported type.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If Context Is Nothing Then Throw New InvalidOperationException(My.Resources.err_ValueCannotBeNull.f("Context"))
        Dim Count = TypeTools.DynamicCast(Of Integer)(parameter)
        If TypeOf value Is Material Then
            Return From item In Context.Caps Where item.MaterialID = DirectCast(value, Material).MaterialID Order By Context.NewID Take Count
        ElseIf TypeOf value Is CapType Then
            Return From item In Context.Caps Where item.captypeID = DirectCast(value, CapType).CapTypeID Order By Context.NewID Take Count
        ElseIf TypeOf value Is Shape Then
            Return From item In Context.Caps Where item.ShapeID = DirectCast(value, Shape).ShapeID Order By Context.NewID Take Count
        ElseIf TypeOf value Is MainType Then
            Return From item In Context.Caps Where item.MainTypeID = DirectCast(value, MainType).MainTypeID Order By Context.NewID Take Count
        ElseIf TypeOf value Is Product Then
            Return From item In Context.Caps Where item.ProductID = DirectCast(value, Product).ProductID Order By Context.NewID Take Count
        ElseIf TypeOf value Is Company Then
            Return From item In Context.Caps Where item.CompanyID = DirectCast(value, Company).CompanyID Order By Context.NewID Take Count
        ElseIf TypeOf value Is ProductType Then
            Return From item In Context.Caps Where item.ProductTypeID = DirectCast(value, ProductType).ProductTypeID Order By Context.NewID Take Count
        ElseIf TypeOf value Is Storage Then
            Return From item In Context.Caps Where item.StorageID = DirectCast(value, Storage).StorageID Order By Context.NewID Take Count
        ElseIf TypeOf value Is Target Then
            Return From item In Context.Caps Where item.TargetID = DirectCast(value, Target).TargetID Order By Context.NewID Take Count
        ElseIf TypeOf value Is Category Then
            Return From cap In Context.Caps Join c_c In Context.Cap_Category_Ints On Cap.CapID Equals c_c.capid Where c_c.CategoryID = DirectCast(value, Category).CategoryID Order By Context.NewID Take Count
        ElseIf TypeOf value Is Keyword Then
            Return From cap In Context.Caps Join c_k In Context.Cap_Keyword_Ints On Cap.CapID Equals c_k.capid Where c_k.KeywordID = DirectCast(value, Keyword).KeywordID Order By Context.NewID Take Count
        ElseIf value Is Nothing Then
            Return Nothing
        Else
            Throw New TypeMismatchException(My.Resources.ex_UnsupportedTypeOfEntity, value)
        End If
    End Function
    ''' <summary>Converts object back (this operation is not supported)</summary>
    ''' <param name="culture">ignored</param>
    ''' <param name="parameter">ignored</param>
    ''' <param name="targetType">ignored</param>
    ''' <param name="value">ignored unless null</param>
    ''' <returns>If <paramref name="value"/> is null returns null.</returns>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is not null.</exception>
    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class