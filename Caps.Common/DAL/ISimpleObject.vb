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
        ''' <summary>Gets images stored in database associated with current objects</summary>
        ''' <param name="context">Data context to get images from</param>
        ''' <exception cref="ArgumentNullException"><paramref name="context"/> is null</exception>
        Function GetStoredImages(ByVal context As CapsDataContext) As IEnumerable(Of StoredImage)
        ''' <summary>Associates an image with current object</summary>
        ''' <param name="image">Image to associate with current object</param>
        ''' <remarks>Association is done besd on object ID not by referenceing the object, so it's save do make association across data contexts.</remarks>
        ''' <exception cref="ArgumentNullException"><paramref name="image"/> is null</exception>
        Sub AssociateImage(ByVal image As StoredImage)
    End Interface

    ''' <summary>An item related to <see cref="Cap"/></summary>
    Public Interface IRelatedToCap
        ''' <summary>Gets caps this item is related to</summary>
        ReadOnly Property Caps As IEnumerable(Of Cap)
    End Interface
End Namespace