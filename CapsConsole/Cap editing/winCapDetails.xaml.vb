﻿Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Tools.CollectionsT.GenericT

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
        If mBox.Modal_PTIB(My.Resources.msg_q_DelCap, My.Resources.txt_DeleteCap, mBox.GetIconDelegate(mBox.MessageBoxIcons.Question), mBox.MessageBoxButton.Yes, mBox.MessageBoxButton.No) <> Forms.DialogResult.Yes Then Exit Sub

        Dim osi As Integer = lstCaps.SelectedIndex
        Dim Context As New CapsDataDataContext(Main.Connection)
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
        If lstCaps.SelectedItems.Count <> 1 Then
            mBox.Modal_PTI(My.Resources.err_SelectExactlyOneCap, My.Resources.txt_InvalidSelection, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Information)
            Exit Sub
        End If
        Dim Cap As Cap = DirectCast(lstCaps.SelectedItem, Cap)
        Dim win = New winCapEditor(Cap.CapID)
        win.Owner = Me
        e.Handled = True
        If win.ShowDialog() Then
            Dim context As New CapsDataDataContext(Main.Connection)
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
End Class