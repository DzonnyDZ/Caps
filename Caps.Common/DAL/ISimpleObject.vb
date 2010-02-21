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

    ''' <summary>An item related to <see cref="Cap"/></summary>
    Public Interface IRelatedToCap
        ''' <summary>Gets caps this item is related to</summary>
        ReadOnly Property Caps As IEnumerable(Of Cap)
    End Interface
End Namespace