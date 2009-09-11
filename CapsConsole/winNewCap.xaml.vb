Imports Tools.CollectionsT.GenericT, Tools.ExtensionsT
Imports Tools.DrawingT.ImageTools
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
''' <summary>Creates a new cap</summary>
Partial Public Class winNewCap

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles caeEditor.CancelClicked
        Me.Close()
    End Sub



    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles caeEditor.SaveClicked
        With caeEditor
            Dim Cap As New Cap
            Dim NewType As CapType = Nothing
            Dim NewProduct As Product = Nothing
            'Tests
            If Not caeEditor.Tests Then Exit Sub
            'New cap type
            If .CapTypeSelection = CapEditor.CreatableItemSelection.NewItem Then
                If Not IO.File.Exists(.CapTypeImagePath) Then
                    Select Case mBox.ModalF_PTBIa(My.Resources.msg_FileNotExists_ContinueWOImage, My.Resources.txt_CapTypeImage, mBox.MessageBoxButton.Buttons.Yes Or mBox.MessageBoxButton.Buttons.No, mBox.MessageBoxIcons.Question, .CapTypeImagePath)
                        Case Forms.DialogResult.Yes
                        Case Else : Exit Sub
                    End Select
                ElseIf IO.Path.GetExtension(.CapTypeImagePath).ToLower <> ".png" Then
                    mBox.Modal_PTI(My.Resources.msg_OnlyPNG, My.Resources.txt_CapTypeImage, mBox.MessageBoxIcons.Exclamation)
                    Exit Sub
                End If
                If .CapTypeName = "" Then mBox.Modal_PTI(My.Resources.msg_CapTypeNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : .txtCapTypeName.Focus() : Exit Sub
                If (From CapType In .Context.CapTypes Where CapType.TypeName = .CapTypeName).Any Then _
                    mBox.Modal_PTI(My.Resources.msg_CapTypeAreadyExists, My.Resources.txt_DuplicateEntry, mBox.MessageBoxIcons.Exclamation) : .txtCapTypeName.SelectAll() : .txtCapName.Focus() : Exit Sub
                NewType = New CapType With {.TypeName = caeEditor.CapTypeName, _
                                            .Description = caeEditor.CapTypeDescription, _
                                            .MainType = caeEditor.CapMainType, _
                                            .Shape = caeEditor.CapShape, _
                                            .Size = caeEditor.Size1, _
                                            .Size2 = If(caeEditor.CapShape.Size2Name Is Nothing, New Integer?, caeEditor.Size2), _
                                            .Height = caeEditor.CapHeight, _
                                            .Material = caeEditor.Material}
                Cap.CapType = NewType
            ElseIf .CapTypeSelection = CapEditor.CreatableItemSelection.SelectedItem Then
                Cap.CapType = .CapType
            End If
            'New product
            If .ProductSelection = CapEditor.CreatableItemSelection.NewItem Then
                If .ProductName = "" Then mBox.Modal_PTI(My.Resources.msg_ProductNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation) : .txtProductName.Focus() : Exit Sub
                If (From Product In .Context.Products Where Product.ProductName = .txtProductName.Text).Any Then _
                    mBox.Modal_PTI(My.Resources.msg_ProductWithAreadyExists, My.Resources.txt_DuplicateEntry, mBox.MessageBoxIcons.Exclamation) : .txtProductName.SelectAll() : .txtProductName.Focus() : Exit Sub
                NewProduct = New Product With {.ProductName = caeEditor.ProductName, _
                                               .Description = caeEditor.ProductDescription, _
                                               .Company = caeEditor.CapCompany, _
                                               .ProductType = caeEditor.CapProductType}
                Cap.Product = NewProduct
            ElseIf .ProductSelection = CapEditor.CreatableItemSelection.SelectedItem Then
                Cap.Product = .Product
            End If
            'Cap values
            Cap.CapName = .CapName
            Cap.MainText = .MainText
            Cap.SubTitle = .SubTitle
            Cap.MainPicture = .MainPicture
            Cap.MainType = .CapMainType
            Cap.Shape = .CapShape
            Cap.Size = .Size1
            If .CapShape.Size2Name IsNot Nothing Then Cap.Size2 = .Size2
            Cap.Height = .CapHeight
            Cap.Material = .Material
            Cap.BackColor1 = System.Drawing.Color.FromArgb(.CapBackgroundColor1.A, .CapBackgroundColor1.R, .CapBackgroundColor1.G, .CapBackgroundColor1.B).ToArgb
            If .CapBackgroundColor2.HasValue Then Cap.BackColor2 = System.Drawing.Color.FromArgb(.CapBackgroundColor2.Value.A, .CapBackgroundColor2.Value.R, .CapBackgroundColor2.Value.G, .CapBackgroundColor2.Value.B).ToArgb()
            If .CapForegroundColor1.HasValue Then Cap.ForeColor = System.Drawing.Color.FromArgb(.CapForegroundColor1.Value.A, .CapForegroundColor1.Value.R, .CapForegroundColor1.Value.G, .CapForegroundColor1.Value.B).ToArgb()
            If .CapForegroundColor1.HasValue Then Cap.ForeColor2 = System.Drawing.Color.FromArgb(.CapForegroundColor1.Value.A, .CapForegroundColor1.Value.R, .CapForegroundColor1.Value.G, .CapForegroundColor1.Value.B).ToArgb()
            Cap.Is3D = .Is3D
            Cap.Surface = If(Not .IsGlossy, "M"c, "G"c)
            Cap.TopText = .TopText
            Cap.SideText = .SideText
            Cap.BottomText = .BottomText
            Cap.Note = .CapNote
            Cap.Year = .Year
            If .Country <> "" AndAlso Not .Country Like "[ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz][ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz][ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz]" Then _
                mBox.Modal_PTI(My.Resources.msg_CountryCodeMustBeISO3Code, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : .txtCountryCode.SelectAll() : .txtCountryCode.Focus() : Exit Sub
            If Cap.CountryCode <> "" Then Cap.CountryCode = .Country
            Cap.Storage = .Storage
            Cap.ProductType = .CapProductType
            Cap.Company = .CapCompany
            Cap.AnotherPictures = .AnotherPictures
            Cap.HasBottom = .HasBottom
            Cap.HasSide = .HasSide
            Cap.PictureType = .PictureType
            'Categories
            Dim CreatedDBCatInts As New List(Of Cap_Category_Int)
            If .SelectedCategories IsNot Nothing Then
                For Each Category In .SelectedCategories
                    CreatedDBCatInts.Add(New Cap_Category_Int(Cap, Category))
                Next
            End If
            'Images
            Dim IntroducedImages As List(Of String) = CopyImages()
            If IntroducedImages Is Nothing Then Exit Sub
            'Prepare for commit
            Dim CreatedDBImages = (From item In IntroducedImages Select New Image() With {.Cap = Cap, .RelativePath = item}).ToArray
            .Context.Images.InsertAllOnSubmit(CreatedDBImages)
            Dim CreatedDBKeywords As Keyword()
            Dim CreatedDBKwInts As Cap_Keyword_Int()
            If .Keywords IsNot Nothing Then
                Dim AllCapKeywords = (From kw In .Keywords Select If( _
                                        (From InDbKw In .Context.Keywords Where InDbKw.Keyword = kw Select New With {.Keyword = InDbKw, .IsNew = False}).FirstOrDefault, _
                                        New With {.Keyword = New Keyword(kw), .IsNew = True})).ToArray
                CreatedDBKeywords = (From kw In AllCapKeywords Where kw.IsNew Select kw.Keyword).ToArray
                .Context.Keywords.InsertAllOnSubmit(CreatedDBKeywords)
                CreatedDBKwInts = (From kw In AllCapKeywords Select New Cap_Keyword_Int(Cap, kw.Keyword)).ToArray
                .Context.Cap_Keyword_Ints.InsertAllOnSubmit(CreatedDBKwInts)
            Else
                CreatedDBKeywords = New Keyword() {}
                CreatedDBKwInts = New Cap_Keyword_Int() {}
            End If
            .Context.Cap_Category_Ints.InsertAllOnSubmit(CreatedDBCatInts)
            If NewType IsNot Nothing Then .Context.CapTypes.InsertOnSubmit(NewType)
            If NewProduct IsNot Nothing Then .Context.Products.InsertOnSubmit(NewProduct)
            .Context.Caps.InsertOnSubmit(Cap)
            'Commit
            Try
                .Context.SubmitChanges()
            Catch ex As Exception
                mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCommittingChangesToDatabase, My.Resources.txt_DatabaseError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.OK)
                'Undo
                .Context.Cap_Category_Ints.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Cap_Category_Int))
                .Context.Images.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Image))
                .Context.Keywords.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Keyword))
                .Context.Cap_Keyword_Ints.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Cap_Keyword_Int))
                .Context.CapTypes.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of CapType))
                .Context.Products.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Product))
                .Context.Caps.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Cap))
                Dim FaildedDeletes As New System.Text.StringBuilder
                For Each img In IntroducedImages
                    For Each folder In New String() {"original", "64_64", "256_256"}
                        Dim imgpath = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, folder), img)
                        If IO.File.Exists(imgpath) Then
                            Try
                                IO.File.Delete(imgpath)
                            Catch delex As Exception
                                FaildedDeletes.AppendLine(String.Format("{0}{1}{2}{1}({3})", img, vbTab, ex.Message, imgpath))
                            End Try
                        End If
                    Next
                Next
                If FaildedDeletes.Length > 0 Then
                    mBox.Modal_PTI(My.Resources.err_DeleteingOfSomeImagesFailed & vbCrLf & FaildedDeletes.ToString & vbCrLf & My.Resources.msg_ThereAreImagesThatDoNotBelongToAnyCap, My.Resources.txt_Error, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Error)
                End If
                Exit Sub
            End Try
            'For newly introduced cap type copy image
            If .CapTypeSelection = CapEditor.CreatableItemSelection.NewItem AndAlso IO.File.Exists(.CapTypeImagePath) Then
                Dim targpath = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "CapType"), NewType.CapTypeID.ToString(System.Globalization.CultureInfo.InvariantCulture) & ".png")
                If Not IO.File.Exists(targpath) OrElse mBox.MsgBox(My.Resources.msg_CapImageExistsOverwrite.f(.CapTypeImagePath), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, My.Resources.txt_OwervriteFile) = MsgBoxResult.Yes Then
                    Try
                        IO.File.Copy(.CapTypeImagePath, targpath)
                    Catch ex As Exception
                        mBox.MsgBox(My.Resources.err_CopyCapTypeImageFailed.f(vbCrLf, ex.Message), MsgBoxStyle.Exclamation, My.Resources.txt_CopyFile)
                    End Try
                End If
            End If
            Me.Close()
        End With
    End Sub
#Region "Copy images"
    ''' <summary>Copies images from <see cref="caeEditor"/>.<see cref="CapEditor.Images">Images</see> to image directory and creates resized images</summary>
    ''' <returns>List of names of copied images; null on error</returns>
    Private Function CopyImages() As List(Of String)
        Dim Imgs = New List(Of String)
        CopyImages = Imgs
        If caeEditor.Images Is Nothing Then Exit Function
        Dim folOrig = IO.Path.Combine(My.Settings.ImageRoot, "original")
        Dim fol64 = IO.Path.Combine(My.Settings.ImageRoot, "64_64")
        Dim fol256 = IO.Path.Combine(My.Settings.ImageRoot, "256_256")
        Dim CreatedFiles As New List(Of String)
        Dim Exception As Exception = Nothing
        Try
            For Each Item As NewImage In caeEditor.Images
                'Copy original size file
CopyFile:       Dim newName = IO.Path.GetFileName(Item.RelativePath)
                Dim newName1 = newName
                Dim i As Integer = 0
                While IO.File.Exists(IO.Path.Combine(folOrig, newName)) OrElse IO.File.Exists(IO.Path.Combine(fol64, newName)) OrElse IO.File.Exists(IO.Path.Combine(fol256, newName))
                    i += 1
                    newName = String.Format(System.Globalization.CultureInfo.InvariantCulture, _
                                            "{0}_{1}{2}", IO.Path.GetFileNameWithoutExtension(newName1), i, IO.Path.GetExtension(newName1))
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
                        For Each NewKw In caeEditor.Keywords
                            If Not keywords.Contains(NewKw) Then keywords.Add(NewKw)
                        Next
                        For Each cat As Category In caeEditor.SelectedCategories
                            keywords.Add(cat.CategoryName)
                        Next
                        IPTC.Keywords = keywords.ToArray
                        If caeEditor.Country <> "" Then IPTC.CountryPrimaryLocationCode = caeEditor.Country
                        IPTC.ObjectName = caeEditor.CapName
                        If caeEditor.MainText <> "" Then IPTC.Headline = caeEditor.MainText
                        If caeEditor.TopText <> "" OrElse caeEditor.SideText <> "" OrElse caeEditor.BottomText <> "" Then
                            Dim strlist As New List(Of String)
                            If caeEditor.TopText <> "" Then strlist.Add(caeEditor.TopText)
                            If caeEditor.SideText <> "" Then strlist.Add(caeEditor.SideText)
                            If caeEditor.BottomText <> "" Then strlist.Add(caeEditor.BottomText)
                            IPTC.CaptionAbstract = strlist.Join(vbCrLf)
                        End If
                        If caeEditor.Year.HasValue > 0 Then IPTC.DateCreated = New Tools.MetadataT.IptcT.IptcDataTypes.OmmitableDate(caeEditor.Year)
                        IPTC.ReleaseDate = Now.ToUniversalTime.Date
                        IPTC.ReleaseTime = New Tools.MetadataT.IptcT.IptcDataTypes.Time(Now.ToUniversalTime.Date.TimeOfDay)
                        If caeEditor.CapNote <> "" Then IPTC.SpecialInstructions = caeEditor.CapNote.Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ")
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
                    Case Else : Throw New InvalidOperationException(My.Resources.err_UnknownImageExtension.f(IO.Path.GetExtension(OrigFilePath)))
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
End Class
