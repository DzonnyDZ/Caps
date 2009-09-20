Imports System.Data.SqlClient
Partial Public Class winSelectDatabase
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(ByVal ConnectionString As String)
        Me.New()
        _ConnectionString = New SqlConnectionStringBuilder(ConnectionString)
    End Sub
    Private _ConnectionString As New SqlConnectionStringBuilder(My.Settings.UserConnectionString)
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub winSelectDatabase_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        prgProperties.SelectedObject = ConnectionString
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Me.DialogResult = True
        Me.Close()
    End Sub
    Public Property ConnectionString() As SqlConnectionStringBuilder
        Get
            Return _ConnectionString
        End Get
        Set(ByVal value As SqlConnectionStringBuilder)
            _ConnectionString = value
        End Set
    End Property

    Private Sub btnWizard_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnWizard.Click
        Dim wiz As New winDatabaseWizard
        wiz.Owner = Me
        wiz.ShowDialog()
        'TODO: Read connection string
    End Sub
End Class
