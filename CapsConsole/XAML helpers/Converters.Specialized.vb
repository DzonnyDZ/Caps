Imports Caps.Data

Imports Tools, Tools.ExtensionsT, Tools.TypeTools
Imports System.ComponentModel

''' <summary>Converts relative path of caps image to absolute path to image of given size or to <see cref="BitmapImage"/> based on such path</summary>
Public Class CapImageConverter
    Implements IValueConverter

    ''' <summary>Converts a value. </summary>
    ''' <returns>A converted value. Either <see cref="String"/> or <see cref="BitmapSource"/> depending on <paramref name="targetType"/>. Null if <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="BitmapSource"/> but requested image file does not exist.</returns>
    ''' <param name="value">The value produced by the binding source. It must be <see cref="String"/>.</param>
    ''' <param name="targetType">The type of the binding target property. If this parameter is <see cref="String"/>, <see cref="String"/> is returned; otherwise if type passed to this parameters <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="BitmapImage"/>, <see cref="BitmapImage"/> is returned.</param>
    ''' <param name="parameter">Can define <see cref="Integer"/> value - maximum size of image.</param>
    ''' <param name="culture">The culture to use in the converter. (ignored)</param>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is not string</exception>
    ''' <exception cref="ArgumentException"><paramref name="parameter"/> is neither null nor <see cref="Integer"/>.</exception>
    ''' <exception cref="NotSupportedException"><paramref name="targetType"/> is neither <see cref="String"/> nor type <see cref="Type.IsAssignableFrom">assignable from</see> <see cref="BitmapImage"/>.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        'If Not TypeOf value Is String AndAlso TypeOf value Is IConvertible Then _
        '    value = DirectCast(value, IConvertible).ToString(culture)
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Dim folders$()
        If parameter Is Nothing OrElse (TypeOf parameter Is Integer AndAlso DirectCast(parameter, Integer) > 256) Then
            folders = New String() {"original", "256_256", "64_64"}
        ElseIf TypeOf parameter Is Integer AndAlso DirectCast(parameter, Integer) <= 64 Then
            folders = New String() {"64_64", "256_256", "original"}
        ElseIf TypeOf parameter Is Integer AndAlso DirectCast(parameter, Integer) <= 256 Then
            folders = New String() {"256_256", "64_64", "original"}
        Else
            Throw New ArgumentException(My.Resources.ex_CapImageConverterParameter, "parameter")
        End If
        Dim path$ = Nothing
        Dim found As Boolean = False
        For Each folder In folders
            path = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, folder), value)
            If IO.File.Exists(path) Then found = True : Exit For
        Next
        If Not found Then path = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "original"), value)
        If targetType.Equals(GetType(String)) Then Return path
        If Not targetType.IsAssignableFrom(GetType(BitmapImage)) Then Throw New NotSupportedException(My.Resources.err_CanConvertOnlyTo1And2.f(Me.GetType.Name, GetType(String).Name, GetType(BitmapImage).Name))
        If path IsNot Nothing AndAlso IO.File.Exists(path) Then
            Try
                Dim img As New BitmapImage
                img.BeginInit()
                img.CacheOption = BitmapCacheOption.OnLoad
                img.UriSource = New Uri(path)
                img.EndInit()
                Return img
            Catch : End Try
        End If
        Return Nothing
    End Function

    ''' <summary>Converts a value back.</summary>
    ''' <returns>A converted value. Filename obtained from <paramref name="value"/> either as filename forom string path or as last segment of <see cref="BitmapImage.UriSource"/>.</returns>
    ''' <param name="value">The value that is produced by the binding target. This shall be <see cref="String"/> representing path or <see cref="BitmapImage"/></param>
    ''' <param name="targetType">Ignored - this method always returns <see cref="String"/>.</param>
    ''' <param name="parameter">Ignored</param>
    ''' <param name="culture">Ignored</param>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neithrt <see cref="BitmapSource"/> nor <see cref="String"/>.</exception>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        If TypeOf value Is BitmapImage Then
            Dim uri = DirectCast(value, BitmapImage).UriSource
            If uri Is Nothing Then Return Nothing
            Return uri.Segments.Last
        End If
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Return IO.Path.GetFileName(value)
    End Function
End Class

''' <summary>Converts image ID of given type to path to that image or <see cref="BitmapSource"/> of that image</summary>
Public Class MyImageIDConverter
    Implements IValueConverter

    ''' <summary>Converts a value. </summary>
    ''' <returns>A converted value. Depending on <paramref name="targetType"/> its path to image file of <see cref="BitmapImage"/> initialized to that path. If <paramref name="targetType"/> <see cref="Type.IsAssignableFrom">is assignable from</see> <see cref="BitmapImage"/> and requested file does not exist, returns null.</returns>
    ''' <param name="value">The value produced by the binding source. String representation of this value shall be intergal number.</param>
    ''' <param name="targetType">The type of the binding target property. It shall be <see cref="String"/> or type <see cref="Type.IsAssignableFrom">assignable from</see> <see cref="BitmapImage"/>.</param>
    ''' <param name="parameter">String value indicating type of object to get image for. In non-string is passes <see cref="[Object].ToString"/> is used.</param>
    ''' <param name="culture">Ignored - <see cref="System.Globalization.CultureInfo.InvariantCulture"/> is used.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="parameter"/> is null.</exception>
    ''' <exception cref="NotSupportedException"><paramref name="targetType"/> is neither <see cref="String"/> nor type <see cref="Type.IsAssignableFrom">assignable from</see> <see cref="BitmapImage"/></exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If parameter Is Nothing Then Throw New ArgumentNullException("parameter")
        Dim Path = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, parameter.ToString), String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", value) & ".png")
        If targetType.Equals(GetType(String)) Then Return Path
        If Not targetType.IsAssignableFrom(GetType(BitmapImage)) Then Throw New NotSupportedException(My.Resources.err_CanConvertOnlyTo1And2.f(Me.GetType.Name, GetType(String).Name, GetType(BitmapImage).Name))
        If targetType.IsAssignableFrom(GetType(BitmapImage)) AndAlso IO.File.Exists(Path) Then
            Try
                Dim img As New BitmapImage
                img.BeginInit()
                img.CacheOption = BitmapCacheOption.OnLoad
                img.UriSource = New Uri(Path)
                img.EndInit()
                Return img
            Catch : End Try
        End If
        Return Nothing
    End Function

    ''' <summary>Converts a value back.</summary>
    ''' <returns>A converted value. <see cref="Integer"/> obtained as file name (without extension) from <paramref name="value"/>.</returns>
    ''' <param name="value">The value that is produced by the binding target. It shall be <see cref="String"/> or <see cref="BitmapImage"/></param>
    ''' <param name="targetType">Ignored - this method always returns <see cref="String"/>.</param>
    ''' <param name="parameter">Ignored</param>
    ''' <param name="culture">Ignored</param>
    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim path As String
        If TypeOf value Is String Then
            path = value
        ElseIf TypeOf value Is BitmapImage Then
            If DirectCast(value, BitmapImage).UriSource Is Nothing Then Return Nothing
            path = DirectCast(value, BitmapImage).UriSource.Segments.Last
        Else
            Throw New TypeMismatchException("value", value, GetType(String))
        End If
        Return Integer.Parse(IO.Path.GetFileNameWithoutExtension(path), System.Globalization.CultureInfo.InvariantCulture)
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
        If TypeOf value Is IQueryable AndAlso GetType(System.Data.Objects.ObjectSet(Of )).MakeGenericType(DirectCast(value, IQueryable).ElementType).IsAssignableFrom(value.GetType) AndAlso TypeOf value.Context Is CapsDataContext Then
            Dim context As CapsDataContext = value.Context
            Dim list = DirectCast(value, IQueryable)
            Dim count As Integer
            Dim oldc = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture
            Try
                count = TypeTools.DynamicCast(Of Integer)(parameter)
            Finally
                System.Threading.Thread.CurrentThread.CurrentCulture = oldc
            End Try
            Return From item As Object In list Order By context.NewID Take count
        Else : Throw New NotSupportedException(My.Resources.ex_CanConvertOnlyFromValuesImplementingAndHaving.f(Me.GetType.Name, GetType(IQueryable).Name, GetType(System.Data.Objects.ObjectSet(Of )).Name, "Context", GetType(CapsDataContext).Name))
        End If
    End Function

    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class

''' <summary>gets caps associated with certain object</summary>
Public Class GetCapsOfConverter
    Implements IValueConverter, IDisposable
    ''' <summary>Context to be used when <see cref="Context"/> is not set</summary>
    Private OriginalContext As CapsDataContext = If(Main.Connection IsNot Nothing, New CapsDataContext(Main.Connection), Nothing) 'If(...) - for designer
    ''' <summary>CTor</summary>
    Public Sub New()
    End Sub
    ''' <summary>CTor with data context</summary>
    ''' <param name="Context">Data context to use</param>
    Public Sub New(ByVal Context As CapsDataContext)
        Me.Context = Context
    End Sub
    ''' <summary>Contains value of the <see cref="Context"/> property</summary>
    Private _Context As CapsDataContext
    ''' <summary>Gtes or sets data context used for querying for caps</summary>
    Public Property Context() As CapsDataContext
        <DebuggerStepThrough()> Get
            Return If(_Context, OriginalContext)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As CapsDataContext)
            If value Is Context Then Exit Property
            If Not OriginalContext.IsDisposed Then OriginalContext.Dispose()
            _Context = value
        End Set
    End Property
    ''' <summary>Performs conversion from object to gets caps associated with it</summary>
    ''' <param name="value">Objects to get caps for. It must imnplement <see cref="IRelatedToCap"/> or be null.</param>
    ''' <param name="targetType">Ignored. Always returns <see cref="IEnumerable(Of Cap)"/>[<see cref="Cap"/>].</param>
    ''' <param name="parameter">Maximal count of items to be returned. Value must be <see cref="TypeTools.DynamicCast">dynamicly castable</see> to <see cref="Integer"/>.</param>
    ''' <param name="culture">Ignored.</param>
    ''' <returns><see cref="IEnumerable(Of Cap)"/>[<see cref="Cap"/>] containing maximally <paramref name="parameter"/> random items associated by <see cref="Object"/>. Null when <paramref name="value"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="value"/> is neither null nor <see cref="IRelatedToCap"/></exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If Context Is Nothing Then Throw New InvalidOperationException(My.Resources.err_ValueCannotBeNull.f("Context"))
        Dim Count = TypeTools.DynamicCast(Of Integer)(parameter)
        If TypeOf value Is IRelatedToCap Then
            Return From item In DirectCast(value, IRelatedToCap).Caps Order By NewGuid Take Count
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

#Region "IDisposable Support"
    ''' <summary> To detect redundant calls to <see cref="Dispose"/></summary>
    Private disposedValue As Boolean

    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True when disposing</param>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                OriginalContext.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub
    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

''' <summary>Converts ISO-2 country code to path of resource containing flag of that country.</summary>
Public Class CountryCodeFlagConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        Return String.Format("/CapsConsole;component/Resources/Flags/{0}.png", value)
    End Function

    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return Nothing
        Dim last = value.ToString.Split("/"c).Last
        Dim li = last.LastIndexOf("."c)
        If li >= 0 Then
            Return last.Substring(0, li)
        Else
            Return last
        End If
    End Function
End Class

''' <summary>Converts <see cref="Integer"/> representing [A]RGB value of color or color of type <see cref="Color"/> or <see cref="System.Drawing.Color"/> to localized name of the color.</summary>
Public Class IntColorNameConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        If TypeOf value Is Color Then
            value = DirectCast(value, Color).ToColor
        ElseIf Not TypeOf value Is System.Drawing.Color Then
            value = System.Drawing.Color.FromArgb(TypeTools.DynamicCast(Of Integer)(value))
        End If
        Dim color As System.Drawing.Color = value
        Select Case color.ToArgb
            Case System.Drawing.Color.White.ToArgb : Return My.Resources.clr_White
            Case System.Drawing.Color.Black.ToArgb : Return My.Resources.clr_Black
            Case System.Drawing.Color.Gray.ToArgb : Return My.Resources.clr_Gray
            Case System.Drawing.Color.Blue.ToArgb : Return My.Resources.clr_Blue
            Case System.Drawing.Color.Red.ToArgb : Return My.Resources.clr_Red
            Case System.Drawing.Color.Yellow.ToArgb : Return My.Resources.clr_Yellow
            Case System.Drawing.Color.Orange.ToArgb : Return My.Resources.clr_Orange
            Case System.Drawing.Color.Green.ToArgb : Return My.Resources.clr_Green
            Case System.Drawing.Color.Pink.ToArgb : Return My.Resources.clr_Pink
            Case System.Drawing.Color.Brown.ToArgb : Return My.Resources.clr_Brown
            Case System.Drawing.Color.Magenta.ToArgb : Return My.Resources.clr_Magenta
            Case System.Drawing.Color.Silver.ToArgb : Return My.Resources.clr_Silver
            Case System.Drawing.Color.LightBlue.ToArgb : Return My.Resources.clr_LightBlue
            Case System.Drawing.Color.Gold.ToArgb : Return My.Resources.clr_Gold
            Case System.Drawing.Color.Transparent.ToArgb : Return My.Resources.clr_Transparent
            Case System.Drawing.Color.Lime.ToArgb : Return My.Resources.clr_Lime
            Case Else : Return color.Name
        End Select
    End Function


    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If value Is Nothing Then Return System.Drawing.Color.Transparent.ToArgb
        If Not TypeOf value Is String Then Throw New TypeMismatchException("value", value, GetType(String))
        Select Case DirectCast(value, String)
            Case My.Resources.clr_White : Return System.Drawing.Color.White.ToArgb
            Case My.Resources.clr_Black : Return System.Drawing.Color.Black.ToArgb
            Case My.Resources.clr_Gray : Return System.Drawing.Color.Gray.ToArgb
            Case My.Resources.clr_Blue : Return System.Drawing.Color.Blue.ToArgb
            Case My.Resources.clr_Red : Return System.Drawing.Color.Red.ToArgb
            Case My.Resources.clr_Yellow : Return System.Drawing.Color.Yellow.ToArgb
            Case My.Resources.clr_Orange : Return System.Drawing.Color.Orange.ToArgb
            Case My.Resources.clr_Green : Return System.Drawing.Color.Green.ToArgb
            Case My.Resources.clr_Pink : Return System.Drawing.Color.Pink.ToArgb
            Case My.Resources.clr_Brown : Return System.Drawing.Color.Brown.ToArgb
            Case My.Resources.clr_Magenta : Return System.Drawing.Color.Magenta.ToArgb
            Case My.Resources.clr_Silver : Return System.Drawing.Color.Silver.ToArgb
            Case My.Resources.clr_LightBlue : Return System.Drawing.Color.LightBlue.ToArgb
            Case My.Resources.clr_Gold : Return System.Drawing.Color.Gold.ToArgb
            Case My.Resources.clr_Transparent : Return System.Drawing.Color.Transparent.ToArgb
            Case My.Resources.clr_Lime : Return System.Drawing.Color.Lime.ToArgb
            Case Else : Return System.Drawing.Color.FromName(value).ToArgb
        End Select
    End Function
End Class

''' <summary>Converts 2-letters country code to localized country name</summary>
Public Class CountryCodeNameConverter
    Implements IValueConverter
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return Country.GetCountryNameFromCode(value)
    End Function

    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotImplementedException("{0} cannot convert back".f(Me.GetType.Name))
    End Function

End Class