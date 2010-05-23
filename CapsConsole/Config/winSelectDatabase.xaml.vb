Imports System.Data.SqlClient
Imports Microsoft.Win32, Tools.WindowsT.InteropT.InteropExtensions
Imports Tools.WindowsT.NativeT
Imports Caps.Data

''' <summary>Dialog to select database connection</summary>
Partial Public Class winSelectDatabase
    ''' <summary>CTor - creates a new instance of the <see cref="winSelectDatabase"/> class</summary>
    Public Sub New()
        Try
            _ConnectionString = New SqlConnectionStringBuilder(My.Settings.UserConnectionString)
        Catch
            _ConnectionString = New SqlConnectionStringBuilder
        End Try
        InitializeComponent()
    End Sub
    ''' <summary>CTor - creates a new instance of the <see cref="winSelectDatabase"/> class with given connection string</summary>
    ''' <param name="ConnectionString">A connection string to be edited by new instance</param>
    Public Sub New(ByVal ConnectionString As String)
        Me.New()
        _ConnectionString = New SqlConnectionStringBuilder(ConnectionString)
        ImageRoot = My.Settings.ImageRoot
    End Sub
    ''' <summary>Contains value of the <see cref="ConnectionString"/> property</summary>
    Private _ConnectionString As SqlConnectionStringBuilder
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub winSelectDatabase_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        prgProperties.SelectedObject = ConnectionString
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        If ImageRoot <> "" AndAlso Not IO.Directory.Exists(ImageRoot) Then
            mBox.MsgBoxFW(My.Resources.msg_DirectoryDoesNotExist, MsgBoxStyle.Exclamation, My.Resources.txt_ImageRoot, Me, ImageRoot)
            Return
        End If
        If Not ConnectionString.MultipleActiveResultSets Then
            If mBox.Modal_PTWBIO(My.Resources.msg_MultipleActiveResultsetsFalse & vbCrLf & My.Resources.lbl_ClickCancelToChangeConnectionString, My.Resources.txt_MultipleActiveResultsets, Me, mBox.MessageBoxButton.Buttons.Ignore Or mBox.MessageBoxButton.Buttons.Cancel, mBox.GetIcon(mBox.MessageBoxIcons.Warning)) <> Forms.DialogResult.Ignore Then
                Return
            End If
        End If
        Me.DialogResult = True
        Me.Close()
    End Sub

    ''' <summary>Gets or sets connection string selected by this dialog</summary>
    Public Property ConnectionString() As SqlConnectionStringBuilder
        Get
            Return _ConnectionString
        End Get
        Set(ByVal value As SqlConnectionStringBuilder)
            _ConnectionString = value
        End Set
    End Property

    ''' <summary>Gets or sets path of folder where images are stored</summary>
    Public Property ImageRoot$
        Get
            Return txtImageRoot.Text
        End Get
        Set(ByVal value$)
            txtImageRoot.Text = value
        End Set
    End Property

    Private Sub btnWizard_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnWizard.Click
        Dim wiz As New winDatabaseWizard
        wiz.Owner = Me
        If wiz.ShowDialog() Then
            _ConnectionString = wiz.WizardData.FinalConnectionString
            prgProperties.SelectedObject = _ConnectionString
            txtImageRoot.Text = wiz.WizardData.FinalImageRoot
        End If
    End Sub

    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnTest.Click
        Try
            Using conn As New SqlConnection(ConnectionString.ToString)
                conn.Open()
            End Using
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Return
        End Try
        mBox.Modal_PTIW(My.Resources.msg_TestConnectionSucceeded, My.Resources.txt_TestConnection, mBox.MessageBoxIcons.OK, Me)
    End Sub

    Private Sub btnImageRoot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnImageRoot.Click
        Dim dlg As New Forms.FolderBrowserDialog
        Try
            dlg.SelectedPath = txtImageRoot.Text
        Catch : End Try
        If dlg.ShowDialog(Me) Then
            txtImageRoot.Text = dlg.SelectedPath
        End If
    End Sub

    Private Sub btnDbSettings_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDbSettings.Click
        Try
            Using context As New CapsDataContext(New System.Data.EntityClient.EntityConnection(CapsDataContext.DefaultMetadataWorkspace, New SqlConnection(ConnectionString.ToString)))
                Dim win As New winDatabaseSettings(context) With {.Owner = Me}
                win.ShowDialog()
            End Using
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
        End Try
    End Sub
End Class
