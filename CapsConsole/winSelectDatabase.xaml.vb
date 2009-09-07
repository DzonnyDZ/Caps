Imports System.Data.SqlClient
Partial Public Class winSelectDatabase
    Private _ConnectionString As New SqlConnectionStringBuilder(My.Settings.CapsDataConnectionString)
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
End Class
