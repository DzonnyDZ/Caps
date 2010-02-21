Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel, Caps.Data
Imports mBox = Tools.WindowsT.IndependentT.MessageBox

Partial Public Class winNewShape
    ''' <summary>CTor</summary>
    ''' <param name="Context">Data context</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New(ByVal Context As CapsDataDataContext)
        InitializeComponent()
        If Context Is Nothing Then Throw New ArgumentNullException("Context")
        Me.Context = Context
    End Sub

    Private Context As CapsDataDataContext
    Private _NewObject As Shape
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
            _NewObject = New Shape() With {.Name = txtName.Text, .Description = txtDescription.Text, .Size1Name = txtSize1Name.Text, .Size2Name = txtSize2Name.Text}
            Context.Shapes.InsertOnSubmit(_NewObject)
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            Context.Shapes.DeleteAllNew()
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        If IO.File.Exists(txtImagePath.Text) Then
            If Not IO.Directory.Exists(IO.Path.Combine(My.Settings.ImageRoot, "Shape")) Then
                Try
                    IO.Directory.CreateDirectory(IO.Path.Combine(My.Settings.ImageRoot, "Shape"))
                Catch ex As Exception
                    mBox.Error_XPTIBWO(ex, My.Resources.err_CreatingDirectoryShape, My.Resources.txt_FileSystemError, mBox.MessageBoxIcons.Exclamation, , Me)
                    Me.DialogResult = True
                    Me.Close()
                End Try
            End If
            Try
                IO.File.Copy(txtImagePath.Text, IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "Shape"), NewObject.ShapeID & ".png"))
            Catch ex As Exception
                mBox.Error_XPTIBWO(ex, My.Resources.msg_CopyShapeImageError, My.Resources.txt_FileSystemError, mBox.MessageBoxIcons.Exclamation, , Me)
            End Try
        End If
        Me.DialogResult = True
        Me.Close()
    End Sub

    Public ReadOnly Property NewObject() As Shape
        Get
            Return _NewObject
        End Get
    End Property

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
