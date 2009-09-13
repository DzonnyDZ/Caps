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
                If Not .TestNewCapType Then Exit Sub
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
            Dim IntroducedImages As List(Of String) = .CopyImages()
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
                '.Context.Cap_Category_Ints.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Cap_Category_Int))
                '.Context.Images.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Image))
                '.Context.Keywords.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Keyword))
                '.Context.Cap_Keyword_Ints.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Cap_Keyword_Int))
                '.Context.CapTypes.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of CapType))
                '.Context.Products.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Product))
                '.Context.Caps.DeleteAllOnSubmit(.Context.GetChangeSet.Inserts.OfType(Of Cap))
                .ResetContext()
                CapEditor.UndoCopyImages(IntroducedImages)
                Exit Sub
            End Try
            'For newly introduced cap type copy image
            If .CapTypeSelection = CapEditor.CreatableItemSelection.NewItem AndAlso IO.File.Exists(.CapTypeImagePath) Then
                .CopyTypeImage(NewType)
            End If
            Me.Close()
        End With
    End Sub

End Class
