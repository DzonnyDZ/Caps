Imports Tools.TypeTools
Imports System.ComponentModel, Caps.Data

''' <summary>Dialog used to create a new instance of <see cref="Shape"/> class</summary>
Partial Public Class winNewShape
    Inherits CreateNewObjectDialogBase(Of Shape)
    ''' <summary>CTor</summary>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        If Not IO.File.Exists(txtImagePath.Text) Then
            Select Case mBox.ModalF_PTWBIa(My.Resources.msg_FileNotExists_ContinueWOImage, My.Resources.txt_ShapeImage, Me, mBox.MessageBoxButton.Buttons.Yes Or mBox.MessageBoxButton.Buttons.No, mBox.GetIcon(mBox.MessageBoxIcons.Question), txtImagePath.Text)
                Case Forms.DialogResult.Yes
                Case Else : Exit Sub
            End Select
        ElseIf IO.Path.GetExtension(txtImagePath.Text).ToLower <> ".png" Then
            mBox.Modal_PTIW(My.Resources.msg_OnlyPNG, My.Resources.txt_ShapeImage, mBox.MessageBoxIcons.Exclamation, Me)
            Exit Sub
        End If
        Try
            NewObject = New Shape() With {.Name = txtName.Text, .Description = txtDescription.Text, .Size1Name = txtSize1Name.Text, .Size2Name = txtSize2Name.Text}
            Context.Shapes.AddObject(NewObject)
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            'Context.Shapes.DeleteAllNew()
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        If IO.File.Exists(txtImagePath.Text) Then
            If Not IO.Directory.Exists(IO.Path.Combine(My.Settings.ImageRoot, Shape.ImageStorageFolderName)) Then
                Try
                    IO.Directory.CreateDirectory(IO.Path.Combine(My.Settings.ImageRoot, Shape.ImageStorageFolderName))
                Catch ex As Exception
                    mBox.Error_XPTIBWO(ex, My.Resources.err_CreatingDirectoryShape, My.Resources.txt_FileSystemError, mBox.MessageBoxIcons.Exclamation, , Me)
                    Me.DialogResult = True
                    Me.Close()
                End Try
            End If
            Try
                IO.File.Copy(txtImagePath.Text, IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, Shape.ImageStorageFolderName), NewObject.ShapeID & ".png"))
            Catch ex As Exception
                mBox.Error_XPTIBWO(ex, My.Resources.msg_CopyShapeImageError, My.Resources.txt_FileSystemError, mBox.MessageBoxIcons.Exclamation, , Me)
            End Try
        End If
        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub btnImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnImage.Click
        Dim dlg As New Forms.OpenFileDialog With {.DefaultExt = "png", .Filter = My.Resources.fil_PNG}
        Try
            If txtImagePath.Text <> "" Then dlg.FileName = txtImagePath.Text
        Catch : End Try
        If dlg.ShowDialog Then
            txtImagePath.Text = dlg.FileName
        End If
    End Sub
End Class
