Imports mBox = Tools.WindowsT.IndependentT.MessageBox, Tools.ExtensionsT
Partial Public Class winCapEditor
    Private Cap As DataAccess.Cap
    Private Context As DataAccess.Entities
    ''' <summary>CTor</summary>
    ''' <param name="CapID">ID of Cap to edit</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Cap"/> is null -or- <paramref name="Context"/> is null</exception>
    Public Sub New(ByVal CapID%)
        'If Cap Is Nothing Then Throw New ArgumentNullException("Cap")
        'If Context Is Nothing Then Throw New ArgumentNullException("Context")
        InitializeComponent()
        Context = caeEditor.Context
        Me.Cap = Context.Caps.First(Function(cap) cap.CapID = CapID)
        If Cap Is Nothing Then Throw New ArgumentException(My.Resources.ex_CapIDNotFound.f(CapID))
        'Me.Context = New CapsDataDataContext(Main.Connection)
        caeEditor.Initialize()
        Me.DataContext = Cap
        caeEditor.Keywords = From kw In Cap.Keywords Select kw.KeywordName
        OldKeywords = Cap.Keywords.ToArray
        caeEditor.SelectedCategories = Cap.Categories
        OldCategories = Cap.Categories.ToArray
        caeEditor.Images.AddRange(Cap.Images)
        OldImages = Cap.Images.ToArray
    End Sub
    Private OldCategories As DataAccess.Category()
    Private OldKeywords As DataAccess.Keyword()
    Private OldImages As DataAccess.Image()
    ''' <summary>True when <see cref="winCapEditor_Closing"/> shall do nothing</summary>
    Private IsClosing As Boolean = False

    Private Sub caeEditor_CancelClicked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles caeEditor.CancelClicked
        Me.Close()
    End Sub

    'Private Sub caeEditor_SaveClicked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles caeEditor.SaveClicked
    '    If Not caeEditor.Tests Then Exit Sub
    '    'Introduce new CapType
    '    Dim NewType As DataAccess.CapType = Nothing
    '    If caeEditor.CapTypeSelection = CapEditor.CreatableItemSelection.NewItem Then
    '        If Not caeEditor.TestNewCapType Then Exit Sub
    '        NewType = New DataAccess.CapType With {.TypeName = caeEditor.CapTypeName, _
    '                                         .Description = caeEditor.CapTypeDescription, _
    '                                         .MainType = caeEditor.CapMainType, _
    '                                         .Height = caeEditor.CapHeight, _
    '                                         .Material = caeEditor.Material, _
    '                                         .Size = caeEditor.Size1, _
    '                                         .Size2 = caeEditor.Size2}
    '    End If
    '    'Introduce product
    '    Dim NewProduct As DataAccess.Product = Nothing
    '    If caeEditor.ProductSelection = CapEditor.CreatableItemSelection.NewItem Then
    '        caeEditor.TestNewProduct()
    '        NewProduct = New DataAccess.Product With {.ProductName = caeEditor.ProductName, _
    '                                       .Description = caeEditor.ProductDescription, _
    '                                       .Company = caeEditor.CapCompany, _
    '                                       .ProductType = caeEditor.CapProductType}
    '    End If
    '    'Image files
    '    Dim IntroducedImages As List(Of String) = caeEditor.CopyImages()
    '    If IntroducedImages Is Nothing Then Exit Sub
    '    'Categories
    '    Context.Cap_Category_Ints.DeleteAllOnSubmit(From cat In OldCategories Where Not caeEditor.SelectedCategories.Contains(cat.Category))
    '    Context.Cap_Category_Ints.InsertAllOnSubmit(From cat In caeEditor.SelectedCategories Where Not (From ci In OldCategories Select ci.Category).Contains(cat) Select New Cap_Category_Int(Cap, cat))
    '    'Keywords
    '    Context.Cap_Keyword_Ints.DeleteAllOnSubmit(From kw In OldKeywords Where Not caeEditor.Keywords.Contains(kw.Keyword.Keyword))
    '    Dim KeywordsToAssociate = From kw In caeEditor.Keywords Where Not (From oldk In OldKeywords Select oldk.Keyword.Keyword).Contains(kw) Select kw, DbKw = (From kwdb In Context.Keywords Where kwdb.Keyword = kw).FirstOrDefault
    '    Context.Cap_Keyword_Ints.InsertAllOnSubmit(From kw In KeywordsToAssociate Where kw.DbKw IsNot Nothing Select New Cap_Keyword_Int(Cap, kw.DbKw))
    '    Dim NewKeywords = From kw In KeywordsToAssociate Select New Keyword(kw.kw)
    '    Context.Keywords.InsertAllOnSubmit(NewKeywords)
    '    Context.Cap_Keyword_Ints.InsertAllOnSubmit(From kw In NewKeywords Select New Cap_Keyword_Int(Cap, kw))
    '    'Images
    '    Context.Images.InsertAllOnSubmit(From img In caeEditor.Images.OfType(Of NewImage)())
    '    Context.Images.DeleteAllOnSubmit(From img In OldImages Where Not caeEditor.Images.Contains(img))
    '    For Each NewImage In caeEditor.Images.OfType(Of NewImage)()
    '        NewImage.Cap = Cap
    '    Next
    '    'Prepare for commit
    '    If NewType IsNot Nothing Then Cap.CapType = NewType : Context.CapTypes.InsertOnSubmit(NewType)
    '    If NewProduct IsNot Nothing Then Cap.Product = NewProduct : Context.Products.InsertOnSubmit(NewProduct)
    '    Try
    '        Context.SubmitChanges()
    '    Catch ex As Exception
    '        mBox.Error_XT(ex, My.Resources.txt_ErrorUpdatingCap)
    '        'Undo
    '        Context = caeEditor.ResetContext
    '        Dim OldCap = Cap
    '        Cap = Context.Caps.FirstOrDefault(Function(cap) cap.CapID = OldCap.CapID)
    '        Cap.CapName = OldCap.CapName
    '        Cap.MainText = OldCap.MainText
    '        Cap.SubTitle = OldCap.SubTitle
    '        Cap.MainPicture = OldCap.MainPicture
    '        Cap.AnotherPictures = OldCap.AnotherPictures
    '        Cap.PictureType = OldCap.PictureType
    '        Cap.CapType = Context.CapTypes.FirstOrDefault(Function(itm) itm.CapTypeID = OldCap.CapTypeID)
    '        Cap.MainType = Context.MainTypes.FirstOrDefault(Function(itm) itm.MainTypeID = OldCap.MainTypeID)
    '        Cap.Shape = Context.Shapes.FirstOrDefault(Function(itm) itm.ShapeID = OldCap.ShapeID)
    '        Cap.Size = OldCap.Size
    '        Cap.Size2 = OldCap.Size2
    '        Cap.Height = OldCap.Height
    '        Cap.Material = Context.Materials.FirstOrDefault(Function(itm) itm.MaterialID = OldCap.MaterialID)
    '        Cap.BackColor1 = OldCap.BackColor1
    '        Cap.BackColor2 = OldCap.BackColor2
    '        Cap.ForeColor = Cap.ForeColor
    '        Cap.ForeColor2 = Cap.ForeColor2
    '        Cap.Is3D = OldCap.Is3D
    '        Cap.Surface = OldCap.Surface
    '        Cap.TopText = OldCap.TopText
    '        Cap.SideText = OldCap.Size2
    '        Cap.BottomText = OldCap.BottomText
    '        Cap.Note = OldCap.Note
    '        Cap.Year = OldCap.Year
    '        Cap.CountryCode = OldCap.CountryCode
    '        Cap.Storage = Context.Storages.FirstOrDefault(Function(itm) itm.StorageID = OldCap.StorageID)
    '        Cap.HasBottom = OldCap.HasBottom
    '        Cap.HasSide = OldCap.HasSide
    '        Cap.Product = Context.Products.FirstOrDefault(Function(itm) itm.ProductID = OldCap.ProductID)
    '        Cap.ProductType = Context.ProductTypes.FirstOrDefault(Function(itm) itm.ProductTypeID = OldCap.ProductTypeID)
    '        Cap.Company = Context.Companies.FirstOrDefault(Function(itm) itm.CompanyID = OldCap.CompanyID)
    '        Me.DataContext = Cap
    '        Exit Sub
    '    End Try
    '    If NewType IsNot Nothing Then
    '        caeEditor.CopyTypeImage(NewType)
    '    End If
    '    IsClosing = True
    '    Me.DialogResult = True
    '    Me.Close()
    'End Sub

    Private Sub winCapEditor_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        If Not IsClosing Then
            Me.DialogResult = False
        End If
    End Sub

    Public Shared Function GetSelectionType(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object
        If value Is Nothing Then Return CapEditor.CreatableItemSelection.AnonymousItem Else Return CapEditor.CreatableItemSelection.SelectedItem
    End Function
    Public Shared Function GetGlossy(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object
        Return TypeOf value Is Char AndAlso DirectCast(value, Char) = "G"c
    End Function
    Public Shared Function GetGlossyBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object
        If DirectCast(value, Boolean) Then
            Return "G"c
        Else
            Return "M"c
        End If
    End Function
End Class
