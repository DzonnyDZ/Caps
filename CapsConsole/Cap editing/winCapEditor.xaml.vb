﻿Imports mBox = Tools.WindowsT.IndependentT.MessageBox, Tools.ExtensionsT
Partial Public Class winCapEditor
    Private Cap As Cap
    Private Context As CapsDataDataContext
    ''' <summary>CTor</summary>
    ''' <param name="CapID">ID of Cap to edit</param>
    ''' <exception cref="ArgumentException">Cap with ID <paramref name="CapID"/> cannot be found</exception>
    Public Sub New(ByVal CapID%)
        'If Cap Is Nothing Then Throw New ArgumentNullException("Cap")
        'If Context Is Nothing Then Throw New ArgumentNullException("Context")
        InitializeComponent()
        Context = caeEditor.Context
        Me.Cap = Context.Caps.First(Function(cap) cap.CapID = CapID)
        If Cap Is Nothing Then Throw New ArgumentException(My.Resources.ex_CapIDNotFound.f(CapID))
        'Me.Context = New CapsDataDataContext(Main.Connection)
        caeEditor.Initialize(True)
        Me.DataContext = Cap
        caeEditor.Keywords = From kw In Cap.Cap_Keyword_Ints Select kw.Keyword.Keyword
        OldKeywords = Cap.Cap_Keyword_Ints.ToArray
        caeEditor.SelectedCategories = From cat In Cap.Cap_Category_Ints Select cat.Category
        OldCategories = Cap.Cap_Category_Ints.ToArray
        caeEditor.Images.AddRange(Cap.Images)
        OldImages = Cap.Images.ToArray
        caeEditor.SelectedCapSigns = From csi In Cap.Cap_CapSign_Ints Select csi.CapSign
        OldCapSigns = Cap.Cap_CapSign_Ints.ToArray
    End Sub
    ''' <summary>Original categories of cap before editing</summary>
    Private OldCategories As Cap_Category_Int()
    ''' <summary>Original keywords of cap before editing</summary>
    Private OldKeywords As Cap_Keyword_Int()
    ''' <summary>Original images of cap before editing</summary>
    Private OldImages As Image()
    ''' <summary>Original signs of cap before editing</summary>
    Private OldCapSigns As Cap_CapSign_Int()
    ''' <summary>True when <see cref="winCapEditor_Closing"/> shall do nothing</summary>
    Private IsClosing As Boolean = False

    Private Sub caeEditor_CancelClicked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles caeEditor.CancelClicked
        Me.Close()
    End Sub

    Private Sub caeEditor_SaveClicked(ByVal sender As Object, ByVal e As CapEditor.SaveClickedEventArgs) Handles caeEditor.SaveClicked
        If Me.IsClosing Then Exit Sub 'TODO: This is workaround. Why event is raised one window is closed?
        If e.Mode.IsSave Then
            If Not caeEditor.Tests Then Exit Sub
            'Introduce new CapType
            Dim NewType As CapType = Nothing
            If caeEditor.CapTypeSelection = CapEditor.CreatableItemSelection.NewItem Then
                If Not caeEditor.TestNewCapType Then Exit Sub
                NewType = New CapType With {.TypeName = caeEditor.CapTypeName, _
                                                 .Description = caeEditor.CapTypeDescription, _
                                                 .MainType = caeEditor.CapMainType, _
                                                 .Height = caeEditor.CapHeight, _
                                                 .Material = caeEditor.Material, _
                                                 .Size = caeEditor.Size1, _
                                                 .Size2 = caeEditor.Size2}
            End If
            'Introduce product
            Dim NewProduct As Product = Nothing
            If caeEditor.ProductSelection = CapEditor.CreatableItemSelection.NewItem Then
                caeEditor.TestNewProduct()
                NewProduct = New Product With {.ProductName = caeEditor.ProductName, _
                                               .Description = caeEditor.ProductDescription, _
                                               .Company = caeEditor.CapCompany, _
                                               .ProductType = caeEditor.CapProductType}
            End If
            'Image files
            Dim IntroducedImages = caeEditor.CopyImages()
            If IntroducedImages Is Nothing Then Exit Sub
            'Categories
            Context.Cap_Category_Ints.DeleteAllOnSubmit((From cat In OldCategories Where Not caeEditor.SelectedCategories.Contains(cat.Category)).ToArray)
            Context.Cap_Category_Ints.InsertAllOnSubmit((From cat In caeEditor.SelectedCategories Where Not (From ci In OldCategories Select ci.Category).Contains(cat) Select New Cap_Category_Int(Cap, cat)).ToArray)
            'CapSigns
            Context.Cap_CapSign_Ints.DeleteAllOnSubmit((From cs In OldCapSigns Where Not caeEditor.SelectedCapSigns.Contains(cs.CapSign)).ToArray)
            Context.Cap_CapSign_Ints.InsertAllOnSubmit((From cs In caeEditor.SelectedCapSigns Where Not (From ci In OldCapSigns Select ci.CapSign).Contains(cs) Select New Cap_CapSign_Int(Cap, cs)).ToArray)
            'Keywords
            Context.Cap_Keyword_Ints.DeleteAllOnSubmit((From kw In OldKeywords Where Not caeEditor.Keywords.Contains(kw.Keyword.Keyword)).ToArray)
            Dim KeywordsToAssociate = From kw In caeEditor.Keywords Where Not (From oldk In OldKeywords Select oldk.Keyword.Keyword).Contains(kw) Select kw, DbKw = (From kwdb In Context.Keywords Where kwdb.Keyword = kw).FirstOrDefault
            Context.Cap_Keyword_Ints.InsertAllOnSubmit((From kw In KeywordsToAssociate Where kw.DbKw IsNot Nothing Select New Cap_Keyword_Int(Cap, kw.DbKw)).ToArray)
            Dim NewKeywords = (From kw In KeywordsToAssociate Where kw.DbKw Is Nothing Select New Keyword(kw.kw)).ToArray
            Context.Keywords.InsertAllOnSubmit(NewKeywords)
            Context.Cap_Keyword_Ints.InsertAllOnSubmit((From kw In NewKeywords Select New Cap_Keyword_Int(Cap, kw)).ToArray)
            'Images
            For Each img In IntroducedImages
                img.Cap = Cap
            Next
            Context.Images.InsertAllOnSubmit(IntroducedImages)
            Context.Images.DeleteAllOnSubmit((From img In OldImages Where Not caeEditor.Images.Contains(img)).ToArray)
            'Prepare for commit                                                                                      
            If NewType IsNot Nothing Then Cap.CapType = NewType : Context.CapTypes.InsertOnSubmit(NewType)
            If NewProduct IsNot Nothing Then Cap.Product = NewProduct : Context.Products.InsertOnSubmit(NewProduct)
            Try
                Context.SubmitChanges()
            Catch ex As Exception
                mBox.Error_XTW(ex, My.Resources.txt_ErrorUpdatingCap, Me)
                'Undo
                Context = caeEditor.ResetContext
                Dim OldCap = Cap
                Cap = Context.Caps.FirstOrDefault(Function(cap) cap.CapID = OldCap.CapID)
                Cap.CapName = OldCap.CapName
                Cap.MainText = OldCap.MainText
                Cap.SubTitle = OldCap.SubTitle
                Cap.MainPicture = OldCap.MainPicture
                Cap.AnotherPictures = OldCap.AnotherPictures
                Cap.PictureType = OldCap.PictureType
                Cap.CapType = Context.CapTypes.FirstOrDefault(Function(itm) itm.CapTypeID = OldCap.CapTypeID)
                Cap.MainType = Context.MainTypes.FirstOrDefault(Function(itm) itm.MainTypeID = OldCap.MainTypeID)
                Cap.Shape = Context.Shapes.FirstOrDefault(Function(itm) itm.ShapeID = OldCap.ShapeID)
                Cap.Size = OldCap.Size
                Cap.Size2 = OldCap.Size2
                Cap.Height = OldCap.Height
                Cap.Material = Context.Materials.FirstOrDefault(Function(itm) itm.MaterialID = OldCap.MaterialID)
                Cap.BackColor1 = OldCap.BackColor1
                Cap.BackColor2 = OldCap.BackColor2
                Cap.ForeColor = Cap.ForeColor
                Cap.ForeColor2 = Cap.ForeColor2
                Cap.Is3D = OldCap.Is3D
                Cap.Surface = OldCap.Surface
                Cap.TopText = OldCap.TopText
                Cap.SideText = OldCap.SideText
                Cap.BottomText = OldCap.BottomText
                Cap.Note = OldCap.Note
                Cap.Year = OldCap.Year
                Cap.CountryCode = OldCap.CountryCode
                Cap.Storage = Context.Storages.FirstOrDefault(Function(itm) itm.StorageID = OldCap.StorageID)
                Cap.HasBottom = OldCap.HasBottom
                Cap.HasSide = OldCap.HasSide
                Cap.Product = Context.Products.FirstOrDefault(Function(itm) itm.ProductID = OldCap.ProductID)
                Cap.ProductType = Context.ProductTypes.FirstOrDefault(Function(itm) itm.ProductTypeID = OldCap.ProductTypeID)
                Cap.Company = Context.Companies.FirstOrDefault(Function(itm) itm.CompanyID = OldCap.CompanyID)
                Me.DataContext = Cap
                caeEditor.UndoCopyImages(From img In IntroducedImages Select img.RelativePath)
                Exit Sub
            End Try
            If NewType IsNot Nothing Then
                caeEditor.CopyTypeImage(NewType)
            End If
        End If
        Select Case e.Mode
            Case CapEditor.SaveMode.SaveAndClose
                IsClosing = True
                Me.DialogResult = True
                Me.Close()
            Case CapEditor.SaveMode.SaveAndNext, CapEditor.SaveMode.NextNoSave, CapEditor.SaveMode.PreviousNoSave, CapEditor.SaveMode.SaveAndPrevious, CapEditor.SaveMode.Reset
                Me.DataContext = Nothing
                caeEditor.ResetContext()
                Me.Context = caeEditor.Context
                Dim CapToLoad As Cap = Nothing
                Dim mode = e.Mode
GetNextCap:
                Select Case mode
                    Case CapEditor.SaveMode.SaveAndNext, CapEditor.SaveMode.NextNoSave
                        CapToLoad = (From item In Context.Caps Where item.CapID > Cap.CapID Order By item.CapID Ascending Take 1).FirstOrDefault
                    Case CapEditor.SaveMode.SaveAndPrevious, CapEditor.SaveMode.PreviousNoSave
                        CapToLoad = (From item In Context.Caps Where item.CapID < Cap.CapID Order By item.CapID Descending Take 1).FirstOrDefault
                    Case CapEditor.SaveMode.Reset
                        CapToLoad = (From item In Context.Caps Where item.CapID = Cap.CapID Take 1).FirstOrDefault
                End Select

                If CapToLoad Is Nothing Then
                    Dim msg$ = Nothing
                    Dim title$ = Nothing
                    Select Case mode
                        Case CapEditor.SaveMode.SaveAndNext : msg = My.Resources.msg_NoMoreCaps : title = My.Resources.txt_SaveAndNext
                        Case CapEditor.SaveMode.NextNoSave : msg = My.Resources.msg_NoMoreCaps : title = My.Resources.txt_GoToNextCap
                        Case CapEditor.SaveMode.PreviousNoSave : msg = My.Resources.msg_ThisWasFirstCap : title = My.Resources.txt_GoToPreviousCap
                        Case CapEditor.SaveMode.SaveAndPrevious : msg = My.Resources.msg_ThisWasFirstCap : title = My.Resources.txt_SaveAndPrevious
                        Case CapEditor.SaveMode.Reset : msg = My.Resources.msg_CapRemovedFormDatabase.f(Cap.CapID) : title = My.Resources.txt_CapMissing
                    End Select
                    mBox.MsgBox(msg, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, title, Me)
                    If mode <> CapEditor.SaveMode.Reset Then
                        mode = CapEditor.SaveMode.Reset
                        GoTo GetNextCap
                    End If
                    IsClosing = True
                    Me.DialogResult = True
                    Me.Close()
                Else
                    Me.Cap = CapToLoad
                    caeEditor.Reset()
                    caeEditor.Initialize(True)
                    Me.DataContext = Cap
                    caeEditor.Keywords = From kw In Cap.Cap_Keyword_Ints Select kw.Keyword.Keyword
                    OldKeywords = Cap.Cap_Keyword_Ints.ToArray
                    caeEditor.SelectedCategories = From cat In Cap.Cap_Category_Ints Select cat.Category
                    OldCategories = Cap.Cap_Category_Ints.ToArray
                    OldCapSigns = Cap.Cap_CapSign_Ints.ToArray
                    caeEditor.Images.AddRange(Cap.Images)
                    caeEditor.lvwImages.Items.Refresh() 'TODO: This is workaround, AddRange should work out-of-the-box
                    OldImages = Cap.Images.ToArray
                End If
            Case CapEditor.SaveMode.SaveAndNew
                Dim Win As New winNewCap
                Me.DialogResult = True
                Me.Hide()
                My.Settings.winCapEditorLoc = Me.GetWindowPosition
                NoSaveSettings = True
                Win.ShowDialog()
                IsClosing = True
                Me.Close()
        End Select
    End Sub
    Private NoSaveSettings As Boolean
    Private Sub winCapEditor_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        If Not NoSaveSettings Then My.Settings.winCapEditorLoc = Me.GetWindowPosition
    End Sub

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

    Private Sub winCapEditor_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.SetWindowPosition(My.Settings.winCapEditorLoc)
    End Sub
End Class
