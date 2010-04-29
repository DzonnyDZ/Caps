Imports System.Runtime.CompilerServices
Imports Caps.Data, System.Globalization.CultureInfo
Imports System.Drawing, Tools.ExtensionsT
Imports System.Drawing.Imaging
Imports System.Data.Objects.DataClasses
Imports Tools.DrawingT

''' <summary>Contains extension foctions for Caps data entities</summary>
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
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>
    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists. -or- An I/O error occured while copying file to destionation directory.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    <Extension()>
    Function SaveImage(ByVal image As Data.Image, ByVal source As String) As SaveImageUndoOperation
        If image Is Nothing Then Throw New ArgumentNullException("capSign")
        If image.ImageID = 0 Then Throw New ArgumentException(My.Resources.err_SaveImageForId0)
        If source Is Nothing Then Throw New ArgumentNullException("source")
        If Not IO.File.Exists(source) Then Throw New IO.FileNotFoundException(My.Resources.err_FileNotFound, source)

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
                    Dim targetFolder = IO.Path.Combine(My.Settings.ImageRoot, If(dimension = 0, "original", String.Format(InvariantCulture, "{0}_{0}", dimension)))
                    While IO.File.Exists(IO.Path.Combine(targetFolder, filename))
                        i += 1
                        filename = String.Format(InvariantCulture, "{0}_{1}{2}", IO.Path.GetFileNameWithoutExtension(source), i, IO.Path.GetExtension(source))
                    End While
                Next
                Dim savedImages As New List(Of String)(storeInFS.Length)
                Try
                    For Each dimension In storeInFS
                        Dim targetFolder As IOt.Path = IO.Path.Combine(My.Settings.ImageRoot, If(dimension = 0, "original", String.Format(InvariantCulture, "{0}_{0}", dimension)))
                        Dim targetFile = IO.Path.Combine(targetFolder, filename)
                        If Not targetFolder.IsDirectory Then targetFolder.CreateDirectory()
                        If dimension = 0 Then
                            IO.File.Copy(source, targetFile)
                        ElseIf dimension = minSize OrElse dimension <= original.Width OrElse dimension <= original.Height Then  'Do not enlarge
                            Using thumb = original.GetThumbnail(New Size(dimension, dimension))
                                thumb.Save(targetFile, original.RawFormat)
                            End Using
                        End If
                    Next
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
                      context As New CapsDataContext(Main.EntityConnection)
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
                                Dim si As New StoredImage With {.FileName = IO.Path.GetFileName(source),
                                                                .MIME = GetImageMimeType(IO.Path.GetExtension(source)),
                                                                .Width = imgToSave.Width,
                                                                .Height = imgToSave.Height,
                                                                .Size = saveStream.Length,
                                                                .Data = saveStream.GetBuffer,
                                                                .ImageID = image.ImageID
                                                               }
                                context.StoredImages.AddObject(si)
                                Dim undo = New UndoDatabaseImageSaveOperation(si)
                                If ret Is Nothing Then ret = undo Else ret += undo
                            End Using
                        Finally
                            If imgToSave IsNot original Then imgToSave.Dispose()
                        End Try
                    Next
                    context.SaveChanges()
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
            Dim targetPath = IO.Path.Combine(My.Settings.ImageRoot, "CapSign", obj.ID.ToString(InvariantCulture) & ".png")
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
End Module


#Region "Save image undo classes"
''' <summary>This class can undo operation of image saving</summary>
Public MustInherit Class SaveImageUndoOperation
    Implements IDisposable
    ''' <summary>Reverts the image saval action</summary>
    Public MustOverride Sub Undo()

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
        If operations.Count(Function(item) item Is Nothing) > 0 Then Throw New ArgumentException("An operation to combine cannot be null")
        Me.operations = New List(Of SaveImageUndoOperation)(operations)
    End Sub
    ''' <summary>Contains operations this operation combines</summary>
    Private operations As List(Of SaveImageUndoOperation)
    ''' <summary>Reverts the image saval action</summary>
    Public Overrides Sub Undo()
        If disposed Then Throw New ObjectDisposedException(Me.GetType.Name)
        For Each item In operations
            item.Undo()
        Next
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
        If imagePaths.Length = 0 Then Throw New ArgumentException(String.Format("{0} cannot be empty", "imagePaths"), "imagePaths")
        If imagePaths.Count(Function(item) item Is Nothing) > 0 Then Throw New ArgumentNullException("imagePaths")
    End Sub
    ''' <summary>Deletes the files created when image was saved</summary>
    Public Overrides Sub Undo()
        If imagePaths Is Nothing Then Throw New ObjectDisposedException(Me.GetType.Name)
        For Each Path In imagePaths
            Try
                IO.File.Delete(Path)
            Catch : End Try
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
    ''' <exception cref="ArgumentNullException"><paramref name="image"/> is null</exception>
    Public Sub New(ByVal image As StoredImage, Optional ByVal context As CapsDataContext = Nothing)
        If image Is Nothing Then Throw New ArgumentNullException("image")
        Me.image = image
        Me.context = context
    End Sub
    ''' <summary>Context to be used to delete the image</summary>
    Private context As CapsDataContext
    ''' <summary>Image to be deleted</summary>
    Private image As StoredImage
    ''' <summary>Deletes a stored image form database</summary>
    Public Overrides Sub Undo()
        If context IsNot Nothing Then
            If context.IsDisposed Then Throw New ObjectDisposedException(Me.GetType.Name)
            Try
                context.DeleteObject(image)
                context.SaveChanges()
            Catch : End Try
        Else
            Using context As New CapsDataContext(Main.EntityConnection)
                Try
                    context.DeleteObject((From item In context.StoredImages Where item.StoredImageID = image.StoredImageID).FirstOrDefault)
                    context.SaveChanges()
                Catch : End Try
            End Using
        End If
    End Sub
    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True to dispose managed state</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso context IsNot Nothing AndAlso Not context.IsDisposed Then
            context.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
#End Region