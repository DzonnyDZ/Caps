Imports Tools.CollectionsT.GenericT, Caps.Data, Tools.DataT.ObjectsT
Imports Tools.WindowsT.WPF

Partial Public Class winCapDetails
    '''' <summary>Data context</summary>
    'Private Context As CapsDataDataContext
    ''' <summary>CTor to show preselected caps</summary>
    ''' <param name="Caps">Caps to show</param>
    Public Sub New(ByVal Caps As IEnumerable(Of Cap)) ', Optional ByVal Context As CapsDataDataContext = Nothing)
        InitializeComponent()
        lstCaps.ItemsSource = New ListWithEvents(Of Cap)(Caps)
    End Sub
    <DebuggerStepThrough()> _
    Private Sub cmdDelete_CanExecute(ByVal sender As Object, ByVal e As System.Windows.Input.CanExecuteRoutedEventArgs) Handles cmdDelete.CanExecute
        e.CanExecute = lstCaps.SelectedItems.Count > 0
    End Sub
    <DebuggerStepThrough()> _
    Private Sub cmdEdit_CanExecute(ByVal sender As Object, ByVal e As System.Windows.Input.CanExecuteRoutedEventArgs) Handles cmdEdit.CanExecute
        e.CanExecute = lstCaps.SelectedItems.Count = 1 'AndAlso Context IsNot Nothing
    End Sub

    Private Sub cmdDelete_Executed(ByVal sender As Object, ByVal e As System.Windows.Input.ExecutedRoutedEventArgs) Handles cmdDelete.Executed
        e.Handled = True
        If mBox.Modal_PTWBIO(My.Resources.msg_q_DelCap, My.Resources.txt_DeleteCap, Me, mBox.MessageBoxButton.Buttons.Yes Or mBox.MessageBoxButton.Buttons.No, mBox.GetIconDelegate(mBox.MessageBoxIcons.Question)) <> Forms.DialogResult.Yes Then Exit Sub

        Dim osi As Integer = lstCaps.SelectedIndex
        Dim Context As New CapsDataContext(Main.EntityConnection)
        Dim selectedIDs = (From cap As Cap In lstCaps.SelectedItems Select cap.CapID).ToArray
        Dim CapsToDel = (From ccap In Context.Caps Where selectedIDs.Contains(ccap.CapID)).ToArray
        Context.Caps.DeleteObjects(CapsToDel)
        Try
            Context.SaveChanges()
        Catch ex As Exception
            mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorDeletingCaps, My.Resources.txt_DatabaseError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.OK, Me)
            Exit Sub
        End Try
        Dim OriginalCaps = (From itm As Cap In lstCaps.Items).ToArray
        lstCaps.ItemsSource = From cap In OriginalCaps Where Not (From ctd In CapsToDel Select ctd.CapID).Contains(cap.CapID)
        lstCaps.SelectedIndex = If(lstCaps.Items.Count > osi, osi, lstCaps.Items.Count - 1)
        If lstCaps.Items.Count = 0 Then
            mBox.Modal_PTIW(My.Resources.msg_AllCapsDeletedClose, My.Resources.txt_DeleteCap, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Information, Me)
            Me.Close()
        End If
    End Sub

    Private Sub cmdEdit_Executed(ByVal sender As Object, ByVal e As System.Windows.Input.ExecutedRoutedEventArgs) Handles cmdEdit.Executed
        If lstCaps.SelectedItems.Count <> 1 Then
            mBox.Modal_PTIW(My.Resources.err_SelectExactlyOneCap, My.Resources.txt_InvalidSelection, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Information, Me)
            Exit Sub
        End If
        Dim Cap As Cap = DirectCast(lstCaps.SelectedItem, Cap)
        Dim win = New winCapEditor(Cap.CapID)
        win.Owner = Me
        e.Handled = True
        If win.ShowDialog() Then
            Dim context As New CapsDataContext(Main.EntityConnection)
            Dim SelectedItemID As Integer = DirectCast(lstCaps.SelectedItem, Cap).CapID
            With DirectCast(lstCaps.ItemsSource, ListWithEvents(Of Cap))
                .Item(lstCaps.SelectedIndex) = context.Caps.First(Function(newcap) newcap.CapID = Cap.CapID)
            End With

            lstCaps.Items.Refresh()
            Dim i% = 0
            For Each item As Cap In lstCaps.Items
                If item.CapID = SelectedItemID Then
                    lstCaps.SelectedIndex = i
                    Exit For
                End If
                i += 1
            Next
        End If
    End Sub

    Private Sub winCapDetails_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        My.Settings.winCapDetailsLoc = Me.GetWindowPosition
    End Sub

    Private Sub winCapDetails_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.SetWindowPosition(My.Settings.winCapDetailsLoc)
    End Sub

    Private Sub Image_MouseDown(ByVal sender As FrameworkElement, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If e.ClickCount = 2 AndAlso e.ChangedButton = MouseButton.Left Then
            e.Handled = True
            Dim item As Image = sender.DataContext
            Dim path$
            If IO.Path.IsPathRooted(item.RelativePath) Then
                path = item.RelativePath
            Else
                path = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, Image.OriginalSizeImageStorageFolderName), item.RelativePath)
            End If
            Try
                Process.Start(path)
            Catch ex As Exception
                mBox.Error_XTW(ex, ex.GetType.Name, Me)
            End Try
        End If
    End Sub
End Class
