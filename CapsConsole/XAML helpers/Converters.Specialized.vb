Imports Caps.Data
Imports Tools.TypeTools
Imports System.ComponentModel, Tools.WindowsT.InteropT

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
            Return From item As Object In list Order By Guid.NewGuid Take count
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
    Private OriginalContext As CapsDataContext = If(Main.EntityConnection IsNot Nothing, New CapsDataContext(Main.EntityConnection), Nothing) 'If(...) - for designer
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
            Return From item In DirectCast(value, IRelatedToCap).Caps Order By Guid.NewGuid Take Count
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

''' <summary>COnverter which converts <see cref="IObjectWithImage"/> to immage</summary>
''' <remarks>This converter is one-way</remarks>
Public Class ObjectImageConverter
    Implements IValueConverter

    ''' <summary>Converts a value.</summary>
    ''' <returns>A converted value - image of <paramref name="value"/>; null when image is not available or <paramref name="value"/> is null.</returns>
    ''' <param name="value">The value produced by the binding source. It must be one of known types implementing <see cref="IObjectWithImage"/> or <see cref="Cap"/> or <see cref="StoredImage"/>.</param>
    ''' <param name="targetType">The type of the binding target property. Type must be assignable from either <see cref="BitmapImage"/>, <see cref="System.Drawing.Bitmap"/> or <see cref="IO.Stream"/>.</param>
    ''' <param name="parameter">The converter parameter to use. Used only when <paramref name="value"/> is <see cref="Cap"/> or <see cref="Image"/>. Then it must contain string (parseable to integer), integer or value convertible to integer representing requested size of image to be got.</param>
    ''' <param name="culture">ignored</param>
    ''' <exception cref="InvalidCastException"><paramref name="value"/> is <see cref="Cap"/> or <see cref="Image"/> and <paramref name="parameter"/> is neither null nor can be converted to <see cref="Integer"/>.</exception>
    ''' <exception cref="FormatException"><paramref name="value"/> is <see cref="Cap"/> or <see cref="Image"/> and <paramref name="parameter"/> is <see cref="String"/> which cannot be parsed as <see cref="Integer"/></exception>
    ''' <exception cref="OverflowException"><paramref name="value"/> is <see cref="Cap"/> or <see cref="Image"/> and <paramref name="parameter"/> is neither null nor <see cref="Integer"/> and arithmetic overflow occured while converting it to <see cref="Integer"/>.</exception>
    ''' <exception cref="NotSupportedException"><paramref name="value"/> is neither one of supported types nor null. -or- <paramref name="targetType"/> is neither null nor <see cref="Type.IsAssignableFrom">is assignable from</see> one of supported types.</exception>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then Return Nothing
        Dim source As ImageProvider
        If TypeOf value Is Image Then
            Dim expectedSize As Integer = 0
            If TypeOf parameter Is String Then : expectedSize = Int32.Parse(parameter, Globalization.CultureInfo.InvariantCulture)
            ElseIf TypeOf parameter Is Integer Then : expectedSize = DirectCast(parameter, Integer)
            ElseIf parameter IsNot Nothing Then : expectedSize = DynamicCast(Of Integer)(parameter)
            End If
            source = DirectCast(value, Image).GetImage(expectedSize)
        ElseIf TypeOf value Is CapSign Then
            source = DirectCast(value, CapSign).GetImages.FirstOrDefault
        ElseIf TypeOf value Is CapType Then
            source = DirectCast(value, CapType).GetImages.FirstOrDefault
        ElseIf TypeOf value Is MainType Then
            source = DirectCast(value, MainType).GetImages.FirstOrDefault
        ElseIf TypeOf value Is Storage Then
            source = DirectCast(value, Storage).GetImages.FirstOrDefault
        ElseIf TypeOf value Is Shape Then
            source = DirectCast(value, Shape).GetImages.FirstOrDefault
        ElseIf (TypeOf value Is StoredImage) Then
            source = New DatabaseImageProvider(value)
        ElseIf TypeOf value Is Cap Then
            Dim cap As Cap = value
            Dim capImage = (From image In cap.ImagesOrdered Take 1).FirstOrDefault
            If capImage IsNot Nothing Then Return Convert(capImage, targetType, parameter, culture)
            Return Nothing
        Else
            Throw New NotSupportedException(My.Resources.err_TypeIsNotSupported.f(value.GetType.Name, Me.GetType.Name))
        End If
        If source Is Nothing Then Return Nothing
        If targetType.IsAssignableFrom(GetType(BitmapImage)) Then
            Return source.GetImageSource
        ElseIf targetType.IsAssignableFrom(GetType(System.Drawing.Bitmap)) Then
            Return source.GetImageBitmap
        ElseIf targetType.IsAssignableFrom(GetType(IO.Stream)) Then
            Return source.GetImageStream
        Else
            Throw New NotSupportedException(My.Resources.err_TargetTypeNotSupported.f(targetType.FullName))
        End If
    End Function

    Private Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotSupportedException(My.Resources.ex_CannotConvertBack.f(Me.GetType.Name))
    End Function
End Class