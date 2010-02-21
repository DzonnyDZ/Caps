Imports Tools.CollectionsT.GenericT, Tools.ExtensionsT
Imports Tools.DrawingT.ImageTools
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Caps.Data

''' <summary>Creates a new cap</summary>
Partial Public Class winNewCap
    Private lastSavedID%?
    Private Sub caeEditor_CancelClicked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles caeEditor.CancelClicked
        Me.Close()
    End Sub

    Private Sub caeEditor_SaveClicked(ByVal sender As System.Object, ByVal e As CapEditor.SaveClickedEventArgs) Handles caeEditor.SaveClicked
        Dim PrevLastSavedID = Me.lastSavedID
        With caeEditor
            Dim Cap As Cap = Nothing
            If e.Mode.IsSave Then
                Cap = New Cap
                Dim NewType As CapType = Nothing
                Dim NewProduct As Product = Nothing
                'Tests
                If Not caeEditor.Tests Then Exit Sub
                'New cap type
                If .CapTypeSelection = CapEditor.CreatableItemSelection.NewItem Then
                    If Not .TestNewCapType Then Exit Sub
                    NewType = New CapType With {.TypeName = caeEditor.CapTypeName, _
                                                .Description = caeEditor.CapTypeDescription, _
                                                .MainType = caeEditor.CapMainType, _
                                                .Shape = caeEditor.CapShape, _
                                                .Size = caeEditor.Size1, _
                                                .Size2 = If(caeEditor.CapShape.Size2Name Is Nothing, New Integer?, caeEditor.Size2), _
                                                .Height = caeEditor.CapHeight, _
                                                .Material = caeEditor.Material, _
                                                .Target = caeEditor.Target}
                    Cap.CapType = NewType
                ElseIf .CapTypeSelection = CapEditor.CreatableItemSelection.SelectedItem Then
                    Cap.CapType = .CapType
                End If
                'New product
                If .ProductSelection = CapEditor.CreatableItemSelection.NewItem Then
                    If Not .TestNewProduct() Then Exit Sub
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
                If .CapForegroundColor2.HasValue Then Cap.ForeColor2 = System.Drawing.Color.FromArgb(.CapForegroundColor2.Value.A, .CapForegroundColor2.Value.R, .CapForegroundColor2.Value.G, .CapForegroundColor2.Value.B).ToArgb()
                Cap.Is3D = .Is3D
                Cap.Surface = If(Not .IsGlossy, "M"c, "G"c)
                Cap.TopText = .TopText
                Cap.SideText = .SideText
                Cap.BottomText = .BottomText
                Cap.Note = .CapNote
                Cap.Year = .Year
                'If .Country <> "" AndAlso Not .Country Like "[ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz][ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz][ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz]" Then _
                '    mBox.Modal_PTIw(My.Resources.msg_CountryCodeMustBeISO3Code, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation) : .txtCountryCode.SelectAll() : .txtCountryCode.Focus() : Exit Sub
                If .Country <> "" Then Cap.CountryCode = .Country
                Cap.Storage = .Storage
                Cap.ProductType = .CapProductType
                Cap.Company = .CapCompany
                Cap.AnotherPictures = .AnotherPictures
                Cap.HasBottom = .HasBottom
                Cap.HasSide = .HasSide
                Cap.PictureType = .PictureType
                If .CountryOfOrigin <> "" Then Cap.CountryOfOrigin = .CountryOfOrigin
                Cap.IsDrink = .IsDrink
                Cap.State = .State
                Cap.Target = .Target
                Cap.IsAlcoholic = .IsAlcoholic

                'Images
                Dim IntroducedImages = .CopyImages()
                If IntroducedImages Is Nothing Then Exit Sub
                'Prepare for commit
                For Each img In IntroducedImages
                    img.Cap = Cap
                Next
                .Context.Images.AddObjects(IntroducedImages)
                If .Keywords IsNot Nothing Then
                    'Dim AllCapKeywords = (From kw In .Keywords Select If( _
                    '                        (From InDbKw In .Context.Keywords Where InDbKw.Keyword = kw Select New With {.Keyword = InDbKw, .IsNew = False}).FirstOrDefault, _
                    '                        New With {.Keyword = New Keyword(kw), .IsNew = True})).ToArray
                    Dim Keywords = From kw In .Keywords
                                   Select existingKw = (From indbkw In .Context.Keywords Where indbkw.KeywordName = kw).FirstOrDefault, kw
                                   Select existing = existingKw IsNot Nothing, keyword = If(existingKw, New Keyword(kw))
                    .Context.Keywords.AddObjects(From kw In Keywords Where kw.existing Select kw.keyword)
                    Cap.Keywords.AddRange(From kw In Keywords Select kw.keyword)
                End If
                Cap.Categories.AddRange(.SelectedCategories)
                Cap.CapSigns.Add(.SelectedCapSigns)
                If NewType IsNot Nothing Then .Context.CapTypes.AddObject(NewType)
                If NewProduct IsNot Nothing Then .Context.Products.AddObject(NewProduct)
                .Context.Caps.AddObject(Cap)
                'Commit
                Try
                    .Context.SaveChanges()
                Catch ex As Exception
                    mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCommittingChangesToDatabase, My.Resources.txt_DatabaseError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.OK, Me)
                    'Undo
                    .ResetContext()
                    .UndoCopyImages(From img In IntroducedImages Select img.RelativePath)
                    Exit Sub
                End Try
                'For newly introduced cap type copy image
                If .CapTypeSelection = CapEditor.CreatableItemSelection.NewItem AndAlso IO.File.Exists(.CapTypeImagePath) Then
                    .CopyTypeImage(NewType)
                End If
                mBox.MsgBox(My.Resources.msg_CapsSavedID.f(Cap.CapID), MsgBoxStyle.Information, My.Resources.txt_CapSaved, Me)
                Me.lastSavedID = Cap.CapID
            End If
            Select Case e.Mode
                Case CapEditor.SaveMode.SaveAndClose : Me.Close()
                Case CapEditor.SaveMode.SaveAndNext, CapEditor.SaveMode.SaveAndNew, CapEditor.SaveMode.NextNoSave, CapEditor.SaveMode.Reset
                    .Reset()
                Case CapEditor.SaveMode.PreviousNoSave, CapEditor.SaveMode.SaveAndPrevious
                    If PrevLastSavedID.HasValue Then
                        Dim SubDialog As winCapEditor
                        Try
                            SubDialog = New winCapEditor(PrevLastSavedID)
                        Catch ex As ArgumentException
                            mBox.Error_XTW(ex, ex.GetType.Name, Me)
                            .Reset()
                            Exit Sub
                        End Try
                        My.Settings.winNewCapLoc = Me.GetWindowPosition
                        NoSaveSettings = True
                        Me.DialogResult = True
                        Me.Hide()
                        SubDialog.ShowDialog()
                        Me.Close()
                    Else
                        mBox.MsgBox(My.Resources.err_NoPrevItem, MsgBoxStyle.Exclamation, My.Resources.txt_PreviousCap, Me)
                        .Reset()
                    End If
                Case CapEditor.SaveMode.SaveAndNextNoClean
                    .ActualizeChangedLists()
                    If Cap IsNot Nothing AndAlso Cap.CapType IsNot Nothing Then .CapType = Cap.CapType
                    If Cap IsNot Nothing AndAlso Cap.Product IsNot Nothing Then .Product = Cap.Product
                    .InitFocus()
            End Select
        End With
    End Sub
    Private NoSaveSettings As Boolean
    Private Sub winNewCap_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        If Not NoSaveSettings Then My.Settings.winNewCapLoc = Me.GetWindowPosition
    End Sub

    Private Sub winNewCap_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.SetWindowPosition(My.Settings.winNewCapLoc)
    End Sub
End Class
