Imports Tools.CollectionsT.GenericT, Tools.ExtensionsT
Imports Tools.DrawingT.ImageTools
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
''' <summary>Creates a new cap</summary>
Partial Public Class winNewCap
    ''' <summary>Database context</summary>
    Private Context As New CapsDataDataContext(Main.Connection)

    Private Sub winNewCap_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        Context.Dispose()
    End Sub

    Private Sub winNewCap_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cmbCapType.ItemsSource = New ListWithEvents(Of CapType)(From item In Context.CapTypes Order By item.TypeName)
        cmbMainType.ItemsSource = New ListWithEvents(Of MainType)(From item In Context.MainTypes Order By item.TypeName)
        cmbShape.ItemsSource = New ListWithEvents(Of Shape)(From item In Context.Shapes Order By item.Name)
        cmbMaterial.ItemsSource = New ListWithEvents(Of Material)(From item In Context.Materials Order By item.Name)
        cmbStorage.ItemsSource = New ListWithEvents(Of Storage)(From item In Context.Storages Order By item.StorageNumber)
        cmbProduct.ItemsSource = New ListWithEvents(Of Product)(From item In Context.Products Order By item.ProductName)
        Dim ProductTypesList As ListWithEvents(Of ProductType) = New ListWithEvents(Of ProductType)(From item In Context.ProductTypes Order By item.ProductTypeName)
        ProductTypesList.Add(Nothing)
        cmbProductType.ItemsSource = ProductTypesList
        Dim CompaniesList As ListWithEvents(Of Company) = New ListWithEvents(Of Company)(From item In Context.Companies Order By item.CompanyName)
        CompaniesList.Add(Nothing)
        cmbCompany.ItemsSource = CompaniesList
        lstCategories.ItemsSource = New ListWithEvents(Of CategoryProxy)(From item In Context.Categories Order By item.CategoryName Select New CategoryProxy(item))
        kweKeywords.AutoCompleteStable = New ListWithEvents(Of String)(From item In Context.Keywords Order By item.Keyword Select item.Keyword)
        lvwImages.ItemTemplate = My.Application.Resources("ImageListDataTemplate")
        lvwImages.ItemsSource = New ListWithEvents(Of Image)()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
#Region "New"
    Private Sub btnNewMainType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewMainType.Click
        Dim win As New winNewMainType
        If win.ShowDialog Then
            DirectCast(cmbMainType.ItemsSource, ListWithEvents(Of MainType)).Add(win.NewObject)
            cmbMainType.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub cmbNewShape_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmbNewShape.Click
        Dim win As New winNewShape
        If win.ShowDialog Then
            DirectCast(cmbShape.ItemsSource, ListWithEvents(Of Shape)).Add(win.NewObject)
            cmbShape.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewMaterial_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewMaterial.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.Material)
        If win.ShowDialog Then
            DirectCast(cmbMaterial.ItemsSource, ListWithEvents(Of Material)).Add(DirectCast(win.NewObject, Material))
            cmbMaterial.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewStorage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewStorage.Click
        Dim win As New winNewStorage
        If win.ShowDialog Then
            DirectCast(cmbStorage.ItemsSource, ListWithEvents(Of Storage)).Add(win.NewObject)
            cmbStorage.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewProductType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewProductType.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.ProductType)
        If win.ShowDialog Then
            DirectCast(cmbProductType.ItemsSource, ListWithEvents(Of ProductType)).Add(DirectCast(win.NewObject, ProductType))
            cmbProductType.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewCompany_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewCompany.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.Company)
        If win.ShowDialog Then
            DirectCast(cmbCompany.ItemsSource, ListWithEvents(Of Company)).Add(DirectCast(win.NewObject, Company))
            cmbCompany.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewCategory_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewCategory.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.Category)
        If win.ShowDialog Then
            DirectCast(lstCategories.ItemsSource, ListWithEvents(Of CategoryProxy)).Add(New CategoryProxy(win.NewObject, True))
        End If
    End Sub
#End Region
    ''' <summary>Category proxy that adds <see cref="CategoryProxy.Checked"/> property</summary>
    Private Class CategoryProxy
        ''' <summary>Contains value of the <see cref="Category"/> property</summary>
        Private ReadOnly _Category As Category
        ''' <summary>Contains value of the <see cref="Checked"/> property</summary>
        Private _Checked As Boolean
        ''' <summary>Gets category</summary>
        Public ReadOnly Property Category() As Category
            Get
                Return _Category
            End Get
        End Property
        ''' <summary>Gets value indicating if the category is selected (checked)</summary>
        Public Property Checked() As Boolean
            Get
                Return _Checked
            End Get
            Set(ByVal value As Boolean)
                _Checked = value
            End Set
        End Property
        ''' <summary>CTor</summary>
        ''' <param name="Category">A category</param>
        ''' <param name="Checked">Indicates if category is selected (checked)</param>
        ''' <exception cref="ArgumentNullException"><paramref name="Category"/> is null</exception>
        Public Sub New(ByVal Category As Category, Optional ByVal Checked As Boolean = False)
            If Category Is Nothing Then Throw New ArgumentNullException("Category")
            _Category = Category
            _Checked = Checked
        End Sub
    End Class

    ''' <summary>Regulare expression for image name. It parses out 4 numbers from image name.</summary>
    Private Shared ImageNameRegExp As New System.Text.RegularExpressions.Regex( _
        "(?<Before>.*)(?<Number>[0-9]{4})(\.?<After>.*)", Text.RegularExpressions.RegexOptions.Compiled Or Text.RegularExpressions.RegexOptions.CultureInvariant Or Text.RegularExpressions.RegexOptions.ExplicitCapture)

    Private Sub btnAddImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAddImage.Click
        Dim dlg As New Forms.OpenFileDialog With {.Multiselect = True, .DefaultExt = "jpg", .Filter = My.Resources.fil_ImageTypes}
        If My.Settings.LastImageName <> "" Then
            Dim match = ImageNameRegExp.Match(My.Settings.LastImageName)
            If match.Success Then
                Dim Number As Integer = Integer.Parse(match.Groups!Number.Value, System.Globalization.CultureInfo.InvariantCulture)
                Dim NewNumber As String = (Number + 1).ToString("0000")
                Dim NewPath = match.Groups!Before.Value & NewNumber & match.Groups!After.Value
                Try
                    dlg.FileName = NewPath
                Catch : End Try
            End If
        End If
        If dlg.ShowDialog() Then
            If dlg.FileNames.Length > 0 Then
                DirectCast(lvwImages.ItemsSource, ListWithEvents(Of Image)).AddRange( _
                    From path In dlg.FileNames Select DirectCast(New NewImage(path), Image))
                My.Settings.LastImageName = dlg.FileNames(dlg.FileNames.Length - 1)
            End If
        End If
    End Sub
    ''' <summary>Allows to distinguish image already in database and a new image</summary>
    ''' <remarks><see cref="NewImage.RelativePath"/> does not store relative path, but absolute path.</remarks>
    Private Class NewImage : Inherits Image
        ''' <summary>CTor with path</summary>
        ''' <param name="Path">Path to image</param>
        Public Sub New(ByVal Path As String)
            MyBase.New()
            Me.RelativePath = Path
        End Sub
    End Class

    Private Sub btnBrowseForCapTypeImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBrowseForCapTypeImage.Click
        Dim dlg As New Forms.OpenFileDialog With {.DefaultExt = "png", .Filter = My.Resources.fil_PNG}
        Try
            If txtCapTypeImagePath.Text <> "" Then dlg.FileName = txtCapTypeImagePath.Text
        Catch : End Try
        If dlg.ShowDialog Then
            txtCapTypeImagePath.Text = dlg.FileName
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        Dim Cap As New Cap
        Dim NewType As CapType = Nothing
        Dim NewProduct As Product = Nothing
        'Tests
        If cmbMainType.SelectedItem Is Nothing Then mBox.Modal_PTI(My.Resources.msg_MainTypeMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : cmbMainType.Focus() : Exit Sub
        If cmbShape.SelectedItem Is Nothing Then mBox.Modal_PTI(My.Resources.msg_ShapeMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : cmbShape.Focus() : Exit Sub
        If cmbMainType.SelectedItem Is Nothing Then mBox.Modal_PTI(My.Resources.msg_MaterialMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : cmbMaterial.Focus() : Exit Sub
        If cmbStorage.SelectedItem Is Nothing Then mBox.Modal_PTI(My.Resources.msg_StorageMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : cmbStorage.Focus() : Exit Sub
        If txtCapName.Text = "" Then mBox.Modal_PTI(My.Resources.msg_CapNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : txtCapName.Focus() : Exit Sub
        If DirectCast(lvwImages.ItemsSource, ListWithEvents(Of Image)).Count = 0 Then mBox.Modal_PTI(My.Resources.msg_AtLeastOneImageMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : btnAddImage.Focus() : Exit Sub
        If txtSideText.Text <> "" AndAlso Not chkHasSide.IsChecked Then mBox.Modal_PTI(My.Resources.msg_SideText_HasSide, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : chkHasSide.Focus() : Exit Sub
        If txtBottomText.Text <> "" AndAlso Not chkHasBottom.IsChecked Then mBox.Modal_PTI(My.Resources.msg_BottomText_HasBottom, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : chkHasBottom.Focus() : Exit Sub
        If txtMainPicture.Text <> "" AndAlso (cmbPictureType.SelectedItem Is cmiImageNo OrElse cmbPictureType.SelectedItem Is Nothing) Then mBox.Modal_PTI(My.Resources.msg_MainPicture_PictureType, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : cmbPictureType.Focus() : Exit Sub
        If txtAnotherPictures.Text <> "" AndAlso txtMainPicture.Text = "" Then mBox.Modal_PTI(My.Resources.msg_AnotherPictures_MainPicture, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : txtMainPicture.Focus() : Exit Sub
        If copForeground2.Color.HasValue AndAlso Not copForeground.Color.HasValue Then mBox.Modal_PTI(My.Resources.msg_ForeColor_ForeColor2, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : copForeground.Focus() : Exit Sub
        'New cap type
        If optCapTypeNew.IsChecked Then
            If Not IO.File.Exists(txtCapTypeImagePath.Text) Then
                Select Case mBox.ModalF_PTBIa(My.Resources.msg_FileNotExists_ContinueWOImage, My.Resources.txt_CapTypeImage, mBox.MessageBoxButton.Buttons.Yes Or mBox.MessageBoxButton.Buttons.No, mBox.MessageBoxIcons.Question, txtCapTypeImagePath.Text)
                    Case Forms.DialogResult.Yes
                    Case Else : Exit Sub
                End Select
            ElseIf IO.Path.GetExtension(txtCapTypeImagePath.Text).ToLower <> ".png" Then
                mBox.Modal_PTI(My.Resources.msg_OnlyPNG, My.Resources.txt_CapTypeImage, mBox.MessageBoxIcons.Exclamation)
                Exit Sub
            End If
            If txtCapTypeName.Text = "" Then mBox.Modal_PTI(My.Resources.msg_CapTypeNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : txtCapTypeName.Focus() : Exit Sub
            If (From CapType In Context.CapTypes Where CapType.TypeName = txtCapName.Text).Any Then _
                mBox.Modal_PTI(My.Resources.msg_CapTypeAreadyExists, My.Resources.txt_DuplicateEntry, mBox.MessageBoxIcons.Exclamation) : txtCapTypeName.SelectAll() : txtCapName.Focus() : Exit Sub
            NewType = New CapType With {.TypeName = txtCapTypeName.Text, _
                                        .Description = txtCapTypeDesc.Text, _
                                        .MainType = cmbMainType.SelectedItem, _
                                        .Shape = cmbShape.SelectedItem, _
                                        .Size = nudSize1.Value, _
                                        .Size2 = If(DirectCast(cmbShape.SelectedItem, Shape).Size2Name Is Nothing, New Integer?, CInt(nudSize2.Value)), _
                                        .Height = nudHeight.Value, _
                                        .Material = cmbMaterial.SelectedItem}
            Cap.CapType = NewType
        End If
        'New product
        If optProductNew.IsChecked Then
            If txtProductName.Text = "" Then mBox.Modal_PTI(My.Resources.msg_ProductNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : txtProductName.Focus() : Exit Sub
            If (From Product In Context.Products Where Product.ProductName = txtProductName.Text).Any Then _
                mBox.Modal_PTI(My.Resources.msg_ProductWithAreadyExists, My.Resources.txt_DuplicateEntry, mBox.MessageBoxIcons.Exclamation) : txtProductName.SelectAll() : txtProductName.Focus() : Exit Sub
            NewProduct = New Product With {.ProductName = txtProductName.Text, _
                                           .Description = txtProductDescription.Text, _
                                           .Company = cmbCompany.SelectedItem, _
                                           .ProductType = cmbProductType.SelectedItem}
            Cap.Product = NewProduct
        End If
        'Cap values
        Cap.CapName = txtCapName.Text
        Cap.MainText = txtMainText.Text
        Cap.SubTitle = txtSubTitle.Text
        Cap.MainPicture = txtMainPicture.Text
        Cap.MainType = cmbMainType.SelectedItem
        Cap.Shape = cmbShape.SelectedItem
        Cap.Size = nudSize1.Value
        If DirectCast(cmbShape.SelectedItem, Shape).Size2Name IsNot Nothing Then Cap.Size2 = nudSize2.Value
        Cap.Height = nudHeight.Value
        Cap.Material = cmbMaterial.SelectedItem
        Cap.BackColor1 = System.Drawing.Color.FromArgb(copBackground.Color.Value.A, copBackground.Color.Value.R, copBackground.Color.Value.G, copBackground.Color.Value.B).ToArgb
        If copSecondaryBackground.Color.HasValue Then Cap.BackColor2 = System.Drawing.Color.FromArgb(copSecondaryBackground.Color.Value.A, copSecondaryBackground.Color.Value.R, copSecondaryBackground.Color.Value.G, copSecondaryBackground.Color.Value.B).ToArgb()
        If copForeground.Color.HasValue Then Cap.ForeColor = System.Drawing.Color.FromArgb(copForeground.Color.Value.A, copForeground.Color.Value.R, copForeground.Color.Value.G, copForeground.Color.Value.B).ToArgb()
        If copForeground2.Color.HasValue Then Cap.ForeColor2 = System.Drawing.Color.FromArgb(copForeground2.Color.Value.A, copForeground2.Color.Value.R, copForeground2.Color.Value.G, copForeground.Color.Value.B).ToArgb()
        Cap.Is3D = chk3D.IsChecked
        Cap.Surface = If(optMatting.IsChecked, "M"c, "G"c)
        Cap.TopText = txtTopText.Text
        Cap.SideText = txtSideText.Text
        Cap.BottomText = txtBottomText.Text
        Cap.Note = txtNote.Text
        If nudYear.Value > 0 Then Cap.Year = nudYear.Value
        If txtCountryCode.Text <> "" AndAlso Not txtCountryCode.Text Like "[ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz][ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz][ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz]" Then _
            mBox.Modal_PTI(My.Resources.msg_CountryCodeMustBeISO3Code, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : txtCountryCode.SelectAll() : txtCountryCode.Focus() : Exit Sub
        Cap.CountryCode = txtCountryCode.Text.ToUpperInvariant
        Cap.Storage = cmbStorage.SelectedItem
        Cap.ProductType = cmbProductType.SelectedItem
        Cap.Company = cmbCompany.SelectedItem
        Cap.AnotherPictures = txtAnotherPictures.Text
        Cap.HasBottom = chkHasBottom.IsChecked
        Cap.HasSide = chkHasSide.IsChecked
        Cap.PictureType = If(cmbPictureType.SelectedItem Is cmiImageGeometry, "G"c, _
                          If(cmbPictureType.SelectedItem Is cmiImageLogo, "L"c, _
                          If(cmbPictureType.SelectedItem Is cmiImageDrawing, "D"c, _
                          If(cmbPictureType.SelectedItem Is cmiImagePhoto, "P"c, New Char?))))
        'Categories
        Dim CreatedDBCatInts As New List(Of Cap_Category_Int)
        For Each Category As CategoryProxy In lstCategories.ItemsSource
            If Category.Checked Then CreatedDBCatInts.Add(New Cap_Category_Int(Cap, Category.Category))
        Next
        'Images
        Dim IntroducedImages As List(Of String) = CopyImages()
        If IntroducedImages Is Nothing Then Exit Sub
        'Prepare for commit
        Dim CreatedDBImages = (From item In IntroducedImages Select New Image() With {.Cap = Cap, .RelativePath = item}).ToArray
        Context.Images.InsertAllOnSubmit(CreatedDBImages)
        Dim AllCapKeywords = (From kw In kweKeywords.KeyWords Select If( _
                                (From InDbKw In Context.Keywords Where InDbKw.Keyword = kw Select New With {.Keyword = InDbKw, .IsNew = False}).FirstOrDefault, _
                                New With {.Keyword = New Keyword(kw), .IsNew = True})).ToArray
        Dim CreatedDBKeywords = (From kw In AllCapKeywords Where kw.IsNew Select kw.Keyword).ToArray
        Context.Keywords.InsertAllOnSubmit(CreatedDBKeywords)
        Dim CreatedDBKwInts = (From kw In AllCapKeywords Select New Cap_Keyword_Int(Cap, kw.Keyword)).ToArray
        Context.Cap_Keyword_Ints.InsertAllOnSubmit(CreatedDBKwInts)
        Context.Cap_Category_Ints.InsertAllOnSubmit(CreatedDBCatInts)
        If NewType IsNot Nothing Then Context.CapTypes.InsertOnSubmit(NewType)
        If NewProduct IsNot Nothing Then Context.Products.InsertOnSubmit(NewProduct)
        Context.Caps.InsertOnSubmit(Cap)
        'Commit
        Try
            Context.SubmitChanges()
        Catch ex As Exception
            mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCommittingChangesToDatabase, My.Resources.txt_DatabaseError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.OK)
            'Undo
            Context.Cap_Category_Ints.DeleteAllOnSubmit(CreatedDBCatInts)
            Context.Images.DeleteAllOnSubmit(CreatedDBImages)
            Context.Keywords.DeleteAllOnSubmit(CreatedDBKeywords)
            Context.Cap_Keyword_Ints.DeleteAllOnSubmit(CreatedDBKwInts)
            If NewType IsNot Nothing Then Context.CapTypes.DeleteOnSubmit(NewType)
            If NewProduct IsNot Nothing Then Context.Products.DeleteOnSubmit(NewProduct)
            Context.Caps.DeleteOnSubmit(Cap)
            Exit Sub
        End Try
        Me.Close()
    End Sub
#Region "Copy images"
    ''' <summary>Copies images from <see cref="lvwImages"/> to image directory and creates resized images</summary>
    ''' <returns>List of names of copied images; null on error</returns>
    Private Function CopyImages() As List(Of String)
        Dim Imgs = New List(Of String)
        CopyImages = Imgs
        Dim folOrig = IO.Path.Combine(My.Settings.ImageRoot, "original")
        Dim fol64 = IO.Path.Combine(My.Settings.ImageRoot, "64_64")
        Dim fol256 = IO.Path.Combine(My.Settings.ImageRoot, "256_256")
        Dim CreatedFiles As New List(Of String)
        Dim Exception As Exception = Nothing
        Try
            For Each Item As NewImage In lvwImages.ItemsSource
                'Copy original size file
CopyFile:       Dim newName = IO.Path.GetFileName(Item.RelativePath)
                Dim i As Integer = 0
                While IO.File.Exists(IO.Path.Combine(folOrig, newName)) OrElse IO.File.Exists(IO.Path.Combine(fol64, newName)) OrElse IO.File.Exists(IO.Path.Combine(fol256, newName))
                    i += 1
                    newName = String.Format(System.Globalization.CultureInfo.InvariantCulture, _
                                            "{0}_{1}{2}", IO.Path.GetFileNameWithoutExtension(newName), i, IO.Path.GetExtension(newName))
                End While
                Dim OrigFilePath As String = IO.Path.Combine(folOrig, newName)
                Try
                    IO.File.Copy(Item.RelativePath, OrigFilePath)
                Catch ex As Exception
                    If mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCopyingFile, My.Resources.txt_ImageCopyError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Abort) = Forms.DialogResult.Retry Then GoTo CopyFile
                    Exception = ex
                    Return Nothing
                End Try
                Dim OrigFileInfo As New IO.FileInfo(OrigFilePath)
                With OrigFileInfo 'If file is readonly, make it RW
                    If (OrigFileInfo.Attributes And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then _
                                        .Attributes = .Attributes And Not IO.FileAttributes.ReadOnly
                End With

                CreatedFiles.Add(OrigFilePath)
                'Write metadata
                Dim IPTC As Tools.MetadataT.IptcT.Iptc = Nothing
                Try
                    If IO.Path.GetExtension(OrigFilePath).ToLower = ".jpg" OrElse IO.Path.GetExtension(OrigFilePath).ToLower = ".jpeg" Then
                        Using JPEG As New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(OrigFilePath, False)
                            If JPEG.ContainsIptc Then
                                IPTC = New Tools.MetadataT.IptcT.Iptc(JPEG)
                            Else
                                IPTC = New Tools.MetadataT.IptcT.Iptc
                            End If
                        End Using
                        Dim keywords = New List(Of String)(If(IPTC.Keywords, New String() {}))
                        For Each NewKw In My.Resources.CapKeywords.Split(","c)
                            If Not keywords.Contains(NewKw) Then keywords.Add(NewKw)
                        Next
                        For Each NewKw In kweKeywords.KeyWords
                            If Not keywords.Contains(NewKw) Then keywords.Add(NewKw)
                        Next
                        For Each cat As CategoryProxy In lstCategories.ItemsSource
                            If cat.Checked Then
                                keywords.Add(cat.Category.CategoryName)
                            End If
                        Next
                        IPTC.Keywords = keywords.ToArray
                        If txtCountryCode.Text <> "" Then IPTC.CountryPrimaryLocationCode = txtCountryCode.Text
                        IPTC.ObjectName = txtCapName.Text
                        If txtMainText.Text <> "" Then IPTC.Headline = txtMainText.Text
                        If txtTopText.Text <> "" OrElse txtSideText.Text <> "" OrElse txtBottomText.Text <> "" Then
                            Dim strlist As New List(Of String)
                            If txtTopText.Text <> "" Then strlist.Add(txtTopText.Text)
                            If txtSideText.Text <> "" Then strlist.Add(txtSideText.Text)
                            If txtSubTitle.Text <> "" Then strlist.Add(txtSubTitle.Text)
                            IPTC.CaptionAbstract = strlist.Join(vbCrLf)
                        End If
                        If nudYear.Value > 0 Then IPTC.DateCreated = New Tools.MetadataT.IptcT.IptcDataTypes.OmmitableDate(nudYear.Value)
                        IPTC.ReleaseDate = Now.ToUniversalTime.Date
                        IPTC.ReleaseTime = New Tools.MetadataT.IptcT.IptcDataTypes.Time(Now.ToUniversalTime.Date.TimeOfDay)
                        If txtNote.Text <> "" Then IPTC.SpecialInstructions = txtNote.Text.Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ")
                        Using JPEG As New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(OrigFilePath, True)
                            JPEG.IPTCEmbed(IPTC.GetBytes)
                        End Using
                    End If
                Catch ex As Exception
                    If mBox.Error_XPTIBWO(ex, My.Resources.msg_IPTCError, My.Resources.txt_IPTC, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore) <> Forms.DialogResult.Ignore Then
                        Exception = ex
                        Return Nothing
                    End If
                End Try
                'Resize file to 64px
                Dim File64 As String = IO.Path.Combine(fol64, newName)
Resize64:       Try
                    SaveResizedImage(OrigFilePath, File64, 64, CreatedFiles, IPTC)
                Catch ex As Exception
                    Dim result As System.Windows.Forms.DialogResult = mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCreatingResizedFile.f(64), My.Resources.txt_ImageResizeError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Ignore)
                    If result = Forms.DialogResult.Retry OrElse result = Forms.DialogResult.Ignore Then
                        Try
                            IO.File.Delete(File64)
                        Catch ex2 As Exception
                            mBox.Error_XPTIBWO(ex2, My.Resources.msg_CreatedFileWasNotDeleted.f(File64), My.Resources.txt_ErrorRemovingFile, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Ignore)
                        End Try
                        CreatedFiles.Remove(File64)
                    End If
                    Select Case result
                        Case Forms.DialogResult.Retry : GoTo Resize64
                        Case Forms.DialogResult.Ignore 'Do nothing
                        Case Else : Exception = ex : Return Nothing
                    End Select
                End Try
                'Resize file to 256px
                Dim File256 As String = IO.Path.Combine(fol256, newName)
Resize256:      Try
                    SaveResizedImage(OrigFilePath, File256, 256, CreatedFiles, IPTC)
                Catch ex As Exception
                    Dim result As System.Windows.Forms.DialogResult = mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCreatingResizedFile.f(256), My.Resources.txt_ImageResizeError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Ignore)
                    If result = Forms.DialogResult.Retry OrElse result = Forms.DialogResult.Ignore Then
                        Try
                            IO.File.Delete(File256)
                        Catch ex2 As Exception
                            mBox.Error_XPTIBWO(ex2, My.Resources.msg_CreatedFileWasNotDeleted.f(File256), My.Resources.txt_ErrorRemovingFile, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Ignore)
                        End Try
                        CreatedFiles.Remove(File256)
                    End If
                    Select Case result
                        Case Forms.DialogResult.Retry : GoTo Resize256
                        Case Forms.DialogResult.Ignore 'Do nothing
                        Case Else : Exception = ex : Return Nothing
                    End Select
                End Try
                Imgs.Add(newName)
            Next
        Finally
            If Exception IsNot Nothing Then
                For Each file In CreatedFiles
                    Try
                        IO.File.Delete(file)
                    Catch ex As Exception
                        mBox.Error_XPTIBWO(ex, My.Resources.msg_CreatedFileWasNotDeleted.f(file), My.Resources.txt_ErrorRemovingFile, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Ignore)
                    End Try
                Next
            End If
        End Try
    End Function
    ''' <summary>Resizes image and saves it under given name</summary>
    ''' <param name="OrigFilePath">Path of image to be resized</param>
    ''' <param name="TargetFilePath">Path to save resized image to</param>
    ''' <param name="Size">Desired maximum square size</param>
    ''' <param name="CreatedFiles">Procedure adds path to any file it creates to this list</param>
    Private Shared Sub SaveResizedImage(ByVal OrigFilePath As String, ByVal TargetFilePath$, ByVal Size%, ByVal CreatedFiles As List(Of String), ByVal IPTC As Tools.MetadataT.IptcT.Iptc)
        Dim Extension As String = IO.Path.GetExtension(OrigFilePath).ToLower
        If Extension <> ".wmp" AndAlso Extension <> ".hdp" Then
            Using bmp As New System.Drawing.Bitmap(OrigFilePath)
                Using small = bmp.GetThumbnail(New System.Drawing.Size(Size, Size))
                    Dim format As System.Drawing.Imaging.ImageFormat
                    Select Case Extension
                        Case ".jpg", ".jpeg" : format = System.Drawing.Imaging.ImageFormat.Jpeg
                        Case ".bmp", ".dib" : format = System.Drawing.Imaging.ImageFormat.Bmp
                        Case ".tif", ".tiff" : format = System.Drawing.Imaging.ImageFormat.Tiff
                        Case ".gif" : format = System.Drawing.Imaging.ImageFormat.Gif
                        Case ".png" : format = System.Drawing.Imaging.ImageFormat.Png
                            'Case ".wmp", ".hdp" 
                            'Case ".ico" : encoder = New IconBitmapEncoder
                        Case Else : Throw New InvalidOperationException(My.Resources.err_UnknownImageExtension.f(IO.Path.GetExtension(OrigFilePath)))
                    End Select
                    small.Save(TargetFilePath, format)
                    CreatedFiles.Add(TargetFilePath)
                End Using
                If IPTC IsNot Nothing AndAlso (Extension = ".jpg" OrElse Extension = ".jpeg") Then
                    Using smalljpeg = New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(TargetFilePath, True)
                        smalljpeg.IPTCEmbed(IPTC.GetBytes)
                    End Using
                End If
            End Using
        Else
            Using OrigFile = IO.File.Open(OrigFilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                Dim imgA As New BitmapImage
                imgA.BeginInit()
                imgA.StreamSource = OrigFile
                imgA.CreateOptions = BitmapCreateOptions.PreservePixelFormat
                imgA.CacheOption = BitmapCacheOption.None
                imgA.EndInit()
                Dim img = New BitmapImage
                img.BeginInit()
                img.StreamSource = OrigFile
                img.CreateOptions = BitmapCreateOptions.PreservePixelFormat
                img.CacheOption = BitmapCacheOption.None
                If imgA.Height > imgA.Width Then img.DecodePixelHeight = Size _
                Else img.DecodePixelWidth = Size
                OrigFile.Position = 0
                img.EndInit()
                Dim encoder As BitmapEncoder
                Select Case Extension
                    'Case ".jpg", ".jpeg" : encoder = New JpegBitmapEncoder()
                    'Case ".bmp", ".dib" : encoder = New BmpBitmapEncoder
                    'Case ".tif", ".tiff" : encoder = New TiffBitmapEncoder
                    'Case ".gif" : encoder = New GifBitmapEncoder()
                    'Case ".png" : encoder = New PngBitmapEncoder()
                    Case ".wmp", ".hdp" : encoder = New WmpBitmapEncoder()
                        'Case ".ico" : encoder = New IconBitmapEncoder
                        'Case Else : Throw New InvalidOperationException(My.Resources.err_UnknownImageExtension.f(IO.Path.GetExtension(OrigFilePath)))
                End Select
                encoder.Frames.Add(BitmapFrame.Create(img))
                Using ostream = IO.File.Open(TargetFilePath, IO.FileMode.Create, IO.FileAccess.ReadWrite)
                    CreatedFiles.Add(TargetFilePath)
                    encoder.Save(ostream)
                End Using
            End Using
        End If
    End Sub
#End Region
#Region "Select / new / anonymous selection"
    Private Sub cmbCapType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbCapType.SelectionChanged
        If cmbCapType.SelectedItem IsNot Nothing Then
            With DirectCast(cmbCapType.SelectedItem, CapType)
                cmbMainType.SelectedItem = .MainType
                cmbShape.SelectedItem = .Shape
                nudSize1.Value = .Size
                If .Size2.HasValue Then nudSize2.Value = .Size2
                nudHeight.Value = .Height
                cmbMaterial.SelectedItem = .Material
            End With
        End If
    End Sub

    Private Sub cmbProduct_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbProduct.SelectionChanged
        If cmbProduct.SelectedItem IsNot Nothing Then
            With DirectCast(cmbProduct.SelectedItem, Product)
                cmbProductType.SelectedItem = .ProductType
                cmbCompany.SelectedItem = .Company
            End With
        End If
    End Sub
#End Region

    Private Sub txtSideText_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtSideText.TextChanged
        If txtSideText.Text <> "" Then chkHasSide.IsChecked = True
    End Sub

    Private Sub txtBottomText_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtBottomText.TextChanged
        If txtBottomText.Text <> "" Then chkHasBottom.IsChecked = True
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSearch.Click
        Dim SearchResults = Context.GetSimilarCaps( _
               If(optCapTypeSelect.IsChecked AndAlso cmbCapType.SelectedItem IsNot Nothing, DirectCast(cmbCapType.SelectedItem, CapType).CapTypeID, Nothing), _
               If(cmbMainType.SelectedItem IsNot Nothing, DirectCast(cmbMainType.SelectedItem, MainType).MainTypeID, Nothing), _
               If(cmbShape.SelectedItem IsNot Nothing, DirectCast(cmbShape.SelectedItem, Shape).ShapeID, Nothing), _
               txtCapName.Text, _
               txtMainText.Text, _
               txtSubTitle.Text, _
               copBackground.Color.ToArgb, _
               copSecondaryBackground.Color.ToArgb, _
               copForeground.Color.ToArgb, _
               txtMainPicture.Text, _
               txtTopText.Text, _
               txtSideText.Text, _
               txtBottomText.Text, _
               If(cmbMaterial.SelectedItem IsNot Nothing, DirectCast(cmbMaterial.SelectedItem, Material).MaterialID, Nothing), _
               If(optMatting.IsChecked, "M"c, If(optGlossy.IsChecked, "G"c, Nothing)), _
               nudSize1.Value, _
               If(cmbShape.SelectedItem IsNot Nothing AndAlso DirectCast(cmbShape.SelectedItem, Shape).Size2Name IsNot Nothing, nudSize2.Value, Nothing), _
               nudHeight.Value, _
               chk3D.IsChecked, _
               If(nudYear.Value = 0, Nothing, nudYear.Value), _
               If(txtCountryCode.Text <> "", txtCountryCode.Text, Nothing), _
               txtNote.Text, _
               If(cmbCompany.SelectedItem IsNot Nothing, DirectCast(cmbCompany.SelectedItem, Company).CompanyID, Nothing), _
               If(optProductSelected.IsChecked AndAlso cmbProduct.SelectedItem IsNot Nothing, DirectCast(cmbProduct.SelectedItem, Product).ProductID, Nothing), _
               If(cmbProductType.SelectedItem IsNot Nothing, DirectCast(cmbProductType.SelectedItem, ProductType).ProductTypeID, Nothing), _
               If(cmbStorage.SelectedItem IsNot Nothing, DirectCast(cmbStorage.SelectedItem, Storage).StorageID, Nothing), _
               copForeground2.Color.ToArgb, _
               If(cmbPictureType.SelectedItem Is cmiImageGeometry, "G"c, If(cmbPictureType.SelectedItem Is cmiImageLogo, "L"c, If(cmbPictureType.SelectedItem Is cmiImageDrawing, "D"c, If(cmbPictureType.SelectedItem Is cmiImagePhoto, "P"c, Nothing)))), _
               chkHasBottom.IsChecked, _
               chkHasSide.IsChecked, _
               txtAnotherPictures.Text _
        )
        Dim win As New winCapDetails(From result In SearchResults Select DirectCast(result, Cap))
        win.Owner = Me
        win.Title = "Výsledky vyhledávání"
        win.Show()
    End Sub
End Class
