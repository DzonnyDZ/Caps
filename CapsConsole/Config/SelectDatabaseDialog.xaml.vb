Imports System.Data.SqlClient


''' <summary>DIalog to be used to select database</summary>
Public Class SelectDatabaseDialog
    ''' <summary>Contains connection string to database server</summary>
    Private connectionString As String
    ''' <summary>CTor - creates a new instance of the <see cref="SelectDatabaseDialog"/> class</summary>
    ''' <param name="connectionString">Connection string to database server to show databases from</param>
    ''' <exception cref="ArgumentNullException"><paramref name="connectionString"/> is null</exception>
    Public Sub New(ByVal connectionString As String)
        If connectionString Is Nothing Then Throw New ArgumentNullException("connectionString")
        Me.InitializeComponent()
        Me.connectionString = connectionString
    End Sub

    Private Sub SelectDatabaseDialog_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim cmd As New SqlCommand("SELECT name FROM sys.databases WHERE owner_sid <> 0x01") With {.Connection = conn}
                Dim items As New List(Of String)
                Using r = cmd.ExecuteReader
                    While r.Read
                        items.Add(r!name)
                    End While
                End Using
                If items.Count = 0 Then
                    mBox.MsgBox(My.Resources.wiz_txt_NoDatabasesFound, vbExclamation, My.Resources.wiz_txt_DatabaseList, Me)
                    Close()
                Else
                    lstDatabases.ItemsSource = items
                    If DatabaseName <> "" Then lstDatabases.SelectedItem = DatabaseName
                End If
            End Using
        Catch ex As Exception
            mBox.MsgBox(My.Resources.wiz_err_DatabaseList & vbCrLf & ex.Message, MsgBoxStyle.Critical, My.Resources.wiz_txt_DatabaseList, Me)
            DialogResult = False
            Close()
        End Try
    End Sub

    Private _DatabaseName As String
    ''' <summary>Gets or sets database selected by user</summary>
    ''' <remarks>This property should not be set once dialog is shown</remarks>
    Public Property DatabaseName() As String
        Get
            Return _DatabaseName
        End Get
        Set(ByVal value As String)
            _DatabaseName = value
        End Set
    End Property
    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdOk.Click
        DialogResult = True
        Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdCancel.Click
        DialogResult = False
        Close()
    End Sub

    Private Sub lst_SelectionChanged(ByVal sender As ListBox, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles lstDatabases.SelectionChanged
        cmdOk.IsEnabled = sender.SelectedIndex <> -1
        If lstDatabases.SelectedIndex <> -1 Then DatabaseName = lstDatabases.SelectedItem Else DatabaseName = Nothing
    End Sub
End Class
