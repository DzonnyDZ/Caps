Imports Tools.TypeTools
Imports System.ComponentModel
Imports Tools.CollectionsT.GenericT
Imports Caps.Data, Tools.WindowsT.WPF

''' <summary>Dialog used to create a new instance of <see cref="Storage"/> class</summary>
Partial Public Class winNewStorage
    Inherits CreateNewObjectDialogBase(Of Storage)

    ''' <summary>Conmtains storegaes created by this window and windows opened from this window</summary>
    Private newObjects As New List(Of Storage)
    ''' <summary>Gets storages created by this window and windows opened by this window transformed to given context</summary>
    ''' <param name="context">Context to transform <see cref="NewObject"/> to. <see cref="NewObject"/> is not transformed when this argument is null.</param>
    ''' <returns><see cref="NewObject"/> transformed to <paramref name="context"/>. <see cref="NewObject"/> without transformation, when <paramref name="context"/> is null</returns>
    Public Shadows Function GetNewObjects(Optional ByVal context As CapsDataContext = Nothing) As IEnumerable(Of Storage)
        If context Is Nothing Then Return newObjects
        Return (From item In newObjects Select DirectCast(context.GetObjectByKey(item.EntityKey), Storage)).ToArray
    End Function

    ''' <summary>CTor</summary>
    ''' <param name="HasCapsState">Defines state of the "Has caps" checkbox</param>
    Public Sub New(ByVal HasCapsState As CheckBoxState)
        InitializeComponent()
        chkHasCaps.IsEnabled = HasCapsState.HasFlag(CheckBoxState.Enabled)
        chkHasCaps.IsChecked = HasCapsState.HasFlag(CheckBoxState.Checked)
        chkHasCaps.Visibility = If(HasCapsState.HasFlag(CheckBoxState.Visible), Visibility.Visible, Visibility.Hidden)
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        If Not IO.File.Exists(txtImagePath.Text) Then
            Select Case mBox.ModalF_PTWBIa(My.Resources.msg_FileNotExists_ContinueWOImage, My.Resources.txt_StorageImage, Me, mBox.MessageBoxButton.Buttons.Yes Or WindowsT.IndependentT.MessageBox.MessageBoxButton.Buttons.No, mBox.GetIcon(mBox.MessageBoxIcons.Question), txtImagePath.Text)
                Case Forms.DialogResult.Yes
                Case Else : Exit Sub
            End Select
        End If
        Try
            NewObject = New Storage() With {
                .StorageNumber = txtNumber.Text,
                .Description = txtDescription.Text,
                .StorageTypeID = cmbStorageType.SelectedValue,
                .HasCaps = chkHasCaps.IsChecked,
                .ParentStorageID = cmbParent.SelectedValue
            }
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
        If IO.File.Exists(txtImagePath.Text) Then
            Dim imagePath = txtImagePath.Text
SaveImage:  Try
                NewObject.SaveImage(imagePath, True)
            Catch ex As Exception
                If mBox.Error_XPTIBWO(ex, My.Resources.msg_CopyStorageImageError & vbCrLf & My.Resources.txt_SelectAnotherImageQ, My.Resources.txt_FileSystemError, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Yes Or mBox.MessageBoxButton.Buttons.Ignore, Me) = Forms.DialogResult.Yes Then
                    imagePath = GetImage(imagePath)
                    If imagePath IsNot Nothing Then GoTo SaveImage
                End If
            End Try
        End If
        Me.DialogResult = True
        newObjects.Add(NewObject)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub cmdNewType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdNewType.Click
        Using win As New winNewSimple(Of StorageType) With {.Owner = Me}
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
        cmbParent.ItemsSource = New ListWithEvents(Of Storage)(From item In Context.Storages Order By item.StorageNumber)
    End Sub

    Private Sub cmdNewParent_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdNewParent.Click
        Using win As New winNewStorage(CheckBoxState.Visible Or CheckBoxState.Enabled) With {.Owner = Me.FindLogicalAncestor(Of Window)()}
            Dim result = win.ShowDialog
            Dim newObjects = win.GetNewObjects(Context)
            DirectCast(cmbParent.ItemsSource, ListWithEvents(Of Storage)).AddRange(newObjects)
            cmbParent.Items.Refresh()
            If result Then
                cmbParent.SelectedItem = newObjects.Last
            End If
            Me.newObjects.AddRange(newObjects)
        End Using
    End Sub

    Private Sub btnImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnImage.Click
        txtImagePath.Text = If(GetImage(txtImagePath.Text), txtImagePath.Text)
    End Sub
End Class
