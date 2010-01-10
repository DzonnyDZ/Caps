Imports System.Globalization

<DebuggerDisplay("ShapeFullTranslation {Name} for {ShapeID}")>
Partial Class ShapeFullTranslation

End Class
<DebuggerDisplay("SimpleFullTranslation {Name}")>
Partial Class SimpleFullTranslation

End Class
<DebuggerDisplay("CapFullTranslation {CapName} for {CapID}")>
Partial Class CapFullTranslation

End Class

<DebuggerDisplay("SimpleTranslation {CultureName} {Name} ({SimpleTranslationID})")>
Partial Class SimpleTranslation

End Class
<DebuggerDisplay("CapTranslation {CultureName} {CapName} ({CapTranslationID})")>
Partial Class CapTranslation

End Class
<DebuggerDisplay("ShapeTranslation {CultureName} {Name} ({ShapeTranslationID})")>
Partial Class ShapeTranslation

End Class

<DebuggerDisplay("StoredImage {FileName} ({StoredImageID})")> _
Partial Class StoredImage

End Class

<DebuggerDisplay("CapInstance ID {CapInstanceID}")>
Partial Class CapInstance
    Implements ISimpleObject
End Class

<DebuggerDisplay("Cap {CapID} <-> CapSign {CapSignID}")>
Partial Class Cap_CapSign_Int
    Public Sub New(ByVal Cap As Cap, ByVal CapSign As CapSign)
        Me.New()
        Me.Cap = Cap
        Me.CapSign = CapSign
    End Sub
End Class

<DebuggerDisplay("Cap {CapID} <-> PseudoCategory {PseudoCategoryID}")> _
Partial Class Cap_PseudoCategory_Int

End Class

<DebuggerDisplay("PseudoCategory {Name} ({PseudoCategoryID}) Condition: {Condition}")>
Partial Class PseudoCategory

End Class

<DebuggerDisplay("Image {RelativePath} ({ImageID})")> _
Partial Class Image

End Class
<DebuggerDisplay("Material {Name} ({MaterialID})")> _
Partial Class Material
    Implements ISimpleObject
End Class

<DebuggerDisplay("CapType {TypeName} ({CapTypeID})")> _
Partial Class CapType
    Implements ISimpleObject
End Class

<DebuggerDisplay("Cap {CapName} ({CapID})")> _
Partial Class Cap
    ''' <summary>Gets the <see cref="Images"/> collection ordered</summary>
    ''' <returns>The <see cref="Images"/> collection ordered by <see cref="Image.IsMain"/> descending first and then by <see cref="Image.RelativePath"/> ascending</returns>
    Public ReadOnly Property ImagesOrdered() As IOrderedEnumerable(Of Image)
        Get
            Return From img In Images Order By img.IsMain Descending, img.RelativePath Ascending
        End Get
    End Property
End Class
<DebuggerDisplay("Shape {Name} ({ShapeID})")> _
Partial Class Shape

End Class

<DebuggerDisplay("MainType {TypeName} ({MainTypeID})")> _
Partial Class MainType
    Implements ISimpleObject
End Class
<DebuggerDisplay("CapSign {Name} ({CapSignID})")>
Partial Class CapSign
    Implements ISimpleObject
End Class

<DebuggerDisplay("Category {CategoryName} ({CategoryID})")> _
Partial Class Category
    Implements ISimpleObject
End Class


<DebuggerDisplay("Product {ProductName} ({ProductID})")> _
Partial Class Product
    Implements ISimpleObject
End Class

<DebuggerDisplay("Company {CompanyName} ({CompanyID})")> _
Partial Class Company
    Implements ISimpleObject
End Class
<DebuggerDisplay("ProductType {ProducTypeName} ({ProductTypeID})")> _
Partial Class ProductType
    Implements ISimpleObject
End Class
<DebuggerDisplay("Target {Name} ({TargetID})")>
Partial Class Target
    Implements ISimpleObject
End Class
<DebuggerDisplay("Storage {StorageNumber} ({StorageID})")> _
Partial Class Storage
    Implements ISimpleObject
End Class
<DebuggerDisplay("StorageType {Name} ({StorageTypeID})")> _
Partial Class StorageType
    Implements ISimpleObject
End Class

<DebuggerDisplay("Cap {CapID} <-> Category {CategoryID}")> _
Partial Class Cap_Category_Int
    Public Sub New(ByVal Cap As Cap, ByVal Category As Category)
        Me.New()
        Me.Cap = Cap
        Me.Category = Category
    End Sub


End Class
<DebuggerDisplay("Cap {CapID} <-> Keyword {KeywordID}")> _
Partial Class Cap_Keyword_Int
    Public Sub New(ByVal Cap As Cap, ByVal Keyword As Keyword)
        Me.New()
        Me.Cap = Cap
        Me.Keyword = Keyword
    End Sub
End Class

<DebuggerDisplay("Keyword {Keyword} ({KeywordID})")> _
Partial Class Keyword
    Implements ISimpleObject
    Public Sub New(ByVal Keyword$)
        Me.New()
        Me._Keyword = Keyword
    End Sub
End Class

Partial Class CapsDataDataContext


#If DEBUG Then
    Private Sub OnCreated()
        MyBase.Log = New DebugLog
    End Sub
#End If
    ''' <summary>Contains value of the <see cref="IsDisposed"/> property</summary>
    Private _isDisposed As Boolean
    ''' <summary>Raised whrn this instance is disposed of finalied</summary>
    Public Event Disposed As EventHandler
    ''' <summary>Gets or stets value indicating if this instance have already been disposed</summary>
    Public ReadOnly Property IsDisposed() As Boolean
        Get
            Return _isDisposed
        End Get
    End Property
    ''' <summary>Releases resources used by the System.Data.Linq.DataContext.</summary>
    ''' <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        MyBase.Dispose(disposing)
        _isDisposed = True
        RaiseEvent Disposed(Me, EventArgs.Empty)
    End Sub
    ''' <summary>Allows an System.Object to attempt to free resources and perform other cleanup operations before the System.Object is reclaimed by garbage collection.</summary>
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        _isDisposed = True
        RaiseEvent Disposed(Me, EventArgs.Empty)
    End Sub

#Region "Translations"
#Region "Cap"
    ''' <summary>Returns full trasnslation for given culture of <see cref="Cap"/> represented by ID</summary>
    ''' <param name="capID"><see cref="Cap.CapID">CapID</see> of <see cref="Cap"/> to get translation of</param>
    ''' <param name="culture">Culture to get translation for</param>
    ''' <returns>Translation of localizable properties of given <see cref="Cap"/> represented by <paramref name="capID"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
    Public Function TranslateCap(ByVal capID%, ByVal culture As CultureInfo) As CapFullTranslation
        Dim Table As New HelperDataSet.VarCharTableDataTable
        Dim cult As CultureInfo = culture
        While cult IsNot Nothing AndAlso Not cult.Equals(CultureInfo.InvariantCulture)
            Table.AddVarCharTableRow(cult.Name)
            cult = cult.Parent
        End While
        Dim ta As New HelperDataSetTableAdapters.TranslateCapTableAdapter With {.Connection = Me.Connection}
        Using r = ta.Read(capID, Table)
            Return Me.Translate(Of CapFullTranslation)(r).FirstOrDefault
        End Using
    End Function
    ''' <summary>Returns full trasnslation for given culture of given <see cref="Cap"/></summary>
    ''' <param name="cap"><see cref="Cap"/> to get translation of</param>
    ''' <param name="culture">Culture to get translation for</param>
    ''' <returns>Translation of localizable properties of given <see cref="Cap"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="cap"/> is null</exception>
    Public Function TranslateCap(ByVal cap As Cap, ByVal culture As CultureInfo) As CapFullTranslation
        If cap Is Nothing Then Throw New ArgumentNullException("cap")
        Return TranslateCap(cap.CapID, culture)
    End Function
    ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of <see cref="Cap"/> represented by ID</summary>
    ''' <param name="capID"><see cref="Cap.CapID">CapID</see> of <see cref="Cap"/> to get translation of</param>
    ''' <returns>Translation of localizable properties of given <see cref="Cap"/> represented by <paramref name="capID"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
    Public Function TranslateCap(ByVal capID%) As CapFullTranslation
        Return TranslateCap(capID, CultureInfo.CurrentUICulture)
    End Function
    ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of given <see cref="Cap"/></summary>
    ''' <param name="cap"><see cref="Cap"/> to get translation of</param>
    ''' <returns>Translation of localizable properties of given <see cref="Cap"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="cap"/> is null</exception>
    Public Function TranslateCap(ByVal cap As Cap) As CapFullTranslation
        Return TranslateCap(cap, CultureInfo.CurrentUICulture)
    End Function
#End Region
#Region "Shape"
    ''' <summary>Returns full trasnslation for given culture of <see cref="Shape"/> represented by ID</summary>
    ''' <param name="shapeID"><see cref="Shape.ShapeID">ShapeID</see> of <see cref="Shape"/> to get translation of</param>
    ''' <param name="culture">Culture to get translation for</param>
    ''' <returns>Translation of localizable properties of given <see cref="Shape"/> represented by <paramref name="ShapeID"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
    Public Function TranslateShape(ByVal shapeID%, ByVal culture As CultureInfo) As ShapeFullTranslation
        Dim Table As New HelperDataSet.VarCharTableDataTable
        Dim cult As CultureInfo = culture
        While cult IsNot Nothing AndAlso Not cult.Equals(CultureInfo.InvariantCulture)
            Table.AddVarCharTableRow(cult.Name)
            cult = cult.Parent
        End While
        Dim ta As New HelperDataSetTableAdapters.TranslateShapeTableAdapter With {.Connection = Me.Connection}
        Using r = ta.Read(ShapeID, Table)
            Return Me.Translate(Of ShapeFullTranslation)(r).FirstOrDefault
        End Using
    End Function
    ''' <summary>Returns full trasnslation for given culture of given <see cref="Shape"/></summary>
    ''' <param name="shape"><see cref="Shape"/> to get translation of</param>
    ''' <param name="culture">Culture to get translation for</param>
    ''' <returns>Translation of localizable properties of given <see cref="Shape"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="simpleObject"/> is null</exception>
    Public Function TranslateShape(ByVal shape As Shape, ByVal culture As CultureInfo) As ShapeFullTranslation
        If shape Is Nothing Then Throw New ArgumentNullException("shape")
        Return TranslateShape(shape.ShapeID, culture)
    End Function
    ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of <see cref="Shape"/> represented by ID</summary>
    ''' <param name="shapeID"><see cref="Shape.ShapeID">ShapeID</see> of <see cref="Shape"/> to get translation of</param>
    ''' <returns>Translation of localizable properties of given <see cref="Shape"/> represented by <paramref name="ShapeID"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
    Public Function TranslateShape(ByVal shapeID%) As ShapeFullTranslation
        Return TranslateShape(ShapeID, CultureInfo.CurrentUICulture)
    End Function
    ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of given <see cref="Shape"/></summary>
    ''' <param name="shape"><see cref="Shape"/> to get translation of</param>
    ''' <returns>Translation of localizable properties of given <see cref="Shape"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="shape"/> is null</exception>
    Public Function TranslateShape(ByVal shape As Shape) As ShapeFullTranslation
        Return TranslateShape(Shape, CultureInfo.CurrentUICulture)
    End Function
#End Region
#Region "SimpleObject"
    ''' <summary>Returns full trasnslation for given culture of simple object represented by ID</summary>
    ''' <param name="objectID">ID of simple object to get translation of</param>
    ''' <param name="culture">Culture to get translation for</param>
    ''' <returns>Translation of localizable properties of given simple object represented by <paramref name="objectID"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
    Public Function TranslateSimpleObject(ByVal objectType$, ByVal objectID%, ByVal culture As CultureInfo) As SimpleFullTranslation
        Dim Table As New HelperDataSet.VarCharTableDataTable
        Dim cult As CultureInfo = culture
        While cult IsNot Nothing AndAlso Not cult.Equals(CultureInfo.InvariantCulture)
            Table.AddVarCharTableRow(cult.Name)
            cult = cult.Parent
        End While
        Dim ta As New HelperDataSetTableAdapters.TranslateSimpleObjectTableAdapter With {.Connection = Me.Connection}
        Using r = ta.Read(objectType, objectID, Table)
            Return Me.Translate(Of SimpleFullTranslation)(r).FirstOrDefault
        End Using
    End Function
    ''' <summary>Returns full trasnslation for given culture of given <see cref="ISimpleObject"/></summary>
    ''' <param name="SimpleObject"><see cref="ISimpleObject"/> to get translation of</param>
    ''' <param name="culture">Culture to get translation for</param>
    ''' <returns>Translation of localizable properties of given <see cref="ISimpleObject"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="simpleObject"/> is null</exception>
    Public Function TranslateSimpleObject(ByVal simpleObject As ISimpleObject, ByVal culture As CultureInfo) As SimpleFullTranslation
        If simpleObject Is Nothing Then Throw New ArgumentNullException("simpleObject")
        Return TranslateSimpleObject(simpleObject.ObjectName, simpleObject.ID, culture)
    End Function
    ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of simpe object represented by ID</summary>
    ''' <param name="objectID">ID of simple object to get translation of</param>
    ''' <returns>Translation of localizable properties of given simple object represented by <paramref name="objectID"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
    Public Function TranslateSimpleObject(ByVal objectType$, ByVal objectID%) As SimpleFullTranslation
        Return TranslateSimpleObject(objectType, objectID, CultureInfo.CurrentUICulture)
    End Function
    ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of given <see cref="ISimpleObject"/></summary>
    ''' <param name="SimpleObject"><see cref="ISimpleObject"/> to get translation of</param>
    ''' <returns>Translation of localizable properties of given <see cref="ISimpleObject"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="simpleObject"/> is null</exception>
    Public Function TranslateSimpleObject(ByVal simpleObject As ISimpleObject) As SimpleFullTranslation
        Return TranslateSimpleObject(simpleObject, CultureInfo.CurrentUICulture)
    End Function
#End Region
#End Region

    ''' <summary>Opens reader to read <see cref="Cap">Caps</see> similar to cap with given properties</summary>
    ''' <param name="CapTypeID">ID of <see cref="CapType"/></param>
    ''' <param name="MainTypeID">ID of <see cref="MainType"/></param>
    ''' <param name="ShapeID">ID of <see cref="Shape"/></param>
    ''' <param name="CapName">Name of cap</param>
    ''' <param name="MainText">Main text of cap</param>
    ''' <param name="SubTitle">Sub-main text of cap</param>
    ''' <param name="BackColor1">Primary background color of cap (ARGB value)</param>
    ''' <param name="BackColor2">Secondary background color of cap (ARGB value)</param>
    ''' <param name="ForeColor">Primary fore color of cap (ARGB value)</param>
    ''' <param name="MainPicture">Description of main picture fo cap</param>
    ''' <param name="TopText">Full text of cap top side</param>
    ''' <param name="SideText">Full text of cap side side</param>
    ''' <param name="BottomText">Full text of cap bottom side</param>
    ''' <param name="MaterialID">ID of <see cref="Material"/></param>
    ''' <param name="Surface">Identifies cap surface - 'G' or 'M'</param>
    ''' <param name="Size">Primary size of cap (in mms)</param>
    ''' <param name="Size2">Secondary size of cap (in mms)</param>
    ''' <param name="Height">Height of cap (in mms)</param>
    ''' <param name="Is3D">Idicates if cap surface is three-dimensional or not</param>
    ''' <param name="Year">Yer whan cap was found</param>
    ''' <param name="CountryCode">Code (ISO-3) of country where the cap was found</param>
    ''' <param name="Note">Note on cap</param>
    ''' <param name="CompanyID">ID of <see cref="Company"/></param>
    ''' <param name="ProductID">ID of <see cref="Product"/></param>
    ''' <param name="ProductTypeID">ID of <see cref="ProductType"/></param>
    ''' <param name="StorageID">ID of <see cref="Storage"/></param>
    ''' <param name="ForeColor2">Secondary foreground color of cap (ARGB value)</param>
    ''' <param name="PictureType">Type of picture of cap - 'P', 'D', 'L' or 'G'</param>
    ''' <param name="HasBottom">Value indicating if cap has significant bottom side</param>
    ''' <param name="HasSide">Value indicating if cap has significant side side</param>
    ''' <param name="AnotherPictures">Describes all but main pictures of cap</param>
    ''' <param name="CategoryIDs">IDs of <see cref="Category">Categories</see></param>
    ''' <param name="Keywords">Keywords</param>
    ''' <param name="CountryOfOrigin">Code (ISO-3) of country cap originates from</param>
    ''' <param name="IsDrink">Indicates if product is dring or not</param>
    ''' <param name="State">Cap state 1-5 (1-best, 5-worst)</param>
    ''' <param name="TargetID">Id of <see cref="Target"/></param>
    ''' <param name="IsAlcoholic">Indicates if cap product is alcoholic</param>
    ''' <param name="CapSignIDs">IDs of <see cref="CapSign">CapSigns</see></param>
    ''' <returns><see cref="Cap">Caps</see> similar to caps represented by parameters of this procedure ordered from most similar to least similar</returns>
    Public Function GetSimilarCaps(
                ByVal CapTypeID%?, ByVal MainTypeID%?, ByVal ShapeID%?, ByVal CapName$,
                ByVal MainText$, ByVal SubTitle$, ByVal BackColor1%?, ByVal BackColor2%?,
                ByVal ForeColor%?, ByVal MainPicture$, ByVal TopText$, ByVal SideText$,
                ByVal BottomText$, ByVal MaterialID%?, ByVal Surface As Char?, ByVal Size%?,
                ByVal Size2%?, ByVal Height%?, ByVal Is3D As Boolean?, ByVal Year%?,
                ByVal CountryCode$, ByVal Note$, ByVal CompanyID%?, ByVal ProductID%?,
                ByVal ProductTypeID%?, ByVal StorageID%?, ByVal ForeColor2%?, ByVal PictureType As Char?,
                ByVal HasBottom As Boolean?, ByVal HasSide As Boolean?, ByVal AnotherPictures$, ByVal CategoryIDs%(),
                ByVal Keywords$(), ByVal CountryOfOrigin$, ByVal IsDrink As Boolean?, ByVal State As Short?,
                ByVal TargetID%?, ByVal IsAlcoholic%?, ByVal CapSignIDs%()) As IEnumerable(Of Cap)
        Dim ta As New HelperDataSetTableAdapters.GetSimilarCapsTableAdapter With {.Connection = Me.Connection}
        Using r = ta.ReadSimilarCaps(CapTypeID:=CapTypeID, MainTypeID:=MainTypeID, ShapeID:=ShapeID, CapName:=CapName,
                                     MainText:=MainText, SubTitle:=SubTitle, BackColor1:=BackColor1, BackColor2:=BackColor2,
                                     ForeColor:=ForeColor, MainPicture:=MainPicture, TopText:=TopText, SideText:=SideText,
                                     BottomText:=BottomText, MaterialID:=MaterialID,
                                     Surface:=If(Surface.HasValue, CStr(Surface.Value), Nothing), Size:=Size,
                                     Size2:=Size2, Height:=Height, Is3D:=Is3D, Year:=Year,
                                     CountryCode:=CountryCode, Note:=Note, CompanyID:=CompanyID, ProductID:=ProductID,
                                     ProductTypeID:=ProductTypeID, StorageID:=StorageID, ForeColor2:=ForeColor2,
                                     PictureType:=If(PictureType.HasValue, CStr(PictureType.Value), Nothing),
                                     HasBottom:=HasBottom, HasSide:=HasSide, AnotherPictures:=AnotherPictures, CategoryIDs:=CategoryIDs,
                                     Keywords:=Keywords, CountryOfOrigin:=CountryOfOrigin, IsDrink:=IsDrink, State:=State,
                                     TargetID:=TargetID, IsAlcoholic:=IsAlcoholic, CapSignIDs:=CapSignIDs)
            Return Me.Translate(Of Cap)(r)
        End Using
    End Function
End Class
#If DEBUG Then
''' <summary><see cref="IO.TextWriter"/> over <see cref="Debug"/></summary>
Friend Class DebugLog
    Inherits IO.TextWriter
    ''' <summary>Contains value of the <see cref="Enabled"/> property</summary>
    Private Shared _Enabled As Boolean = True
    ''' <summary>Gets or sets value indicating if logging is enabled</summary>
    ''' <value>Ture to enable logging (default); false to ignore all logging attempts</value>
    Public Shared Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property
    ''' <summary>Gtes value indicating if debuger is attached to this proces and logging is enabled</summary>
    ''' <returns>True when <see cref="Enabled"/> is true and <see cref="Debugger.IsAttached"/> is true</returns>
    Private ReadOnly Property EnabledInternal() As Boolean
        Get
            Return Enabled AndAlso Debugger.IsAttached
        End Get
    End Property

    ''' <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
    Public Overrides Sub Flush()
        If EnabledInternal Then Debug.Flush()
    End Sub
    ''' <summary>When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"></see> in which the output is written.</summary>
    ''' <returns>The Encoding in which the output is written.</returns>
    Public Overrides ReadOnly Property Encoding() As System.Text.Encoding
        Get
            Return System.Text.Encoding.UTF8
        End Get
    End Property
    ''' <summary>Writes a subarray of characters to the text stream.</summary>
    ''' <param name="count">The number of characters to write. </param>
    ''' <param name="buffer">The character array to write data from. </param>
    ''' <param name="index">Starting index in the buffer. </param>
    ''' <exception cref="T:System.ArgumentOutOfRangeException">index or count is negative. </exception>
    ''' <exception cref="T:System.ArgumentException">The buffer length minus index is less than count. </exception>
    ''' <exception cref="T:System.ArgumentNullException">The buffer parameter is null. </exception>
    Public Overrides Sub Write(ByVal buffer() As Char, ByVal index As Integer, ByVal count As Integer)
        If EnabledInternal Then Write(New String(buffer, index, count))
    End Sub
    ''' <summary>Writes a string followed by a line terminator to the text stream.</summary>
    ''' <param name="value">The string to write. If value is null, only the line termination characters are written. </param>
    Public Overrides Sub Write(ByVal value As String)
        If EnabledInternal Then Debug.Write(value)
    End Sub
End Class
#End If