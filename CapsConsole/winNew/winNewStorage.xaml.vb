Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Tools.CollectionsT.GenericT

Partial Public Class winNewStorage

    ''' <summary>CTor</summary>
    ''' <param name="Context">Data context</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New(ByVal Context As CapsDataDataContext)
        InitializeComponent()
        If Context Is Nothing Then Throw New ArgumentNullException("Context")
        Me.Context = Context
    End Sub

    Private Context As CapsDataDataContext
    Private _NewObject As Storage
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            _NewObject = New Storage() With {.StorageNumber = txtNumber.Text, .Description = txtDescription.Text, .StorageTypeID = cmbStorageType.SelectedValue}
            Context.Storages.InsertOnSubmit(_NewObject)
        Catch ex As Exception
            mBox.Error_X(ex)
            Exit Sub
        End Try
        Try
            Context.SubmitChanges()
        Catch ex As Exception
            Context.Storages.DeleteAllNew()
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
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.StorageType, Context)
        If win.ShowDialog Then
            DirectCast(cmbStorageType.ItemsSource, ListWithEvents(Of StorageType)).Add(DirectCast(win.NewObject, StorageType))
            cmbStorageType.Items.Refresh()
            cmbStorageType.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub winNewStorage_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cmbStorageType.ItemsSource = New ListWithEvents(Of StorageType)(From item In Context.StorageTypes Order By item.Name)
    End Sub
End Class
