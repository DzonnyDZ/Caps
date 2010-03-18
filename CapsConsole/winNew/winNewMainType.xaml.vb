Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Caps.Data


''' <summary>Dialog used to create a new instance of <see cref="MainType"/> class</summary>
Partial Public Class winNewMainType
    Inherits CreateNewObjectDialogBase(Of MainType)
    ''' <summary>CTor</summary>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        If Not IO.File.Exists(txtImagePath.Text) Then
            Select Case mBox.ModalF_PTWBIa(My.Resources.msg_FileNotExists_ContinueWOImage, My.Resources.txt_MainTypeImage, Me, mBox.MessageBoxButton.Buttons.Yes Or WindowsT.IndependentT.MessageBox.MessageBoxButton.Buttons.No, mBox.GetIcon(mBox.MessageBoxIcons.Question), txtImagePath.Text)
                Case Forms.DialogResult.Yes
                Case Else : Exit Sub
            End Select
        ElseIf IO.Path.GetExtension(txtImagePath.Text).ToLower <> ".png" Then
            mBox.Modal_PTIW(My.Resources.msg_OnlyPNG, My.Resources.txt_MainTypeImage, WindowsT.IndependentT.MessageBox.MessageBoxIcons.Exclamation, Me)
            Exit Sub
        End If
        NewObject = New MainType With {.Description = txtDescription.Text, .TypeName = txtName.Text}
        Try
            Context.MainTypes.AddObject(NewObject)
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            'Context.MainTypes.DeleteAllNew()
            Exit Sub
        End Try
        If IO.File.Exists(txtImagePath.Text) Then
            If Not IO.Directory.Exists(IO.Path.Combine(My.Settings.ImageRoot, "MainType")) Then
                Try
                    IO.Directory.CreateDirectory(IO.Path.Combine(My.Settings.ImageRoot, "MainType"))
                Catch ex As Exception
                    mBox.Error_XPTIBWO(ex, My.Resources.err_CreatingDirectoryMainType, My.Resources.txt_FileSystemError, mBox.MessageBoxIcons.Exclamation, , Me)
                    Me.DialogResult = True
                    Me.Close()
                End Try
            End If
            Try
                IO.File.Copy(txtImagePath.Text, IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "MainType"), NewObject.MainTypeID & ".png"))
            Catch ex As Exception
                mBox.Error_XPTIBWO(ex, My.Resources.msg_CopyMainTypeImageError, My.Resources.txt_FileSystemError, WindowsT.IndependentT.MessageBox.MessageBoxIcons.Exclamation, , Me)
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
        If dlg.ShowDialog = Forms.DialogResult.OK Then
            txtImagePath.Text = dlg.FileName
        End If
    End Sub
End Class
