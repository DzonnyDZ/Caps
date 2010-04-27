Imports System.Runtime.CompilerServices
Imports Caps.Data, System.Globalization.CultureInfo
Imports System.Drawing, Tools.ExtensionsT
Imports System.Drawing.Imaging
Imports System.Data.Objects.DataClasses

Module CapsDataExtensions
#Region "Save image"
    <Extension()>
    Sub SaveImage(ByVal capSign As CapSign, ByVal source As String, ByVal replace As Boolean)
        SaveImage(Of CapSign)(capSign, source, replace)
    End Sub
    <Extension()>
    Sub SaveImage(ByVal capType As CapType, ByVal source As String, ByVal replace As Boolean)
        SaveImage(Of CapType)(capType, source, replace)
    End Sub
    <Extension()>
    Sub SaveImage(ByVal mainType As MainType, ByVal source As String, ByVal replace As Boolean)
        SaveImage(Of MainType)(mainType, source, replace)
    End Sub
    <Extension()>
    Sub SaveImage(ByVal shape As Shape, ByVal source As String, ByVal replace As Boolean)
        SaveImage(Of Shape)(shape, source, replace)
    End Sub
    <Extension()>
    Sub SaveImage(ByVal storage As Storage, ByVal source As String, ByVal replace As Boolean)
        SaveImage(Of Storage)(storage, source, replace)
    End Sub
    <Extension()>
    Sub SaveImage(ByVal image As Image, ByVal source As String, ByVal replace As Boolean)
        'TODO: This must be done different way
    End Sub

    ''' <summary>Stores image of <see cref="IObjectWithImage"/> in appropriate storage</summary>
    ''' <param name="obj">A <see cref="IObjectWithImage"/> to save image of</param>
    ''' <param name="source">Path of image file to read image from</param>
    ''' <param name="replace">True to replace existing image (or add new when image does not exist), false not to replace existing image (an exception is thrown when image already exists).</param>
    ''' <exception cref="ArgumentNullException"><paramref name="capSign"/> or <paramref name="source"/> is null</exception>
    ''' <exception cref="IO.FileNotFoundException">File <paramref name="source"/> does not exist</exception>
    ''' <exception cref="ArgumentException">Images are saved in database and file extension of <paramref name="source"/> is none of recognized extensions (png, bmp, dib, jpg, jpeg, jfif, tif, tiff, gif, exif)</exception>

    ''' <exception cref="UnauthorizedAccessException">Images are stored in file system and the caller does not have the required permission.</exception>
    ''' <exception cref="IO.IOException">Images are saved in file system and: The directory for storing images of particular type does not exists and parent directory is read-only or is not empty or a file with the same name and location exists.</exception>
    ''' <exception cref="IO.DirectoryNotFoundException">Images are stored in file system and: The directory for storing images does not exist and it's path is invalid (for example, it is on an unmapped drive).</exception>
    Private Sub SaveImage(Of T As {IObjectWithImage, EntityObject})(ByVal obj As T, ByVal source As String, ByVal replace As Boolean)
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
        Else
            'Save image in database
            Using context As New CapsDataContext(Main.EntityConnection),
                  bmp As New Bitmap(source)
                If replace Then
                    Dim oldImage = obj.GetStoredImages.FirstOrDefault
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
            End Using
        End If
    End Sub


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