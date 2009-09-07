Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Tools.CollectionsT.GenericT

Partial Public Class winNewStorage


    Private Context As New CapsDataDataContext(Main.Connection)
    Private _NewObject As Storage
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            _NewObject = New Storage() With {.StorageNumber = nudNumber.Value, .Description = txtDescription.Text, .StorageTypeID = cmbStorageType.SelectedValue}
            Context.Storages.InsertOnSubmit(_NewObject)
        Catch ex As Exception
            mBox.Error_X(ex)
            Exit Sub
        End Try
        Try
            Context.SubmitChanges()
        Catch ex As Exception
            Context.Storages.DeleteOnSubmit(_NewObject)
            mBox.Error_X(ex)
            Exit Sub
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    Public ReadOnly Property NewObject() As Storage
        Get
            Return _NewObject
        End Get
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub



    Private Sub cmdNewType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdNewType.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.StorageType)
        If win.ShowDialog Then
            DirectCast(cmbStorageType.ItemsSource, ListWithEvents(Of StorageType)).Add(DirectCast(win.NewObject, StorageType))
            cmbStorageType.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub winNewStorage_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cmbStorageType.ItemsSource = New ListWithEvents(Of StorageType)(From item In Context.StorageTypes Order By item.Name)
    End Sub
End Class
