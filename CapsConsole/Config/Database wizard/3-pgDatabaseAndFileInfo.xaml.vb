Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Microsoft.Win32

''' <summary>A wizard page used to collect creadentials to connect to a database</summary>
Public Class pgCredentials
    Inherits PageFunction(Of Boolean)

    ''' <summary>Wizard data used and filled by this wizard step</summary>
    Private wizardData As WizardData

    ''' <summary>CTor - creates a new instance of the <see cref="pgCredentials"/> class</summary>
    ''' <param name="wizardData">Wizard data partially initialized by previous steps</param>
    Public Sub New(ByVal wizardData As WizardData)
        Me.InitializeComponent()
        MyBase.DataContext = wizardData
        Me.wizardData = wizardData
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
        If (wizardData.DatabaseType = DatabaseType.AttachFile OrElse wizardData.DatabaseType = DatabaseType.UserInstance) AndAlso (txtDatabaseFile.Text = "" OrElse (wizardData.FileConnectionType = FileConnectionType.Existing AndAlso Not IO.File.Exists(txtDatabaseFile.Text))) Then
            mBox.MsgBox(If(wizardData.FileConnectionType = FileConnectionType.Existing, My.Resources.wiz_txt_SelectExistingDbFile, My.Resources.wiz_txt_SelectDbFile),
                        MsgBoxStyle.Information, My.Resources.wiz_txt_DatabaseFile, Me.FindAncestor(Of Window))
            Return
        End If
        If wizardData.DatabaseType = DatabaseType.ServerDatabase AndAlso txtDatabaseName.Text = "" Then
            mBox.MsgBox(My.Resources.wiz_txt_EnterDbName, MsgBoxStyle.Information, My.Resources.wiz_txt_DatabaseName2, Me.FindAncestor(Of Window))
            Return
        End If
        If optSQLAuth.IsChecked AndAlso (txtUserName.Text = "" OrElse txtPassword.Password = "") Then
            mBox.MsgBox(My.Resources.wiz_txt_EnterUserNameAndPassword, MsgBoxStyle.Information, My.Resources.wiz_txt_SQLServerAuthentification2, Me.FindAncestor(Of Window))
            Return
        End If
        Dim nextPage = New pgImageStorage(wizardData)
        AddHandler nextPage.Return, New ReturnEventHandler(Of Boolean)(AddressOf Me.wizardPage_Return)
        MyBase.NavigationService.Navigate(nextPage)
    End Sub
    Public Sub wizardPage_Return(ByVal sender As Object, ByVal e As ReturnEventArgs(Of Boolean))
        Me.OnReturn(e)
    End Sub

    Private Sub btnDbBrowse_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDbBrowse.Click
        Dim dlg As FileDialog
        If wizardData.FileConnectionType = FileConnectionType.Existing Then
            dlg = New OpenFileDialog
        Else
            dlg = New SaveFileDialog
        End If
        dlg.Filter = My.Resources.fil_MDF
        Try
            dlg.FileName = txtDatabaseFile.Text
        Catch :End Try
        If dlg.ShowDialog(Me.FindAncestor(Of Window)) Then
            wizardData.FilePath = dlg.FileName
        End If
    End Sub

    Private Sub txtPassword_PasswordChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles txtPassword.PasswordChanged
        wizardData.Password = txtPassword.Password
    End Sub
End Class

