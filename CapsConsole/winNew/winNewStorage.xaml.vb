Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Tools.CollectionsT.GenericT
Imports Caps.Data

''' <summary>Dialog used to create a new instance of <see cref="Storage"/> class</summary>
Partial Public Class winNewStorage
    Inherits CreateNewObjectDialogBase(Of Storage)
    ''' <summary>CTor</summary>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            NewObject = New Storage() With {.StorageNumber = txtNumber.Text, .Description = txtDescription.Text, .StorageTypeID = cmbStorageType.SelectedValue}
            Context.Storages.AddObject(NewObject)
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            'Context.Storages.DeleteAllNew()
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub cmdNewType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdNewType.Click
        Using win As New winNewSimple(Of StorageType)
            If win.ShowDialog Then
                Dim newObject As StorageType = win.GetNewObject(Context)
                DirectCast(cmbStorageType.ItemsSource, ListWithEvents(Of StorageType)).Add(newObject)
                cmbStorageType.Items.Refresh()
                cmbStorageType.SelectedItem = newObject
            End If
        End Using
    End Sub

    Private Sub winNewStorage_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cmbStorageType.ItemsSource = New ListWithEvents(Of StorageType)(From item In Context.StorageTypes Order By item.Name)
    End Sub
End Class
