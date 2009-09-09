Imports mBox = Tools.WindowsT.IndependentT.MessageBox

Partial Public Class winCapDetails
    Public Sub New(ByVal Caps As IEnumerable(Of Cap))
        InitializeComponent()
        lstCaps.ItemsSource = Caps
    End Sub

    Private Sub cmdDelete_CanExecute(ByVal sender As Object, ByVal e As System.Windows.Input.CanExecuteRoutedEventArgs) Handles cmdDelete.CanExecute
        e.CanExecute = lstCaps.SelectedItems.Count > 0
    End Sub

    Private Sub cmdEdit_CanExecute(ByVal sender As Object, ByVal e As System.Windows.Input.CanExecuteRoutedEventArgs) Handles cmdEdit.CanExecute
        e.CanExecute = lstCaps.SelectedItems.Count = 1
    End Sub

    Private Sub cmdDelete_Executed(ByVal sender As Object, ByVal e As System.Windows.Input.ExecutedRoutedEventArgs) Handles cmdDelete.Executed
        e.Handled = True
        If mBox.Modal_PTIB(My.Resources.msg_q_DelCap, My.Resources.txt_DeleteCap, mBox.GetIconDelegate(mBox.MessageBoxIcons.Question), mBox.MessageBoxButton.Yes, mBox.MessageBoxButton.No) <> Forms.DialogResult.Yes Then Exit Sub

        Dim osi As Integer = lstCaps.SelectedIndex
        Dim Context As New CapsDataDataContext
        Dim CapsToDel = (From ccap In Context.Caps Where (From cap As Cap In lstCaps.SelectedItems Select cap.CapID).Contains(ccap.CapID)).ToArray
        Context.Caps.DeleteAllOnSubmit(CapsToDel)
        Try
            Context.SubmitChanges()
        Catch ex As Exception
            mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorDeletingCaps, My.Resources.txt_DatabaseError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.OK)
            Exit Sub
        End Try
        Dim OriginalCaps = (From itm As Cap In lstCaps.Items).ToArray
        lstCaps.ItemsSource = From cap In OriginalCaps Where Not (From ctd In CapsToDel Select ctd.CapID).Contains(cap.CapID)
        lstCaps.SelectedIndex = If(lstCaps.Items.Count > osi, osi, lstCaps.Items.Count - 1)
        If lstCaps.Items.Count = 0 Then
            mBox.Modal_PTI(My.Resources.msg_AllCapsDeletedClose, My.Resources.txt_DeleteCap, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Information)
            Me.Close()
        End If
    End Sub

    Private Sub cmdEdit_Executed(ByVal sender As Object, ByVal e As System.Windows.Input.ExecutedRoutedEventArgs) Handles cmdEdit.Executed
        'TODO: Implement editor
        mBox.Modal_PTI("This functionality is not implemented yet.", "Not implemented", Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Stop)
        e.Handled = True
    End Sub
End Class
