Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Tools.ExtensionsT, Tools.LinqT
Imports Caps.Data

''' <summary>Main application window</summary>
Class winMain
    Private Const ConnectionString$ = "ConnectionString"
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

    Private Context As CapsDataContext

    Private Sub winMain_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        My.Settings.winMainLoc = Me.GetWindowPosition
    End Sub
    Private Sub winMain_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.SetWindowPosition(My.Settings.winMainLoc)
        Dim Args = ParseParameters(Environment.GetCommandLineArgs, True)
        If Args.ContainsKey(ConnectionString) AndAlso Args(ConnectionString).Count > 0 AndAlso Args(ConnectionString)(0) <> "" Then
            Try
                Main.SqlConnection = New System.Data.SqlClient.SqlConnection(Args(ConnectionString)(0))
            Catch ex As Exception
                mBox.Error_XPTIBWO(ex, String.Format(My.Resources.err_InvalidCommandLineConnectionString, Args(ConnectionString)(0)), "Invalid connection string", mBox.MessageBoxIcons.Error, , Me)
            End Try
        End If

        Dim Redo As Boolean = False
Connect: If Main.SqlConnection Is Nothing OrElse Redo Then
            Dim win = If(Main.SqlConnection Is Nothing, New winSelectDatabase, New winSelectDatabase(Main.SqlConnection.ConnectionString))
            win.Owner = Me
            If win.ShowDialog Then
                Main.SqlConnection = New System.Data.SqlClient.SqlConnection(win.ConnectionString.ToString)
            Else
                Environment.Exit(1)
            End If
        End If
        Try
            Main.EntityConnection = New System.Data.EntityClient.EntityConnection(CapsDataContext.DefaultMetadataWorkspace,Main.SqlConnection)
            EntityConnection.Open()
            VerifyDatabaseVersionWithUpgrade(SqlConnection, Me)
        Catch ex As Exception
            Try : EntityConnection.Close() : Catch : End Try
            If mBox.Error_XBWI(ex, mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Abort, Me) = Forms.DialogResult.Retry Then
                Redo = True
                GoTo Connect
            Else
                Environment.Exit(2)
            End If
        End Try
        My.Settings.UserConnectionString = SqlConnection.ConnectionString
        My.Settings.Save()

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
        Me.Context = New CapsDataContext(Main.EntityConnection)
        lblCapsCount.Content = Context.Caps.Count
        lblNewestCap.Content = (From itm In Context.Caps Order By itm.DateCreated Descending Select CType(itm.DateCreated, Date?)).FirstOrDefault
        lblOldestcap.Content = (From itm In Context.Caps Order By itm.DateCreated Ascending Select CType(itm.DateCreated, Date?)).FirstOrDefault
        itmNewest.ItemsSource = From itm In Context.Caps Order By itm.DateCreated Descending Take 10
        itmRandom.ItemsSource = From itm In Context.Caps Order By Guid.NewGuid Take 10
        Dim BiggestCategory = (From itm In Context.Categories Order By itm.Caps.Count Descending Select CType(itm.Caps.Count, Integer?)).FirstOrDefault
        Dim SmallestCategory = If((From itm In Context.Categories Order By itm.Caps.Count Ascending Select CType(itm.Caps.Count, Integer?)).FirstOrDefault, 0)
        Dim BiggestKeyword = (From itm In Context.Keywords Order By itm.Caps.Count Descending Select CType(itm.Caps.Count, Integer?)).FirstOrDefault
        Dim SmallestKeyword = If((From itm In Context.Keywords Order By itm.Caps.Count Ascending Select CType(itm.Caps.Count, Integer?)).FirstOrDefault, 0)
        Const FontMax% = 50
        Const FontMin% = 8
        Dim mup As Double = If(Not BiggestCategory.HasValue OrElse BiggestCategory = 0, 0, (FontMax - FontMin) / (BiggestCategory - SmallestCategory))
        itmCategories.ItemsSource = From itm In Context.Categories _
                                    Select Count = itm.Caps.Count, Name = itm.CategoryName, ID = itm.CategoryID, Size = mup * (itm.Caps.Count - SmallestCategory) + FontMin, Type = "C" _
                                    Order By Count Descending
        mup = If(Not BiggestKeyword.HasValue OrElse BiggestKeyword = 0, 0, (FontMax - FontMin) / (BiggestKeyword - SmallestKeyword))
        itmKeywords.ItemsSource = From itm In Context.Keywords _
                                   Select Count = itm.Caps.Count, Name = itm.KeywordName, ID = itm.KeywordID, Size = mup * (itm.Caps.Count - SmallestKeyword) + FontMin, Type = "K" _
                                   Order By Count Descending
    End Sub

    Private Sub mniSettings_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniSettings.Click
        Dim originalLanguage As String = My.Settings.Language
        Dim win As New winSettings
        If win.ShowDialog() Then
            My.Settings.Save()
            If My.Settings.Language <> originalLanguage Then
                Try
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(My.Settings.Language)
                Catch : End Try
                Select Case mBox.MsgBox(My.Resources.msg_LanguageChangedRestart, MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.txt_LanguageChange, Me)
                    Case MsgBoxResult.Yes
                        Dim newP As New Process
                        newP.StartInfo.Arguments = (From param In Environment.GetCommandLineArgs.Skip(1) Select If(param.Contains(" "c) OrElse param.Contains(""""c), """" & param.Replace("""", """""") & """", param)).Join(" ")
                        Dim OrigArgs = ParseParameters(Environment.GetCommandLineArgs, True)
                        If Not OrigArgs.ContainsKey(ConnectionString) AndAlso newP.StartInfo.Arguments <> "" Then newP.StartInfo.Arguments &= " "
                        If Not OrigArgs.ContainsKey(ConnectionString) Then
                            newP.StartInfo.Arguments &= """" & ConnectionString & "=" & Main.EntityConnection.ConnectionString.Replace("""", """""") & """"
                        End If
                        newP.StartInfo.FileName = IO.Path.Combine(Reflection.Assembly.GetEntryAssembly.Location)
                        newP.StartInfo.WorkingDirectory = Environment.CurrentDirectory
                        Try
                            newP.Start()
                        Catch ex As Exception
                            mBox.Error_XPTIBWO(ex, My.Resources.err_CannotRestart, My.Resources.txt_ErrorRestarting, Owner:=Me)
                        End Try
                        Me.Close()
                End Select
            End If
        End If
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
            src = From itm In Context.Caps
                  Where itm.Keywords.Contains((From kw In Context.Keywords Where kw.KeywordID = id).FirstOrDefault)
                  Select itm Order By itm.CapName
        ElseIf ItemType = "C"c Then
            src = From itm In Context.Caps
                  Where itm.Categories.Contains((From cat In Context.Categories Where cat.CategoryID = id).FirstOrDefault)
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
            mBox.MsgBox(My.Resources.msg_NoFilesToDelete, MsgBoxStyle.Information, My.Resources.txt_ImageCleanup, Me)
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
            msg.MidControl = New TextBlock() With {.Text = My.Resources.txt_ClearImagesNote, .TextWrapping = TextWrapping.Wrap, .HorizontalAlignment = Windows.HorizontalAlignment.Stretch, .TextAlignment = TextAlignment.Left}
            msg.SetButtons(mBox.MessageBoxButton.Buttons.OK Or mBox.MessageBoxButton.Buttons.Cancel)
            msg.Title = My.Resources.txt_ImageCleanup
            If msg.ShowDialog(Me) = Forms.DialogResult.OK Then
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
                    mBox.MsgBox(My.Resources.msg_AllFilesDeleted, MsgBoxStyle.Information, My.Resources.txt_ImageCleanup, Me)
                Else
                    mBox.MsgBox(My.Resources.err_UnableToDeleteFiles.f(ErrNo), MsgBoxStyle.Exclamation, My.Resources.txt_ImageCleanup, Me)
                End If
            End If
        End If
    End Sub

    Private Sub mniGoto_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniGoto.Click
        Dim msg = mBox.GetDefault
        msg.Prompt = My.Resources.lbl_TypeCapID
        msg.Title = My.Resources.mni_Goto
        Dim txt = New TextBox
        msg.MidControl = txt
        msg.Buttons.Clear()
        msg.Buttons.AddRange(mBox.MessageBoxButton.GetButtons(Forms.MessageBoxButtons.OKCancel))
        AddHandler msg.Shown, AddressOf msgCapID_Shown
        If msg.ShowDialog(Me) = Forms.DialogResult.OK Then
            Dim ID%
            Try
                ID = Integer.Parse(txt.Text)
            Catch ex As Exception
                mBox.Error_XTW(ex, ex.GetType.Name, Me)
                Exit Sub
            End Try
            Dim cap = (From item In Context.Caps Where item.CapID = ID).FirstOrDefault
            If cap Is Nothing Then
                mBox.MsgBox(My.Resources.err_CapIdNotFound.f(ID), MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, My.Resources.txt_CapNotFound, Me)
                Exit Sub
            End If
            Dim win As New winCapDetails(New Cap() {cap})
            Me.Hide()
            win.ShowDialog()
            Me.Show()
            Bind()
        End If
    End Sub
    Private Sub msgCapID_Shown(ByVal sender As mBox, ByVal e As EventArgs)
        DirectCast(sender.MidControl, TextBox).Focus()
    End Sub
End Class
