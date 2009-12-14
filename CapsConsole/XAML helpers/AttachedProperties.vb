Imports Tools, Tools.LinqT

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
    ''' <remarks>The SetAllOnFocus attached dependency proeprty, when set to true, causes that all text of <see cref="TextBox"/> is selected when it gets focus.</remarks>
    Public ReadOnly SelectAllOnFocusProperty As DependencyProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", _
                           GetType(Boolean), GetType(AttachedProperties), New FrameworkPropertyMetadata(False))
#End Region

#Region "ItemsWithAutomaticRows"
    ''' <summary>Sets value of the SetItemsWithAutomaticRows attached dependency property</summary>
    ''' <param name="element">Object (<see cref="Grid"/>) to set value for</param>
    ''' <param name="value">Array of <see cref="UIElement">UIElement</see> to be added to <paramref name="element"/>. If given elements requires more rows than <paramref name="element"/> currently has, new rows with automatic height are added.</param>
    ''' <exception cref="ArgumentNullException"><paramref name="element"/> is null</exception>
    ''' <remarks>This property does not have get method since it is meaningless. Use <see cref="Grid.Children"/> instead.</remarks>
    Public Sub SetItemsWithAutomaticRows(ByVal element As Grid, ByVal value As UIElement())
        If element Is Nothing Then      Throw New ArgumentNullException("element")
        element.SetValue(ItemsWithAutomaticRowsProperty, value)
    End Sub
    ''' <summary>Metadata of the SetItemsWithAutomaticRows attached dependency property</summary>
    ''' <remarks>
    ''' The SetItemsWithAutomaticRows shall be treated as write-only property used for setting <see cref="Grid.Children">Children</see> of <see cref="Grid"/> when the <see cref="Grid"/> does not have enough rows for all children.
    ''' When set, this method ensures that there is enough rows in the <see cref="Grid"/> (as defined by maximum value of <see cref="Grid.RowProperty"/> + <see cref="Grid.RowSpanProperty"/> - 1).
    ''' It there is not enough rows, new <see cref="RowDefinition">RowDefinition</see> with <see cref="RowDefinition.Height"/> set to automatic height are added to <see cref="Grid.RowDefinitions"/>.
    ''' If there is more rows than required, rows are NOT removed.
    ''' Before possibly adding rows, setter <see cref="UIElementCollection.Clear">clears</see> <see cref="Grid.Children">Children</see> collection of the <see cref="Grid"/>.
    ''' After possibly adding rows, setter adds all items from array passed as property value to <see cref="Grid.Children">Children</see> collection of the <see cref="Grid"/>.
    ''' <example>This exaple shows typical usage of SetItemsWithAutomaticRows attached dependency property
    ''' <code lang="XAML"><![CDATA[<Grid>
    '''     <Grid.ColumnDefinitions>
    '''         <ColumnDefinition Width="auto"/>
    '''         <ColumnDefinition Width="*"/>
    '''     </Grid.ColumnDefinitions>
    '''     <ns:AttachedProperties.ItemsWithAutomaticRows>
    '''         <x:Array Type="UIElement">
    '''             <Label Content="Name" Grid.Row="0" Grid.Column="0"/>
    '''             <TextBox Name="txtName" Grid.Column="1" Grid.Row="0"/>
    '''             <Label Content="Description" Grid.Row="1" Grid.Column="0"/>
    '''             <TextBox Name="txtDesc" Grid.Column="1" Grid.Row="1"/>
    '''             <Label Content="Note" Grid.Row="2" Grid.Column="0"/>
    '''             <TextBox Name="txtNote" Grid.Column="1" Grid.Row="2"/>
    '''         </x:Array>
    '''     </ns:AttachedProperties.ItemsWithAutomaticRows>
    ''' </Grid>]]></code>
    ''' </example>
    ''' </remarks>
    Public ReadOnly ItemsWithAutomaticRowsProperty As DependencyProperty = DependencyProperty.RegisterAttached("ItemsWithAutomaticRows", _
                           GetType(UIElement()), GetType(AttachedProperties), _
                           New FrameworkPropertyMetadata(New UIElement() {}, AddressOf SetItemsWithAutomaticRows))
    ''' <summary>Performs action defined for setter of the <see cref="ItemsWithAutomaticRowsProperty"/></summary>
    ''' <param name="d"><see cref="Grid"/> value of the property has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not of type <see cref="Grid"/> -or- <paramref name="e"/>.<see cref="DependencyPropertyChangedEventArgs.NewValue">NewValue</see> is neither null nor <see cref="IEnumerable(Of T)"/>[<see cref="UIElement"/>].</exception>
    Private Sub SetItemsWithAutomaticRows(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If d Is Nothing Then Throw New ArgumentNullException("d")
        If Not TypeOf d Is Grid Then Throw New TypeMismatchException("d", d, GetType(Grid))
        If Not e.NewValue Is Nothing AndAlso Not TypeOf e.NewValue Is IEnumerable(Of UIElement) Then Throw New TypeMismatchException("e.Newvalue", e.NewValue, GetType(IEnumerable(Of UIElement)))
        Dim newElements As IEnumerable(Of UIElement) = e.NewValue
        Dim RequiredRowsCount As Integer
        Dim Grid As Grid = d
        Grid.Children.Clear()
        If newElements Is Nothing OrElse newElements.IsEmpty Then
            RequiredRowsCount = 0
        Else
            RequiredRowsCount = (From item In newElements Where item IsNot Nothing Select Grid.GetRow(item) + Grid.GetRowSpan(item) - 1).Max + 1
        End If
           For i As Integer = Grid.RowDefinitions.Count To RequiredRowsCount - 1
            Grid.RowDefinitions.Add(New RowDefinition With {.Height = New GridLength(0, GridUnitType.Auto)})
        Next
        If newElements IsNot Nothing Then
            For Each item In newElements
                Grid.Children.Add(item)
            Next
        End If
    End Sub

#End Region


End Module
