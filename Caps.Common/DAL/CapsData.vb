Imports System.Globalization
Imports System.Data.EntityClient
Imports System.ComponentModel

Namespace Data
#Region "Localization"
    '#Region "FullTranslation"
    '    <DebuggerDisplay("ShapeFullTranslation {Name} for {ShapeID}")>
    '    Partial Class ShapeFullTranslation

    '    End Class
    '    <DebuggerDisplay("SimpleFullTranslation {Name}")>
    '    Partial Class SimpleFullTranslation

    '    End Class
    '    <DebuggerDisplay("CapFullTranslation {CapName} for {CapID}")>
    '    Partial Class CapFullTranslation

    '    End Class

    '#End Region

#Region "Translation"
    <DebuggerDisplay("SimpleTranslation {CultureName} {Name} ({SimpleTranslationID})")>
    Partial Class SimpleTranslation

    End Class
    <DebuggerDisplay("CapTranslation {CultureName} {CapName} ({CapTranslationID})")>
    Partial Class CapTranslation

    End Class
    <DebuggerDisplay("ShapeTranslation {CultureName} {Name} ({ShapeTranslationID})")>
    Partial Class ShapeTranslation

    End Class
#End Region
#End Region

#Region "Support"
    <DebuggerDisplay("StoredImage {FileName} ({StoredImageID})")> _
    Partial Class StoredImage

    End Class
    <DebuggerDisplay("PseudoCategory {Name} ({PseudoCategoryID}) Condition: {Condition}")>
    Partial Class PseudoCategory

    End Class
    <DebuggerDisplay("Image {RelativePath} ({ImageID})")> _
    Partial Class Image

    End Class
#End Region

#Region "Basic objects"
    <DebuggerDisplay("CapInstance ID {CapInstanceID}")>
    Partial Class CapInstance
        Implements ISimpleObject
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Note
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Note = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return CapInstanceID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return "CapInstance " + CapInstanceID
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Throw New NotSupportedException(String.Format(My.Resources.err_PropertyIsNotSupported, "ISimpleObject.Name", Me.GetType.Name))
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "CapInstance"
            End Get
        End Property
#End Region
    End Class

    <DebuggerDisplay("Material {Name} ({MaterialID})")> _
    Partial Class Material
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return MaterialID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return Name
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Name = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "Material"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("CapType {TypeName} ({CapTypeID})")> _
    Partial Class CapType
        Implements ISimpleObject, IRelatedToCap

#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return CapTypeID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return TypeName()
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                TypeName = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "CapType"
            End Get
        End Property
#End Region


        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
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
        Implements IRelatedToCap
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("MainType {TypeName} ({MainTypeID})")> _
    Partial Class MainType
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return MainTypeID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return TypeName()
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                TypeName = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "MainType"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("CapSign {Name} ({CapSignID})")>
    Partial Class CapSign
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return CapSignID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return Name
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Name = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "CapSign"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("Category {CategoryName} ({CategoryID})")> _
    Partial Class Category
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return CategoryID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return CategoryName
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                CategoryName = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "Category"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("Product {ProductName} ({ProductID})")> _
    Partial Class Product
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return ProductID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return ProductName
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                ProductName = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "Product"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("Company {CompanyName} ({CompanyID})")> _
    Partial Class Company
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return CompanyID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return CompanyName
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                CompanyName = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "Company"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("ProductType {ProductTypeName} ({ProductTypeID})")> _
    Partial Class ProductType
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return ProductTypeID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return ProductTypeName
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                ProductTypeName = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "ProductType"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("Target {Name} ({TargetID})")>
    Partial Class Target
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return TargetID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return Name
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Name = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "Target"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("Storage {StorageNumber} ({StorageID})")> _
    Partial Class Storage
        Implements ISimpleObject, IRelatedToCap
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return StorageID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return StorageNumber
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                StorageNumber = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "Storage"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class

    <DebuggerDisplay("StorageType {Name} ({StorageTypeID})")> _
    Partial Class StorageType
        Implements ISimpleObject
#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return Description
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Description = value
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return StorageTypeID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return Name
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Name = value
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "StorageType"
            End Get
        End Property
#End Region
    End Class

    <DebuggerDisplay("Keyword {Keyword} ({KeywordID})")> _
    Partial Class Keyword
        Implements ISimpleObject, IRelatedToCap
        Public Sub New()
        End Sub
        Public Sub New(ByVal Keyword$)
            Me.New()
            Me.KeywordName = Keyword
        End Sub


        ''' <summary>Gets or sets text of the keyword.</summary>
        ''' <remarks>Due to Enitity Framework limitations, this property cannot be used in LINQ queries. Use <see cref="KeywordName"/> instead</remarks>
        <EditorBrowsable(EditorBrowsableState.Never), Obsolete("Due to EF limitations this property cannot be used in LINQ queries. Use KeywordName instead.")> _
        Public Property Keyword() As Global.System.String
            Get
                Return KeywordName
            End Get
            Set(ByVal value As Global.System.String)
                KeywordName = value
            End Set
        End Property

#Region "ISimpleObject"
        Private Property ISimpleObject_Description As String Implements ISimpleObject.Description
            <DebuggerStepThrough()> Get
                Return "Keyword " + KeywordName
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Throw New NotSupportedException(String.Format(My.Resources.err_PropertyIsNotSupported, "ISimpleObject.Description", Me.GetType.Name))
            End Set
        End Property
        Private ReadOnly Property ISimpleObject_ID As Integer Implements ISimpleObject.ID
            <DebuggerStepThrough()> Get
                Return KeywordID
            End Get
        End Property

        Private Property ISimpleObject_Name As String Implements ISimpleObject.Name
            <DebuggerStepThrough()> Get
                Return KeywordName
            End Get
            <DebuggerStepThrough()> Set(ByVal value As String)
                Throw New NotSupportedException(String.Format(My.Resources.err_PropertyIsReadOnly, "ISimpleObject.Name", Me.GetType.Name))
            End Set
        End Property

        Private ReadOnly Property ISimpleObject_ObjectName As String Implements ISimpleObject.ObjectName
            <DebuggerStepThrough()> Get
                Return "Keyword"
            End Get
        End Property
#End Region
        ''' <summary>Gets caps this item is related to</summary>
        Private ReadOnly Property IRelatedToCap_Caps As System.Collections.Generic.IEnumerable(Of Cap) Implements IRelatedToCap.Caps
            Get
                Return Caps
            End Get
        End Property
    End Class
#End Region

    Partial Class CapsDataContext

        ''' <summary>Gets default medata (Entity Framework) workspace to be used with this data context</summary>
        Public Shared ReadOnly Property DefaultMetadataWorkspace As Metadata.Edm.MetadataWorkspace
            Get
                Static ret As New Metadata.Edm.MetadataWorkspace({"res://*/DAL.CapsData.csdl", "res://*/DAL.CapsData.ssdl", "res://*/DAL.CapsData.msl"}, {GetType(CapsDataContext).Assembly})
                Return ret
            End Get
        End Property

        Private _connection As SqlClient.SqlConnection

        ' ''' <summary>CTor from connection string to SQL server</summary>
        ' ''' <param name="connectionString">SQL Server connection string used to create connection to SQL server</param>
        'Public Sub New(ByVal connectionString As String)
        '    MyBase.New(String.Format("metadata=res://*/DAL.CapsData.csdl|res://*/DAL.CapsData.ssdl|res://*/DAL.CapsData.msl;provider=System.Data.SqlClient;provider connection string=""{0}""", connectionString))
        '    OnContextCreated()
        'End Sub


        ' ''' <summary>CTor from SQL server connection</summary>
        ' ''' <param name="connection">Connection to SQL server</param>
        'Public Sub New(ByVal connection As SqlClient.SqlConnection)
        '    Me.New(New EntityConnection(New Metadata.Edm.MetadataWorkspace({"res://*/DAL.CapsData.csdl", "res://*/DAL.CapsData.ssdl", "res://*/DAL.CapsData.msl"},
        '                                                                   {GetType(CapsDataContext).Assembly}),
        '                                connection))
        'End Sub

        '#If DEBUG Then
        '        Private Sub OnCreated()
        '            MyBase.Log = New DebugLog
        '        End Sub
        '#End If
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

        '#Region "Translations"
        '#Region "Cap"
        '        ''' <summary>Returns full trasnslation for given culture of <see cref="Cap"/> represented by ID</summary>
        '        ''' <param name="capID"><see cref="Cap.CapID">CapID</see> of <see cref="Cap"/> to get translation of</param>
        '        ''' <param name="culture">Culture to get translation for</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Cap"/> represented by <paramref name="capID"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
        '        Public Function TranslateCap(ByVal capID%, ByVal culture As CultureInfo) As CapFullTranslation
        '            Dim Table As New HelperDataSet.VarCharTableDataTable
        '            Dim cult As CultureInfo = culture
        '            While cult IsNot Nothing AndAlso Not cult.Equals(CultureInfo.InvariantCulture)
        '                Table.AddVarCharTableRow(cult.Name)
        '                cult = cult.Parent
        '            End While
        '            Dim ta As New HelperDataSetTableAdapters.TranslateCapTableAdapter With {.Connection = Me.Connection}
        '            Using r = ta.Read(capID, Table)
        '                Return Me.Translate(Of CapFullTranslation)(r).FirstOrDefault
        '            End Using
        '        End Function
        '        ''' <summary>Returns full trasnslation for given culture of given <see cref="Cap"/></summary>
        '        ''' <param name="cap"><see cref="Cap"/> to get translation of</param>
        '        ''' <param name="culture">Culture to get translation for</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Cap"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
        '        ''' <exception cref="ArgumentNullException"><paramref name="cap"/> is null</exception>
        '        Public Function TranslateCap(ByVal cap As Cap, ByVal culture As CultureInfo) As CapFullTranslation
        '            If cap Is Nothing Then Throw New ArgumentNullException("cap")
        '            Return TranslateCap(cap.CapID, culture)
        '        End Function
        '        ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of <see cref="Cap"/> represented by ID</summary>
        '        ''' <param name="capID"><see cref="Cap.CapID">CapID</see> of <see cref="Cap"/> to get translation of</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Cap"/> represented by <paramref name="capID"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
        '        Public Function TranslateCap(ByVal capID%) As CapFullTranslation
        '            Return TranslateCap(capID, CultureInfo.CurrentUICulture)
        '        End Function
        '        ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of given <see cref="Cap"/></summary>
        '        ''' <param name="cap"><see cref="Cap"/> to get translation of</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Cap"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
        '        ''' <exception cref="ArgumentNullException"><paramref name="cap"/> is null</exception>
        '        Public Function TranslateCap(ByVal cap As Cap) As CapFullTranslation
        '            Return TranslateCap(cap, CultureInfo.CurrentUICulture)
        '        End Function
        '#End Region
        '#Region "Shape"
        '        ''' <summary>Returns full trasnslation for given culture of <see cref="Shape"/> represented by ID</summary>
        '        ''' <param name="shapeID"><see cref="Shape.ShapeID">ShapeID</see> of <see cref="Shape"/> to get translation of</param>
        '        ''' <param name="culture">Culture to get translation for</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Shape"/> represented by <paramref name="ShapeID"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
        '        Public Function TranslateShape(ByVal shapeID%, ByVal culture As CultureInfo) As ShapeFullTranslation
        '            Dim Table As New HelperDataSet.VarCharTableDataTable
        '            Dim cult As CultureInfo = culture
        '            While cult IsNot Nothing AndAlso Not cult.Equals(CultureInfo.InvariantCulture)
        '                Table.AddVarCharTableRow(cult.Name)
        '                cult = cult.Parent
        '            End While
        '            Dim ta As New HelperDataSetTableAdapters.TranslateShapeTableAdapter With {.Connection = Me.Connection}
        '            Using r = ta.Read(shapeID, Table)
        '                Return Me.Translate(Of ShapeFullTranslation)(r).FirstOrDefault
        '            End Using
        '        End Function
        '        ''' <summary>Returns full trasnslation for given culture of given <see cref="Shape"/></summary>
        '        ''' <param name="shape"><see cref="Shape"/> to get translation of</param>
        '        ''' <param name="culture">Culture to get translation for</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Shape"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
        '        ''' <exception cref="ArgumentNullException"><paramref name="simpleObject"/> is null</exception>
        '        Public Function TranslateShape(ByVal shape As Shape, ByVal culture As CultureInfo) As ShapeFullTranslation
        '            If shape Is Nothing Then Throw New ArgumentNullException("shape")
        '            Return TranslateShape(shape.ShapeID, culture)
        '        End Function
        '        ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of <see cref="Shape"/> represented by ID</summary>
        '        ''' <param name="shapeID"><see cref="Shape.ShapeID">ShapeID</see> of <see cref="Shape"/> to get translation of</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Shape"/> represented by <paramref name="ShapeID"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
        '        Public Function TranslateShape(ByVal shapeID%) As ShapeFullTranslation
        '            Return TranslateShape(shapeID, CultureInfo.CurrentUICulture)
        '        End Function
        '        ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of given <see cref="Shape"/></summary>
        '        ''' <param name="shape"><see cref="Shape"/> to get translation of</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="Shape"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
        '        ''' <exception cref="ArgumentNullException"><paramref name="shape"/> is null</exception>
        '        Public Function TranslateShape(ByVal shape As Shape) As ShapeFullTranslation
        '            Return TranslateShape(shape, CultureInfo.CurrentUICulture)
        '        End Function
        '#End Region
        '#Region "SimpleObject"
        '        ''' <summary>Returns full trasnslation for given culture of simple object represented by ID</summary>
        '        ''' <param name="objectID">ID of simple object to get translation of</param>
        '        ''' <param name="culture">Culture to get translation for</param>
        '        ''' <returns>Translation of localizable properties of given simple object represented by <paramref name="objectID"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
        '        Public Function TranslateSimpleObject(ByVal objectType$, ByVal objectID%, ByVal culture As CultureInfo) As SimpleFullTranslation
        '            Dim Table As New HelperDataSet.VarCharTableDataTable
        '            Dim cult As CultureInfo = culture
        '            While cult IsNot Nothing AndAlso Not cult.Equals(CultureInfo.InvariantCulture)
        '                Table.AddVarCharTableRow(cult.Name)
        '                cult = cult.Parent
        '            End While
        '            Dim ta As New HelperDataSetTableAdapters.TranslateSimpleObjectTableAdapter With {.Connection = Me.Connection}
        '            Using r = ta.Read(objectType, objectID, Table)
        '                Return Me.Translate(Of SimpleFullTranslation)(r).FirstOrDefault
        '            End Using
        '        End Function
        '        ''' <summary>Returns full trasnslation for given culture of given <see cref="ISimpleObject"/></summary>
        '        ''' <param name="SimpleObject"><see cref="ISimpleObject"/> to get translation of</param>
        '        ''' <param name="culture">Culture to get translation for</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="ISimpleObject"/> to given <paramref name="culture"/> or one of its parent cultures.</returns>
        '        ''' <exception cref="ArgumentNullException"><paramref name="simpleObject"/> is null</exception>
        '        Public Function TranslateSimpleObject(ByVal simpleObject As ISimpleObject, ByVal culture As CultureInfo) As SimpleFullTranslation
        '            If simpleObject Is Nothing Then Throw New ArgumentNullException("simpleObject")
        '            Return TranslateSimpleObject(simpleObject.ObjectName, simpleObject.ID, culture)
        '        End Function
        '        ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of simpe object represented by ID</summary>
        '        ''' <param name="objectID">ID of simple object to get translation of</param>
        '        ''' <returns>Translation of localizable properties of given simple object represented by <paramref name="objectID"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
        '        Public Function TranslateSimpleObject(ByVal objectType$, ByVal objectID%) As SimpleFullTranslation
        '            Return TranslateSimpleObject(objectType, objectID, CultureInfo.CurrentUICulture)
        '        End Function
        '        ''' <summary>Returns full trasnslation for <see cref="CultureInfo.CurrentUICulture">current UI culture</see> of given <see cref="ISimpleObject"/></summary>
        '        ''' <param name="SimpleObject"><see cref="ISimpleObject"/> to get translation of</param>
        '        ''' <returns>Translation of localizable properties of given <see cref="ISimpleObject"/> to <see cref="CultureInfo.CurrentUICulture">current UI culture</see> or one of its parent cultures.</returns>
        '        ''' <exception cref="ArgumentNullException"><paramref name="simpleObject"/> is null</exception>
        '        Public Function TranslateSimpleObject(ByVal simpleObject As ISimpleObject) As SimpleFullTranslation
        '            Return TranslateSimpleObject(simpleObject, CultureInfo.CurrentUICulture)
        '        End Function
        '#End Region
        '#End Region

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
            Dim ta As New HelperDataSetTableAdapters.GetSimilarCapsTableAdapter With {.Connection = If(TypeOf Me.Connection Is EntityConnection, DirectCast(Me.Connection, EntityConnection).StoreConnection, Me.Connection)}
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
                Return Me.Translate(Of Cap)(r).ToArray
            End Using
        End Function

    End Class

    '#If DEBUG Then
    '    ''' <summary><see cref="IO.TextWriter"/> over <see cref="Debug"/></summary>
    '    Friend Class DebugLog
    '        Inherits IO.TextWriter
    '        ''' <summary>Contains value of the <see cref="Enabled"/> property</summary>
    '        Private Shared _Enabled As Boolean = True
    '        ''' <summary>Gets or sets value indicating if logging is enabled</summary>
    '        ''' <value>Ture to enable logging (default); false to ignore all logging attempts</value>
    '        Public Shared Property Enabled() As Boolean
    '            Get
    '                Return _Enabled
    '            End Get
    '            Set(ByVal value As Boolean)
    '                _Enabled = value
    '            End Set
    '        End Property
    '        ''' <summary>Gtes value indicating if debuger is attached to this proces and logging is enabled</summary>
    '        ''' <returns>True when <see cref="Enabled"/> is true and <see cref="Debugger.IsAttached"/> is true</returns>
    '        Private ReadOnly Property EnabledInternal() As Boolean
    '            Get
    '                Return Enabled AndAlso Debugger.IsAttached
    '            End Get
    '        End Property

    '        ''' <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
    '        Public Overrides Sub Flush()
    '            If EnabledInternal Then Debug.Flush()
    '        End Sub
    '        ''' <summary>When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"></see> in which the output is written.</summary>
    '        ''' <returns>The Encoding in which the output is written.</returns>
    '        Public Overrides ReadOnly Property Encoding() As System.Text.Encoding
    '            Get
    '                Return System.Text.Encoding.UTF8
    '            End Get
    '        End Property
    '        ''' <summary>Writes a subarray of characters to the text stream.</summary>
    '        ''' <param name="count">The number of characters to write. </param>
    '        ''' <param name="buffer">The character array to write data from. </param>
    '        ''' <param name="index">Starting index in the buffer. </param>
    '        ''' <exception cref="T:System.ArgumentOutOfRangeException">index or count is negative. </exception>
    '        ''' <exception cref="T:System.ArgumentException">The buffer length minus index is less than count. </exception>
    '        ''' <exception cref="T:System.ArgumentNullException">The buffer parameter is null. </exception>
    '        Public Overrides Sub Write(ByVal buffer() As Char, ByVal index As Integer, ByVal count As Integer)
    '            If EnabledInternal Then Write(New String(buffer, index, count))
    '        End Sub
    '        ''' <summary>Writes a string followed by a line terminator to the text stream.</summary>
    '        ''' <param name="value">The string to write. If value is null, only the line termination characters are written. </param>
    '        Public Overrides Sub Write(ByVal value As String)
    '            If EnabledInternal Then Debug.Write(value)
    '        End Sub
    '    End Class
    '#End If
End Namespace