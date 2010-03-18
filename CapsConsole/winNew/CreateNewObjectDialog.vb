Imports System.Data.Objects.DataClasses
Imports Caps.Data
Imports System.ComponentModel

''' <summary>Common abstract base class used as base for all dialogs for creation of new objects</summary>
<EditorBrowsable(ComponentModel.EditorBrowsableState.Advanced)>
Public MustInherit Class CreateNewObjectDialogBase
    Inherits Window
    Implements IDisposable
    ''' <summary>CTor - creates a new instance of the <see cref="CreateNewObjectDialogBase"/></summary>
    Protected Sub New()
        _Context = New CapsDataContext(Main.EntityConnection)
    End Sub

    ''' <summary>Contains value of the <see cref="Context"/> property</summary>
    Private _Context As CapsDataContext
    ''' <summary>Gets data context used by the window</summary>
    Public ReadOnly Property Context As CapsDataContext
        Get
            Return _Context
        End Get
    End Property
    ''' <summary>Contains value of the <see cref="NewObject"/> property</summary>
    Private _NewObject As EntityObject
    ''' <summary>When window is closed by OK button, gets object created by the window</summary>
    ''' <remarks>
    ''' This property is only valid after window was closed with <see cref="DialogResult"/> True.
    ''' Object returned by this property belongs to <see cref="Context"/> of window.
    ''' <note type="inheritinfo">Value of this property must be set before the window is closed.</note>
    ''' </remarks>
    Public Property NewObject As EntityObject
        Get
            Return _NewObject
        End Get
        Protected Set(ByVal value As EntityObject)
            _NewObject = value
        End Set
    End Property

    ''' <summary>Gets object created by this window transformed to given context</summary>
    ''' <param name="context">Context to transform <see cref="NewObject"/> to. <see cref="NewObject"/> is not transformed when this argument is null.</param>
    ''' <returns><see cref="NewObject"/> transformed to <paramref name="context"/>. <see cref="NewObject"/> without transformation, when <paramref name="context"/> is null</returns>
    Public Function GetNewObject(Optional ByVal context As CapsDataContext = Nothing) As EntityObject
        If context Is Nothing Then Return NewObject
        If NewObject Is Nothing Then Return Nothing
        Return context.GetObjectByKey(_NewObject.EntityKey)
    End Function

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


''' <summary>Type-safe base class for dialogs for creation of new objects</summary>
''' <typeparam name="T">Type of object created by the dialog</typeparam>
<EditorBrowsable(ComponentModel.EditorBrowsableState.Advanced)>
Public MustInherit Class CreateNewObjectDialogBase(Of T As {EntityObject})
    Inherits CreateNewObjectDialogBase
    ''' <summary>CTor - creates a new instance of the <see cref="CreateNewObjectDialogBase(Of T)"/> class</summary>
    Protected Sub New()
        MyBase.New()
    End Sub
    ''' <summary>When window is closed by OK button, gets object created by the window</summary>
    ''' <remarks>
    ''' This property is only valid after window was closed with <see cref="DialogResult"/> True.
    ''' Object returned by this property belongs to <see cref="Context"/> of window.
    ''' <note type="inheritinfo">Value of this property must be set before the window is closed.</note>
    ''' </remarks>
    Public Shadows Property NewObject As T
        Get
            Return MyBase.NewObject
        End Get
        Protected Set(ByVal value As T)
            MyBase.NewObject = value
        End Set
    End Property
    ''' <summary>Gets object created by this window transformed to given context</summary>
    ''' <param name="context">Context to transform <see cref="NewObject"/> to. <see cref="NewObject"/> is not transformed when this argument is null.</param>
    ''' <returns><see cref="NewObject"/> transformed to <paramref name="context"/>. <see cref="NewObject"/> without transformation, when <paramref name="context"/> is null</returns>
    Public Shadows Function GetNewObject(Optional ByVal context As CapsDataContext = Nothing) As T
        If context Is Nothing Then Return NewObject
        Return context.GetObjectByKey(NewObject.EntityKey)
    End Function



End Class
