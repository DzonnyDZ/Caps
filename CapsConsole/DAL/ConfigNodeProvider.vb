Imports Caps.Data
Imports System.ComponentModel
Imports Tools, Tools.ExtensionsT

''' <summary>Provides access to database-stored configuration nodes</summary>
Public Class ConfigNodeProvider
    Implements IDisposable
#Region "Common"
    Private Shared _Default As ConfigNodeProvider
    ''' <summary>Gets default instance of <see cref="ConfigNodeProvider"/></summary>
    Public Shared ReadOnly Property [Default] As ConfigNodeProvider
        Get
            If _Default Is Nothing Then
                _Default = New ConfigNodeProvider(New CapsDataContext(Main.EntityConnection))
                _Default.privateContext = True
            End If
            Return _Default
        End Get
    End Property

    Private ReadOnly _Context As CapsDataContext
    ''' <summary>Indicates if context was created by this instance or passed from outside.</summary>
    ''' <remarks>When context was created by this instance it'll be disposed on this instance disposal.</remarks>
    Private privateContext As Boolean
    ''' <summary>Gets data context used by this instance</summary>
    Public Overridable ReadOnly Property Context As CapsDataContext
        Get
            Return _Context
        End Get
    End Property

#Region "CTors"
    ''' <summary>CTor - creates a new instance of the <see cref="ConfigNodeProvider"/> class with specified data context</summary>
    ''' <param name="context">Data context to read values from</param>
    ''' <exception cref="ArgumentNullException"><paramref name="context"/> is null</exception>
    Public Sub New(ByVal context As CapsDataContext)
        If context Is Nothing Then Throw New ArgumentNullException("context")
        _Context = context
        privateContext = False
    End Sub
    ''' <summary>CTor to be used from derived class</summary>
    ''' <remarks><note type="inheritinfo">If derived class calls this CTor it must override the <see cref="Context"/> property</note></remarks>
    Protected Sub New()
        privateContext = False
    End Sub
#End Region

#Region "Get/Set"
    ''' <summary>Gets typed value of configuration node</summary>
    ''' <param name="key">Key of node to get vallue of</param>
    ''' <param name="default">Default value to return when configuration node with given <paramref name="key"/> does not exist</param>
    ''' <returns>Value of configuration node converted to type <typeparamref name="T"/>; <paramref name="default"/> when node with given <paramref name="key"/> does not exist.</returns>
    ''' <typeparam name="T">Type of value to convert node value to. <see cref="String"/> must be convertible to this type.</typeparam>
    ''' <exception cref="InvalidCastException">Value of configuration node cannot be converted to type <typeparamref name="T"/></exception>
    Public Overridable Function GetConfigNodeValue(Of T)(ByVal key As String, Optional ByVal [default] As T = Nothing) As T
        Dim setting = (From item In Context.Settings Where item.Key = key).FirstOrDefault
        If setting Is Nothing Then Return [default]
        Return CTypeDynamic(Of T)(setting.Value)
    End Function

    ''' <summary>Gets value of configuration node</summary>
    ''' <param name="key">Key of node to get value of</param>
    ''' <returns>Value of configuration node with given <paramref name="key"/>; null where there is no such node (or node value is null)</returns>
    Public Overridable Function GetConfigNodeValue(ByVal key As String) As String
        Dim setting = (From item In Context.Settings Where item.Key = key).FirstOrDefault
        If setting Is Nothing Then Return Nothing
        Return setting.Value
    End Function

    ''' <summary>Sets value of configuration node from object</summary>
    ''' <param name="key">Key of node to set value of</param>
    ''' <param name="value">Value to be set</param>
    ''' <exception cref="InvalidCastException"><paramref name="value"/> cannot be <see cref="CTypeDynamic">dynamicly casted</see> to <see cref="String"/></exception>
    Public Sub SetConfigNodeValue(ByVal key As String, ByVal value As Object)
        SetConfigNodeValue(key, CTypeDynamic(Of String)(value))
    End Sub
    ''' <summary>Sets value of configuration node from string</summary>
    ''' <param name="key">Key of node to set value of</param>
    ''' <param name="value">Value to be set</param>
    Public Overridable Sub SetConfigNodeValue(ByVal key As String, ByVal value As String)
        Dim setting = (From item In Context.Settings Where item.Key = key).FirstOrDefault
        If setting Is Nothing Then
            setting = New Setting With {.Key = key}
            Context.Settings.AddObject(setting)
        End If
        setting.Value = value
        Context.SaveChanges()
    End Sub

    ''' <summary>Gets or sets value of cinfiguration node as string</summary>
    ''' <param name="key">Key of node to get or set value of</param>
    Default Public Property Value(ByVal key As String) As String
        Get
            Return GetConfigNodeValue(key)
        End Get
        Set(ByVal value As String)
            SetConfigNodeValue(key, value)
        End Set
    End Property
#End Region

    ''' <summary>Gets all existing configuration node keys</summary>
    Public Overridable ReadOnly Property Keys As IEnumerable(Of String)
        Get
            Return From stg In Context.Settings Select stg.Key
        End Get
    End Property
#End Region

#Region "IDisposable Support"
    ''' <summary>To detect redundant calls</summary>
    Private disposedValue As Boolean

    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' dispose managed state (managed objects).
                If privateContext Then Context.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub
    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    ''' <filterpriority>2</filterpriority>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "Specialized"
#Region "Images"
    ''' <summary>Gets provider to access images-related configuration nodes</summary>
    Public ReadOnly Property Images As ImagesProvider
        Get
            Static _images As New ImagesProvider(Me)
            Return _images
        End Get
    End Property
    ''' <summary>Specialized provider for accessing images-related nodes (nodes with <c>Images.</c> prefix)</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Class ImagesProvider
        Inherits ConfigNodeSubProvider
        ''' <summary>CTor - creates a new instance of the <see cref="ImagesProvider"/> class</summary>
        ''' <param name="parent">Parent provider</param>
        ''' <exception cref="ArgumentNullException"><paramref name="parent"/> is null</exception>
        Public Sub New(ByVal parent As ConfigNodeProvider)
            MyBase.New("Images", parent)
        End Sub
        ''' <summary>Gets or sets sizes of cap images to be stored in database</summary>
        ''' <remarks>An array of integer values indicating longer-side sizes of cap images (in pixels) stored in database. An empty array if no images are stored in database.</remarks>
        ''' <value>An array of integer values indicating longer-side sizes of cap images (in pixels) to be stored in database. An empty array or null not to store any cap images in database.</value>
        Public Property CapsInDatabase As Integer()
            Get
                Dim val = Me!CapsInDatabase
                If val = "" Then Return New Integer() {}
                Return (From item In val.Split(",") Select Integer.Parse(val.Trim, System.Globalization.CultureInfo.InvariantCulture)).Distinct.ToArray
            End Get
            Set(ByVal values As Integer())
                Dim val$
                If values Is Nothing OrElse values.Length = 0 Then val = Nothing _
                Else val = (From int In values.Distinct Select int.ToString(System.Globalization.CultureInfo.InvariantCulture)).Join(",")
                Me!CapsInDatabase = val
            End Set
        End Property
        ''' <summary>Gets or sets sizes of cap images to be stored in file system</summary>
        ''' <remarks>An array of integer values indicating longer-side sizes of cap images (in pixels) stored in file system. An empty array if no images are stored in file system.</remarks>
        ''' <value>An array of integer values indicating longer-side sizes of cap images (in pixels) to be stored in file system. An empty array or null not to store any cap images in file system.</value>
        Public Property CapsInFileSystem As Integer()
            Get
                Dim val = Me!CapsInFileSystem
                If val = "" Then Return New Integer() {}
                Return (From item In val.Split(",") Select Integer.Parse(val.Trim, System.Globalization.CultureInfo.InvariantCulture)).Distinct.ToArray
            End Get
            Set(ByVal values As Integer())
                Dim val$
                If values Is Nothing Then val = Nothing Else val = (From int In values.Distinct Select int.ToString(System.Globalization.CultureInfo.InvariantCulture)).Join(",")
                Me!CapsInFileSystem = val
            End Set
        End Property
        ''' <summary>Possible storages for images</summary>
        Public Enum Storage
            ''' <summary>Images are stored in file system</summary>
            FileSystem
            ''' <summary>Images are stored in database</summary>
            Database
        End Enum
        ''' <summary>Gets or sets storage where cap sign images are stored</summary>
        <DefaultValue(Storage.FileSystem)> _
        Public Property CapSignStorage As Storage
            Get
                Dim val As String = Me!CapSignStorage
                If val = "" Then Return Storage.FileSystem
                Return [Enum].Parse(GetType(Storage), val, True)
            End Get
            Set(ByVal value As Storage)
                Me!CapSignStorage = value.ToString.ToLower
            End Set
        End Property
        ''' <summary>Gets or sets storage where cap sign images are stored</summary>
        <DefaultValue(Storage.FileSystem)> _
        Public Property CapTypeStorage As Storage
            Get
                Dim val As String = Me!CapTypeStorage
                If val = "" Then Return Storage.FileSystem
                Return [Enum].Parse(GetType(Storage), val, True)
            End Get
            Set(ByVal value As Storage)
                Me!CapTypeStorage = value.ToString.ToLower
            End Set
        End Property
        ''' <summary>Gets or sets storage where cap sign images are stored</summary>
        <DefaultValue(Storage.FileSystem)> _
        Public Property MainTypeStorage As Storage
            Get
                Dim val As String = Me!MainTypeStorage
                If val = "" Then Return Storage.FileSystem
                Return [Enum].Parse(GetType(Storage), val, True)
            End Get
            Set(ByVal value As Storage)
                Me!MainTypeStorage = value.ToString.ToLower
            End Set
        End Property
        ''' <summary>Gets or sets storage where cap sign images are stored</summary>
        <DefaultValue(Storage.FileSystem)> _
        Public Property ShapeStorage As Storage
            Get
                Dim val As String = Me!ShapeStorage
                If val = "" Then Return Storage.FileSystem
                Return [Enum].Parse(GetType(Storage), val, True)
            End Get
            Set(ByVal value As Storage)
                Me!ShapeStorage = value.ToString.ToLower
            End Set
        End Property
        ''' <summary>Gets or sets storage where cap sign images are stored</summary>
        <DefaultValue(Storage.FileSystem)> _
        Public Property StorageStorage As Storage
            Get
                Dim val As String = Me!StorageStorage
                If val = "" Then Return Storage.FileSystem
                Return [Enum].Parse(GetType(Storage), val, True)
            End Get
            Set(ByVal value As Storage)
                Me!StorageStorage = value.ToString.ToLower
            End Set
        End Property
        ''' <summary>Sets value of <see cref="CapSignStorage"/>, <see cref="CapTypeStorage"/>, <see cref="MainTypeStorage"/>, <see cref="ShapeStorage"/> and <see cref="StorageStorage"/> to one common value</summary>
        ''' <param name="value">Vallue to set <see cref="CapSignStorage"/>, <see cref="CapTypeStorage"/>, <see cref="MainTypeStorage"/>, <see cref="ShapeStorage"/> and <see cref="StorageStorage"/> properties to.</param>
        Public Sub SetOtherImagesStorage(ByVal value As Storage)
            CapSignStorage = value
            CapTypeStorage = value
            MainTypeStorage = value
            ShapeStorage = value
            StorageStorage = value
        End Sub
    End Class
#End Region
#End Region
End Class

''' <summary>Provides accesss to subset of configuration nodes with specific prefix</summary>
<EditorBrowsable(EditorBrowsableState.Advanced)> _
Public Class ConfigNodeSubProvider
    Inherits ConfigNodeProvider
    Private ReadOnly _Parent As ConfigNodeProvider
    Private ReadOnly _Prefix As String
    ''' <summary>CTor - creates a new instance of the <see cref="ConfigNodeSubProvider"/> class</summary>
    ''' <param name="parent">Provider to provide accsee through</param>
    ''' <param name="prefix">Prefix (without trailing dot (.)) of nodes provided by this sub-provider</param>
    ''' <exception cref="ArgumentNullException"><paramref name="prefix"/> or <paramref name="parent"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="prefix"/> is an empty string</exception>
    Public Sub New(ByVal prefix$, ByVal parent As ConfigNodeProvider)
        MyBase.New()
        If parent Is Nothing Then Throw New ArgumentNullException("parent")
        If prefix Is Nothing Then Throw New ArgumentNullException("prefix")
        If prefix = "" Then Throw New ArgumentException("Prefix cannot be an empty string", "prefix")
        _Parent = parent
        _Prefix = prefix
    End Sub
    ''' <summary>Gets data context used by this instance</summary>
    Public Overrides ReadOnly Property Context As Data.CapsDataContext
        Get
            Return MyBase.Context
        End Get
    End Property
    ''' <summary>Gets parent provider this provider provides access to subset of nodes of</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public ReadOnly Property Parent() As ConfigNodeProvider
        Get
            Return _Parent
        End Get
    End Property
    ''' <summary>Gets prefix this sub-provider preppends to all node names</summary>
    ''' <remarks>The prefix is returned without trailing dot (.).</remarks>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public ReadOnly Property Prefix() As String
        Get
            Return _Prefix
        End Get
    End Property
    ''' <summary>Gets value of configuration node</summary>
    ''' <param name="key">Key of node to get value of</param>
    ''' <returns>Value of configuration node with given <paramref name="key"/>; null where there is no such node (or node value is null)</returns>
    Public Overrides Function GetConfigNodeValue(ByVal key As String) As String
        Return MyBase.GetConfigNodeValue(Prefix & "." & key)
    End Function
    ''' <summary>Gets typed value of configuration node</summary>
    ''' <param name="key">Key of node to get vallue of</param>
    ''' <param name="default">Default value to return when configuration node with given <paramref name="key"/> does not exist</param>
    ''' <returns>Value of configuration node converted to type <typeparamref name="T"/>; <paramref name="default"/> when node with given <paramref name="key"/> does not exist.</returns>
    ''' <typeparam name="T">Type of value to convert node value to. <see cref="String"/> must be convertible to this type.</typeparam>
    ''' <exception cref="InvalidCastException">Value of configuration node cannot be converted to type <typeparamref name="T"/></exception>
    Public Overrides Function GetConfigNodeValue(Of T)(ByVal key As String, Optional ByVal [default] As T = Nothing) As T
        Return MyBase.GetConfigNodeValue(Of T)(Prefix & "." & key, [default])
    End Function
    ''' <summary>Sets value of configuration node from string</summary>
    ''' <param name="key">Key of node to set value of</param>
    ''' <param name="value">Value to be set</param>
    Public Overrides Sub SetConfigNodeValue(ByVal key As String, ByVal value As String)
        MyBase.SetConfigNodeValue(Prefix & "." & key, value)
    End Sub
    ''' <summary>Gets all existing configuration node keys</summary>
    Public Overrides ReadOnly Property Keys As System.Collections.Generic.IEnumerable(Of String)
        Get
            Return From key In MyBase.Keys Where key.StartsWith(Prefix & ".") Select key.Substring(Prefix.Length + 2)
        End Get
    End Property
End Class

