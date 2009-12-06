Imports mBox = Tools.WindowsT.WPF.DialogsT.MessageBox
Imports Tools.DrawingT.ImageTools, Tools.ExtensionsT
''' <summary>Allows to create new cap type based on suggestion</summary>
Public Class winCreateNewType : Implements IDisposable
    ''' <summary>CTor</summary>
    ''' <param name="Context">Data contetx</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New(ByVal Context As CapsDataDataContext)
        If Context Is Nothing Then Throw New ArgumentNullException("Context")
        Me.Context = Context
        InitializeComponent()
        cmbMainType.ItemsSource = Context.MainTypes
        cmbMaterial.ItemsSource = Context.Materials
        cmbShape.ItemsSource = Context.Shapes
        Dim Targets = Context.Targets.ToList
        Targets.Add(Nothing)
        cmbTarget.ItemsSource = Targets
    End Sub
    ''' <summary>Data context</summary>
    Private Context As CapsDataDataContext
    ''' <summary>List of temporaryr file created during form life time</summary>
    Private TemporaryFiles As New List(Of String)

    Private Sub mniImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim sourcePath = IO.Path.Combine(My.Settings.ImageRoot, DirectCast(DirectCast(sender, MenuItem).DataContext, Image).RelativePath)
        Try
            Dim img As New System.Drawing.Bitmap(sourcePath)
            Dim targImage As System.Drawing.Bitmap
            If img.Width > 32 OrElse img.Height > 32 Then
                targImage = img.GetThumbnail(New System.Drawing.Size(32, 32))
            Else
                targImage = img
            End If
            Dim tempFile = IO.Path.GetTempFileName & ".png"
            targImage.Save(tempFile, System.Drawing.Imaging.ImageFormat.Png)
            TemporaryFiles.Add(tempFile)
            txtPicturePath.Text = tempFile
        Catch ex As Exception
            mBox.MsgBox("Failed to generate image thumbnail in PNG format:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, ex.GetType.Name, Me)
        End Try
    End Sub

    Private Sub cmdOK_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdOK.Click
        'Chcecks
        If String.IsNullOrEmpty(txtName.Text) Then
            mBox.MsgBox(My.Resources.msg_EnterTypeName, MsgBoxStyle.Exclamation, My.Resources.txt_IncompleteEntry, Me)
            txtName.Focus()
            Exit Sub
        End If
        If txtPicturePath.Text = "" Then
            If mBox.MsgBox(My.Resources.err_TypeImageNotEnteredContinue, MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.txt_ImageNotSet, Me) <> MsgBoxResult.Yes Then
                txtPicturePath.Focus()
                Exit Sub
            End If
        ElseIf Not IO.File.Exists(txtPicturePath.Text) Then
            If mBox.MsgBox(My.Resources.msg_FileNotExists_ContinueWOImage.f(txtPicturePath.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.txt_ImageDoesNotExist, Me) <> MsgBoxResult.Yes Then
                txtPicturePath.Focus()
                Exit Sub
            End If
        ElseIf IO.Path.GetExtension(txtPicturePath.Text).ToLower <> ".png" Then
            mBox.MsgBox(My.Resources.err_ImageMustBePNG, MsgBoxStyle.Exclamation, My.Resources.txt_UnsupportedImageType, Me)
            txtPicturePath.Focus()
            Exit Sub
        End If
        'Type
        Dim Type As New CapType With {
            .TypeName = txtName.Text,
            .MainType = cmbMainType.SelectedItem,
            .Shape = cmbShape.SelectedItem,
            .Size = nudSize1.Value,
            .Size2 = If(.Shape.Size2Name IsNot Nothing, New Integer?(nudSize2.Value), New Integer?),
            .Height = nudHeight.Value,
            .Material = cmbMaterial.SelectedItem,
            .Target = cmbTarget.SelectedItem,
            .Description = txtDesciption.Text
        }
        Context.CapTypes.Attach(Type)
        'Caps for type
        For Each Cap In CheckedCaps
            Cap.CapType = Type
        Next

        Try
            Context.SubmitChanges()
        Catch ex As Exception
            'Undo
            For Each Cap In CheckedCaps
                Cap.CapType = Nothing
            Next
            Context.CapTypes.DeleteOnSubmit(Type)
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        'Image                                                                   
        Try
            IO.File.Copy(txtPicturePath.Text, IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "CapType"), Type.CapTypeID & ".png"))
        Catch ex As Exception
            mBox.Error_XPTIBWO(ex, My.Resources.msg_CopyCapTypeImageError, My.Resources.txt_CopyFile, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Error, , Me)
        End Try
        'Close
        _CreatedType = Type
        Me.DialogResult = True
        Me.Close()
    End Sub
    Private _CreatedType As CapType
    ''' <summary>When windows is closed by OK button, gets <see cref="CapType"/> that was created by the window</summary>
    Public ReadOnly Property CreatedType As CapType
        Get
            Return _CreatedType
        End Get
    End Property

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub
    ''' <summary>Contains list of caps selected for inclusion in newly created type</summary>
    Private CheckedCaps As New List(Of Cap)

    Private Sub chkCap_Checked(ByVal sender As CheckBox, ByVal e As System.Windows.RoutedEventArgs)
        CheckedCaps.Add(sender.DataContext)
    End Sub

    Private Sub chkCap_Unchecked(ByVal sender As CheckBox, ByVal e As System.Windows.RoutedEventArgs)
        CheckedCaps.Remove(sender.DataContext)
    End Sub

    Private Sub cmdPicturePath_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdPicturePath.Click
        Dim dlg As New System.Windows.Forms.OpenFileDialog() With {
            .DefaultExt = "png",
            .Filter = My.Resources.fil_PNG
        }
        If txtPicturePath.Text <> "" Then
            Try
                dlg.FileName = txtPicturePath.Text
            Catch : End Try
        End If
        If dlg.ShowDialog = Forms.DialogResult.OK Then
            txtPicturePath.Text = dlg.FileName
        End If
    End Sub

#Region "IDisposable Support"
    ''' <summary>To detect redundant calls</summary>
    Private disposedValue As Boolean

    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True if disposing, false if finalizing</param>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            For Each file In TemporaryFiles
                Try : IO.File.Delete(file)
                Catch : End Try
            Next
            TemporaryFiles.Clear()
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>Allows an <see cref="System.Object"/> to attempt to free resources and perform other cleanup operations before the <see cref="System.Object"/> is reclaimed by garbage collection.</summary>
    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
