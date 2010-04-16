Imports System.Data.SqlClient
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Microsoft.Win32
Imports Tools.WindowsT.NativeT

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
        wiz.ShowDialog()
        'TODO: Read connection string
    End Sub

    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnTest.Click
        Try
            Using conn As New SqlConnection(ConnectionString.ToString)
                conn.Open()
            End Using
        Catch ex As Exception
            mBox.Error_X(ex)
            Return
        End Try
        mBox.Modal_PTI(My.Resources.msg_TestConnectionSucceeded, My.Resources.txt_TestConnection, mBox.MessageBoxIcons.OK)
    End Sub

    Private Sub btnImageRoot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnImageRoot.Click
        Dim dlg As New Forms.FolderBrowserDialog
        Try
            dlg.SelectedPath = txtImageRoot.Text
        Catch : End Try
        If dlg.ShowDialog(New Win32Window(Me)) Then
            txtImageRoot.Text = dlg.SelectedPath
        End If
    End Sub
End Class
