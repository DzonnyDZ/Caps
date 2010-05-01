Namespace Data
    ''' <summary>Interface of simple object that supports simple translation</summary>
    ''' <seealso cref="SimpleTranslation"/>
    ''' <seealso cref="SimpleFullTranslation"/>
    Public Interface ISimpleObject
        ''' <summary>Gets type name of the object</summary>
        ''' <returns>Name of table in database the object is stored in</returns>
        ReadOnly Property ObjectName$
        ''' <summary>Gets ID of the object</summary>
        ''' <returns>Database ID of currrent <see cref="ISimpleObject"/></returns>
        ReadOnly Property ID%
        ''' <summary>Gets or sets name of the object</summary>
        ''' <value>New name of the object</value>
        ''' <returns>Name of the object</returns>
        ''' <exception cref="NotSupportedException">Current implementation of <see cref="ISimpleObject"/> does not support Name</exception>
        Property Name$
        ''' <summary>Gets or sets description of the object</summary>
        ''' <value>New description of the object</value>
        ''' <returns>Description of the object</returns>
        ''' <exception cref="NotSupportedException">Current implementation of <see cref="ISimpleObject"/> does not support Description</exception>
        Property Description$
    End Interface

    ''' <summary>Represents object which has relation to image stored in database</summary>
    Public Interface IObjectWithImage
        ''' <summary>Gets ID of current object</summary>
        ReadOnly Property ID%
        ''' <summary>Gets images stored in database associated with current objects in given data context</summary>
        ''' <param name="context">Data context to get images from</param>
        ''' <exception cref="ArgumentNullException"><paramref name="context"/> is null</exception>
        Function GetStoredImages(ByVal context As CapsDataContext) As IEnumerable(Of StoredImage)
        ''' <summary>Gets images associated with current object in object's data context</summary>
        ReadOnly Property StoredImages As IEnumerable(Of StoredImage)
        ''' <summary>Associates an image with current object</summary>
        ''' <param name="image">Image to associate with current object</param>
        ''' <remarks>Association is done besd on object ID not by referenceing the object, so it's save do make association across data contexts.</remarks>
        ''' <exception cref="ArgumentNullException"><paramref name="image"/> is null</exception>
        Sub AssociateImage(ByVal image As StoredImage)
        ''' <summary>Gets name of folder images for current class are stored in</summary>
        ''' <returns>Implementing class can return null from this property whan there is no common storage folder for all images of this class</returns>
        ''' <remarks>Single implementing class should return same value for across all instances (value of this property should be static-like)</remarks>
        ReadOnly Property ImageStorageFolderName$
        ''' <summary>Gets name of file image of this object is stored in when images are stored in file system</summary>
        ReadOnly Property FileSystemStorageFileName$
    End Interface

    ''' <summary>An item related to <see cref="Cap"/></summary>
    Public Interface IRelatedToCap
        ''' <summary>Gets caps this item is related to</summary>
        ReadOnly Property Caps As IEnumerable(Of Cap)
    End Interface
End Namespace