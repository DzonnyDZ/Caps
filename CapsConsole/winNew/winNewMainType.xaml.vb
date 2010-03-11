Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Caps.Data

Partial Public Class winNewMainType
    Implements IDisposable
    ''' <summary>CTor</summary>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New()
        InitializeComponent()
        Me.Context = New CapsDataContext(Main.EntityConnection)
    End Sub
    ''' <summary>Data context</summary>
    Private Context As CapsDataContext
    Private _NewObject As MainType
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
        _NewObject = New MainType With {.Description = txtDescription.Text, .TypeName = txtName.Text}
        Try
            Context.MainTypes.AddObject(_NewObject)
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

    Public ReadOnly Property NewObject() As MainType
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
        If dlg.ShowDialog = Forms.DialogResult.OK Then
            txtImagePath.Text = dlg.FileName
        End If
    End Sub

#Region "IDisposable Support"
    ''' <summary>To detect redundant calls</summary>
    Private disposedValue As Boolean

    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">Trie whan called from <see cref="Dispose"/></param>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If Context IsNot Nothing Then Context.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub


    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    ''' <filterpriority>2</filterpriority>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
