Imports System.Runtime.CompilerServices
Imports Caps.Data, System.Globalization.CultureInfo
Imports System.Drawing, Tools.ExtensionsT
Imports System.Drawing.Imaging
Imports System.Data.Objects.DataClasses
Imports Tools.DrawingT, Tools.LinqT

''' <summary>Contains extension functions for Caps data entities</summary>
Module CapsDataExtensions
#Region "Save image"
#Region "Typed methods"
    ''' <summary>Stores image of <see cref="CapSign"/> in appropriate storage</summary>
    ''' <param name="capSign">A <see cref="CapSign"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="replace">True to replace existing image (or add new when image does not exist), false not to replace existing image (an exception is thrown when image already exists).</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    <Extension()>
    Function SaveImage(ByVal capSign As CapSign, ByVal source As String, ByVal replace As Boolean) As SaveImageUndoOperation
        Return SaveImage(Of CapSign)(capSign, source, replace)
    End Function
    ''' <summary>Stores image of <see cref="CapType"/> in appropriate storage</summary>
    ''' <param name="capType">A <see cref="CapType"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="replace">True to replace existing image (or add new when image does not exist), false not to replace existing image (an exception is thrown when image already exists).</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    <Extension()>
    Function SaveImage(ByVal capType As CapType, ByVal source As String, ByVal replace As Boolean) As SaveImageUndoOperation
        Return SaveImage(Of CapType)(capType, source, replace)
    End Function
    ''' <summary>Stores image of <see cref="MainType"/> in appropriate storage</summary>
    ''' <param name="mainType">A <see cref="MainType"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="replace">True to replace existing image (or add new when image does not exist), false not to replace existing image (an exception is thrown when image already exists).</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    <Extension()>
    Function SaveImage(ByVal mainType As MainType, ByVal source As String, ByVal replace As Boolean) As SaveImageUndoOperation
        Return SaveImage(Of MainType)(mainType, source, replace)
    End Function
    ''' <summary>Stores image of <see cref="Shape"/> in appropriate storage</summary>
    ''' <param name="shape">A <see cref="Shape"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="replace">True to replace existing image (or add new when image does not exist), false not to replace existing image (an exception is thrown when image already exists).</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    <Extension()>
    Function SaveImage(ByVal shape As Shape, ByVal source As String, ByVal replace As Boolean) As SaveImageUndoOperation
        Return SaveImage(Of Shape)(shape, source, replace)
    End Function
    ''' <summary>Stores image of <see cref="storage"/> in appropriate storage</summary>
    ''' <param name="storage">A <see cref="storage"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="replace">True to replace existing image (or add new when image does not exist), false not to replace existing image (an exception is thrown when image already exists).</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    <Extension()>
    Function SaveImage(ByVal storage As Storage, ByVal source As String, ByVal replace As Boolean) As SaveImageUndoOperation
        Return SaveImage(Of Storage)(storage, source, replace)
    End Function
#End Region
    ''' <summary>Stores image of <see cref="Data.Image"/> in appropriate storage</summary>
    ''' <param name="image">A <see cref="Data.Image"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="save">True to save changes to database immediatelly, false to just associate changes with <paramref name="image"/> (affects only database saving). When <paramref name="save"/> is true, <paramref name="image"/>.<see cref="Image.ImageID">ImageID</see> must not be zero.</param>
    ''' <param name="iptc">Optional IPTC data to embded to each image (use only for JPEGs). <note>If it is not possible to embded IPTC data, failure is silent.</note></param>
    ''' <param name="dataContext">Optional data context to provide operations in (required when <paramref name="save"/> is false)</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null -or- <paramref name="save"/> is false and <paramref name="dataContext"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif) -or- <paramref name="image"/>.<see cref="Image.ImageID">ImageID</see> is 0 and <paramref name="save"/> is true.</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    ''' <remarks>When saving to file system this method may change <paramref name="image"/>.<see cref="Image.RelativePath">RelativePath</see>.</remarks>
    <Extension()>
    Public Function SaveImage(ByVal image As Data.Image, ByVal source As String, ByVal save As Boolean, Optional ByVal dataContext As CapsDataContext = Nothing, Optional ByVal iptc As Tools.MetadataT.IptcT.Iptc = Nothing) As SaveImageUndoOperation
        If image Is Nothing Then Throw New ArgumentNullException("capSign")
        If save AndAlso image.ImageID = 0 Then Throw New ArgumentException(My.Resources.err_SaveImageForId0)
        If source Is Nothing Then Throw New ArgumentNullException("source")
        If Not IO.File.Exists(source) Then Throw New IO.FileNotFoundException(My.Resources.err_FileNotFound, source)
        If Not save AndAlso dataContext Is Nothing Then Throw New ArgumentNullException("dataContext")

        Dim storeInFS = Settings.Images.CapsInFileSystem
        Dim storeInDB = Settings.Images.CapsInDatabase
        Dim ret As SaveImageUndoOperation = Nothing
        Dim filename$ = Nothing
        If storeInFS.Length > 0 Then
            'Save in file system
            Using original As New Bitmap(source)
                filename = IO.Path.GetFileName(source)
                Dim i% = 0
                Dim minSize As Integer = Integer.MaxValue
                For Each dimension In storeInFS
                    minSize = Math.Min(minSize, dimension)
                    Dim targetFolder = IO.Path.Combine(My.Settings.ImageRoot, If(dimension = 0,
                                                                                 Data.Image.OriginalSizeImageStorageFolderName,
                                                                                 String.Format(InvariantCulture, "{0}_{0}", dimension)))
                    While IO.File.Exists(IO.Path.Combine(targetFolder, filename))
                        i += 1
                        filename = String.Format(InvariantCulture, "{0}_{1}{2}", IO.Path.GetFileNameWithoutExtension(source), i, IO.Path.GetExtension(source))
                    End While
                Next
                Dim savedImages As New List(Of String)(storeInFS.Length)
                Try
                    For Each dimension In storeInFS
                        Dim targetFolder As IOt.Path = IO.Path.Combine(My.Settings.ImageRoot, If(dimension = 0,
                                                                                                 Data.Image.OriginalSizeImageStorageFolderName,
                                                                                                 String.Format(InvariantCulture, "{0}_{0}", dimension)))
                        Dim targetFile = IO.Path.Combine(targetFolder, filename)
                        If Not targetFolder.IsDirectory Then targetFolder.CreateDirectory()
                        If dimension = 0 Then
                            IO.File.Copy(source, targetFile)
                            Dim newFI As New IO.FileInfo(targetFile)
                            'If file is read-only make it read-write
                            If (newFI.Attributes And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then _
                                newFI.Attributes = newFI.Attributes And Not IO.FileAttributes.ReadOnly
                        ElseIf dimension = minSize OrElse dimension <= original.Width OrElse dimension <= original.Height Then  'Do not enlarge
                            Using thumb = original.GetThumbnail(New Size(dimension, dimension))
                                thumb.Save(targetFile, original.RawFormat)
                            End Using
                        End If
                        If iptc IsNot Nothing Then 'IPTC
                            Try
                                Using JPEG As New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(targetFile, True)
                                    JPEG.IPTCEmbed(iptc.GetBytes)
                                End Using
                            Catch : End Try
                        End If
                    Next
                    image.RelativePath = filename
                Catch When savedImages.Count > 0
                    Using undo As New UndoFileSystemImageSaveOperation(savedImages.ToArray)
                        undo.Undo()
                    End Using
                    Throw
                End Try
                ret = New UndoFileSystemImageSaveOperation(savedImages.ToArray)
            End Using
        End If
        If storeInDB.Length > 0 Then
            'Save in database
            Dim originalUndo = ret
            Try
                Using original As New Bitmap(source),
                      dispContext = If(save OrElse dataContext Is Nothing, New CapsDataContext(Main.EntityConnection), Nothing)
                    Dim context = If(dispContext, dataContext)
                    Dim minSize = Integer.MaxValue
                    For Each dimension In storeInDB
                        minSize = Math.Min(dimension, minSize)
                    Next
                    For Each dimension In storeInDB
                        If dimension > original.Width AndAlso dimension > original.Height AndAlso dimension <> minSize Then Continue For 'Do not enlarge
                        Dim imgToSave = If(dimension = 0, original, original.GetThumbnail(New Size(dimension, dimension)))
                        Try
                            Using saveStream As New IO.MemoryStream
                                imgToSave.Save(saveStream, original.RawFormat)
                                If iptc IsNot Nothing Then 'IPTC
                                    saveStream.Position = 0
                                    Try
                                        Using JPEG As New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(saveStream)
                                            JPEG.IPTCEmbed(iptc.GetBytes)
                                        End Using
                                    Catch : End Try
                                End If
                                'Database stored image
                                Dim si As New StoredImage With {.FileName = IO.Path.GetFileName(source),
                                                                .MIME = GetImageMimeType(IO.Path.GetExtension(source)),
                                                                .Width = imgToSave.Width,
                                                                .Height = imgToSave.Height,
                                                                .Size = saveStream.Length,
                                                                .Data = saveStream.GetBuffer
                                                               }
                                If image.ImageID <> 0 Then
                                    si.ImageID = image.ImageID
                                    context.StoredImages.AddObject(si)
                                Else
                                    image.StoredImages.Add(si)
                                End If
                                Dim undo = New UndoDatabaseImageSaveOperation(si, dataContext, save)
                                If ret Is Nothing Then ret = undo Else ret += undo
                            End Using
                        Finally
                            If imgToSave IsNot original Then imgToSave.Dispose()
                        End Try
                    Next
                    If save Then context.SaveChanges()
                End Using
            Catch When originalUndo IsNot Nothing
                originalUndo.Undo()
                Throw
            End Try
        End If
        If filename IsNot Nothing Then image.RelativePath = filename
        Return ret
    End Function

    ''' <summary>Stores image of <see cref="IObjectWithImage"/> in appropriate storage</summary>
    ''' <param name="obj">A <see cref="IObjectWithImage"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="replace">True to replace existing image (or add new when image does not exist), false not to replace existing image (an exception is thrown when image already exists).</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    Private Function SaveImage(Of T As {IObjectWithImage, EntityObject})(ByVal obj As T, ByVal source As String, ByVal replace As Boolean) As SaveImageUndoOperation
        If obj Is Nothing Then Throw New ArgumentNullException("capSign")
        If obj.ID = 0 Then Throw New ArgumentException(My.Resources.err_SaveImageForId0)
        If source Is Nothing Then Throw New ArgumentNullException("source")
        If Not IO.File.Exists(source) Then Throw New IO.FileNotFoundException(My.Resources.err_FileNotFound, source)

        If Settings.Images.CapSignStorage = ConfigNodeProvider.ImagesProvider.Storage.FileSystem Then
            'Save image in file system
            Dim targetPath = IO.Path.Combine(My.Settings.ImageRoot, obj.ImageStorageFolderName, obj.ID.ToString(InvariantCulture) & ".png")
            Dim targDir As New IOt.Path(IO.Path.GetDirectoryName(targetPath))
            If Not targDir.IsDirectory Then targDir.CreateDirectory()
            If IO.Path.GetExtension(source).ToLowerInvariant = ".png" Then
                IO.File.Copy(source, targetPath, replace)
            Else
                If IO.File.Exists(targetPath) AndAlso Not replace Then
                    Throw New IO.IOException(My.Resources.err_FileExists)
                End If
                Using bmp As New Bitmap(source)
                    bmp.Save(targetPath, ImageFormat.Png)
                End Using
            End If
            Return New UndoFileSystemImageSaveOperation(targetPath)
        Else
            'Save image in database
            Using context As New CapsDataContext(Main.EntityConnection),
                  bmp As New Bitmap(source)
                If replace Then
                    Dim oldImage = obj.GetStoredImages(context).FirstOrDefault
                    If oldImage IsNot Nothing Then context.StoredImages.DeleteObject(oldImage)
                End If
                Dim si As New StoredImage With {.FileName = IO.Path.GetFileName(source),
                                                .MIME = GetImageMimeType(IO.Path.GetExtension(source)),
                                                .Width = bmp.Width,
                                                .Height = bmp.Height,
                                                .Size = My.Computer.FileSystem.GetFileInfo(source).Length,
                                                .Data = My.Computer.FileSystem.ReadAllBytes(source)
                                               }
                obj.AssociateImage(si)
                context.StoredImages.AddObject(si)
                context.SaveChanges()
                Return New UndoDatabaseImageSaveOperation(si)
            End Using
        End If
    End Function

    ''' <summary>Gets MIME type of bitmap image by extension</summary>
    ''' <param name="extension">Extension (with or without leading dot) to get MIME type for</param>
    ''' <returns>MIME type of image type represented by extension</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="extension"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="extension"/> is not of one of types recoginized by the <see cref="Bitmap"/> class (PNG, BMP, JPEG, GIF, Exif)</exception>
    Private Function GetImageMimeType(ByVal extension As String) As String
        If extension Is Nothing Then Throw New ArgumentNullException("extension")
        If extension.StartsWith("."c) Then extension = extension.Substring(1)
        Select Case extension.ToLowerInvariant
            Case "png" : Return "image/png"
            Case "bmp", "dib" : Return "image/x-ms-bmp "
            Case "jpg", "jpeg", "jfif" : Return "image/jpeg"
            Case "tif", "tiff" : Return "image/tiff"
            Case "gif" : Return "image/gif"
            Case "exif" : Return "image/exif"
            Case Else : Throw New ArgumentException(My.Resources.err_UnknownImageExtension.f(extension))
        End Select
    End Function
#End Region

#Region "Get Image"
#Region "Typed functions"
    ''' <summary>Gets images associated with given object</summary>
    ''' <param name="capSign">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> is null</exception>
    <Extension()>
    Public Function GetImages(ByVal capSign As CapSign) As IEnumerable(Of ImageProvider)
        Return GetImages(Of CapSign)(capSign)
    End Function
    ''' <summary>Gets images associated with given object</summary>
    ''' <param name="capType">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capType"/> is null</exception>
    <Extension()>
    Public Function GetImages(ByVal capType As CapType) As IEnumerable(Of ImageProvider)
        Return GetImages(Of CapType)(capType)
    End Function
    ''' <summary>Gets images associated with given object</summary>
    ''' <param name="mainType">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="mainType"/> is null</exception>
    <Extension()>
    Public Function GetImages(ByVal mainType As MainType) As IEnumerable(Of ImageProvider)
        Return GetImages(Of MainType)(mainType)
    End Function
    ''' <summary>Gets images associated with given object</summary>
    ''' <param name="shape">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="shape"/> is null</exception>
    <Extension()>
    Public Function GetImages(ByVal shape As Shape) As IEnumerable(Of ImageProvider)
        Return GetImages(Of Shape)(shape)
    End Function
    ''' <summary>Gets images associated with given object</summary>
    ''' <param name="storage">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="storage"/> is null</exception>
    <Extension()>
    Public Function GetImages(ByVal storage As Storage) As IEnumerable(Of ImageProvider)
        Return GetImages(Of Storage)(storage)
    End Function
#End Region

    ''' <summary>Regular expression matching folder name for storing images</summary>
    Friend ReadOnly imageFolderNameRegExp As New System.Text.RegularExpressions.Regex(
        "^(?<Size>\d+)_\k<Size>$", System.Text.RegularExpressions.RegexOptions.Compiled Or System.Text.RegularExpressions.RegexOptions.CultureInvariant Or System.Text.RegularExpressions.RegexOptions.ExplicitCapture)

    ''' <summary>Gets images associated with given image</summary>
    ''' <param name="image">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="image"/> is null</exception>
    <Extension()>
    Public Function GetImages(ByVal image As Data.Image) As IEnumerable(Of ImageProvider)
        If image Is Nothing Then Throw New ArgumentNullException("obj")
        Dim retFS As IEnumerable(Of ImageProvider) = Nothing
        If My.Settings.ImageRoot <> "" AndAlso IO.Directory.Exists(My.Settings.ImageRoot) Then
            retFS = From dir In IO.Directory.EnumerateDirectories(My.Settings.ImageRoot)
                    Let dirname = IO.Path.GetFileName(dir), match = imageFolderNameRegExp.Match(dirname)
                    Where (dirname.ToLowerInvariant = Data.Image.OriginalSizeImageStorageFolderName OrElse imageFolderNameRegExp.IsMatch(match.Success)) AndAlso
                          IO.File.Exists(IO.Path.Combine(dir, image.RelativePath))
                    Select New FileSystemImageProvider(IO.Path.Combine(dir, image.RelativePath),
                                                    If(dirname.ToLowerInvariant = Data.Image.OriginalSizeImageStorageFolderName, 0, Integer.Parse(match.Groups!size.Value)))
        End If
        Dim retDB = From item In image.StoredImages.AsEnumerable
                    Select New DatabaseImageProvider(item)
        If retFS Is Nothing Then Return retDB
        Return retFS.UnionAll(retDB)
    End Function
    ''' <summary>Gets images associated with given image of given size</summary>
    ''' <param name="image">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="image"/> is null</exception>
    <Extension()>
    Public Function GetImage(ByVal image As Data.Image, ByVal expectedSize As Integer) As ImageProvider
        If image Is Nothing Then Throw New ArgumentNullException("obj")
        Dim retFS As FileSystemImageProvider = Nothing
        If My.Settings.ImageRoot <> "" AndAlso IO.Directory.Exists(My.Settings.ImageRoot) Then
            'Get file system images
            Dim fsImage = (
                From dir In IO.Directory.EnumerateDirectories(My.Settings.ImageRoot)
                Let dirname = IO.Path.GetFileName(dir),
                    match = imageFolderNameRegExp.Match(dirname),
                    file = IO.Path.Combine(dir, image.RelativePath)
                Where (dirname.ToLowerInvariant = Data.Image.OriginalSizeImageStorageFolderName OrElse match.Success) AndAlso
                      IO.File.Exists(IO.Path.Combine(dir, image.RelativePath))
                Let size = If(dirname.ToLowerInvariant = Data.Image.OriginalSizeImageStorageFolderName, 0, Integer.Parse(match.Groups!Size.Value))
                Select size, file
                Order By If(size = expectedSize, 0, 1) Ascending,
                         If(size = 0 OrElse size > expectedSize, 0, 1) Ascending,
                         If(size = 0, Integer.MaxValue, size) Ascending
                          ).FirstOrDefault
            If fsImage IsNot Nothing Then retFS = New FileSystemImageProvider(fsImage.file, fsImage.size)
        End If
        'Get database images
        Dim dbImage = (
            From item In image.StoredImages
            Order By If(item.Width = expectedSize OrElse item.Height = expectedSize, 0, 1) Ascending,
                      If(item.Width > expectedSize OrElse item.Height > expectedSize, 0, 1) Ascending,
                      Math.Max(item.Height, item.Width) Ascending
                    ).FirstOrDefault
        Dim retDB As DatabaseImageProvider = Nothing
        If dbImage IsNot Nothing Then retDB = New DatabaseImageProvider(dbImage)
        If retDB Is Nothing Then Return retFS
        If retFS Is Nothing Then Return retDB
        'Both - DB & FS suitable - get the best one
        Return (
            From item In New ImageProvider() {retFS, retDB}
            Order By If(item.Height = expectedSize OrElse item.Width = expectedSize, 0, 1) Ascending,
                     If(item.Height > expectedSize OrElse item.Width > expectedSize, 0, 1) Ascending,
                     item.Height Ascending,
                     If(TypeOf item Is FileSystemImageProvider, 0, 1) Ascending
               ).First
    End Function

    ''' <summary>Gets images associated with given object</summary>
    ''' <param name="obj">Object to get images associated with</param>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> is null</exception>
    Private Function GetImages(Of T As {IObjectWithImage, EntityObject})(ByVal obj As T) As IEnumerable(Of ImageProvider)
        If obj Is Nothing Then Throw New ArgumentNullException("obj")
        Dim retFS As IEnumerable(Of ImageProvider) = Nothing
        If My.Settings.ImageRoot <> "" Then
            Dim path As String = IO.Path.Combine(My.Settings.ImageRoot, obj.ImageStorageFolderName, obj.FileSystemStorageFileName)
            If IO.File.Exists(path) Then
                retFS = {New FileSystemImageProvider(path, 0)}
            End If
        End If
        Dim retDB = From item In obj.StoredImages Select New DatabaseImageProvider(item)
        If retFS Is Nothing Then Return retDB
        Return retFS.UnionAll(retDB)
    End Function
#End Region
End Module

#Region "Image providers"
''' <summary>Base class for image providers</summary>
Public MustInherit Class ImageProvider
    ''' <summary>CTor - creates a new instance of the <see cref="ImageProvider"/> class</summary>
    ''' <param name="width">Width of image in pixels</param>
    ''' <param name="height">height of image in pixels</param>
    Protected Sub New(ByVal width%, ByVal height%)
        Me._width = width
        Me._height = height
    End Sub
#Region "Image retrieval"
    ''' <summary>When overriden in base class gets stream containing image data</summary>
    ''' <returns>Stream containing image data</returns>
    Public MustOverride Function GetImageStream() As IO.Stream
    ''' <summary>Gets <see cref="System.Drawing.Bitmap"/> displaying this image</summary>
    ''' <returns>Object containing image data</returns>
    Public Overridable Function GetImageBitmap() As System.Drawing.Bitmap
        Using str = GetImageStream()
            Return New System.Drawing.Bitmap(str)
        End Using
    End Function
    ''' <summary>Gets <see cref="Windows.Media.Imaging.BitmapImage"/> displayling this image</summary>
    ''' <returns>Object containing image data</returns>
    Public Overridable Function GetImageSource() As Windows.Media.Imaging.BitmapImage
        Dim ret = New Windows.Media.Imaging.BitmapImage()
        ret.BeginInit()
        ret.CacheOption = Windows.Media.Imaging.BitmapCacheOption.OnLoad
        Using str = GetImageStream()
            ret.StreamSource = str
            ret.EndInit()
        End Using
        Return ret
    End Function
#End Region

#Region "Properties"
    Private ReadOnly _width%
    ''' <summary>Gets actual width of image in pixels</summary>
    Public ReadOnly Property Width As Integer
        Get
            Return _width
        End Get
    End Property
    Private ReadOnly _height%
    ''' <summary>Gets actual height of image in pixels</summary>
    Public ReadOnly Property Height As Integer
        Get
            Return _height
        End Get
    End Property
#End Region
End Class

''' <summary>Provides image stored in database</summary>
Friend NotInheritable Class DatabaseImageProvider
    Inherits ImageProvider
    ''' <summary>CTor - creates a new instance of the <see cref="DatabaseImageProvider"/> class</summary>
    ''' <param name="image">Database-stored image</param>
    ''' <exception cref="ArgumentNullException"><paramref name="image"/> is null</exception>
    Public Sub New(ByVal image As StoredImage)
        MyBase.New(image.ThrowIfNull("image").Width, image.Height)
    End Sub
    ''' <summary>database image</summary>
    Private image As StoredImage
    ''' <summary>Gets stream containing image data</summary>
    ''' <returns>Stream containing image data</returns>
    Public Overrides Function GetImageStream() As System.IO.Stream
        Return image.GetImageStream
    End Function
    ''' <summary>Gets <see cref="System.Drawing.Bitmap"/> displaying this image</summary>
    ''' <returns>Object containing image data</returns>
    Public Overrides Function GetImageBitmap() As System.Drawing.Bitmap
        Return image.GetImageBitmap
    End Function
    ''' <summary>Gets <see cref="Windows.Media.Imaging.BitmapImage"/> displayling this image</summary>
    ''' <returns>Object containing image data</returns>
    Public Overrides Function GetImageSource() As System.Windows.Media.Imaging.BitmapImage
        Return image.GetImageSource
    End Function
End Class

''' <summary>Provides image stored in file system</summary>
Friend NotInheritable Class FileSystemImageProvider
    Inherits ImageProvider
    ''' <summary>Path of file image is stored in</summary>
    Private imagePath As String
    ''' <summary>Helper CTor - calls base class constructor witz <see cref="System.Drawing.Size"/> deimensions</summary>
    ''' <param name="size">Size of current image. {0, 0} means original image (used only whan determination of actual image size failed)</param>
    Private Sub New(ByVal size As System.Drawing.Size)
        MyBase.New(size.width, size.height)
    End Sub
    ''' <summary>CTor - creates a new instance of the <see cref="FileSystemImageProvider"/> class</summary>
    ''' <param name="imagePath">Path to file image is stored in</param>
    ''' <param name="expectedSize">Expected size of image (0 means original size); used when image size determination from file fails</param>
    ''' <exception cref="ArgumentNullException"><paramref name="expectedSize"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="expectedSize"/> is an empty string</exception>
    ''' <exception cref="ArgumentOutOfRangeException"><paramref name="expectedSize"/> is less than zero</exception>
    Public Sub New(ByVal imagePath$, ByVal expectedSize%)
        Me.New(GetImageSize(imagePath, expectedSize))
        Me.imagePath = imagePath
    End Sub

    ''' <summary>When overriden in base class gets stream containing image data</summary>
    ''' <returns>Stream containing image data</returns>
    Public Overrides Function GetImageStream() As System.IO.Stream
        Return IO.File.Open(imagePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
    End Function
    ''' <summary>Gets <see cref="System.Drawing.Bitmap"/> displaying this image</summary>
    ''' <returns>Object containing image data</returns>
    Public Overrides Function GetImageBitmap() As System.Drawing.Bitmap
        Return New Bitmap(imagePath)
    End Function
    ''' <summary>Gets size of image in given file in pixels</summary>
    ''' <param name="imagePath">Path to file to get size of image stored in</param>
    ''' <param name="expectedSize">Expected size of image. Used as image size in case an error occurs while reading image file. 0 means original size of image.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="expectedSize"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="expectedSize"/> is an empty string</exception>
    ''' <exception cref="ArgumentOutOfRangeException"><paramref name="expectedSize"/> is less than zero</exception>
    Private Shared Function GetImageSize(ByVal imagePath As String, ByVal expectedSize%) As System.Drawing.Size
        If imagePath Is Nothing Then Throw New ArgumentNullException("imagePath")
        If imagePath = "" Then Throw New ArgumentException(My.Resources.ex_CannotBeAnEmptyString, "imagePath")
        If expectedSize < 0 Then Throw New ArgumentOutOfRangeException("expectedSize")
        Try
            Using bmp As New Bitmap(imagePath)
                Return bmp.Size
            End Using
        Catch
            Return New System.Drawing.Size(expectedSize, expectedSize)
        End Try
    End Function
End Class
#End Region

#Region "Save image undo classes"
''' <summary>This class can undo operation of image saving</summary>
Public MustInherit Class SaveImageUndoOperation
    Implements IDisposable
    ''' <summary>Reverts the image saval action</summary>
    ''' <param name="throwException">True to throw an excpetion when something bad happends during undo operation</param>
    ''' <exception cref="Exception">An error ocured during undo operation and <paramref name="throwException"/> was true</exception>
    Public MustOverride Sub Undo(Optional ByVal throwException As Boolean = False)

#Region "IDisposable Support"
    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True to dispose managed state</param>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    End Sub

    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    ''' <summary>Combines two <see cref="SaveImageUndoOperation">SaveImageUndoOperations</see></summary>
    ''' <param name="a">A <see cref="SaveImageUndoOperation"/></param>
    ''' <param name="b">A <see cref="SaveImageUndoOperation"/></param>
    ''' <returns>A <see cref="SaveImageUndoOperation"/> which invokes both operations <paramref name="a"/> and <paramref name="b"/>. Null when both - <paramref name="a"/> and <paramref name="b"/> are null.</returns>
    Public Shared Operator +(ByVal a As SaveImageUndoOperation, ByVal b As SaveImageUndoOperation) As SaveImageUndoOperation
        If a Is Nothing AndAlso b Is Nothing Then Return Nothing
        If a Is Nothing Then Return b
        If b Is Nothing Then Return a
        Return New CombinedSaveImageUndoOperation({a, b})
    End Operator
#End Region

End Class

''' <summary>Implements <see cref="CombinedSaveImageUndoOperation"/> which combines more operations to one</summary>
Friend NotInheritable Class CombinedSaveImageUndoOperation
    Inherits SaveImageUndoOperation
    ''' <summary>CTor - creates a new instance of the <see cref="CombinedSaveImageUndoOperation"/> class</summary>
    ''' <param name="operations">Operations to combine</param>
    ''' <exception cref="ArgumentException">An item in <paramref name="operations"/> collection is null</exception>
    ''' <exception cref="ArgumentNullException"><paramref name="operations"/> is null</exception>
    Public Sub New(ByVal operations As IEnumerable(Of SaveImageUndoOperation))
        If operations Is Nothing Then Throw New ArgumentNullException("operations")
        If operations.Count(Function(item) item Is Nothing) > 0 Then Throw New ArgumentException(My.Resources.err_OperationToCombineCannotBeNull)
        Me.operations = New List(Of SaveImageUndoOperation)(operations)
    End Sub
    ''' <summary>Contains operations this operation combines</summary>
    Private operations As List(Of SaveImageUndoOperation)
    ''' <summary>Reverts the image saval action</summary>
    ''' <param name="throwException">True to throw an excpetion when something bad happends during undo operation</param>
    ''' <exception cref="Tools.ComponentModelT.MultipleException">An error ocured during undo operation and <paramref name="throwException"/> was true</exception>
    Public Overrides Sub Undo(Optional ByVal throwException As Boolean = False)
        If disposed Then Throw New ObjectDisposedException(Me.GetType.Name)
        Dim exc As New List(Of Exception)
        For Each item In operations
            Try
                item.Undo(throwException)
            Catch ex As Tools.ComponentModelT.MultipleException When throwException
                exc.AddRange(ex.Exceptions)
            Catch ex As Exception When throwException
                exc.Add(ex)
            End Try
        Next
        If throwException AndAlso exc.Count > 0 Then
            Throw New Tools.ComponentModelT.MultipleException(exc)
        End If
    End Sub
    ''' <summary>Indicate sif this object was already disposed or not</summary>
    Private disposed As Boolean
    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True to dispose managed state</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso Not disposed Then
            disposed = True
            For Each item In operations
                item.Dispose()
            Next
            operations = Nothing
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class

''' <summary>Implements <see cref="SaveImageUndoOperation"/> for images stored in file system</summary>
Friend NotInheritable Class UndoFileSystemImageSaveOperation
    Inherits SaveImageUndoOperation
    ''' <summary>Paths of files to be deleted</summary>
    Private imagePaths$()
    ''' <summary>CTor - creates a new instance of the <see cref="UndoFileSystemImageSaveOperation"/> class</summary>
    ''' <param name="imagePaths">Paths of images to be deleted</param>
    ''' <exception cref="ArgumentNullException"><paramref name="imagePaths"/> or an item in <paramref name="imagePaths"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="imagePaths"/> is an empty array</exception>
    Public Sub New(ByVal ParamArray imagePaths$())
        If imagePaths Is Nothing Then Throw New ArgumentNullException("imagePaths")
        If imagePaths.Length = 0 Then Throw New ArgumentException(String.Format(My.Resources.err_CannotBeEmpty, "imagePaths"), "imagePaths")
        If imagePaths.Count(Function(item) item Is Nothing) > 0 Then Throw New ArgumentNullException("imagePaths")
    End Sub
    ''' <summary>Reverts the image saval action</summary>
    ''' <param name="throwException">True to throw an excpetion when something bad happends during undo operation</param>
    ''' <exception cref="Exception">An error ocured during undo operation and <paramref name="throwException"/> was true</exception>
    Public Overrides Sub Undo(Optional ByVal throwException As Boolean = False)
        If imagePaths Is Nothing Then Throw New ObjectDisposedException(Me.GetType.Name)
        For Each Path In imagePaths
            Try
                IO.File.Delete(Path)
            Catch When Not throwException : End Try
        Next
    End Sub
    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True to dispose managed state</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            imagePaths = Nothing
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class

''' <summary>Implements <see cref="SaveImageUndoOperation"/> for image stored in database</summary>
Friend NotInheritable Class UndoDatabaseImageSaveOperation
    Inherits SaveImageUndoOperation
    ''' <summary>CTor - creates a new instance of the <see cref="UndoDatabaseImageSaveOperation"/> class</summary>
    ''' <param name="image">An image do be deleted when <see cref="Undo"/> is called</param>
    ''' <param name="context">Optionally provides context in which image was created. If not provided, default context is used.</param>
    ''' <param name="save">True to save context after undo</param>
    ''' <exception cref="ArgumentNullException"><paramref name="image"/> is null</exception>
    Public Sub New(ByVal image As StoredImage, Optional ByVal context As CapsDataContext = Nothing, Optional ByVal save As Boolean = True)
        If image Is Nothing Then Throw New ArgumentNullException("image")
        Me.image = image
        Me.context = context
        Me.save = save
    End Sub
    ''' <summary>Indicates if context is saved after undo</summary>
    Private save As Boolean
    ''' <summary>Context to be used to delete the image</summary>
    Private context As CapsDataContext
    ''' <summary>Image to be deleted</summary>
    Private image As StoredImage
    ''' <summary>Reverts the image saval action</summary>
    ''' <param name="throwException">True to throw an excpetion when something bad happends during undo operation</param>
    ''' <exception cref="Exception">An error ocured during undo operation and <paramref name="throwException"/> was true</exception>
    Public Overrides Sub Undo(Optional ByVal throwException As Boolean = False)
        If disposed Then Throw New ObjectDisposedException(Me.GetType.Name)
        If context IsNot Nothing Then
            Try
                context.DeleteObject(image)
                If save Then context.SaveChanges()
            Catch When Not throwException : End Try
        Else
            Using context As New CapsDataContext(Main.EntityConnection)
                Try
                    context.DeleteObject((From item In context.StoredImages Where item.StoredImageID = image.StoredImageID).FirstOrDefault)
                    If save Then context.SaveChanges()
                Catch When Not throwException : End Try
            End Using
        End If
    End Sub
    ''' <summary>Indicates if this object is disposed</summary>
    Private disposed As Boolean = False
    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True to dispose managed state</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        disposed = True
        If disposing AndAlso context IsNot Nothing AndAlso Not context.IsDisposed Then
            context.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
#End Region