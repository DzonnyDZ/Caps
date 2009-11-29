Imports Tools

''' <summary>Suggests cap types based on cap properties</summary>
Partial Public Class TypeSuggestor

#Region "Dependency properties"
#Region "Input"
#Region "MainType"
    ''' <summary>Gets or sets main type of cap</summary>
    Public Property MainType() As MainType
        <DebuggerStepThrough()> Get
            Return GetValue(MainTypeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As MainType)
            SetValue(MainTypeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="MainType"/> property</summary>
    Public Shared ReadOnly MainTypeProperty As DependencyProperty = _
                           DependencyProperty.Register("MainType", GetType(MainType), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(Nothing, AddressOf OnMainTypeChanged))
    ''' <summary>Called when value of the <see cref="MainType"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="MainType"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnMainTypeChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnMainTypeChanged(e)
    End Sub
    ''' <summary>Called whan välue of the <see cref="MainType"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnMainTypeChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        MakeSuggestions()
    End Sub
#End Region

#Region "Shape"
    ''' <summary>Gets or sets shape of cap</summary>
    Public Property Shape() As Shape
        <DebuggerStepThrough()> Get
            Return GetValue(ShapeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Shape)
            SetValue(ShapeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Shape"/> property</summary>
    Public Shared ReadOnly ShapeProperty As DependencyProperty = _
                           DependencyProperty.Register("Shape", GetType(Shape), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(Nothing, AddressOf OnShapeChanged))
    ''' <summary>Called when value of the <see cref="Shape"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="Shape"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnShapeChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnShapeChanged(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="Shape"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnShapeChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        MakeSuggestions()
    End Sub
#End Region

#Region "Size1"
    ''' <summary>Gets or sets primary size of cap</summary>
    Public Property Size1() As Integer
        <DebuggerStepThrough()> Get
            Return GetValue(Size1Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer)
            SetValue(Size1Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Size1"/> property</summary>
    Public Shared ReadOnly Size1Property As DependencyProperty = _
                           DependencyProperty.Register("Size1", GetType(Integer), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(0, AddressOf OnSize1Changed))
    ''' <summary>Called when value of the <see cref="Size1"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="Size1"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnSize1Changed(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnSize1Changed(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="Size1"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSize1Changed(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        MakeSuggestions()
    End Sub
#End Region

#Region "Size2"
    ''' <summary>Gets or sets secondary size of cap</summary>
    Public Property Size2() As Integer
        <DebuggerStepThrough()> Get
            Return GetValue(Size2Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer)
            SetValue(Size2Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Size2"/> property</summary>
    Public Shared ReadOnly Size2Property As DependencyProperty = _
                           DependencyProperty.Register("Size2", GetType(Integer), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(0, AddressOf OnSize2Changed))
    ''' <summary>Called when value of the <see cref="Size2"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="Size2"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnSize2Changed(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnSize2Changed(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="Size2"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSize2Changed(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        MakeSuggestions()
    End Sub
#End Region

#Region "CapHeight"
    ''' <summary>Gets or sets height of cap</summary>
    Public Property CapHeight() As Integer
        <DebuggerStepThrough()> Get
            Return GetValue(CapHeightProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer)
            SetValue(CapHeightProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapHeight"/> property</summary>
    Public Shared ReadOnly CapHeightProperty As DependencyProperty = _
                           DependencyProperty.Register("CapHeight", GetType(Integer), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(0, AddressOf OnCapHeightChanged))
    ''' <summary>Called when value of the <see cref="CapHeight"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="CapHeight"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnCapHeightChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnCapHeightChanged(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="CapHeight"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapHeightChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        MakeSuggestions()
    End Sub
#End Region

#Region "Material"
    ''' <summary>Gets or sets material of cap</summary>
    Public Property Material() As Material
        <DebuggerStepThrough()> Get
            Return GetValue(MaterialProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Material)
            SetValue(MaterialProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Material"/> property</summary>
    Public Shared ReadOnly MaterialProperty As DependencyProperty = _
                           DependencyProperty.Register("Material", GetType(Material), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(Nothing, AddressOf OnMaterialChanged))
    ''' <summary>Called when value of the <see cref="Material"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="Material"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnMaterialChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnMaterialChanged(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="Material"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnMaterialChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        MakeSuggestions()
    End Sub
#End Region

#Region "TargetObject"
    ''' <summary>Gets or sets target object of cap</summary>
    Public Property TargetObject() As Target
        <DebuggerStepThrough()> Get
            Return GetValue(TargetObjectProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Target)
            SetValue(TargetObjectProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="TargetObject"/> property</summary>
    Public Shared ReadOnly TargetObjectProperty As DependencyProperty = _
                           DependencyProperty.Register("TargetObject", GetType(Target), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(Nothing, AddressOf OnTargetObjectChanged))
    ''' <summary>Called when value of the <see cref="TargetObject"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="TargetObject"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnTargetObjectChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnTargetObjectChanged(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="TargetObject"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnTargetObjectChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        MakeSuggestions()
    End Sub
#End Region

#Region "Context"
    ''' <summary>Gets or sets <see cref="CapsDataDataContext"/> to operate onto</summary>
    Public Property Context() As CapsDataDataContext
        <DebuggerStepThrough()> Get
            Return GetValue(ContextProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As CapsDataDataContext)
            SetValue(ContextProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Context"/> property</summary>
    Public Shared ReadOnly ContextProperty As DependencyProperty = _
                           DependencyProperty.Register("Context", GetType(CapsDataDataContext), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(Nothing, AddressOf OnContextChanged))
    ''' <summary>Called when value of the <see cref="Context"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="Context"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnContextChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If d Is Nothing Then Throw New ArgumentNullException("d")
        DirectCast(d, TypeSuggestor).OnContextChanged(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="Context"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnContextChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If e.OldValue IsNot Nothing Then RemoveHandler DirectCast(e.OldValue, CapsDataDataContext).Disposed, AddressOf Context_Disposed
        If e.NewValue IsNot Nothing Then AddHandler Context.Disposed, AddressOf Context_Disposed
        CType(Resources("GetCapsOfConverter"), GetCapsOfConverter).Context = Me.Context
        MakeSuggestions()
    End Sub
    ''' <summary>Handles the <see cref="Context"/>.<see cref="CapsDataDataContext.Disposed">Disposed</see> event</summary>
    ''' <param name="sender">Source of the event</param>
    ''' <param name="e">Event arguments</param>
    Private Sub Context_Disposed(ByVal sender As Object, ByVal e As EventArgs)
        MakeSuggestions()
    End Sub
#End Region
#End Region
#Region "Output"
#Region "SelectedExistingType"
    ''' <summary>Gets or sets currently selected suggested existing type</summary>
    Public Property SelectedExistingType() As CapType
        <DebuggerStepThrough()> Get
            Return GetValue(SelectedExistingTypeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As CapType)
            SetValue(SelectedExistingTypeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="SelectedExistingType"/> property</summary>
    Public Shared ReadOnly SelectedExistingTypeProperty As DependencyProperty = _
                           DependencyProperty.Register("SelectedExistingType", GetType(CapType), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(Nothing, AddressOf OnSelectedExistingTypeChanged, AddressOf CoerceSelectedExistingTypeValue))
    ''' <summary>Called when value of the <see cref="SelectedExistingType"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="SelectedExistingType"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnSelectedExistingTypeChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If d Is Nothing Then Throw New ArgumentNullException("d")
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        DirectCast(d, TypeSuggestor).OnSelectedExistingTypeChanged(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="SelectedExistingType"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSelectedExistingTypeChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        lstExTypes.SelectedItem = e.NewValue
    End Sub
    ''' <summary>Called whenever a value of the <see cref="SelectedExistingType"/> dependency property is being re-evaluated, or coercion is specifically requested.</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system passes this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns>The coerced value (with appropriate type).</returns>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not of type <see cref="TypeSuggestor"/> -or- <paramref name="baseValue"/> is not of type <see cref="CapType"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    Private Shared Function CoerceSelectedExistingTypeValue(ByVal d As System.Windows.DependencyObject, ByVal baseValue As Object) As Object
        If d Is Nothing Then Throw New ArgumentNullException("d")
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If Not TypeOf baseValue Is String AndAlso Not baseValue Is Nothing Then Throw New Tools.TypeMismatchException("baseValue", baseValue, GetType(CapType))
        Return DirectCast(d, TypeSuggestor).CoerceSelectedExistingTypeValue(baseValue)
    End Function
    ''' <summary>Called whenever a value of the <see cref="SelectedExistingType"/> dependency property is being re-evaluated, or coercion is specifically requested.</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt, but ensured to be of correct type.</param>
    ''' <returns>The coerced value (with appropriate type).</returns>
    Protected Overridable Function CoerceSelectedExistingTypeValue(ByVal baseValue As CapType) As CapType
        For Each item As CapType In lstExTypes.Items
            If item.CapTypeID = baseValue.CapTypeID Then Return item
        Next
        Return Nothing
    End Function
#End Region

#Region "SelectedSuggestedType"
    ''' <summary>Gets or sets currently selected suggested type</summary>
    Public Property SelectedSuggestedType() As Object
        <DebuggerStepThrough()> Get
            Return GetValue(SelectedSuggestedTypeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Object)
            SetValue(SelectedSuggestedTypeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="SelectedSuggestedType"/> property</summary>
    Public Shared ReadOnly SelectedSuggestedTypeProperty As DependencyProperty = _
                           DependencyProperty.Register("SelectedSuggestedType", GetType(Object), GetType(TypeSuggestor), _
                           New FrameworkPropertyMetadata(Nothing, AddressOf OnSelectedSuggestedTypeChanged, AddressOf CoerceSelectedSuggestedTypeValue))
    ''' <summary>Called when value of the <see cref="SelectedSuggestedType"/> property changes for any <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">A <see cref="TypeSuggestor"/> <see cref="SelectedSuggestedType"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    <DebuggerStepThrough()> _
    Private Shared Sub OnSelectedSuggestedTypeChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If d Is Nothing Then Throw New ArgumentNullException("d")
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        DirectCast(d, TypeSuggestor).OnSelectedSuggestedTypeChanged(e)
    End Sub
    ''' <summary>Called whan value of the <see cref="SelectedSuggestedType"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSelectedSuggestedTypeChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        dgNewTypes.SelectedItem = e.NewValue
    End Sub
    ''' <summary>Called whenever a value of the <see cref="SelectedSuggestedType"/> dependency property is being re-evaluated, or coercion is specifically requested.</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system passes this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns>The coerced value (with appropriate type).</returns>
    ''' <exception cref="Tools.TypeMismatchException"><paramref name="d"/> is not of type <see cref="TypeSuggestor"/> -or- <paramref name="baseValue"/> is not of type <see cref="Object"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    Private Shared Function CoerceSelectedSuggestedTypeValue(ByVal d As System.Windows.DependencyObject, ByVal baseValue As Object) As Object
        If d Is Nothing Then Throw New ArgumentNullException("d")
        If Not TypeOf d Is TypeSuggestor Then Throw New Tools.TypeMismatchException("d", d, GetType(TypeSuggestor))
        If Not TypeOf baseValue Is String AndAlso Not baseValue Is Nothing Then Throw New Tools.TypeMismatchException("baseValue", baseValue, GetType(Object))
        Return DirectCast(d, TypeSuggestor).CoerceSelectedSuggestedTypeValue(baseValue)
    End Function
    ''' <summary>Called whenever a value of the <see cref="SelectedSuggestedType"/> dependency property is being re-evaluated, or coercion is specifically requested.</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt, but ensured to be of correct type.</param>
    ''' <returns>The coerced value (with appropriate type).</returns>
    Protected Overridable Function CoerceSelectedSuggestedTypeValue(ByVal baseValue As Object) As Object
        For Each item As Object In dgNewTypes.Items
            If item Is baseValue Then Return baseValue
        Next
        Return Nothing
    End Function
#End Region
#End Region
#End Region

    ''' <summary>Type initializer</summary>
    Shared Sub New()
        TypeSuggestor.IsEnabledProperty.OverrideMetadata(GetType(TypeSuggestor), New FrameworkPropertyMetadata(AddressOf OnIsEnabledChanged))
    End Sub

    ''' <summary>Makes the suggestions</summary>
    ''' <threadsafety>This method is thread-safe</threadsafety>
    Protected Overridable Sub MakeSuggestions()
        If Not Me.Dispatcher.CheckAccess Then 'Required, because this method gets called when context is disposed on application terminate on different thread
            Me.Dispatcher.Invoke(New Action(AddressOf MakeSuggestions))
            Exit Sub
        End If
        lblNewError.Visibility = Windows.Visibility.Collapsed
        lblExError.Visibility = Windows.Visibility.Collapsed
        Dim AnyExType As Boolean = False
        Dim AnyNewType As Boolean = False
        If Context Is Nothing OrElse Context.IsDisposed OrElse Not IsEnabled Then
            grMain.IsEnabled = False
        Else
            grMain.IsEnabled = True
            Const MaxSizeDiff% = 3
            If MainType IsNot Nothing AndAlso Shape IsNot Nothing Then
                'Existing types
                Dim exTypesQ = From type In Context.CapTypes _
                    Where type.MainTypeID = MainType.MainTypeID AndAlso type.ShapeID = Shape.ShapeID AndAlso _
                          (type.Height >= CapHeight - MaxSizeDiff AndAlso type.Height <= CapHeight + MaxSizeDiff) AndAlso _
                          (type.Size >= Size1 - MaxSizeDiff AndAlso type.Size <= Size1 + MaxSizeDiff) AndAlso _
                          (String.IsNullOrEmpty(Shape.Size2Name) OrElse Not type.Size2.HasValue OrElse (type.Size2 >= Size2 - MaxSizeDiff AndAlso type.Size2 <= Size2 + MaxSizeDiff)) _
                   Select Type = type, _
                          Score = MaxSizeDiff - Math.Abs(type.Height - CapHeight) + _
                                  MaxSizeDiff - Math.Abs(type.Size - Size1) + _
                                  If(String.IsNullOrEmpty(Shape.Size2Name) OrElse Not type.Size2.HasValue, 0, MaxSizeDiff - Math.Abs(type.Size2.Value - Size2)) + _
                                  If(If(Material Is Nothing, New Integer?, Material.MaterialID) = type.MaterialID, MaxSizeDiff, 0) + _
                                  If(If(TargetObject Is Nothing, New Integer?, TargetObject.TargetID) = type.TargetID, MaxSizeDiff, 0) _
                   Order By Score Descending _
                   Take 10      'TODO: There's probably bug in LINQ-to-SQL. If If(If( above is rewritten using single If(x IsNot Nothing AndAlso x.xID = type.xID ... it throws NullReferenceException upon ToList() call
                Try
                    Dim exTypes = exTypesQ.ToList
                    AnyExType = exTypes.Count > 0
                    lstExTypes.ItemsSource = exTypes
                Catch ex As Exception
                    lblExError.Visibility = Windows.Visibility.Visible
                    lblExError.ToolTip = ex.GetType.Name & vbCrLf & ex.Message
                End Try
                'Suggets new types
                Dim newTypesCapsQ = From cap In Context.Caps _
                    Where cap.CapTypeID Is Nothing AndAlso _
                        cap.MainTypeID = MainType.MainTypeID AndAlso cap.ShapeID = Shape.ShapeID AndAlso _
                        (cap.Height >= CapHeight - MaxSizeDiff AndAlso cap.Height <= CapHeight + MaxSizeDiff) AndAlso _
                        (cap.Size >= Size1 - MaxSizeDiff AndAlso cap.Size <= Size1 + MaxSizeDiff) AndAlso _
                        (String.IsNullOrEmpty(Shape.Size2Name) OrElse cap.Size2 Is Nothing OrElse (cap.Size2 >= Size2 - MaxSizeDiff AndAlso cap.Size2 <= Size2 + MaxSizeDiff))
                If newTypesCapsQ.Count >= 3 Then
                    Dim newAggregationsQ = From item In newTypesCapsQ Group By item.MaterialID _
                                          Into Group, Size = Average(item.Size), Height = Average(item.Height), Size2 = Average(item.Size2), Count() _
                                          Where Count >= 3 _
                                          Select Caps = Group, _
                                            Size1 = CInt(Math.Round(Size, MidpointRounding.AwayFromZero)), _
                                            Size2 = If(Size2.hasvalue, New Integer?(Math.Round(Size2.value, MidpointRounding.AwayFromZero)), Nothing), _
                                            Height = CInt(Math.Round(Height, MidpointRounding.AwayFromZero)), Count = Count, _
                                            MaterialID, ShapeID = Group.First.ShapeID, MainTypeID = Group.First.MainTypeID _
                                          Order By Count Descending
                    Dim suggTypesQ = From agg In newAggregationsQ _
                                     Select MainTypeID = agg.MainTypeID, ShapeID = agg.ShapeID, _
                                           Size = agg.Size1, Size2 = agg.Size2, Height = agg.Height, _
                                           MaterialID = agg.MaterialID, _
                                           TargetID = If((From cap In agg.Caps Select cap.TargetID Distinct).Count = 1, agg.Caps.First.TargetID, New Integer?), _
                                           Caps = agg.Caps,
                                           MainType = (From mt In Context.MainTypes Where mt.MainTypeID = agg.MainTypeID).FirstOrDefault,
                                           Shape = (From sh In Context.Shapes Where sh.ShapeID = agg.ShapeID).FirstOrDefault,
                                           Target = If((From cap In agg.Caps Select cap.TargetID Distinct).Count = 1, agg.Caps.First.Target, Nothing),
                                           Material = (From mat In Context.Materials Where mat.MaterialID = agg.MaterialID).FirstOrDefault
                    Try
                        Dim suggTypes = suggTypesQ.ToList
                        AnyNewType = suggTypesQ.Count > 0
                        dgNewTypes.ItemsSource = suggTypes
                    Catch ex As Exception
                        lblNewError.Visibility = Windows.Visibility.Visible
                        lblNewError.ToolTip = ex.GetType.Name & vbCrLf & ex.Message
                    End Try
                End If
            End If
        End If
        lstExTypes.Visibility = If(AnyExType, Visibility.Visible, Visibility.Collapsed)
        lblNoExTypes.Visibility = If(AnyExType, Visibility.Collapsed, Visibility.Visible)
        dgNewTypes.Visibility = If(AnyNewType, Visibility.Visible, Visibility.Collapsed)
        lblNoNewTypes.Visibility = If(AnyNewType, Visibility.Collapsed, Visibility.Visible)
    End Sub

    ''' <summary>Called when value of the <see cref="IsEnabled"/> property changes for any instance of <see cref="TypeSuggestor"/></summary>
    ''' <param name="d">Instance of <see cref="TypeSuggestor"/> <see cref="IsEnabled"/> has changed for</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="ArgumentNullException"><paramref name="d"/> is null</exception>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="TypeSuggestor"/></exception>
    Private Shared Sub OnIsEnabledChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If d Is Nothing Then Throw New ArgumentException("d")
        If Not TypeOf d Is TypeSuggestor Then Throw New TypeMismatchException("d", d, GetType(TypeSuggestor))
        DirectCast(d, TypeSuggestor).OnIsEnabledChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="IsEnabled"/> property changes for current instance</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnIsEnabledChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If IsEnabled Then MakeSuggestions()
    End Sub

#Region "Events"
#Region "ApplyExistingType"
    Private Sub ExType_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If e.ClickCount = 2 AndAlso e.ChangedButton = MouseButton.Left Then
            OnApplyExistingType()
        End If
    End Sub
    ''' <summary>Raised when user selects existing sugggested type for application</summary>
    Public Custom Event ApplyExistingType As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(ApplyExistingTypeEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(ApplyExistingTypeEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    Public Shared ReadOnly ApplyExistingTypeEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ApplyExistingType", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(TypeSuggestor))
    ''' <summary>Raises the <see cref="ApplyExistingType"/> event</summary>
    Protected Overridable Sub OnApplyExistingType()
        RaiseEvent ApplyExistingType(Me, New RoutedEventArgs(ApplyExistingTypeEvent, Me))
    End Sub
#End Region
#Region "ApplyNewType"
    Private Sub mniCreateType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        OnApplyNewType()
    End Sub
    ''' <summary>Raised when user selects new suggested type for creation</summary>
    Public Custom Event ApplyNewType As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(ApplyNewTypeEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(ApplyNewTypeEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    Public Shared ReadOnly ApplyNewTypeEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ApplyNewType", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(TypeSuggestor))

    ''' <summary>Raises the <see cref="ApplyNewType"/> event</summary>
    Protected Overridable Sub OnApplyNewType()
        RaiseEvent ApplyNewType(Me, New RoutedEventArgs(ApplyNewTypeEvent, Me))
    End Sub
#End Region
#End Region
End Class

