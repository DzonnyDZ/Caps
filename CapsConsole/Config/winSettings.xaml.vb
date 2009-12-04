Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Partial Public Class winSettings

    Private Sub winSettings_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        txtImageRoot.Text = My.Settings.ImageRoot

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        If Not IO.Directory.Exists(txtImageRoot.Text) Then
            mBox.Modal_PTIW(My.Resources.msg_ImageRootPathError, My.Resources.txt_ImageRoot, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Exclamation, Me)
            Exit Sub
        End If
        My.Settings.ImageRoot = txtImageRoot.Text
        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub btnImageRoot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnImageRoot.Click
        Dim dlg As New Forms.FolderBrowserDialog
        Try
            dlg.SelectedPath = txtImageRoot.Text
        Catch : End Try
        If dlg.ShowDialog = Forms.DialogResult.OK Then
            txtImageRoot.Text = dlg.SelectedPath
        End If
    End Sub
End Class
