''' <summary>Contains various attached dependency  properties</summary>
Public Module AttachedProperties
    ''' <summary>Initializer</summary>
    Sub New()
        EventManager.RegisterClassHandler(GetType(TextBox), TextBox.GotFocusEvent, New RoutedEventHandler(AddressOf TextBox_GotFocus))
    End Sub
#Region "SelectAllOnFocus"
    Private Sub TextBox_GotFocus(ByVal sender As TextBox, ByVal e As RoutedEventArgs)
        If GetSelectAllOnFocus(sender) Then sender.SelectAll()
    End Sub
    ''' <summary>Gets value of the SelectAllOnFocus attached dependency  property</summary>
    ''' <param name="element">Object (<see cref="TextBox"/>) to get value for</param>
    ''' <returns>True when content of text box is selected when it got focus; false otherwise</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
    Public Function GetSelectAllOnFocus(ByVal element As TextBox) As Boolean
        If element Is Nothing Then Throw New ArgumentNullException("element")
        Return element.GetValue(SelectAllOnFocusProperty)
    End Function
    ''' <summary>Sets value of the SelectAllOnFocus attached dependency  property</summary>
    ''' <param name="element">Object (<see cref="TextBox"/>) to set value for</param>
    ''' <param name="value">True to make <see cref="TextBox"/> automatically select all its content when focused;false to prevent such behavior.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
    Public Sub SetSelectAllOnFocus(ByVal element As TextBox, ByVal value As Boolean)
        If element Is Nothing Then Throw New ArgumentNullException("element")
        element.SetValue(SelectAllOnFocusProperty, value)
    End Sub
    ''' <summary>Metadata of the SelectAllOnFocus attached dependency property</summary>
    Public ReadOnly SelectAllOnFocusProperty As  _
                           DependencyProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", _
                           GetType(Boolean), GetType(AttachedProperties), _
                           New FrameworkPropertyMetadata(False))
#End Region
End Module
