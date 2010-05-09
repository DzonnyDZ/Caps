Imports Caps.Data, Tools.WindowsT.InteropT.InteropExtensions
Imports Tools.DataT.ObjectsT.EntityFrameworkExtensions

''' <summary>Window used to migrate images between database and file system</summary>
Public Class winSyncImages

    Private Sub btnChangeImageRoot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnChangeImageRoot.Click
        Dim dlg As New Forms.FolderBrowserDialog
        Try
            dlg.SelectedPath = My.Settings.ImageRoot
        Catch :End Try
        If dlg.ShowDialog(Me) Then
            lblImageRoot.Content = dlg.SelectedPath
            My.Settings.ImageRoot = dlg.SelectedPath
            My.Settings.Save()
        End If
    End Sub

    Private Sub winSyncImages_Loaded(ByVal sender As Window, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        icImagesInDb.ItemsSource = Settings.Images.CapsInDatabase
        icImagesInFS.ItemsSource = Settings.Images.CapsInFileSystem
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Button, ByVal e As System.Windows.RoutedEventArgs) Handles btnAddDb.Click, btnAddFS.Click
        Dim ic As ItemsControl
        If sender Is btnAddDb Then ic = icImagesInDb Else ic = icImagesInFS
        Dim source As Integer() = ic.ItemsSource
        ReDim Preserve source(source.Length)
        ic.ItemsSource = source
    End Sub

    Private Sub btnSaveDb_Click(ByVal sender As Button, ByVal e As System.Windows.RoutedEventArgs) Handles btnSaveDb.Click
        If (From size In DirectCast(icImagesInDb.ItemsSource, Integer()) Group By size Into cnt = Count() Select cnt).Max > 0 Then
            mBox.MsgBox("Unique size must be specified.", MsgBoxStyle.Exclamation, "Save database settings", Me)
            Exit Sub
        End If
        Settings.Images.CapsInDatabase = icImagesInDb.ItemsSource
        icImagesInDb.ItemsSource = Settings.Images.CapsInDatabase
    End Sub

    Private Sub btnSaveFS_Click(ByVal sender As Button, ByVal e As System.Windows.RoutedEventArgs) Handles btnSaveFS.Click
        If (From size In DirectCast(icImagesInFS.ItemsSource, Integer()) Group By size Into cnt = Count() Select cnt).Max > 0 Then
            mBox.MsgBox("Unique size must be specified.", MsgBoxStyle.Exclamation, "Save database settings", Me)
            Exit Sub
        End If
        Settings.Images.CapsInDatabase = icImagesInFS.ItemsSource
        icImagesInFS.ItemsSource = Settings.Images.CapsInDatabase
    End Sub

    Private Sub btnDelFS_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelFS.Click
        If mBox.MsgBox("Do you really want to delete all images of Caps from file system that are not of given sizes? This process examines all folders in your Image Root folder and deletes those which name represents cap image size different than specified.",
                        MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Delete images", Me) = MsgBoxResult.Yes Then
            'Delete images which are not of given sizes (from FS)
            Try
                Dim errcount% = 0
                Dim delcount% = 0
                For Each folder In IO.Directory.EnumerateDirectories(My.Settings.ImageRoot)
                    Dim folderName As String = IO.Path.GetFileName(folder)
                    Dim match = CapsDataExtensions.imageFolderNameRegExp.Match(folderName)
                    If folderName.ToLowerInvariant = "original" OrElse match.Success Then
                        Dim size = If(folder.ToLowerInvariant = "original", 0, Integer.Parse(match.Groups!Size.Value, System.Globalization.CultureInfo.InvariantCulture))
                        If Not DirectCast(icImagesInFS.ItemsSource, Integer()).Contains(size) Then
DeleteFolder:               Try
                                IO.Directory.Delete(folder, True)
                                delcount += 1
                            Catch ex As Exception
                                Select Case mBox.MsgBoxFW("Failed to delete folder {0}: {1}", MsgBoxStyle.Exclamation Or MsgBoxStyle.AbortRetryIgnore, "Delete images", Me, folder, ex.Message)
                                    Case MsgBoxResult.Abort
                                        If delcount > 0 Then mBox.ModalF_PTWBIa("{0} image folders deleted with {1} errors.", "Delete images", Me, mBox.MessageBoxButton.Buttons.OK, mBox.GetIcon(mBox.MessageBoxIcons.Warning), delcount, errcount)
                                        Exit Sub
                                    Case MsgBoxResult.Retry : GoTo DeleteFolder
                                    Case Else : errcount += 1
                                End Select
                            End Try
                        End If
                    End If
                Next
                mBox.ModalF_PTWBIa("{0} image folders deleted with {1} errors.", "Delete images", Me, mBox.MessageBoxButton.Buttons.OK, mBox.GetIcon(If(errcount = 0, mBox.MessageBoxIcons.OK, mBox.MessageBoxIcons.Warning)), delcount, errcount)
            Catch ex As Exception
                mBox.Error_XTW(ex, ex.GetType.Name, Me)
            End Try
        End If
    End Sub

    Private Sub btnDelDb_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelDb.Click
        If mBox.MsgBox("Do you really want to delete all images of Caps from database that are not of given sizes? When size 0 is not about to be deleted biggest of Cap for each images will be always preserved.",
                   MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Delete images", Me) = MsgBoxResult.Yes Then
            'delete images which are not of given sizes (from DB)
            Dim cnt%
            Try
                Using context As New CapsDataContext(Main.EntityConnection)
                    Dim keepSizes As Integer() = icImagesInDb.ItemsSource
                    If keepSizes.Length = 0 Then
                        Dim iToDel = From image In context.StoredImages Where image.ImageID IsNot Nothing
                        cnt = iToDel.Count
                        context.StoredImages.DeleteObjects(iToDel)
                    ElseIf Not keepSizes.Contains(0) Then
                        For Each si In From image In context.StoredImages Where image.ImageID IsNot Nothing
                            If Not keepSizes.Contains(Math.Max(si.Width, si.Height)) Then context.StoredImages.DeleteObject(si) : cnt += 1
                        Next si
                    Else
                        For Each image In From img In context.Images
                            Dim maxSize = (From si In image.StoredImages Select Math.Max(si.Width, si.Height)).Max
                            For Each si In From sii In image.StoredImages Where sii.Width < maxSize AndAlso sii.Height < maxSize
                                If Not keepSizes.Contains(Math.Max(si.Width, si.Height)) Then context.StoredImages.DeleteObject(si) : cnt += 1
                            Next si
                        Next image
                    End If
                    context.SaveChanges()
                End Using
            Catch ex As Exception
                mBox.Error_XTW(ex, ex.GetType.Name, Me)
                Exit Sub
            End Try
            mBox.MsgBoxFW("{0} images deleted successfully", MsgBoxStyle.Information, "Delete images", Me, cnt)
        End If
    End Sub
End Class
