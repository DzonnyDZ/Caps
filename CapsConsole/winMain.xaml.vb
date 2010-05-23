Imports Tools.LinqT, Tools.WindowsT.WPF, Tools.WindowsT.InteropT.InteropExtensions
Imports Caps.Data, Tools.ApplicationServicesT.ApplicationServices, Tools.ThreadingT

''' <summary>Main application window</summary>
Class winMain
    ''' <summary>Command line argument name for connection string</summary>
    Private Const ConnectionString$ = "ConnectionString"
    ''' <summary>Command line argument name for image root</summary>
    Private Const ImageRoot$ = "ImageRoot"
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

    ''' <summary>Data context</summary>
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
                Main.ConnectionString = Args(ConnectionString)(0)
            Catch ex As Exception
                mBox.Error_XPTIBWO(ex, String.Format(My.Resources.err_InvalidCommandLineConnectionString, Args(ConnectionString)(0)), "Invalid connection string", mBox.MessageBoxIcons.Error, , Me)
            End Try
        End If

        Dim Redo As Boolean = False
        Dim ImageRoot As String = If(Args.ContainsKey(Console.winMain.ImageRoot) AndAlso Args(Console.winMain.ImageRoot).Count > 0,
                                     Args(Console.winMain.ImageRoot)(0),
                                     My.Settings.ImageRoot
                                    )
Connect: If Main.SqlConnection Is Nothing OrElse Redo Then
            Dim win = If(Main.SqlConnection Is Nothing, New winSelectDatabase, New winSelectDatabase(Main.SqlConnection.ConnectionString))
            win.ImageRoot = ImageRoot
            win.Owner = Me
            If win.ShowDialog Then
                Main.SqlConnection = New System.Data.SqlClient.SqlConnection(win.ConnectionString.ToString)
                Main.ConnectionString = win.ConnectionString.ToString
                ImageRoot = win.ImageRoot
            Else
                Environment.Exit(1)
            End If
        End If
        Try
            Dim NonMarsCSB As New System.Data.SqlClient.SqlConnectionStringBuilder(Main.ConnectionString)
            NonMarsCSB.MultipleActiveResultSets = False
            Using NonMarsConn As New System.Data.SqlClient.SqlConnection(NonMarsCSB.ToString)
                NonMarsConn.Open()
                VerifyDatabaseVersionWithUpgrade(NonMarsConn, Me)
            End Using
            Main.EntityConnection = New System.Data.EntityClient.EntityConnection(CapsDataContext.DefaultMetadataWorkspace, Main.SqlConnection)
            EntityConnection.Open()
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

        If (My.Settings.ImageRoot = "" OrElse Not IO.Directory.Exists(My.Settings.ImageRoot)) AndAlso (Settings.Images.StoreAnythingInDatabase) Then
            Dim dlg As New Forms.FolderBrowserDialog With {.Description = My.Resources.des_SelectImagesRootDirectory}
            If dlg.ShowDialog(Me) Then
                My.Settings.ImageRoot = dlg.SelectedPath
            Else
                Environment.Exit(3)
            End If
        End If
        My.Settings.ImageRoot = ImageRoot
        Me.Title = Me.Title & " - " & "{0} {1}".f(My.Application.Info.Title, My.Application.Info.Version)
        Bind()
    End Sub

    ''' <summary>Binds data to form</summary>
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
                        Dim OrigArgs = ParseParameters(Environment.GetCommandLineArgs, True)
                        Dim newArgs As New Text.StringBuilder
                        For Each arg In OrigArgs
                            Dim vals As IEnumerable(Of String) = arg.Value
                            Select Case arg.Key
                                Case ConnectionString : vals = {SqlConnection.ConnectionString}
                                Case ImageRoot : vals = {My.Settings.ImageRoot}
                            End Select
                            For Each Val As String In vals
                                If newArgs.Length > 0 Then newArgs.Append(" ")
                                newArgs.AppendFormat("""{0}={1}""", arg.Key.Replace("""", """"""), Val.Replace("""", """"""))
                            Next
                        Next
                        newP.StartInfo.Arguments = newArgs.ToString
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

    Private Sub mniDbSettings_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniDbSettings.Click
        Using dlg As New winDatabaseSettings With {.Owner = Me}
            Dim oldAnythingInDatabase = Settings.Images.StoreAnythingInDatabase
            dlg.ShowDialog()
            If Not oldAnythingInDatabase AndAlso Settings.Images.StoreAnythingInDatabase Then
                Dim fdlg As New Forms.FolderBrowserDialog
                fdlg.Description = My.Resources.txt_ImageRootRequired
                Try
                    fdlg.SelectedPath = My.Settings.ImageRoot
                Catch : End Try
                If fdlg.ShowDialog(Me) Then
                    My.Settings.ImageRoot = fdlg.SelectedPath
                    My.Settings.Save()
                End If
            End If
        End Using
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


#Region "Clear Images"
    Private Sub mniImagesClear_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniImagesClear.Click
        Dim mon As New Tools.WindowsT.WPF.DialogsT.ProgressMonitor With {.DoWorkOnShow = True, .CloseOnFinish = True, .Title = My.Resources.txt_ClearImages, .Prompt = My.Resources.txt_ClearImages, .CanCancel = True}
        AddHandler mon.BackgroundWorker.DoWork,
            Sub(wk As ComponentModel.BackgroundWorker, wke As ComponentModel.DoWorkEventArgs)
                wk.ReportProgress(-1, WindowsT.IndependentT.ProgressBarStyle.Indefinite)
                Dim pSizes As IEnumerable(Of String) = {}
                wk.ReportProgress(-1, My.Resources.lbl_PreparingDirectories)
                Try
                    pSizes = From fol In IO.Directory.EnumerateDirectories(My.Settings.ImageRoot) Where CapsDataExtensions.imageFolderNameRegExp.IsMatch(IO.Path.GetFileName(fol))
                Catch : End Try
                Dim pOriginal = IO.Path.Combine(My.Settings.ImageRoot, Image.OriginalSizeImageStorageFolderName)
                Dim pCapType = IO.Path.Combine(My.Settings.ImageRoot, CapType.ImageStorageFolderName)
                Dim pMainType = IO.Path.Combine(My.Settings.ImageRoot, MainType.ImageStorageFolderName)
                Dim pShape = IO.Path.Combine(My.Settings.ImageRoot, Shape.ImageStorageFolderName)
                Dim pCapSign = IO.Path.Combine(My.Settings.ImageRoot, CapSign.ImageStorageFolderName)
                Dim pStorage = IO.Path.Combine(My.Settings.ImageRoot, Storage.ImageStorageFolderName)
                Dim delSizes = From fol In pSizes
                                               Select
                                                  files = From file In IO.Directory.EnumerateFiles(fol) Where (From img In Me.Context.Images.AsEnumerable Where String.Compare(img.RelativePath, IO.Path.GetFileName(file), StringComparison.InvariantCultureIgnoreCase) = 0).Count = 0,
                                                  size = Integer.Parse(CapsDataExtensions.imageFolderNameRegExp.Match(IO.Path.GetFileName(fol)).Groups!Size.Value)
                                               Where Not files.IsEmpty
                wk.ReportProgress(-1, My.Resources.lbl_CountingUnusedImages)
                delSizes = delSizes.ToList
                Dim delOriginal = If(IO.Directory.Exists(pOriginal), From file In IO.Directory.GetFiles(pOriginal) Where (From img In Me.Context.Images.AsEnumerable Where String.Compare(img.RelativePath, IO.Path.GetFileName(file), StringComparison.InvariantCultureIgnoreCase) = 0).Count = 0, {})
                Dim delCapType = If(IO.Directory.Exists(pCapType), From file In IO.Directory.GetFiles(pCapType) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From ct In Me.Context.CapTypes.AsEnumerable Where ct.CapTypeID.ToString(Globalization.CultureInfo.InvariantCulture) = IO.Path.GetFileNameWithoutExtension(file)).Count = 0, {})
                Dim delMainType = If(IO.Directory.Exists(pMainType), From file In IO.Directory.GetFiles(pMainType) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From mt In Me.Context.MainTypes.AsEnumerable Where mt.MainTypeID.ToString(Globalization.CultureInfo.InvariantCulture) = IO.Path.GetFileNameWithoutExtension(file)).Count = 0, {})
                Dim delShape = If(IO.Directory.Exists(pShape), From file In IO.Directory.GetFiles(pShape) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From sh In Me.Context.Shapes.AsEnumerable Where sh.ShapeID.ToString(Globalization.CultureInfo.InvariantCulture) = IO.Path.GetFileNameWithoutExtension(file)).Count = 0, {})
                Dim delCapSign = If(IO.Directory.Exists(pCapSign), From file In IO.Directory.GetFiles(pCapSign) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From sg In Me.Context.CapSigns.AsEnumerable Where sg.CapSignID.ToString(Globalization.CultureInfo.InvariantCulture) = IO.Path.GetFileNameWithoutExtension(file)).Count = 0, {})
                Dim delStorage = If(IO.Directory.Exists(pStorage), From file In IO.Directory.GetFiles(pStorage) Where String.Compare(IO.Path.GetExtension(file), ".png", StringComparison.InvariantCultureIgnoreCase) <> 0 OrElse (From st In Me.Context.Storages.AsEnumerable Where st.StorageID.ToString(Globalization.CultureInfo.InvariantCulture) = IO.Path.GetFileNameWithoutExtension(file)).Count = 0, {})

                If delSizes.IsEmpty AndAlso delOriginal.IsEmpty AndAlso delCapType.IsEmpty AndAlso delMainType.IsEmpty AndAlso delShape.IsEmpty Then
                    wk.ReportProgress(-1, My.Resources.lbl_CountingFinished)
                    mon.Invoke(Sub() mBox.MsgBox(My.Resources.msg_NoFilesToDelete, MsgBoxStyle.Information, My.Resources.txt_ImageCleanup, mon.Window))
                Else
                    Dim todel As IEnumerable(Of String) = New String() {}
                    Dim chkOriginal = New mBox.MessageBoxCheckBox(My.Resources.lbl_CapImagesOriginal.f(delOriginal.Count), If(delOriginal.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delOriginal.IsEmpty}
                    Dim chkSizes = From item In delSizes
                                   Select New mBox.MessageBoxCheckBox(My.Resources.lbl_CapImageThumbnails.f(item.size, item.files.Count),
                                                                      If(item.files.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)
                                                                     ) With {.Enabled = Not item.files.IsEmpty}
                    chkSizes = chkSizes.ToList
                    If wk.CancellationPending Then wke.Cancel = True : Return
                    Dim chkCapType = New mBox.MessageBoxCheckBox(My.Resources.lbl_CapTypeImages.f(delCapType.Count), If(delCapType.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delCapType.IsEmpty}
                    Dim chkMainType = New mBox.MessageBoxCheckBox(My.Resources.lbl_MainTypeImages.f(delMainType.Count), If(delMainType.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delMainType.IsEmpty}
                    Dim chkShape = New mBox.MessageBoxCheckBox(My.Resources.lbl_ShapeImages.f(delShape.Count), If(delShape.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delShape.IsEmpty}
                    Dim chkCapSign = New mBox.MessageBoxCheckBox(My.Resources.lbl_SignImages.f(delCapSign.Count), If(delCapSign.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delCapSign.IsEmpty}
                    Dim chkStorage = New mBox.MessageBoxCheckBox(My.Resources.lbl_StorageImages.f(delCapSign.Count), If(delStorage.IsEmpty, Forms.CheckState.Unchecked, Forms.CheckState.Checked)) With {.Enabled = Not delStorage.IsEmpty}
                    Dim showMsg =
                        Function()
                            Dim msg = mBox.GetDefault()
                            msg.Prompt = My.Resources.msg_ImagesToDelete
                            If wk.CancellationPending Then wke.Cancel = True : Return False
                            msg.CheckBoxes.Add(chkOriginal)
                            If wk.CancellationPending Then wke.Cancel = True : Return False
                            msg.CheckBoxes.AddRange(chkSizes)
                            If wk.CancellationPending Then wke.Cancel = True : Return False
                            msg.CheckBoxes.AddRange({chkCapType, chkMainType, chkShape, chkCapSign, chkStorage})
                            If wk.CancellationPending Then wke.Cancel = True : Throw New OperationCanceledException
                            msg.MidControl = New TextBlock() With {.Text = My.Resources.txt_ClearImagesNote, .TextWrapping = TextWrapping.Wrap, .HorizontalAlignment = Windows.HorizontalAlignment.Stretch, .TextAlignment = TextAlignment.Left}
                            msg.SetButtons(mBox.MessageBoxButton.Buttons.OK Or mBox.MessageBoxButton.Buttons.Cancel)
                            msg.Title = My.Resources.txt_ImageCleanup
                            If wk.CancellationPending Then wke.Cancel = True : Return False
                            If msg.ShowDialog(mon.Window) = Forms.DialogResult.OK Then
                                If wk.CancellationPending Then wke.Cancel = True : Return False
                                If chkOriginal.State = Forms.CheckState.Checked Then todel = todel.Union(delOriginal)
                                Dim i As Integer = 0
                                For Each chk In chkSizes
                                    If chk.State = Forms.CheckState.Checked Then todel.Union(delSizes(i).files)
                                    i += 1
                                Next
                                If wk.CancellationPending Then wke.Cancel = True : Return False
                                If chkCapType.State = Forms.CheckState.Checked Then todel = todel.Union(delCapType)
                                If chkMainType.State = Forms.CheckState.Checked Then todel = todel.Union(delMainType)
                                If chkShape.State = Forms.CheckState.Checked Then todel = todel.Union(delShape)
                                If chkCapSign.State = Forms.CheckState.Checked Then todel = todel.Union(delCapSign)
                                If chkStorage.State = Forms.CheckState.Checked Then todel = todel.Union(delStorage)
                                If wk.CancellationPending Then wke.Cancel = True : Return False
                                Return True
                            End If
                            Return False
                        End Function
                    wk.ReportProgress(-1, My.Resources.lbl_CountingFinished)
                    wk.ReportProgress(0, WindowsT.IndependentT.ProgressBarStyle.Definite)
                    If mon.Invoke(showMsg) Then
                        If wk.CancellationPending Then wke.Cancel = True : Return
                        Dim ErrNo% = 0
                        wk.ReportProgress(0, My.Resources.lbl_DeletingImages)
                        Dim i As Integer = 0, oldi As Integer = 0
                        Dim count = todel.Count
                        For Each file In todel
                            If wk.CancellationPending Then
                                If i - ErrNo > 0 Then mon.Invoke(Sub() mBox.MsgBoxFW(My.Resources.msg_CleanImagesCancel, MsgBoxStyle.Exclamation, My.Resources.txt_ImageCleanup, mon.Window, i - ErrNo))
                                wke.Cancel = True
                                Return
                            End If
                            Try
                                IO.File.Delete(file)
                            Catch ex As Exception
                                ErrNo += 1
                            End Try
                            i += 1
                            If i - oldi > 10 Then
                                oldi = i
                                wk.ReportProgress(i / count * 100)
                            End If
                        Next
                        wk.ReportProgress(-1, False)
                        wk.ReportProgress(100, My.Resources.lbl_Finished)
                        If ErrNo = 0 Then
                            mon.Invoke(Sub() mBox.MsgBox(My.Resources.msg_AllFilesDeleted, MsgBoxStyle.Information, My.Resources.txt_ImageCleanup, mon.Window))
                        Else
                            mon.Invoke(Sub() mBox.MsgBox(My.Resources.err_UnableToDeleteFiles.f(ErrNo), MsgBoxStyle.Exclamation, My.Resources.txt_ImageCleanup, mon.Window))
                        End If
                    End If
                End If
            End Sub
        mon.ShowDialog(Me)
    End Sub
#End Region

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

    Private Sub mniSyncImg_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles mniSyncImg.Click
        Dim win As New winSyncImages
        win.Owner = Me
        win.ShowDialog()
    End Sub
End Class
