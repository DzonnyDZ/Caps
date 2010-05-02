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
            Dim imagePath = txtImagePath.Text
SaveImage:  Try
                NewObject.SaveImage(imagePath, True)
            Catch ex As Exception
                If mBox.Error_XPTIBWO(ex, My.Resources.msg_CopyShapeImageError & vbCrLf & My.Resources.txt_SelectAnotherImageQ, My.Resources.txt_FileSystemError, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Yes Or mBox.MessageBoxButton.Buttons.Ignore, Me) = vbYes Then
                    imagePath = GetImage(imagePath)
                    If imagePath IsNot Nothing Then GoTo SaveImage
                End If
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
       txtImagePath.Text = If(GetImage(txtImagePath.Text), txtImagePath.Text)
    End Sub
End Class
