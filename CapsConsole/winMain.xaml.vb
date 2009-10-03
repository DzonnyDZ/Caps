﻿Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Tools.ExtensionsT, Tools.LinqT
''' <summary>Main application window</summary>
Class winMain
    ''' <summary>CTor</summary>
    Public Sub New()
        InitializeComponent()
    End Sub
    'Private Sub mniFileExit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniFileExit.Click
    '    Me.Close()
    'End Sub

    Private Sub mniEditLists_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniEditLists.Click
        Dim win As New winEditors
        win.Owner = Me
        win.ShowDialog()
    End Sub

    Private Context As CapsDataDataContext

    Private Sub winMain_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        My.Settings.winMainLoc = Me.GetWindowPosition
    End Sub
    Private Sub winMain_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.SetWindowPosition(My.Settings.winMainLoc)
        Dim win As New winSelectDatabase
Connect: If win.ShowDialog Then
            Main.Connection = New System.Data.SqlClient.SqlConnection(win.ConnectionString.ToString)
            Try
                Connection.Open()
                VerifyDatabaseVersionWithUpgrade(Connection)
            Catch ex As Exception
                Connection.Close()
                If mBox.Error_XBI(ex, Tools.WindowsT.IndependentT.MessageBox.MessageBoxButton.Buttons.Retry Or Tools.WindowsT.IndependentT.MessageBox.MessageBoxButton.Buttons.Abort) = Forms.DialogResult.Retry Then
                    win = New winSelectDatabase(win.ConnectionString.ToString)
                    GoTo Connect
                Else
                    Environment.Exit(2)
                End If
            End Try
            My.Settings.UserConnectionString = Connection.ConnectionString
            My.Settings.Save()
        Else
            Environment.Exit(1)
        End If
        If Not IO.Directory.Exists(My.Settings.ImageRoot) Then
            Dim dlg As New Forms.FolderBrowserDialog With {.Description = My.Resources.des_SelectImagesRootDirectory}
            If dlg.ShowDialog = Forms.DialogResult.OK Then
                My.Settings.ImageRoot = dlg.SelectedPath
            Else
                Environment.Exit(3)
            End If
        End If
        Me.Title = Me.Title & " - " & "{0} {1}".f(My.Application.Info.Title, My.Application.Info.Version)
        Bind()
    End Sub

    Private Sub Bind()
        Me.Context = New CapsDataDataContext(Main.Connection)
        lblCapsCount.Content = Context.Caps.Count
        lblNewestCap.Content = (From itm In Context.Caps Order By itm.DateCreated Descending Select New Date?(itm.DateCreated)).FirstOrDefault
        lblOldestcap.Content = (From itm In Context.Caps Order By itm.DateCreated Ascending Select New Date?(itm.DateCreated)).FirstOrDefault
        itmNewest.ItemsSource = From itm In Context.Caps Order By itm.DateCreated Descending Take 10
        itmRandom.ItemsSource = From itm In Context.Caps Order By Context.NewID Take 10
        Dim BiggestCategory = (From itm In Context.Categories Order By itm.Cap_Category_Ints.Count Descending Select New Integer?(itm.Cap_Category_Ints.Count)).FirstOrDefault
        Dim BiggestKeyword = (From itm In Context.Keywords Order By itm.Cap_Keyword_Ints.Count Descending Select New Integer?(itm.Cap_Keyword_Ints.Count)).FirstOrDefault
        Const FontMax% = 50
        Const FontMin% = 8
        Dim mup As Double = If(Not BiggestCategory.HasValue OrElse BiggestCategory = 0, 0, (FontMax - FontMin) / BiggestCategory)
        itmCategories.ItemsSource = From itm In Context.Categories _
                                    Select Count = itm.Cap_Category_Ints.Count, Name = itm.CategoryName, ID = itm.CategoryID, Size = mup * itm.Cap_Category_Ints.Count + FontMin, Type = "C"c _
                                    Order By Count Descending
        mup = If(Not BiggestKeyword.HasValue OrElse BiggestKeyword = 0, 0, (FontMax - FontMin) / BiggestKeyword)
        itmKeywords.ItemsSource = From itm In Context.Keywords _
                                   Select Count = itm.Cap_Keyword_Ints.Count, Name = itm.Keyword, ID = itm.KeywordID, Size = mup * itm.Cap_Keyword_Ints.Count + FontMin, Type = "K"c _
                                   Order By Count Descending
    End Sub

    Private Sub mniSettings_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniSettings.Click
        Dim win As New winSettings
        win.ShowDialog()
    End Sub

    Private Sub cmdClose_CanExecute(ByVal sender As System.Object, ByVal e As System.Windows.Input.CanExecuteRoutedEventArgs) Handles cmdClose.CanExecute
        e.CanExecute = True
    End Sub

    Private Sub cmdClose_Executed(ByVal sender As System.Object, ByVal e As System.Windows.Input.ExecutedRoutedEventArgs) Handles cmdClose.Executed
        Me.Close()
    End Sub

    Private Sub cmdNew_CanExecute(ByVal sender As System.Object, ByVal e As System.Windows.Input.CanExecuteRoutedEventArgs) Handles cmdNew.CanExecute
        e.CanExecute = True
    End Sub

    Private Sub cmdNew_Executed(ByVal sender As System.Object, ByVal e As System.Windows.Input.ExecutedRoutedEventArgs) Handles cmdNew.Executed
        Dim win As New winNewCap
        Me.Hide()
        win.ShowDialog()
        Me.Show()
        Bind()
    End Sub

    Private Sub mniAbout_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniAbout.Click
        Tools.WindowsT.FormsT.AboutDialog.ShowModalDialog()
    End Sub

    Private Sub Image_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If e.ClickCount = 2 Then
            Dim prd As New winCapDetails(New Cap() {DirectCast(sender, Grid).DataContext})
            prd.Owner = Me
            prd.ShowDialog()
            Bind()
        End If
    End Sub

    Private Sub hylNewest_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles hylNewest.Click
        Dim prd As New winCapDetails(From itm In Context.Caps Order By itm.DateCreated Descending Take 10)
        prd.Owner = Me
        prd.ShowDialog()
        Bind()
    End Sub
    Private Sub Keyword_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim BindItem = DirectCast(sender, Hyperlink).DataContext
        Dim src As IEnumerable(Of Cap)
        Dim id% = BindItem.GetType.GetProperty("ID").GetValue(BindItem, New Object() {})
        Dim ItemType As Char = BindItem.GetType.GetProperty("Type").GetValue(BindItem, New Object() {})
        If ItemType = "K"c Then
            src = From itm In Context.Caps Join ki In Context.Cap_Keyword_Ints On itm.CapID Equals ki.CapID _
                  Where ki.KeywordID = id _
                  Select itm Order By itm.CapName
        ElseIf ItemType = "C"c Then
            src = From itm In Context.Caps Join ci In Context.Cap_Category_Ints On itm.CapID Equals ci.CapID _
                  Where ci.CategoryID = id _
                  Select itm Order By itm.CapName
        Else
            Exit Sub
        End If
        Dim prd As New winCapDetails(src)
        prd.Owner = Me
        prd.ShowDialog()
        Bind()
    End Sub

    Private Sub cmdRefresh_CanExecute(ByVal sender As Object, ByVal e As System.Windows.Input.CanExecuteRoutedEventArgs) Handles cmdRefresh.CanExecute
        e.CanExecute = True
    End Sub

    Private Sub cmdRefresh_Executed(ByVal sender As Object, ByVal e As System.Windows.Input.ExecutedRoutedEventArgs) Handles cmdRefresh.Executed
        Bind()
    End Sub


  
    Private Sub mniImagesClear_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniImagesClear.Click
        Dim p256 = IO.Path.Combine(My.Settings.ImageRoot, "256_256")
        Dim p64 = IO.Path.Combine(My.Settings.ImageRoot, "64_64")
        Dim pOriginal = IO.Path.Combine(My.Settings.ImageRoot, "original")
        Dim pCapType = IO.Path.Combine(My.Settings.ImageRoot, "CapType")
        Dim pMainType = IO.Path.Combine(My.Settings.ImageRoot, "MainType")
        Dim pShape = IO.Path.Combine(My.Settings.ImageRoot, "Shape")

        Dim del256 = From file In IO.Directory.GetFiles(p256) Where (From img In Me.Context.Images Where String.Compare(img.RelativePath, IO.Path.GetFileName(file), StringComparison.InvariantCultureIgnoreCase) = 0).Count = 0
        Dim del64 = From file In IO.Directory.GetFiles(p64) Where (From img In Me.Context.Images Where String.Compare(img.RelativePath, IO.Path.GetFileName(file), StringComparison.InvariantCultureIgnoreCase) = 0).Count = 0
        Dim delOriginal = From file In IO.Directory.GetFiles(pOriginal) Where (From img In Me.Context.Images Where String.Compare(img.RelativePath, IO.Path.GetFileName(file), StringComparison.InvariantCultureIgnoreCase) = 0).Count = 0
        Dim delCapType = From file In IO.Directory.GetFiles(pCapType) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From ct In Me.Context.CapTypes Where ct.CapTypeID = IO.Path.GetFileNameWithoutExtension(file)).Count = 0
        Dim delMainType = From file In IO.Directory.GetFiles(pMainType) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From mt In Me.Context.MainTypes Where mt.MainTypeID = IO.Path.GetFileNameWithoutExtension(file)).Count = 0
        Dim delShape = From file In IO.Directory.GetFiles(pShape) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From sh In Me.Context.Shapes Where sh.ShapeID = IO.Path.GetFileNameWithoutExtension(file)).Count = 0

        If del256.IsEmpty AndAlso del64.IsEmpty AndAlso delOriginal.IsEmpty AndAlso delCapType.IsEmpty AndAlso delMainType.IsEmpty AndAlso delShape.IsEmpty Then
            mBox.MsgBox(My.Resources.msg_NoFilesToDelete, MsgBoxStyle.Information, My.Resources.txt_ImageCleanup)
        Else
            Dim msg = mBox.GetDefault()
            msg.Prompt = My.Resources.msg_ImagesToDelete
            Dim chkOriginal = New mBox.MessageBoxCheckBox(My.Resources.lbl_CapImagesOriginal.f(delOriginal.Count), If(delOriginal.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delOriginal.IsEmpty}
            Dim chk256 = New mBox.MessageBoxCheckBox(My.Resources.lbl_CapImageThumbnails.f(256, del256.Count), If(del256.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not del256.IsEmpty}
            Dim chk64 = New mBox.MessageBoxCheckBox(My.Resources.lbl_CapImageThumbnails.f(64, del64.Count), If(del64.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not del64.IsEmpty}
            Dim chkCapType = New mBox.MessageBoxCheckBox(My.Resources.lbl_CapTypeImages.f(delCapType.Count), If(delCapType.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delCapType.IsEmpty}
            Dim chkMainType = New mBox.MessageBoxCheckBox(My.Resources.lbl_MainTypeImages.f(delMainType.Count), If(delMainType.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delMainType.IsEmpty}
            Dim chkShape = New mBox.MessageBoxCheckBox(My.Resources.lbl_ShapeImages.f(delShape.Count), If(delShape.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delShape.IsEmpty}
            msg.CheckBoxes.AddRange(New mBox.MessageBoxCheckBox() {chkOriginal, chk256, chk64, chkCapType, chkMainType, chkShape})
            msg.MidControl = New TextBlock() With {.Text = My.Resources.txt_ClearImagesNote, .TextWrapping = TextWrapping.WrapWithOverflow, .HorizontalAlignment = Windows.HorizontalAlignment.Stretch, .TextAlignment = TextAlignment.Left}
            msg.SetButtons(mBox.MessageBoxButton.Buttons.OK Or mBox.MessageBoxButton.Buttons.Cancel)
            msg.Title = My.Resources.txt_ImageCleanup
            If msg.ShowDialog = Forms.DialogResult.OK Then
                Dim todel As IEnumerable(Of String) = New String() {}
                If chkOriginal.State = Forms.CheckState.Checked Then todel = todel.Union(delOriginal)
                If chk256.State = Forms.CheckState.Checked Then todel = todel.Union(del256)
                If chk64.State = Forms.CheckState.Checked Then todel = todel.Union(del64)
                If chkCapType.State = Forms.CheckState.Checked Then todel = todel.Union(delCapType)
                If chkMainType.State = Forms.CheckState.Checked Then todel = todel.Union(delMainType)
                If chkShape.State = Forms.CheckState.Checked Then todel = todel.Union(delShape)
                Dim ErrNo% = 0
                For Each file In todel
                    Try
                        IO.File.Delete(file)
                    Catch ex As Exception
                        ErrNo += 1
                    End Try
                Next
                If ErrNo = 0 Then
                    mBox.MsgBox(My.Resources.msg_AllFilesDeleted, MsgBoxStyle.Information, My.Resources.txt_ImageCleanup)
                Else
                    mBox.MsgBox(My.Resources.err_UnableToDeleteFiles.f(ErrNo), MsgBoxStyle.Exclamation, My.Resources.txt_ImageCleanup)
                End If
            End If
        End If
    End Sub
    
End Class
