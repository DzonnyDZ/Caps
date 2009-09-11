Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
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
            Select Case mBox.ModalF_PTBIa(My.Resources.msg_FileNotExists_ContinueWOImage, My.Resources.txt_ShapeImage, WindowsT.IndependentT.MessageBox.MessageBoxButton.Buttons.Yes Or WindowsT.IndependentT.MessageBox.MessageBoxButton.Buttons.No, WindowsT.IndependentT.MessageBox.MessageBoxIcons.Question, txtImagePath.Text)
                Case Forms.DialogResult.Yes
                Case Else : Exit Sub
            End Select
        ElseIf IO.Path.GetExtension(txtImagePath.Text).ToLower <> ".png" Then
            mBox.Modal_PTI(My.Resources.msg_OnlyPNG, My.Resources.txt_ShapeImage, WindowsT.IndependentT.MessageBox.MessageBoxIcons.Exclamation)
            Exit Sub
        End If
        Try
            _NewObject = New Shape() With {.Name = txtName.Text, .Description = txtDescription.Text, .Size1Name = txtSize1Name.Text, .Size2Name = txtSize2Name.Text}
            Context.Shapes.InsertOnSubmit(_NewObject)
        Catch ex As Exception
            mBox.Error_X(ex)
            Exit Sub
        End Try
        Try
            Context.SubmitChanges()
        Catch ex As Exception
            Context.Shapes.DeleteAllNew()
            mBox.Error_X(ex)
            Exit Sub
        End Try
        If IO.File.Exists(txtImagePath.Text) Then
            Try
                IO.File.Copy(txtImagePath.Text, IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "Shape"), NewObject.ShapeID & ".png"))
            Catch ex As Exception
                mBox.Error_XPTIBWO(ex, My.Resources.msg_CopyShapeImageError, My.Resources.txt_FileSystemError, WindowsT.IndependentT.MessageBox.MessageBoxIcons.Exclamation)
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
