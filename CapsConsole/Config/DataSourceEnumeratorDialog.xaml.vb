Imports System.Data.Sql
Imports System.ComponentModel
Imports System.Data

''' <summary>Dialog can be used to enumerate Microsft SQL Server instances</summary>
Public Class DataSourceEnumeratorDialog

    ''' <summary><see cref="BackgroundWorker"/> used to </summary>
    Private WithEvents wk As New BackgroundWorker
    ''' <summary>Contains servers found by <see cref="SqlDataSourceEnumerator"/></summary>
    Private servers As DataTable

    Private Sub DataSourceEnumeratorDialog_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        wk.RunWorkerAsync()
    End Sub
    Private _SelectedServer As String
    ''' <summary>Gets or sets name of selected server</summary>
    ''' <remarks>Value of thsi property should be set only prior window is displayed</remarks>
    Public Property SelectedServer As String
        Get
            Return _SelectedServer
        End Get
        Set(ByVal value As String)
            _SelectedServer = value
        End Set
    End Property


    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOk.Click
        DialogResult = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        DialogResult = False
        Me.Close()
    End Sub

    Private Sub wk_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles wk.DoWork
        servers = SqlDataSourceEnumerator.Instance.GetDataSources()
    End Sub

    Private Sub wk_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles wk.RunWorkerCompleted
        dgrInstances.ItemsSource = servers.DefaultView
        lblWait.Visibility = Windows.Visibility.Collapsed
        dgrInstances.Visibility = Windows.Visibility.Visible
        txbCantSee.Visibility = Windows.Visibility.Visible
        If SelectedServer <> "" Then
            Dim parts = SelectedServer.Split({"\"c}, 2)
            If parts.Length = 2 Then
                Dim i% = 0
                For Each item As DataRowView In dgrInstances.Items
                    If (DirectCast(item!ServerName, String) = parts(0) OrElse (parts(0) = "." AndAlso DirectCast(item!ServerName, String) = "")) AndAlso DirectCast(item!InstanceName, String) = parts(1) Then
                        dgrInstances.SelectedIndex = i
                        Exit Sub
                    End If
                    i += 1
                Next
            End If
        End If
        SelectedServer = Nothing
    End Sub

    Private Sub dgrInstances_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles dgrInstances.SelectionChanged
        If dgrInstances.SelectedIndex = -1 Then
            SelectedServer = Nothing
        Else
            Dim item As DataRowView = dgrInstances.SelectedItem
            SelectedServer = String.Format("{0}\{1}", If(DirectCast(item!ServerName, String) = "", ".", item!ServerName), item!InstanceName)
        End If
        btnOk.IsEnabled = dgrInstances.SelectedIndex <> -1
    End Sub
End Class
