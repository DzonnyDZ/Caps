Imports Caps.Data, Tools.WindowsT.InteropT.InteropExtensions
Imports Tools.DataT.ObjectsT.EntityFrameworkExtensions
Imports Tools.LinqT, Tools.DataT.ObjectsT, Tools.DrawingT

''' <summary>Window used to migrate images between database and file system</summary>
Public Class winSyncImages

    Private Sub btnChangeImageRoot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnChangeImageRoot.Click
        Dim dlg As New Forms.FolderBrowserDialog
        Try
            dlg.SelectedPath = My.Settings.ImageRoot
        Catch : End Try
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


    Private Sub btnMigrate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnMigrate.Click
        'Validation
        If Not chkCapSigns.IsChecked AndAlso Not chkCapTypes.IsChecked AndAlso Not chkMainTypes.IsChecked AndAlso Not chkShapes.IsChecked AndAlso Not chkStorages.IsChecked AndAlso Not chkCaps.IsChecked Then
            mBox.MsgBox("No image type selected. Select at least one image type, please.", MsgBoxStyle.Information, "Migrate images", Me)
            chkCapSigns.Focus()
            Return
        ElseIf My.Settings.ImageRoot = "" Then
            mBox.MsgBox("Image root directory is not set. Please set it before migration", MsgBoxStyle.Information, "Migrate images", Me)
            btnChangeImageRoot.Focus()
            Return
        ElseIf Not IO.Directory.Exists(My.Settings.ImageRoot) Then
            If mBox.MsgBoxFW("Image root directory {0} does not exists." & " " & If(optMigrateDb2FS.IsChecked, "Create it now?", "Select existing directory to migrate images from, please."),
                             If(optMigrateDb2FS.IsChecked, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, MsgBoxStyle.Information),
                             "Migrate images", Me, My.Settings.ImageRoot) = MsgBoxResult.Ok AndAlso optMigrateDb2FS.IsChecked Then
                Try
                    IO.Directory.CreateDirectory(My.Settings.ImageRoot)
                Catch ex As Exception
                    mBox.Error_XW(ex, Me)
                    Return
                End Try
            Else
                btnChangeImageRoot.Focus()
                Return
            End If
        ElseIf optMigrateFS2Db.IsChecked AndAlso chkCaps.IsChecked AndAlso DirectCast(icImagesInDb.ItemsSource, Integer()).Length = 0 Then
            mBox.MsgBox("No image sizes to be stored in database are selected. Please select some or do not migrate cap images.", MsgBoxStyle.Information, "Migrate images", Me)
            icImagesInDb.Focus()
            Exit Sub
        ElseIf optMigrateFS2Db.IsChecked AndAlso chkCaps.IsChecked AndAlso (From size In DirectCast(icImagesInDb.ItemsSource, Integer()) Group By size Into count = Count() Where count > 1).Exists Then
            mBox.MsgBox("Duplicate size of images is selected for database storage.", MsgBoxStyle.Information, "Migrate images", Me)
            icImagesInDb.Focus()
            Exit Sub
        ElseIf optMigrateDb2FS.IsChecked AndAlso chkCaps.IsChecked AndAlso DirectCast(icImagesInFS.ItemsSource, Integer()).Length = 0 Then
            mBox.MsgBox("No image sizes to be stored in file system are selected. Please select some or do not migrate cap images.", MsgBoxStyle.Information, "Migrate images", Me)
            icImagesInFS.Focus()
            Exit Sub
        ElseIf optMigrateDb2FS.IsChecked AndAlso chkCaps.IsChecked AndAlso (From size In DirectCast(icImagesInFS.ItemsSource, Integer()) Group By size Into count = Count() Where count > 1).Exists Then
            mBox.MsgBox("Duplicate size of images is selected for file system storage.", MsgBoxStyle.Information, "Migrate images", Me)
            icImagesInFS.Focus()
            Exit Sub
        End If

        'Migration
        Using context As New CapsDataContext(Main.EntityConnection)
            If optMigrateFS2Db.IsChecked Then
                MigateFs2Db(context)
            ElseIf optMigrateDb2FS.IsChecked Then
                MigrateDb2Fs(context)
            End If
        End Using
    End Sub
    ''' <summary>Performs migration of images from File System to Database</summary>
    ''' <param name="context">Data context</param>
    ''' <exception cref="ArgumentNullException"><paramref name="context"/> is null</exception>
    Private Sub MigateFs2Db(ByVal context As CapsDataContext)
        If context Is Nothing Then Throw New ArgumentNullException("context")
        If chkCapSigns.IsChecked Then
            'CapSigns
            For Each item In context.CapSigns
                Dim itemImages = item.GetImages(ImageSources.FileSystem)
                If Not itemImages.IsEmpty Then
                    If item.StoredImages.Count > 0 Then
                        If chkReplace.IsChecked Then context.StoredImages.DeleteObjects(item.StoredImages) Else Continue For
                    End If
                    Try
                        StoreImageToDb(item)
                    Catch ex As Exception
                        If mBox.Error_XPTIBWO(ex, "Failed to copy image:", ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                          Exit Sub
                    End Try
                End If
            Next
        End If
        If chkCapTypes.IsChecked Then
            'CapTypes
            For Each item In context.CapTypes
                Dim itemImages = item.GetImages(ImageSources.FileSystem)
                If Not itemImages.IsEmpty Then
                    If item.StoredImages.Count > 0 Then
                        If chkReplace.IsChecked Then context.StoredImages.DeleteObjects(item.StoredImages) Else Continue For
                    End If
                    Try
                        StoreImageToDb(item)
                    Catch ex As Exception
                        If mBox.Error_XPTIBWO(ex, "Failed to copy image:", ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                          Exit Sub
                    End Try
                End If
            Next
        End If
        If chkMainTypes.IsChecked Then
            'MainTypes
            For Each item In context.MainTypes
                Dim itemImages = item.GetImages(ImageSources.FileSystem)
                If Not itemImages.IsEmpty Then
                    If item.StoredImages.Count > 0 Then
                        If chkReplace.IsChecked Then context.StoredImages.DeleteObjects(item.StoredImages) Else Continue For
                    End If
                    Try
                        StoreImageToDb(item)
                    Catch ex As Exception
                        If mBox.Error_XPTIBWO(ex, "Failed to copy image:", ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                          Exit Sub
                    End Try
                End If
            Next
        End If
        If chkShapes.IsChecked Then
            'Shapes
            For Each item In context.Shapes
                Dim itemImages = item.GetImages(ImageSources.FileSystem)
                If Not itemImages.IsEmpty Then
                    If item.StoredImages.Count > 0 Then
                        If chkReplace.IsChecked Then context.StoredImages.DeleteObjects(item.StoredImages) Else Continue For
                    End If
                    Try
                        StoreImageToDb(item)
                    Catch ex As Exception
                        If mBox.Error_XPTIBWO(ex, "Failed to copy image:", ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                          Exit Sub
                    End Try
                End If
            Next
            If chkStorages.IsChecked Then
                'Storages
                For Each item In context.Storages
                    Dim itemImages = item.GetImages(ImageSources.FileSystem)
                    If Not itemImages.IsEmpty Then
                        If item.StoredImages.Count > 0 Then
                            If chkReplace.IsChecked Then context.StoredImages.DeleteObjects(item.StoredImages) Else Continue For
                        End If
                        Try
                            StoreImageToDb(item)
                        Catch ex As Exception
                            If mBox.Error_XPTIBWO(ex, "Failed to copy image:", ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                              Exit Sub
                        End Try
                    End If
                Next
            End If
        End If
        If chkCaps.IsChecked Then
            'Caps
            Dim sizeFolders = From folder In IO.Directory.EnumerateDirectories(My.Settings.ImageRoot)
                              Let match = CapsDataExtensions.imageFolderNameRegExp.Match(IO.Path.GetFileName(folder))
                              Where match.Success
                              Select Path = folder, Size = Integer.Parse(match.Groups!Size.Value, System.Globalization.CultureInfo.InvariantCulture)
            For Each Image In context.Images
                Try
                    Dim img = Image
                    Dim biggestImage = New With {Key .Path = "", Key .Size = 0I} : biggestImage = Nothing
                    If IO.File.Exists(IO.Path.Combine(My.Settings.ImageRoot, Image.OriginalSizeImageStorageFolderName, img.RelativePath)) Then
                        biggestImage = New With {
                            Key .Path = IO.Path.Combine(My.Settings.ImageRoot, Image.OriginalSizeImageStorageFolderName, img.RelativePath),
                            Key .Size = 0
                        }
                    Else
                        biggestImage = (From sizeFolder In sizeFolders
                                        Where IO.File.Exists(IO.Path.Combine(sizeFolder.Path, img.RelativePath))
                                        Order By sizeFolder.Size Descending
                                        Select Path = IO.Path.Combine(sizeFolder.Path, img.RelativePath), Size = sizeFolder.Size
                                        ).FirstOrDefault
                    End If
                    If biggestImage IsNot Nothing Then
                        Dim maxDbSize As Integer = (From size In DirectCast(icImagesInDb.ItemsSource, Integer())).Max
                        Using fsImage As New System.Drawing.Bitmap(biggestImage.Path)
                            For Each dbSize In DirectCast(icImagesInDb.ItemsSource, Integer())
                                Dim Data As New IO.MemoryStream
                                Dim saveWidth%, saveHeight%
                                If biggestImage.Size = 0 AndAlso dbSize = 0 OrElse
                                    dbSize = 0 AndAlso Math.Max(fsImage.Width, fsImage.Height) > maxDbSize Then
                                    'Original -> Original or Biggest -> Pseudooriginal
                                    fsImage.Save(Data, fsImage.RawFormat)
                                    saveWidth = fsImage.Width : saveHeight = fsImage.Height
                                ElseIf (biggestImage.Size <> 0 AndAlso dbSize <> 0 AndAlso biggestImage.Size < dbSize) OrElse
                                    biggestImage.Size = 0 AndAlso dbSize <> 0 AndAlso (fsImage.Width >= dbSize OrElse fsImage.Height >= dbSize) Then
                                    'Bigger to smaller
                                    Using smallImage = fsImage.GetThumbnail(New System.Drawing.Size(dbSize, dbSize))
                                        smallImage.Save(Data, fsImage.RawFormat)
                                        saveWidth = smallImage.Width : saveHeight = smallImage.Height
                                    End Using
                                Else 'Smaller to bigger
                                    Continue For
                                End If
                                Dim exisiting = (From isi In Image.StoredImages Where Width = saveWidth AndAlso Height = saveHeight).FirstOrDefault
                                If exisiting IsNot Nothing AndAlso chkReplace.IsChecked Then
                                    context.DeleteObject(exisiting)
                                ElseIf exisiting IsNot Nothing Then
                                    Continue For
                                End If
                                Dim si As New StoredImage With {.Data = Data.GetBuffer,
                                                                .FileName = IO.Path.GetFileName(biggestImage.Path),
                                                                .Height = saveHeight,
                                                                .Width = saveWidth,
                                                                .MIME = CapsDataExtensions.GetImageMimeType(IO.Path.GetExtension(biggestImage.Path))
                                                               }
                                Image.StoredImages.Add(si)
                            Next
                        End Using
                    End If
                Catch ex As Exception
                    If mBox.Error_XPTIBWO(ex, "Failed to copy image:", ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                            Exit Sub
                End Try
            Next
        End If

        Try
            context.SaveChanges()
        Catch ex As Exception
            mBox.Error_XPTIBWO(ex, "Error while saving changes to database. No images were migrated.", "Migrate images", , , Me)
        End Try
    End Sub
    ''' <summary>Stores image of given item to database</summary>
    ''' <param name="item">Item to store image of</param>
    ''' <exception cref="ArgumentNullException"><paramref name="item"/> is null</exception>
    Private Sub StoreImageToDb(ByVal item As IObjectWithImage)
        If item Is Nothing Then Throw New ArgumentNullException("item")
        Using bitmap As New System.Drawing.Bitmap(IO.Path.Combine(My.Settings.ImageRoot, item.ImageStorageFolderName, item.FileSystemStorageFileName)),
                                              ms As New IO.MemoryStream
            bitmap.Save(ms, bitmap.RawFormat)
            ms.Flush()
            Dim si As New StoredImage With {.FileName = item.FileSystemStorageFileName,
                          .MIME = CapsDataExtensions.GetImageMimeType(IO.Path.GetExtension(item.FileSystemStorageFileName)),
                          .Width = bitmap.Width,
                          .Height = bitmap.Height,
                          .Size = ms.Length,
                          .Data = ms.GetBuffer
                         }
            item.AssociateImage(si)
        End Using
    End Sub
    ''' <summary>Performs migration of images from Database to File System</summary>
    ''' <param name="context">Data context</param>
    ''' <exception cref="ArgumentNullException"><paramref name="context"/> is null</exception>
    Private Sub MigrateDb2Fs(ByVal context As CapsDataContext)
        If context Is Nothing Then Throw New ArgumentNullException("context")
        If chkCapSigns.IsChecked Then
            'CapSigns
            For Each storedImage In From si In context.StoredImages Where si.CapSignID IsNot Nothing
                If Not StoreImageToFS(storedImage, CapSign.ImageStorageFolderName, storedImage.CapSignID) Then Exit Sub
            Next
        End If
        If chkCapTypes.IsChecked Then
            'CapTypes
            For Each storedImage In From si In context.StoredImages Where si.CapTypeID IsNot Nothing
                If Not StoreImageToFS(storedImage, CapType.ImageStorageFolderName, storedImage.CapTypeID) Then Exit Sub
            Next
        End If
        If chkMainTypes.IsChecked Then
            'MainTypes
            For Each storedImage In From si In context.StoredImages Where si.MainType IsNot Nothing
                If Not StoreImageToFS(storedImage, MainType.ImageStorageFolderName, storedImage.MainTypeID) Then Exit Sub
            Next
        End If
        If chkShapes.IsChecked Then
            'Shapes
            For Each storedImage In From si In context.StoredImages Where si.ShapeID IsNot Nothing
                If Not StoreImageToFS(storedImage, Shape.ImageStorageFolderName, storedImage.ShapeID) Then Exit Sub
            Next
        End If
        If chkStorages.IsChecked Then
            'Storages
            For Each storedImage In From si In context.StoredImages Where si.StorageID IsNot Nothing
                If Not StoreImageToFS(storedImage, Storage.ImageStorageFolderName, storedImage.StorageID) Then Exit Sub
            Next
        End If
        If chkCaps.IsChecked Then
            'Caps
            For Each image In context.Images
                Dim biggestImage = (From si In image.StoredImages Order By Math.Max(si.Width, si.Height) Descending).FirstOrDefault
                If biggestImage Is Nothing Then Continue For
                For Each fsSize In DirectCast(icImagesInFS.ItemsSource, Integer())
                    If fsSize <> 0 AndAlso fsSize > Math.Max(biggestImage.Width, biggestImage.Height) Then Continue For
                    Dim folder = IO.Path.Combine(My.Settings.ImageRoot, If(fsSize = 0, Caps.Data.Image.OriginalSizeImageStorageFolderName, String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}_{0}", fsSize)))
                    Dim deleted = False
                    Try
                        If Not IO.Directory.Exists(folder) Then IO.Directory.CreateDirectory(folder)
                        Dim file = IO.Path.Combine(folder, image.RelativePath)
                        If IO.File.Exists(file) Then
                            If Not chkReplace.IsChecked Then Continue For
                            IO.File.Delete(file)
                            deleted = True
                        End If
                        If fsSize = 0 Then
                            My.Computer.FileSystem.WriteAllBytes(file, biggestImage.Data, False)
                        Else
                            Using originalMS As New IO.MemoryStream(biggestImage.Data),
                                  bigImage As New System.Drawing.Bitmap(originalMS),
                                  smallIamge = bigImage.GetThumbnail(New System.Drawing.Size(fsSize, fsSize))
                                smallIamge.Save(file, bigImage.RawFormat)
                            End Using
                        End If
                    Catch ex As Exception
                        If mBox.Error_XPTIBWO(ex, "Failed to save image." + If(deleted, vbCrLf & "Warning: Original image was already deleted!", ""), ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                            Return
                    End Try
                Next
            Next
        End If
    End Sub
    ''' <summary>Stores database image of simple object to file system</summary>
    ''' <param name="imageToBeStored">A <see cref="StoredImage"/> to be stored in file system</param>
    ''' <param name="imageFolderName">Folder to store image in</param>
    ''' <param name="objectID">ID of object image is related to</param>
    ''' <returns>True if process should continue with next image, false if it should be interrupted</returns>
    Private Function StoreImageToFS(ByVal imageToBeStored As StoredImage, ByVal imageFolderName As String, ByVal objectID%) As Boolean
        Dim imagePath = IO.Path.Combine(My.Settings.ImageRoot, imageFolderName, String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}.png", objectID))
        Dim deleted = False
        Try
            If IO.File.Exists(imagePath) Then
                If Not chkReplace.IsChecked Then Return True
                IO.File.Delete(imagePath)
                deleted = True
            End If
            Using ms As New IO.MemoryStream(imageToBeStored.Data),
                  bmp As New System.Drawing.Bitmap(ms)
                bmp.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png)
            End Using
        Catch ex As Exception
            If mBox.Error_XPTIBWO(ex, "Failed to save image." + If(deleted, vbCrLf & "Warning: Original image was already deleted!", ""), ex.GetType.Name, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then _
                     Return False
        End Try
        Return True
    End Function
End Class
