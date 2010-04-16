Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports Tools.WindowsT.NativeT
Imports System.Windows.Forms
Imports mBox = Tools.WindowsT.IndependentT.MessageBox

''' <summary>This wizard step is sued to define image storage</summary>
Public Class pgImageStorage
    Inherits PageFunction(Of Boolean)

    ''' <summary>CTor - creates a new instance of the <see cref="pgImageStorage"/> class</summary>
    ''' <param name="wizardData">Wizard data partially initialized by previous steps</param>
    Public Sub New(ByVal wizardData As WizardData)
        Me.InitializeComponent()
        MyBase.DataContext = wizardData
    End Sub

    Private Sub backButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        MyBase.NavigationService.GoBack()
    End Sub

    Private Sub cancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(False))
    End Sub

    Private Sub finishButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(True))
    End Sub

    Private Sub nextButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles nextButton.Click
        If (chkCapImages.IsChecked OrElse chkOtherImages.IsChecked) AndAlso (txtImageRoot.Text = "" OrElse Not IO.Directory.Exists(txtImageRoot.Text)) Then
            mBox.MsgBox("Select folder to store images in, please", MsgBoxStyle.Information, "Image root")
            Return
        End If

        Dim nextPage As New pgSummary(DirectCast(MyBase.DataContext, WizardData))
        AddHandler nextPage.Return, New ReturnEventHandler(Of Boolean)(AddressOf Me.wizardPage_Return)
        MyBase.NavigationService.Navigate(nextPage)
    End Sub
    Public Sub wizardPage_Return(ByVal sender As Object, ByVal e As ReturnEventArgs(Of Boolean))
        Me.OnReturn(e)
    End Sub

    Private Sub chkCapImages_CheckedChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles chkCapImages.Checked, chkOtherImages.Checked, chkCapImages.Unchecked, chkOtherImages.Unchecked
        lblImageRoot.IsEnabled = chkCapImages.IsChecked OrElse chkOtherImages.IsChecked
        txtImageRoot.IsEnabled = chkCapImages.IsChecked OrElse chkOtherImages.IsChecked
        btnImageRoot.IsEnabled = chkCapImages.IsChecked OrElse chkOtherImages.IsChecked
    End Sub

    Private Sub btnImageRoot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnImageRoot.Click
        Dim dlg As New FolderBrowserDialog
        Try
            dlg.SelectedPath = txtImageRoot.Text
        Catch : End Try
        Me.FindAncestor(Of Window)()
        If dlg.ShowDialog(New Win32Window(Me.FindAncestor(Of Window))) = Forms.DialogResult.OK Then
            txtImageRoot.Text = dlg.SelectedPath
        End If
    End Sub
End Class

