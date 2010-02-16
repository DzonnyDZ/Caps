Imports Tools.CollectionsT.GenericT, Tools.ExtensionsT
Imports Tools.DrawingT.ImageTools
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports System.ComponentModel
Imports Tools.ComponentModelT
Imports Tools, Tools.WindowsT.WPF.WpfExtensions

''' <summary>Creates a new cap</summary>
Partial Public Class CapEditor
    ''' <summary>Context to be used when <see cref="Context"/> is not set</summary>
    Private OriginalContext As CapsDataDataContext = If(Main.Connection Is Nothing, Nothing, New CapsDataDataContext(Main.Connection)) 'If(...) - for designer
    ''' <summary>Contains value of the <see cref="Context"/> proeprty</summary>
    Private _Context As CapsDataDataContext = OriginalContext
    Private UnderConstruction As Boolean = True
    ''' <summary>Contains list of all cap signs</summary>
    Private AllCapSigns As New ListWithEvents(Of CapSign)
    ''' <summary>CTor</summary>
    Public Sub New()
        InitializeComponent()
        tysSuggestor.Context = Context
        Images = New ListWithEvents(Of Image)()
        'DirectCast(Me.Resources("GetCapsOfConverter"), GetCapsOfConverter).Context = OriginalContext
        UnderConstruction = False
        chkIsDrink_CheckedChanged(chkIsDrink, New RoutedEventArgs())
    End Sub


    ''' <summary>Database context</summary>
    ''' <exception cref="ArgumentNullException">Value being set is null</exception>
    Public Property Context() As CapsDataDataContext
        <DebuggerStepThrough()> Get
            Return _Context
        End Get
        Set(ByVal value As CapsDataDataContext)
            If value Is Nothing Then Throw New ArgumentNullException("value")
            If value IsNot OriginalContext AndAlso OriginalContext IsNot Nothing Then
                OriginalContext.Dispose()
                OriginalContext = Nothing
            End If
            ' DirectCast(Me.Resources("GetCapsOfConverter"), GetCapsOfConverter).Context = value
            _Context = value
            tysSuggestor.Context = value
        End Set
    End Property
    Private Shadows initialized As Boolean
    Private initializing As Boolean
    Friend Sub Initialize(ByVal ForBinding As Boolean)
        If initializing Then Exit Sub
        initializing = True
        Try
            cmbCapType.ItemsSource = New ListWithEvents(Of CapType)(From item In Context.CapTypes Order By item.TypeName)
            cmbMainType.ItemsSource = New ListWithEvents(Of MainType)(From item In Context.MainTypes Order By item.TypeName)
            cmbShape.ItemsSource = New ListWithEvents(Of Shape)(From item In Context.Shapes Order By item.Name)
            cmbMaterial.ItemsSource = New ListWithEvents(Of Material)(From item In Context.Materials Order By item.Name)
            cmbStorage.ItemsSource = New ListWithEvents(Of Storage)(From item In Context.Storages Order By item.StorageNumber)
            cmbProduct.ItemsSource = New ListWithEvents(Of Product)(From item In Context.Products Order By item.ProductName)
            cmbTarget.ItemsSource = New ListWithEvents(Of Target)(From item In Context.Targets Order By item.Name)
            AllCapSigns.Clear()
            AllCapSigns.AddRange(Context.CapSigns)
            icSigns.ItemsSource = New ListWithEvents(Of Cap_CapSign_Int)
            Dim ProductTypesList As ListWithEvents(Of ProductType) = New ListWithEvents(Of ProductType)(From item In Context.ProductTypes Order By item.ProductTypeName)
            ProductTypesList.Add(Nothing)
            cmbProductType.ItemsSource = ProductTypesList
            Dim CompaniesList As ListWithEvents(Of Company) = New ListWithEvents(Of Company)(From item In Context.Companies Order By item.CompanyName)
            CompaniesList.Add(Nothing)
            cmbCompany.ItemsSource = CompaniesList
            lstCategories.ItemsSource = New ListWithEvents(Of CategoryProxy)(From item In Context.Categories Order By item.CategoryName Select New CategoryProxy(item))
            kweKeywords.AutoCompleteStable = New ListWithEvents(Of String)(From item In Context.Keywords Order By item.Keyword Select item.Keyword)
            'lvwImages.ItemTemplate = My.Application.Resources("ImageListDataTemplate")
            DirectCast(cmbTarget.ItemsSource, ListWithEvents(Of Target)).Add(Nothing)
            If Not ForBinding Then DirectCast(icSigns.ItemsSource, ListWithEvents(Of Cap_CapSign_Int)).Add(New Cap_CapSign_Int)

            If ForBinding Then
                optCapTypeAnonymous.IsChecked = True
                optProductAnonymous.IsChecked = True
            End If
        Finally
            initializing = False
        End Try
        initialized = True
        ShowCount()
    End Sub
    Private Sub winNewCap_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If Not initialized Then Initialize(False)
    End Sub

#Region "CancelClicked"
    ''' <summary>Raised when user clicks the Cancel button</summary>
    Public Custom Event CancelClicked As RoutedEventHandler

        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(CancelClickedEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(CancelClickedEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    ''' <summary>Raises the <see cref="CancelClicked"/> event</summary>
    Protected Overridable Sub OnCancelClick(ByVal e As RoutedEventArgs)
        RaiseEvent CancelClicked(Me, New RoutedEventArgs(CancelClickedEvent, Me))
    End Sub
    ''' <summary>Metadata of the <see cref="CancelClicked"/> event</summary>
    Public Shared ReadOnly CancelClickedEvent As RoutedEvent = _
                      EventManager.RegisterRoutedEvent("CancelClicked", _
                      RoutingStrategy.Bubble, _
                      GetType(RoutedEventHandler), GetType(CapEditor))

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        OnCancelClick(e)
    End Sub
#End Region
#Region "New"
    Private Sub btnNewMainType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewMainType.Click
        Dim win As New winNewMainType(Context)
        If win.ShowDialog Then
            DirectCast(cmbMainType.ItemsSource, ListWithEvents(Of MainType)).Add(win.NewObject)
            cmbMainType.Items.Refresh()
            cmbMainType.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub cmbNewShape_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdNewShape.Click
        Dim win As New winNewShape(Context)
        If win.ShowDialog Then
            DirectCast(cmbShape.ItemsSource, ListWithEvents(Of Shape)).Add(win.NewObject)
            cmbShape.Items.Refresh()
            cmbShape.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewMaterial_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewMaterial.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.Material, Context)
        If win.ShowDialog Then
            DirectCast(cmbMaterial.ItemsSource, ListWithEvents(Of Material)).Add(DirectCast(win.NewObject, Material))
            cmbMaterial.Items.Refresh()
            cmbMaterial.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewStorage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewStorage.Click
        Dim win As New winNewStorage(Context)
        If win.ShowDialog Then
            DirectCast(cmbStorage.ItemsSource, ListWithEvents(Of Storage)).Add(win.NewObject)
            cmbStorage.Items.Refresh()
            cmbStorage.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewProductType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewProductType.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.ProductType, Context)
        If win.ShowDialog Then
            DirectCast(cmbProductType.ItemsSource, ListWithEvents(Of ProductType)).Add(DirectCast(win.NewObject, ProductType))
            cmbProductType.Items.Refresh()
            cmbProductType.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewCompany_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewCompany.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.Company, Context)
        If win.ShowDialog Then
            DirectCast(cmbCompany.ItemsSource, ListWithEvents(Of Company)).Add(DirectCast(win.NewObject, Company))
            cmbCompany.Items.Refresh()
            cmbCompany.SelectedItem = win.NewObject
        End If
    End Sub

    Private Sub btnNewCategory_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewCategory.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.Category, Context)
        If win.ShowDialog Then
            DirectCast(lstCategories.ItemsSource, ListWithEvents(Of CategoryProxy)).Add(New CategoryProxy(win.NewObject, True))
            lstCategories.Items.Refresh()
            lstCategories_CheckedChanged(lstCategories.Items(lstCategories.Items.Count - 1), New RoutedEventArgs(CheckBox.CheckedEvent, lstCategories.Items(lstCategories.Items.Count - 1)))
        End If
    End Sub
    Private Sub btnNewSign_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewSign.Click
        Dim win As New winNewSign(Context)
        If win.ShowDialog Then
            AllCapSigns.Add(win.NewObject)
            Dim Cap_Sign_Ints As List(Of Cap_CapSign_Int) = icSigns.ItemsSource
            For Each item In Cap_Sign_Ints
                If item.CapSign Is Nothing Then
                    item.CapSign = win.NewObject
                    Exit Sub
                End If
            Next
            Cap_Sign_Ints.Add(New Cap_CapSign_Int With {.CapSign = win.NewObject})
            Dim NewValue = (From Cap_Sing_Int In Cap_Sign_Ints Select Cap_Sing_Int.CapSign).ToArray
            SelectedCapSignsValuesNotToBeCoerced.Add(NewValue)
            SelectedCapSigns = NewValue
        End If
    End Sub
    Private Sub cmbSign_KeyDown(ByVal sender As ComboBox, ByVal e As System.Windows.Input.KeyEventArgs)
        DirectCast(icSigns.ItemsSource, ListWithEvents(Of Cap_CapSign_Int)).Remove(DirectCast(sender.DataContext, Cap_CapSign_Int))
    End Sub

    Private Sub cmbSign_Loaded(ByVal sender As ComboBox, ByVal e As System.Windows.RoutedEventArgs)
        sender.ItemsSource = AllCapSigns
    End Sub

    Private Sub btnAddSign_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAddSign.Click
        Dim Cap_Sign_Ints As ListWithEvents(Of Cap_CapSign_Int) = icSigns.ItemsSource
        Cap_Sign_Ints.Add(New Cap_CapSign_Int)
        Dim NewValue = (From Cap_Sing_Int In Cap_Sign_Ints Select Cap_Sing_Int.CapSign).ToArray
        SelectedCapSignsValuesNotToBeCoerced.Add(NewValue)
        SelectedCapSigns = NewValue
    End Sub
#End Region
    ''' <summary>Category proxy that adds <see cref="CategoryProxy.Checked"/> property</summary>
    <DebuggerDisplay("{Category.CategoryName}")> _
    Private Class CategoryProxy : Implements INotifyPropertyChanged
        ''' <summary>Contains value of the <see cref="Category"/> property</summary>
        Private ReadOnly _Category As Category
        ''' <summary>Contains value of the <see cref="Checked"/> property</summary>
        Private _Checked As Boolean
        ''' <summary>Gets category</summary>
        Public ReadOnly Property Category() As Category
            Get
                Return _Category
            End Get
        End Property
        ''' <summary>Gets value indicating if the category is selected (checked)</summary>
        Public Property Checked() As Boolean
            Get
                Return _Checked
            End Get
            Set(ByVal value As Boolean)
                Dim old As Boolean = Checked
                _Checked = value
                If old <> Checked Then OnPropertyChanged(New PropertyChangedEventArgs("Checked"))
            End Set
        End Property
        ''' <summary>CTor</summary>
        ''' <param name="Category">A category</param>
        ''' <param name="Checked">Indicates if category is selected (checked)</param>
        ''' <exception cref="ArgumentNullException"><paramref name="Category"/> is null</exception>
        Public Sub New(ByVal Category As Category, Optional ByVal Checked As Boolean = False)
            If Category Is Nothing Then Throw New ArgumentNullException("Category")
            _Category = Category
            _Checked = Checked
        End Sub
        ''' <summary>raises the <see cref="PropertyChanged"/> event</summary>
        ''' <param name="e">Event arguments</param>
        Protected Overridable Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub
        ''' <summary>Raised when value of a property changes</summary>
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

    ''' <summary>Regulare expression for image name. It parses out 4 numbers from image name.</summary>
    Private Shared ImageNameRegExp As New System.Text.RegularExpressions.Regex( _
        "(?<Before>.*)(?<Number>[0-9]{4,8})(?<After>\..{3,4})", Text.RegularExpressions.RegexOptions.Compiled Or Text.RegularExpressions.RegexOptions.CultureInvariant Or Text.RegularExpressions.RegexOptions.ExplicitCapture)

    Private Sub btnAddImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAddImage.Click
        Dim dlg As New Forms.OpenFileDialog With {.Multiselect = True, .DefaultExt = "jpg", .Filter = My.Resources.fil_ImageTypes}
        If My.Settings.LastImageName <> "" Then
            Dim match = ImageNameRegExp.Match(My.Settings.LastImageName)
            If match.Success Then
                Dim Number As Integer = Integer.Parse(match.Groups!Number.Value, System.Globalization.CultureInfo.InvariantCulture)
                Dim NewNumber As String = (Number + 1).ToString("0000")
                Dim NewPath = match.Groups!Before.Value & NewNumber & match.Groups!After.Value
                Try
                    dlg.FileName = NewPath
                Catch : End Try
            End If
        End If
        If dlg.ShowDialog() = Forms.DialogResult.OK Then
            If dlg.FileNames.Length > 0 Then
                Dim ImagesToAdd = From path In dlg.FileNames Order By IO.Path.GetFileName(path) Ascending _
                                         Select DirectCast(New NewImage(path), Image)
                DirectCast(lvwImages.ItemsSource, ListWithEvents(Of Image)).AddRange(ImagesToAdd)
                My.Settings.LastImageName = ImagesToAdd.Last.RelativePath 'Relative path is absolute
                lvwImages.Items.Refresh()
            End If
        End If
    End Sub


    Private Sub btnBrowseForCapTypeImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBrowseForCapTypeImage.Click
        Dim dlg As New Forms.OpenFileDialog With {.DefaultExt = "png", .Filter = My.Resources.fil_PNG}
        Try
            If txtCapTypeImagePath.Text <> "" Then dlg.FileName = txtCapTypeImagePath.Text
        Catch : End Try
        If dlg.ShowDialog Then
            txtCapTypeImagePath.Text = dlg.FileName
        End If
    End Sub

#Region "SaveClicked"
    ''' <summary>Raised when user clicks the save button</summary>
    Public Custom Event SaveClicked As EventHandler(Of SaveClickedEventArgs)

        AddHandler(ByVal value As EventHandler(Of SaveClickedEventArgs))
            Me.AddHandler(SaveClickedEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As EventHandler(Of SaveClickedEventArgs))
            Me.RemoveHandler(SaveClickedEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As SaveClickedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event
    ''' <summary>Raises the <see cref="SaveClicked"/> event</summary>
    Protected Overridable Sub OnSaveClick(ByVal e As SaveClickedEventArgs)
        RaiseEvent SaveClicked(Me, e)
    End Sub
    ''' <summary>Metadata of the <see cref="SaveClicked"/> event</summary>
    Public Shared ReadOnly SaveClickedEvent As RoutedEvent = _
                      EventManager.RegisterRoutedEvent("SaveClicked", _
                      RoutingStrategy.Bubble, _
                      GetType(EventHandler(Of SaveClickedEventArgs)), GetType(CapEditor))

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click, btnSaveNext.Click, mniNextNoSave.Click, mniPreviousNoSave.Click, mniReset.Click, mniSaveAndNew.Click, mniSaveAndNew.Click, mniSaveAndNext.Click, mniSaveAndNextNoClean.Click, mniSaveAndPrevious.Click
        Dim mode As SaveMode
        If sender Is btnSave Then : mode = SaveMode.SaveAndClose
        ElseIf sender Is btnSaveNext Then : mode = Me.SplitButtonCommand
        ElseIf sender Is mniNextNoSave Then : mode = SaveMode.NextNoSave
        ElseIf sender Is mniPreviousNoSave Then : mode = SaveMode.PreviousNoSave
        ElseIf sender Is mniReset Then : mode = SaveMode.Reset
        ElseIf sender Is mniSaveAndNew Then : mode = SaveMode.SaveAndNew
        ElseIf sender Is mniSaveAndNext Then : mode = SaveMode.SaveAndNext
        ElseIf sender Is mniSaveAndNextNoClean Then : mode = SaveMode.SaveAndNextNoClean
        ElseIf sender Is mniSaveAndPrevious Then : mode = SaveMode.SaveAndPrevious
        End If
        OnSaveClick(New SaveClickedEventArgs(SaveClickedEvent, Me, mode))
    End Sub
    ''' <summary>argumnets of the <see cref="SaveClicked"/> event</summary>
    Public Class SaveClickedEventArgs : Inherits RoutedEventArgs
        ''' <summary>Contains value of the <see cref="SaveMode"/> property</summary>
        Private ReadOnly _Mode As SaveMode
        ''' <summary>CTor</summary>
        ''' <param name="mode">Identifies save mode selected by user.</param>
        ''' <param name="routedEvent">The routed event identifier for this instance of the <see cref="System.Windows.RoutedEventArgs"/> class.</param>
        ''' <param name="source">An alternate source that will be reported when the event is handled. This pre-populates the <see cref="System.Windows.RoutedEventArgs.Source"/> property.</param>
        ''' <exception cref="InvalidEnumArgumentException"><paramref name="mode"/> is npot one of the <see cref="SaveMode"/> values</exception>
        Public Sub New(ByVal routedEvent As RoutedEvent, ByVal source As CapEditor, ByVal mode As SaveMode)
            MyBase.New(routedEvent, source)
            If Not Tools.TypeTools.IsDefined(mode) Then Throw New InvalidEnumArgumentException("mode", mode, mode.GetType)
            _Mode = mode
        End Sub
        ''' <summary>Gets save mode being invoked by user</summary>
        ''' <remarks>Value of this property is always one of the <see cref="SaveMode"/> values. This property does not treat <see cref="SaveMode"/> as flags.</remarks>
        Public ReadOnly Property Mode() As SaveMode
            Get
                Return _Mode
            End Get
        End Property
    End Class
    ''' <summary>Possible save modes for the <see cref="SaveClickedEventArgs"/> class</summary>
    ''' <remarks>This enumeration can be used as flags or as normal enum depending on context</remarks>
    <Flags()> _
    Public Enum SaveMode
        ''' <summary>Save current item and close dialog</summary>
        ''' <remarks>This item acts on Save button rather than on split button menu item</remarks>
        SaveAndClose = 1
        ''' <summary>Save item and proceed to next one/new one (depending on dialog type)</summary>
        SaveAndNext = 2
        ''' <summary>Save item and proceed to next one without cleaning</summary>
        SaveAndNextNoClean = 4
        ''' <summary>Save item and go back to previous</summary>
        SaveAndPrevious = 8
        ''' <summary>Save item and go to new one</summary>
        SaveAndNew = 16
        ''' <summary>Do not save item and proceed to next</summary>
        PreviousNoSave = 32
        ''' <summary>Do not save item and go to previous</summary>
        NextNoSave = 64
        ''' <summary>Do not save item and reset dialog</summary>
        Reset = 128
    End Enum

#End Region


#Region "IsSplitButtonVisible"
    ''' <summary>Gtes or sets value indicating if "Save and next" button is visible or not</summary>
    ''' <returns>True if "Save and Next" button is visible; false if it is not</returns>
    ''' <value>True to make "Save and Next" button visible; false to hide it. Default value is false.</value>
    <DefaultValue(False)> _
    Public Property IsSplitButtonVisible() As Boolean
        Get
            Return GetValue(IsSplitButtonVisibleProperty)
        End Get

        Set(ByVal value As Boolean)
            SetValue(IsSplitButtonVisibleProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="IsSplitButtonVisible"/> property</summary>
    Public Shared ReadOnly IsSplitButtonVisibleProperty As DependencyProperty = _
                           DependencyProperty.Register("IsSplitButtonVisible", _
                           GetType(Boolean), GetType(CapEditor), _
                           New FrameworkPropertyMetadata(False, AddressOf OnIsSplitButtonVisibleChanged))
    ''' <summary>Called when value of the <see cref="IsSplitButtonVisible"/> property changes</summary>
    ''' <param name="d">Instance of <see cref="CapEditor"/> ofr which the change have occured.</param>
    ''' <param name="e">Event arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnIsSplitButtonVisibleChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnIsSplitButtonVisibleChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="IsSplitButtonVisible"/> property changes</summary>
    ''' <param name="e">event arguments</param>
    Protected Overridable Sub OnIsSplitButtonVisibleChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        btnSaveNext.Visibility = If(IsSplitButtonVisible, Visibility.Visible, Visibility.Collapsed)
    End Sub

#End Region

#Region "SplitButtonComamnd"

    ''' <summary>Gets or sets defauklt command associated with additional button turned pon and off using <see cref="IsSplitButtonVisible"/></summary>
    ''' <exception cref="InvalidEnumArgumentException">Value being set is not one of <see cref="SaveMode"/> values</exception>
    ''' <remarks>This property does not treat <see cref="SaveMode"/> as flags.
    ''' <para>Split button command can be set even on command that is disabled via <see cref="EnabledCommands"/> and the command will work.</para></remarks>
    Public Property SplitButtonCommand() As SaveMode
        Get
            Return GetValue(SplitButtonCommandProperty)
        End Get

        Set(ByVal value As SaveMode)
            SetValue(SplitButtonCommandProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="SplitButtonCommand"/> dependency property</summary>
    Public Shared ReadOnly SplitButtonCommandProperty As DependencyProperty = _
                           DependencyProperty.Register("SplitButtonCommand", _
                           GetType(SaveMode), GetType(CapEditor), _
                           New FrameworkPropertyMetadata(SaveMode.SaveAndNext, _
                                                         AddressOf OnSplitButtonCommandChanged, _
                                                         AddressOf CoerceSplitButtonCommandValue))
    Private Shared Function CoerceSplitButtonCommandValue(ByVal d As System.Windows.DependencyObject, ByVal baseValue As Object) As Object
        Dim bv = TypeTools.DynamicCast(Of SaveMode)(baseValue)
        If Not TypeTools.IsDefined(bv) Then Throw New InvalidEnumArgumentException("baseValue", baseValue, bv.GetType)
        Return bv
    End Function
    Private Shared Sub OnSplitButtonCommandChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnSplitButtonCommandChanged(e)
    End Sub
    Protected Overridable Sub OnSplitButtonCommandChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        Select Case SplitButtonCommand
            Case SaveMode.NextNoSave : btnSaveNext.Content = My.Resources.mni_NextNoSave
            Case SaveMode.PreviousNoSave : btnSaveNext.Content = My.Resources.mni_PreviousNoSave
            Case SaveMode.Reset : btnSaveNext.Content = My.Resources.mni_Reset
            Case SaveMode.SaveAndClose : btnSaveNext.Content = My.Resources.cmd_SaveCap
            Case SaveMode.SaveAndNew : btnSaveNext.Content = My.Resources.mni_SaveNew
            Case SaveMode.SaveAndNext : btnSaveNext.Content = My.Resources.cmd_SaveNext
            Case SaveMode.SaveAndNextNoClean : btnSaveNext.Content = My.Resources.mni_SaveNextNoClean
            Case SaveMode.SaveAndPrevious : btnSaveNext.Content = My.Resources.mni_SavePrevious
            Case Else : btnSaveNext.Content = SplitButtonCommand.ToString
        End Select
    End Sub


#End Region

#Region "EnabledCommands"

    ''' <summary>Gets or sets bitmaxk value indicationg which commands in dropdown part of split button enabled/disabled by <see cref="IsSplitButtonVisible"/> are available</summary>
    ''' <remarks>Value of this property has no effect on command assigned to <see cref="SplitButtonCommand"/></remarks>
    Public Property EnabledCommands() As SaveMode
        Get
            Return GetValue(EnabledCommandsProperty)
        End Get

        Set(ByVal value As SaveMode)
            SetValue(EnabledCommandsProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="EnabledCommands"/> property</summary>
    Public Shared ReadOnly EnabledCommandsProperty As DependencyProperty = _
                           DependencyProperty.Register("EnabledCommands", _
                           GetType(SaveMode), GetType(CapEditor), _
                           New FrameworkPropertyMetadata(SaveMode.SaveAndClose Or SaveMode.SaveAndNext, _
                                                         AddressOf OnEnabledCommandsChanged))
    Private Shared Sub OnEnabledCommandsChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnEnabledCommandsChanged(e)
    End Sub
    Protected Overridable Sub OnEnabledCommandsChanged(ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        btnSave.IsEnabled = (EnabledCommands And SaveMode.SaveAndClose) = SaveMode.SaveAndClose
        mniNextNoSave.Visibility = If((EnabledCommands And SaveMode.NextNoSave) = SaveMode.NextNoSave, Visibility.Visible, Visibility.Collapsed)
        mniPreviousNoSave.Visibility = If((EnabledCommands And SaveMode.PreviousNoSave) = SaveMode.PreviousNoSave, Visibility.Visible, Visibility.Collapsed)
        mniReset.Visibility = If((EnabledCommands And SaveMode.Reset) = SaveMode.Reset, Visibility.Visible, Visibility.Collapsed)
        mniSaveAndNew.Visibility = If((EnabledCommands And SaveMode.SaveAndNew) = SaveMode.SaveAndNew, Visibility.Visible, Visibility.Collapsed)
        mniSaveAndNext.Visibility = If((EnabledCommands And SaveMode.SaveAndNext) = SaveMode.SaveAndNext, Visibility.Visible, Visibility.Collapsed)
        mniSaveAndNextNoClean.Visibility = If((EnabledCommands And SaveMode.SaveAndNextNoClean) = SaveMode.SaveAndNextNoClean, Visibility.Visible, Visibility.Collapsed)
        mniSaveAndPrevious.Visibility = If((EnabledCommands And SaveMode.SaveAndPrevious) = SaveMode.SaveAndPrevious, Visibility.Visible, Visibility.Collapsed)
    End Sub


#End Region


    Private txtTitleTextMatched As Boolean = True
    Private txtTopTextMatched As Boolean = True

#Region "Select / new / anonymous selection"
    Private Sub cmbCapType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbCapType.SelectionChanged
        If cmbCapType.SelectedItem IsNot Nothing Then
            With DirectCast(cmbCapType.SelectedItem, CapType)
                cmbMainType.SelectedItem = .MainType
                cmbShape.SelectedItem = .Shape
                nudSize1.Value = .Size
                If .Size2.HasValue Then nudSize2.Value = .Size2
                nudHeight.Value = .Height
                cmbMaterial.SelectedItem = .Material
                cmbTarget.SelectedItem = .Target
            End With
        End If
    End Sub

    Private Sub cmbProduct_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbProduct.SelectionChanged
        If cmbProduct.SelectedItem IsNot Nothing Then
            With DirectCast(cmbProduct.SelectedItem, Product)
                cmbProductType.SelectedItem = .ProductType
                cmbCompany.SelectedItem = .Company
            End With
        End If
    End Sub
#End Region

    Private Sub txtSideText_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtSideText.TextChanged
        If txtSideText.Text <> "" Then chkHasSide.IsChecked = True
    End Sub

    Private Sub txtBottomText_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtBottomText.TextChanged
        If txtBottomText.Text <> "" Then chkHasBottom.IsChecked = True
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSearch.Click

        Dim caps = Context.GetSimilarCaps(
            CapTypeID:=If(CapType Is Nothing, New Integer?, CapType.CapTypeID),
            MainTypeID:=If(CapMainType Is Nothing, New Integer?, CapMainType.MainTypeID),
            ShapeID:=If(CapShape Is Nothing, New Integer?, CapShape.ShapeID),
            CapName:=CapName,
            MainText:=MainText,
            SubTitle:=SubTitle,
            BackColor1:=CapBackgroundColor1.ToArgb,
            BackColor2:=If(CapBackgroundColor2.HasValue, CapBackgroundColor2.ToArgb, New Integer?),
            ForeColor:=If(CapForegroundColor1.HasValue, CapForegroundColor1.ToArgb, New Integer?),
            MainPicture:=MainPicture,
            TopText:=TopText,
            SideText:=SideText,
            BottomText:=BottomText,
            MaterialID:=If(Material Is Nothing, New Integer?, Material.MaterialID),
            Surface:=If(IsGlossy, "G"c, "M"c),
            Size:=If(Size1 = 0, New Integer?, Size1),
            Size2:=If((CapShape IsNot Nothing AndAlso CapShape.Size2Name Is Nothing) OrElse Size2 = 0, New Integer?, Size2),
            Height:=If(CapHeight = 0, New Integer?, CapHeight),
            Is3D:=Is3D,
            Year:=Year,
            CountryCode:=Country,
            Note:=CapNote,
            CompanyID:=If(CapCompany Is Nothing, New Integer?, CapCompany.CompanyID),
            ProductID:=If(Product Is Nothing, New Integer?, Product.ProductID),
            ProductTypeID:=If(CapProductType Is Nothing, New Integer?, CapProductType.ProductTypeID),
            StorageID:=If(Storage Is Nothing, New Integer?, Storage.StorageID),
            ForeColor2:=If(CapForegroundColor2.HasValue, CapForegroundColor2.Value.ToArgb, New Integer?),
            PictureType:=PictureType,
            HasBottom:=HasBottom,
            HasSide:=HasSide,
            AnotherPictures:=AnotherPictures,
            CategoryIDs:=(From cat In SelectedCategories Select cat.CategoryID).ToArray,
            Keywords:=Keywords.ToArray,
            CountryOfOrigin:=CountryOfOrigin,
            IsDrink:=IsDrink,
            State:=State,
            TargetID:=If(Target Is Nothing, New Integer?, Target.TargetID),
            IsAlcoholic:=IsAlcoholic,
            CapSignIDs:=(From sign In SelectedCapSigns Select sign.CapSignID).ToArray
        )


        Dim win As New winCapDetails(caps)
        win.Owner = Me.FindAncestor(Of Window)()
        win.Title = My.Resources.txt_SearchResults
        win.ShowDialog()
    End Sub

    Private Sub CapEditor_Unloaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        If OriginalContext IsNot Nothing Then OriginalContext.Dispose()
    End Sub

#Region "Cap properties"
#Region "CapName"
    ''' <summary>Gets or sets name of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapName() As String
        <DebuggerStepThrough()> Get
            Return GetValue(CapNameProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(CapNameProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapName"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapNameProperty As DependencyProperty = DependencyProperty.Register("CapName", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapNameChanged))
    ''' <summary>Called when value of the property <see cref="CapName"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapNameChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapNameChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapName"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapNameChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If CapName <> txtCapName.Text Then txtCapName.Text = CapName
    End Sub
    ''' <summary>Contains true when <see cref="txtCapName"/>.<see cref="TextBox.Drop">Drop</see> just occured</summary>
    Private txtCapName_JustDrop As Boolean = False
    Private Sub txtCapName_PreviewDrop(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs) Handles txtCapName.PreviewDrop
        txtCapName_JustDrop = True
    End Sub

    Private Sub txtCapName_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtCapName.TextChanged
        If CapName <> txtCapName.Text Then CapName = txtCapName.Text
        If (txtCapName.IsFocused OrElse txtCapName_JustDrop) AndAlso txtTitleTextMatched Then
            txtMainText.Text = txtCapName.Text
        End If
        txtTitleTextMatched = txtCapName.Text = txtMainText.Text
        txtCapName_JustDrop = False
    End Sub
#End Region
#Region "MainText"
    ''' <summary>Gets or sets main (title) text of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property MainText() As String
        <DebuggerStepThrough()> Get
            Return GetValue(MainTextProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(MainTextProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="MainText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly MainTextProperty As DependencyProperty = DependencyProperty.Register("MainText", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnMainTextChanged))
    ''' <summary>Called when value of the property <see cref="MainText"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnMainTextChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnMainTextChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="MainText"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnMainTextChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If MainText <> txtMainText.Text Then txtMainText.Text = MainText
    End Sub
    ''' <summary>Contains true when <see cref="txtMainText"/>.<see cref="TextBox.Drop">Drop</see> just occured</summary>
    Private txtMainText_JustDrop As Boolean = False
    Private Sub txtMainText_PreviewDrop(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs) Handles txtMainText.PreviewDrop
        txtMainText_JustDrop = True
    End Sub
    Private Sub txtMainText_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtMainText.TextChanged
        If MainText <> txtMainText.Text Then MainText = txtMainText.Text
        txtTitleTextMatched = txtCapName.Text = txtMainText.Text
        If txtTopTextMatched AndAlso (txtMainText.IsFocused OrElse txtCapName.IsFocused OrElse txtMainText_JustDrop OrElse txtCapName_JustDrop) Then
            txtTopText.Text = txtMainText.Text & If(txtSubTitle.Text <> "" AndAlso txtMainText.Text <> "", vbCrLf, "") & txtSubTitle.Text
        End If
        txtTopTextMatched = txtTopText.Text = txtMainText.Text & If(txtSubTitle.Text <> "" AndAlso txtMainText.Text <> "", vbCrLf, "") & txtSubTitle.Text
        txtMainText_JustDrop = False
    End Sub
#End Region
#Region "SubTitle"
    ''' <summary>Gets or sets name cap subtitle (2nd main) text</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property SubTitle() As String
        <DebuggerStepThrough()> Get
            Return GetValue(SubTitleProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(SubTitleProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="SubTitle"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly SubTitleProperty As DependencyProperty = DependencyProperty.Register("SubTitle", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnSubTitleChanged))
    ''' <summary>Called when value of the property <see cref="SubTitle"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnSubTitleChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnSubTitleChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="SubTitle"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSubTitleChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If SubTitle <> txtSubTitle.Text Then txtSubTitle.Text = SubTitle
    End Sub
    ''' <summary>Contains true when <see cref="txtSubTitle"/>.<see cref="TextBox.Drop">Drop</see> just occured</summary>
    Private txtSubTitle_JustDrop As Boolean
    Private Sub txtSubTitle_PreviewDrop(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs) Handles txtSubTitle.PreviewDrop
        txtSubTitle_JustDrop = True
    End Sub
    Private Sub txtSubTitle_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtSubTitle.TextChanged
        If SubTitle <> txtSubTitle.Text Then SubTitle = txtSubTitle.Text
        If txtTopTextMatched AndAlso (txtSubTitle.IsFocused OrElse txtSubTitle_JustDrop) Then
            txtTopText.Text = txtMainText.Text & If(txtSubTitle.Text <> "" AndAlso txtMainText.Text <> "", vbCrLf, "") & txtSubTitle.Text
        End If
        txtTopTextMatched = txtTopText.Text = txtMainText.Text & If(txtSubTitle.Text <> "" AndAlso txtMainText.Text <> "", vbCrLf, "") & txtSubTitle.Text
        txtSubTitle_JustDrop = False
    End Sub
#End Region
#Region "MainPicture"
    ''' <summary>Gets or sets description of main picture on cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property MainPicture() As String
        <DebuggerStepThrough()> Get
            Return GetValue(MainPictureProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(MainPictureProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="MainPicture"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly MainPictureProperty As DependencyProperty = DependencyProperty.Register("MainPicture", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnMainPictureChanged))
    ''' <summary>Called when value of the property <see cref="MainPicture"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnMainPictureChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnMainPictureChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="MainPicture"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnMainPictureChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If MainPicture <> txtMainPicture.Text Then txtMainPicture.Text = MainPicture
    End Sub
    Private Sub txtMainPicture_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtMainPicture.TextChanged
        If MainPicture <> txtMainPicture.Text Then MainPicture = txtMainPicture.Text
    End Sub
#End Region
#Region "AnotherPictures"
    ''' <summary>Gets or sets descriptions of another pictures on cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property AnotherPictures() As String
        <DebuggerStepThrough()> Get
            Return GetValue(AnotherPicturesProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(AnotherPicturesProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="AnotherPictures"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly AnotherPicturesProperty As DependencyProperty = DependencyProperty.Register("AnotherPictures", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnAnotherPicturesChanged))
    ''' <summary>Called when value of the property <see cref="AnotherPictures"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnAnotherPicturesChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnAnotherPicturesChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="AnotherPictures"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnAnotherPicturesChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If AnotherPictures <> txtAnotherPictures.Text Then txtAnotherPictures.Text = AnotherPictures
    End Sub
    Private Sub txtAnotherPictures_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtAnotherPictures.TextChanged
        If AnotherPictures <> txtAnotherPictures.Text Then AnotherPictures = txtAnotherPictures.Text
    End Sub
#End Region
#Region "PictureType"
    ''' <summary>Gets or sets type of picture (if any) on cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property PictureType() As Char?
        <DebuggerStepThrough()> Get
            Return GetValue(PictureTypeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Char?)
            SetValue(PictureTypeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="PictureType"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly PictureTypeProperty As DependencyProperty = DependencyProperty.Register("PictureType", GetType(Char?), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnPictureTypeChanged, AddressOf CoercePictureType))
    ''' <summary>Coerces value of the <see cref="PictureType"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><paramref name="baseValue"/> if value is OK (otherwise exception is thrown).</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null, <see cref="Char"/> nor <see cref="Nullable(Of Char)"/> of <see cref="Char"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is <see cref="Char"/> or non-null <see cref="Nullable(Of Char)"/> of <see cref="Char"/> but it is neither G, L, D nor P character.</exception>
    Private Shared Function CoercePictureType(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is Char AndAlso Not TypeOf baseValue Is Char? Then Throw New TypeMismatchException("baseValue", baseValue, GetType(Char?), My.Resources.ex_ValueOfPropertyMustBeNullOrXOrNullableOfX.f("PictureType", GetType(Char).Name, GetType(Nullable(Of )).Name))
        If baseValue IsNot Nothing Then
            Select Case If(TypeOf baseValue Is Char, DirectCast(baseValue, Char), DirectCast(baseValue, Char?).Value)
                Case "G"c, "L"c, "D"c, "P"c 'OK - do nothing
                Case Else : Throw New ArgumentException(My.Resources.ex_ValueOfPropertyMustBeOneOfFollowingValues.f("PictureType", New String() {"G", "L", "D", "P"}.Join(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator)))
            End Select
        End If
        Return baseValue
    End Function
    ''' <summary>Called when value of the property <see cref="PictureType"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnPictureTypeChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnPictureTypeChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="PictureType"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnPictureTypeChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If PictureType Is Nothing Then
            cmbPictureType.SelectedItem = cmiImageNo
        Else
            Select Case PictureType.Value
                Case "G"c : cmbPictureType.SelectedItem = cmiImageGeometry
                Case "L"c : cmbPictureType.SelectedItem = cmiImageLogo
                Case "D"c : cmbPictureType.SelectedItem = cmiImageDrawing
                Case "P"c : cmbPictureType.SelectedItem = cmiImagePhoto
            End Select
        End If
    End Sub
    Private Sub cmbPictureType_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbPictureType.SelectionChanged
        If cmbPictureType.SelectedItem Is cmiImageNo Then : PictureType = Nothing
        ElseIf cmbPictureType.SelectedItem Is cmiImageGeometry Then : PictureType = "G"c
        ElseIf cmbPictureType.SelectedItem Is cmiImageLogo Then : PictureType = "L"c
        ElseIf cmbPictureType.SelectedItem Is cmiImageDrawing Then : PictureType = "D"c
        ElseIf cmbPictureType.SelectedItem Is cmiImagePhoto Then : PictureType = "P"c
        End If
    End Sub
#End Region
#Region "CapTypeSelection"
    ''' <summary>Gets or sets way in which type of cap is set</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapTypeSelection() As CreatableItemSelection
        <DebuggerStepThrough()> Get
            Return GetValue(CapTypeSelectionProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As CreatableItemSelection)
            SetValue(CapTypeSelectionProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapTypeSelection"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapTypeSelectionProperty As DependencyProperty = DependencyProperty.Register("CapTypeSelection", GetType(CreatableItemSelection), GetType(CapEditor), New FrameworkPropertyMetadata(CreatableItemSelection.AnonymousItem, AddressOf OnCapTypeSelectionChanged, AddressOf CoerceCapTypeSelection))
    ''' <summary>Coereces value of the <see cref="CapTypeSelection"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><paramref name="baseValue"/> if value is OK (otherwise exception is thrown).</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither <see cref="CreatableItemSelection"/> nor <see cref="String"/></exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is <see cref="String"/> but it is not name of one of <see cref="CreatableItemSelection"/> members</exception>
    ''' <exception cref="InvalidEnumArgumentException"><paramref name="baseValue"/> is <see cref="CreatableItemSelection"/> but it is not one of <see cref="CreatableItemSelection"/> enumerated constants.</exception>
    Private Shared Function CoerceCapTypeSelection(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If TypeOf baseValue Is String Then
            Return [Enum].Parse(GetType(CreatableItemSelection), baseValue)
        ElseIf TypeOf baseValue Is CreatableItemSelection Then
            If Not DirectCast(baseValue, CreatableItemSelection).IsDefined Then Throw New InvalidEnumArgumentException("baseValue", baseValue, GetType(CreatableItemSelection))
        Else : Throw New TypeMismatchException("baseValue", baseValue, GetType(CreatableItemSelection), "Property {0} can be set only by types {1} and {2}.".f("CapTypeSelection", GetType(CreatableItemSelection), GetType(String)))
        End If
        Return baseValue
    End Function
    ''' <summary>Called when value of the property <see cref="CapTypeSelection"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapTypeSelectionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapTypeSelectionChanged(e)
    End Sub
    ''' <summary>True when <see cref="OnCapNameChanged"/> or <see cref="optCapType_Checked"/> is currently on callstach and thus should not proceed again</summary>
    Private Setting_CapTypeSelection As Boolean = False
    ''' <summary>Called when value of the <see cref="CapTypeSelection"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapTypeSelectionChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If Setting_CapTypeSelection Then Exit Sub
        Setting_CapTypeSelection = True
        Try
            Select Case CapTypeSelection
                Case CreatableItemSelection.NewItem : optCapTypeNew.IsChecked = True
                Case CreatableItemSelection.SelectedItem : optCapTypeSelect.IsChecked = True
                Case Else : optCapTypeAnonymous.IsChecked = True
            End Select
        Finally
            Setting_CapTypeSelection = False
        End Try
    End Sub
    Private Sub optCapType_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles optCapTypeAnonymous.Checked, optCapTypeNew.Checked, optCapTypeSelect.Checked
        If Setting_CapTypeSelection Then Exit Sub
        Setting_CapTypeSelection = True
        Try
            If optCapTypeSelect.IsChecked Then : CapTypeSelection = CreatableItemSelection.SelectedItem
            ElseIf optCapTypeNew.IsChecked Then : CapTypeSelection = CreatableItemSelection.NewItem
            ElseIf optCapTypeAnonymous.IsChecked Then : CapTypeSelection = CreatableItemSelection.AnonymousItem
            End If
        Finally
            Setting_CapTypeSelection = False
        End Try
    End Sub
#End Region
#Region "CapType"
    ''' <summary>Gets or sets sepected cap type. Valid when <see cref="CapTypeSelection"/> is <see cref="CreatableItemSelection.SelectedItem"/></summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapType() As CapType
        <DebuggerStepThrough()> Get
            Return GetValue(CapTypeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As CapType)
            SetValue(CapTypeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapType"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapTypeProperty As DependencyProperty = DependencyProperty.Register("CapType", GetType(CapType), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapTypeChanged, AddressOf CoerceCapType))
    ''' <summary>Coerces value of the <see cref="CapType"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapType"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="CapType"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="CapType.CapTypeID"/> in combobox</exception>
    Private Shared Function CoerceCapType(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is CapType Then Throw New TypeMismatchException("baseValue", baseValue, GetType(CapType))
        Return DirectCast(d, CapEditor).CoerceCapType(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="CapType"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapType"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="CapType.CapTypeID"/> in combobox</exception>
    Protected Overridable Function CoerceCapType(ByVal baseValue As CapType) As CapType
        If Not initialized Then Initialize(False)
        If baseValue Is Nothing Then cmbCapType.SelectedIndex = -1 : Return Nothing
        For Each item As CapType In cmbCapType.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As CapType In cmbCapType.Items
            If item.CapTypeID = baseValue.CapTypeID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownCapType)
    End Function
    ''' <summary>Called when value of the property <see cref="CapType"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapTypeChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapTypeChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapType"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapTypeChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbCapType.SelectedItem = CapType
        If CapType Is Nothing Then optCapTypeAnonymous.IsChecked = True Else optCapTypeSelect.IsChecked = True
    End Sub
    Private Sub cmbCapType_SelectionChanged2(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbCapType.SelectionChanged
        CapType = cmbCapType.SelectedItem
    End Sub
#End Region
#Region "CapTypeName"
    ''' <summary>Gets or sets name of cap type when <see cref="CapTypeSelection"/> is <see cref="CreatableItemSelection.NewItem"/></summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapTypeName() As String
        <DebuggerStepThrough()> Get
            Return GetValue(CapTypeNameProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(CapTypeNameProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapTypeName"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapTypeNameProperty As DependencyProperty = DependencyProperty.Register("CapTypeName", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapTypeNameChanged))
    ''' <summary>Called when value of the property <see cref="CapTypeName"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapTypeNameChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapTypeNameChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapTypeName"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapTypeNameChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If CapTypeName <> txtCapTypeName.Text Then txtCapTypeName.Text = CapTypeName
    End Sub
    Private Sub txtCapTypeName_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtCapTypeName.TextChanged
        If CapTypeName <> txtCapTypeName.Text Then CapTypeName = txtCapTypeName.Text
    End Sub
#End Region
#Region "CapTypeDescription"
    ''' <summary>Gets or sets description of cap type when <see cref="CapTypeSelection"/> is <see cref="CreatableItemSelection.NewItem"/></summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapTypeDescription() As String
        <DebuggerStepThrough()> Get
            Return GetValue(CapTypeDescriptionProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(CapTypeDescriptionProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapTypeDescription"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapTypeDescriptionProperty As DependencyProperty = DependencyProperty.Register("CapTypeDescription", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapTypeDescriptionChanged))
    ''' <summary>Called when value of the property <see cref="CapTypeDescription"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapTypeDescriptionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapTypeDescriptionChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapTypeDescription"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapTypeDescriptionChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If CapTypeDescription <> txtCapTypeDesc.Text Then txtCapTypeDesc.Text = CapTypeDescription
    End Sub
    Private Sub txtCapTypeDescription_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtCapTypeDesc.TextChanged
        If CapTypeDescription <> txtCapTypeDesc.Text Then CapTypeDescription = txtCapTypeDesc.Text
    End Sub
#End Region
#Region "CapTypeImagePath"
    ''' <summary>Gets or sets path to image of cap type when <see cref="CapTypeSelection"/> is <see cref="CreatableItemSelection.NewItem"/></summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapTypeImagePath() As String
        <DebuggerStepThrough()> Get
            Return GetValue(CapTypeImagePathProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(CapTypeImagePathProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapTypeImagePath"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapTypeImagePathProperty As DependencyProperty = DependencyProperty.Register("CapTypeImagePath", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapTypeImagePathChanged))
    ''' <summary>Called when value of the property <see cref="CapTypeImagePath"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapTypeImagePathChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapTypeImagePathChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapTypeImagePath"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapTypeImagePathChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If CapTypeImagePath <> txtCapTypeImagePath.Text Then txtCapTypeImagePath.Text = CapTypeImagePath
    End Sub
    Private Sub txtCapTypeImagePath_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtCapTypeImagePath.TextChanged
        If CapTypeImagePath <> txtCapTypeImagePath.Text Then CapTypeImagePath = txtCapTypeImagePath.Text
    End Sub
#End Region
#Region "CapMainType"
    ''' <summary>Gets or sets main cap type.</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapMainType() As MainType
        <DebuggerStepThrough()> Get
            Return GetValue(CapMainTypeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As MainType)
            SetValue(CapMainTypeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapMainType"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapMainTypeProperty As DependencyProperty = DependencyProperty.Register("CapMainType", GetType(MainType), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapMainTypeChanged, AddressOf CoerceCapMainType))
    ''' <summary>Coerces value of the <see cref="CapMainType"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapMainType"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="CapMainType"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="MainType.MainTypeID"/> in combobox</exception>
    Private Shared Function CoerceCapMainType(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is MainType Then Throw New TypeMismatchException("baseValue", baseValue, GetType(MainType))
        Return DirectCast(d, CapEditor).CoerceCapMainType(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="CapMainType"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapMainType"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="MainType.MainTypeID"/> in combobox</exception>
    Protected Overridable Function CoerceCapMainType(ByVal baseValue As MainType) As MainType
        If baseValue Is Nothing Then cmbMainType.SelectedIndex = -1 : Return Nothing
        For Each item As MainType In cmbMainType.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As MainType In cmbMainType.Items
            If item.MainTypeID = baseValue.MainTypeID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownCapMainType)
    End Function
    ''' <summary>Called when value of the property <see cref="CapMainType"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapMainTypeChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapMainTypeChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapMainType"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapMainTypeChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbMainType.SelectedItem = CapMainType
    End Sub
    Private Sub cmbCapMainType_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbMainType.SelectionChanged
        CapMainType = cmbMainType.SelectedItem
    End Sub
#End Region
#Region "CapShape"
    ''' <summary>Gets or sets cap shape.</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapShape() As Shape
        <DebuggerStepThrough()> Get
            Return GetValue(CapShapeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Shape)
            SetValue(CapShapeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapShape"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapShapeProperty As DependencyProperty = DependencyProperty.Register("CapShape", GetType(Shape), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapShapeChanged, AddressOf CoerceCapShape))
    ''' <summary>Coerces value of the <see cref="CapShape"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapShape"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="CapShape"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Shape.ShapeID"/> in combobox</exception>
    Private Shared Function CoerceCapShape(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is Shape Then Throw New TypeMismatchException("baseValue", baseValue, GetType(Shape))
        Return DirectCast(d, CapEditor).CoerceCapShape(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="CapShape"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapShape"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Shape.ShapeID"/> in combobox</exception>
    Protected Overridable Function CoerceCapShape(ByVal baseValue As Shape) As Shape
        If baseValue Is Nothing Then cmbShape.SelectedIndex = -1 : Return Nothing
        For Each item As Shape In cmbShape.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As Shape In cmbShape.Items
            If item.ShapeID = baseValue.ShapeID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownShape)
    End Function
    ''' <summary>Called when value of the property <see cref="CapShape"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapShapeChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapShapeChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapShape"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapShapeChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbShape.SelectedItem = CapShape
    End Sub
    Private Sub cmbCapShape_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbShape.SelectionChanged
        CapShape = cmbShape.SelectedItem
    End Sub

#End Region
#Region "Size1"
    ''' <summary>Gets or sets cap size (i.e. diameter or width)</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Size1() As Integer
        <DebuggerStepThrough()> Get
            Return GetValue(Size1Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer)
            SetValue(Size1Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Size1"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly Size1Property As DependencyProperty = DependencyProperty.Register("Size1", GetType(Integer), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnSize1Changed))
    ''' <summary>Called when value of the property <see cref="Size1"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnSize1Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnSize1Changed(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Size1"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSize1Changed(ByVal e As DependencyPropertyChangedEventArgs)
        If Size1 <> nudSize1.Value Then nudSize1.Value = Size1
    End Sub
    Private Sub txtSize1_TextChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Decimal)) Handles nudSize1.ValueChanged
        If Size1 <> nudSize1.Value Then Size1 = nudSize1.Value
    End Sub
#End Region
#Region "Size2"
    ''' <summary>Gets or sets caps size 2 (i.e. y-width)</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Size2() As Integer
        <DebuggerStepThrough()> Get
            Return GetValue(Size2Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer)
            SetValue(Size2Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Size2"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly Size2Property As DependencyProperty = DependencyProperty.Register("Size2", GetType(Integer), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnSize2Changed))
    ''' <summary>Called when value of the property <see cref="Size2"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnSize2Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnSize2Changed(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Size2"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSize2Changed(ByVal e As DependencyPropertyChangedEventArgs)
        If Size2 <> nudSize2.Value Then nudSize2.Value = Size2
    End Sub
    Private Sub txtSize2_TextChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Decimal)) Handles nudSize2.ValueChanged
        If Size2 <> nudSize2.Value Then Size2 = nudSize2.Value
    End Sub
#End Region
#Region "State"
    ''' <summary>Gets or sets cap State (i.e. diameter or width)</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property State() As Short
        <DebuggerStepThrough()> Get
            Return GetValue(StateProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Short)
            SetValue(StateProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="State"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly StateProperty As DependencyProperty = DependencyProperty.Register("State", GetType(Short), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnStateChanged, AddressOf CoerceStateValue))
    ''' <summary>value of the <see cref="State"/> property"</summary>
    ''' <param name="d">Instance of <see cref="CapEditor"/> to coerce value for</param>
    ''' <param name="value">Proposed new value of the property</param>
    ''' <returns><paramref name="value"/> <see cref="TypeTools.DynamicCast">dynamicly casted</see> to <see cref="Short"/>; null when <paramref name="value"/> is null</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not of type <see cref="CapEditor"/></exception>
    ''' <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than 1 or greater than 5</exception>
    ''' <exception cref="InvalidCastException">No casting method from type of <paramref name="value"/> to <see cref="Short"/> was found -or- build in conversion from <see cref="System.String"/> to <see cref="Short"/> failed.</exception>
    ''' <exception cref="Reflection.AmbiguousMatchException">Cast operators were found, but no one is most specific.</exception>
    ''' <exception cref="OverflowException">Build in conversion to <see cref="Short"/> failed because <paramref name="value"/> cannot be represented in type <see cref="Short"/> -or- Called cast operator have thrown this exception.</exception>
    Private Shared Function CoerceStateValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If value Is Nothing Then Return Nothing
        Dim val = TypeTools.DynamicCast(Of Short)(value)
        If val < 1 Then Return 1S
        If val > 5 Then Return 5S
        Return val
    End Function
    ''' <summary>Called when value of the property <see cref="State"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnStateChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnStateChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="State"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnStateChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If State <> nudCapState.Value Then nudCapState.Value = State
    End Sub
    Private Sub nudState_ValueChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Decimal)) Handles nudCapState.ValueChanged
        If State <> nudCapState.Value Then State = nudCapState.Value
    End Sub
#End Region
#Region "CapHeight"
    ''' <summary>Gets or sets cap height in mms</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapHeight() As Integer
        <DebuggerStepThrough()> Get
            Return GetValue(CapHeightProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer)
            SetValue(CapHeightProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapHeight"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapHeightProperty As DependencyProperty = DependencyProperty.Register("CapHeight", GetType(Integer), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapHeightChanged))
    ''' <summary>Called when value of the property <see cref="CapHeight"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapHeightChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapHeightChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapHeight"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapHeightChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If CapHeight <> nudHeight.Value Then nudHeight.Value = CapHeight
    End Sub
    Private Sub txtCapHeight_TextChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Decimal)) Handles nudHeight.ValueChanged
        If CapHeight <> nudHeight.Value Then CapHeight = nudHeight.Value
    End Sub
#End Region
#Region "Material"
    ''' <summary>Gets or sets cap material.</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Material() As Material
        <DebuggerStepThrough()> Get
            Return GetValue(MaterialProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Material)
            SetValue(MaterialProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Material"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly MaterialProperty As DependencyProperty = DependencyProperty.Register("Material", GetType(Material), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnMaterialChanged, AddressOf CoerceMaterial))
    ''' <summary>Coerces value of the <see cref="Material"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Material"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="Material"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Material.MaterialID"/> in combobox</exception>
    Private Shared Function CoerceMaterial(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is Material Then Throw New TypeMismatchException("baseValue", baseValue, GetType(Material))
        Return DirectCast(d, CapEditor).CoerceMaterial(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="Material"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Material"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Material.MaterialID"/> in combobox</exception>
    Protected Overridable Function CoerceMaterial(ByVal baseValue As Material) As Material
        If baseValue Is Nothing Then cmbMaterial.SelectedIndex = -1 : Return Nothing
        For Each item As Material In cmbMaterial.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As Material In cmbMaterial.Items
            If item.MaterialID = baseValue.MaterialID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownMaterial)
    End Function
    ''' <summary>Called when value of the property <see cref="Material"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnMaterialChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnMaterialChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Material"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnMaterialChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbMaterial.SelectedItem = Material
    End Sub
    Private Sub cmbMaterial_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbMaterial.SelectionChanged
        Material = cmbMaterial.SelectedItem
    End Sub
#End Region
#Region "Target"
    ''' <summary>Gets or sets cap Target.</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Target() As Target
        <DebuggerStepThrough()> Get
            Return GetValue(TargetProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Target)
            SetValue(TargetProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Target"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly TargetProperty As DependencyProperty = DependencyProperty.Register("Target", GetType(Target), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnTargetChanged, AddressOf CoerceTarget))
    ''' <summary>Coerces value of the <see cref="Target"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Target"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="Target"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Target.TargetID"/> in combobox</exception>
    Private Shared Function CoerceTarget(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is Target Then Throw New TypeMismatchException("baseValue", baseValue, GetType(Target))
        Return DirectCast(d, CapEditor).CoerceTarget(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="Target"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Target"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Target.TargetID"/> in combobox</exception>
    Protected Overridable Function CoerceTarget(ByVal baseValue As Target) As Target
        If baseValue Is Nothing Then cmbTarget.SelectedIndex = -1 : Return Nothing
        For Each item As Target In cmbTarget.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As Target In cmbTarget.Items
            If item.TargetID = baseValue.TargetID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownTarget)
    End Function
    ''' <summary>Called when value of the property <see cref="Target"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnTargetChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnTargetChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Target"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnTargetChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbTarget.SelectedItem = Target
    End Sub
    Private Sub cmbTarget_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbTarget.SelectionChanged
        Target = cmbTarget.SelectedItem
    End Sub
#End Region
#Region "CapBackgroundColor1"
    ''' <summary>Gets or sets cap primary background color</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapBackgroundColor1() As Color
        <DebuggerStepThrough()> Get
            Return GetValue(CapBackgroundColor1Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Color)
            SetValue(CapBackgroundColor1Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapBackgroundColor1"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapBackgroundColor1Property As DependencyProperty = DependencyProperty.Register("CapBackgroundColor1", GetType(Color), GetType(CapEditor), New FrameworkPropertyMetadata(Colors.Transparent, AddressOf OnCapBackgroundColor1Changed))
    ''' <summary>Called when value of the property <see cref="CapBackgroundColor1"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapBackgroundColor1Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapBackgroundColor1Changed(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapBackgroundColor1"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapBackgroundColor1Changed(ByVal e As DependencyPropertyChangedEventArgs)
        If If(CapBackgroundColor1 <> copBackground.Color, True) Then copBackground.Color = CapBackgroundColor1
    End Sub
    Private Sub copCapBackgroundColor1_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles copBackground.ColorChanged
        If If(CapBackgroundColor1 <> copBackground.Color, True) Then CapBackgroundColor1 = copBackground.Color
    End Sub
#End Region
#Region "CapBackgroundColor2"
    ''' <summary>Gets or sets cap secondary background</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapBackgroundColor2() As Color?
        <DebuggerStepThrough()> Get
            Return GetValue(CapBackgroundColor2Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Color?)
            SetValue(CapBackgroundColor2Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapBackgroundColor2"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapBackgroundColor2Property As DependencyProperty = DependencyProperty.Register("CapBackgroundColor2", GetType(Color?), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapBackgroundColor2Changed))
    ''' <summary>Called when value of the property <see cref="CapBackgroundColor2"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapBackgroundColor2Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapBackgroundColor2Changed(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapBackgroundColor2"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapBackgroundColor2Changed(ByVal e As DependencyPropertyChangedEventArgs)
        If If(CapBackgroundColor2 <> copSecondaryBackground.Color, True) Then copSecondaryBackground.Color = CapBackgroundColor2
    End Sub
    Private Sub copCapBackgroundColor2_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles copSecondaryBackground.ColorChanged
        If If(CapBackgroundColor2 <> copSecondaryBackground.Color, True) Then CapBackgroundColor2 = copSecondaryBackground.Color
    End Sub
#End Region
#Region "CapForegroundColor1"
    ''' <summary>Gets or sets cap primary foreground color</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapForegroundColor1() As Color?
        <DebuggerStepThrough()> Get
            Return GetValue(CapForegroundColor1Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Color?)
            SetValue(CapForegroundColor1Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapForegroundColor1"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapForegroundColor1Property As DependencyProperty = DependencyProperty.Register("CapForegroundColor1", GetType(Color?), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapForegroundColor1Changed))
    ''' <summary>Called when value of the property <see cref="CapForegroundColor1"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapForegroundColor1Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapForegroundColor1Changed(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapForegroundColor1"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapForegroundColor1Changed(ByVal e As DependencyPropertyChangedEventArgs)
        If If(CapForegroundColor1 <> copForeground.Color, True) Then copForeground.Color = CapForegroundColor1
    End Sub
    Private Sub copCapForegroundColor1_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles copForeground.ColorChanged
        If If(CapForegroundColor1 <> copForeground.Color, True) Then CapForegroundColor1 = copForeground.Color
    End Sub
#End Region
#Region "CapForegroundColor2"
    ''' <summary>Gets or sets cap secondary foreground color</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapForegroundColor2() As Color?
        <DebuggerStepThrough()> Get
            Return GetValue(CapForegroundColor2Property)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Color?)
            SetValue(CapForegroundColor2Property, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapForegroundColor2"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapForegroundColor2Property As DependencyProperty = DependencyProperty.Register("CapForegroundColor2", GetType(Color?), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapForegroundColor2Changed))
    ''' <summary>Called when value of the property <see cref="CapForegroundColor2"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapForegroundColor2Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapForegroundColor2Changed(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapForegroundColor2"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapForegroundColor2Changed(ByVal e As DependencyPropertyChangedEventArgs)
        If If(CapForegroundColor2 <> copForeground2.Color, True) Then copForeground2.Color = CapForegroundColor2
    End Sub
    Private Sub copCapForegroundColor2_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles copForeground2.ColorChanged
        If If(CapForegroundColor2 <> copForeground2.Color, True) Then CapForegroundColor2 = copForeground2.Color
    End Sub
#End Region
#Region "IS3D"
    ''' <summary>Gets or sets valkue indicating if cap has 3D surface</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Is3D() As Boolean
        <DebuggerStepThrough()> Get
            Return GetValue(Is3DProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Boolean)
            SetValue(Is3DProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Is3D"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly Is3DProperty As DependencyProperty = DependencyProperty.Register("Is3D", GetType(Boolean), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnIs3DChanged))
    ''' <summary>Called when value of the property <see cref="Is3D"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnIs3DChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnIs3DChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Is3D"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnIs3DChanged(ByVal e As DependencyPropertyChangedEventArgs)
        chk3D.IsChecked = Is3D
    End Sub
    Private Sub txtIs3D_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles chk3D.Checked, chk3D.Unchecked
        Is3D = chk3D.IsChecked
    End Sub
#End Region
#Region "IsDrink"
    ''' <summary>Gets or sets valkue indicating if product is drink</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property IsDrink() As Boolean?
        <DebuggerStepThrough()> Get
            Return GetValue(IsDrinkProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Boolean?)
            SetValue(IsDrinkProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="IsDrink"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly IsDrinkProperty As DependencyProperty = DependencyProperty.Register("IsDrink", GetType(Boolean?), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnIsDrinkChanged))
    ''' <summary>Called when value of the property <see cref="IsDrink"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnIsDrinkChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnIsDrinkChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="IsDrink"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnIsDrinkChanged(ByVal e As DependencyPropertyChangedEventArgs)
        chkIsDrink.IsChecked = IsDrink
    End Sub
    Private Sub chkIsDrink_CheckedChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles chkIsDrink.Checked, chkIsDrink.Unchecked, chkIsDrink.Indeterminate
        IsDrink = chkIsDrink.IsChecked
        If UnderConstruction Then Exit Sub
        If chkIsDrink.IsChecked.HasValue AndAlso chkIsDrink.IsChecked.Value Then
            chkIsAlcoholic.IsEnabled = True
        ElseIf chkIsDrink.IsChecked.HasValue AndAlso Not chkIsDrink.IsChecked.Value Then
            chkIsAlcoholic.IsEnabled = False
            chkIsAlcoholic.IsChecked = False
        Else
            chkIsAlcoholic.IsEnabled = False
            chkIsAlcoholic.IsChecked = New Boolean?
        End If
    End Sub
#End Region
#Region "IsAlcoholic"
    ''' <summary>Gets or sets valkue indicating if product is alcoholic drink</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property IsAlcoholic() As Boolean?
        <DebuggerStepThrough()> Get
            Return GetValue(IsAlcoholicProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Boolean?)
            SetValue(IsAlcoholicProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="IsAlcoholic"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly IsAlcoholicProperty As DependencyProperty = DependencyProperty.Register("IsAlcoholic", GetType(Boolean?), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnIsAlcoholicChanged))
    ''' <summary>Called when value of the property <see cref="IsAlcoholic"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnIsAlcoholicChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnIsAlcoholicChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="IsAlcoholic"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnIsAlcoholicChanged(ByVal e As DependencyPropertyChangedEventArgs)
        chkIsAlcoholic.IsChecked = IsAlcoholic
    End Sub
    Private Sub chkIsAlcoholic_CheckedChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles chkIsAlcoholic.Checked, chkIsAlcoholic.Unchecked, chkIsAlcoholic.Indeterminate
        IsAlcoholic = chkIsAlcoholic.IsChecked
    End Sub
#End Region
#Region "IsGlossy"
    ''' <summary>Gets or sets value indicating if cap has glossy or matting surface</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property IsGlossy() As Boolean
        <DebuggerStepThrough()> Get
            Return GetValue(IsGlossyProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Boolean)
            SetValue(IsGlossyProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="IsGlossy"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly IsGlossyProperty As DependencyProperty = DependencyProperty.Register("IsGlossy", GetType(Boolean), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnIsGlossyChanged))
    ''' <summary>Called when value of the property <see cref="IsGlossy"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnIsGlossyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnIsGlossyChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="IsGlossy"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnIsGlossyChanged(ByVal e As DependencyPropertyChangedEventArgs)
        optGlossy.IsChecked = IsGlossy
        optMatting.IsChecked = Not IsGlossy
    End Sub
    Private Sub txtIsGlossy_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles optGlossy.Checked, optGlossy.Unchecked
        IsGlossy = optGlossy.IsChecked
    End Sub
#End Region
#Region "TopText"
    ''' <summary>Gets or sets text of top side of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property TopText() As String
        <DebuggerStepThrough()> Get
            Return GetValue(TopTextProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(TopTextProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="TopText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly TopTextProperty As DependencyProperty = DependencyProperty.Register("TopText", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnTopTextChanged))
    ''' <summary>Called when value of the property <see cref="TopText"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnTopTextChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnTopTextChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="TopText"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnTopTextChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If TopText <> txtTopText.Text Then txtTopText.Text = TopText
    End Sub
    Private Sub txtTopText_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtTopText.TextChanged
        If TopText <> txtTopText.Text Then TopText = txtTopText.Text
        txtTopTextMatched = txtTopText.Text = txtMainText.Text & If(txtSubTitle.Text <> "" AndAlso txtMainText.Text <> "", vbCrLf, "") & txtSubTitle.Text
    End Sub
#End Region
#Region "SideText"
    ''' <summary>Gets or sets text at side of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property SideText() As String
        <DebuggerStepThrough()> Get
            Return GetValue(SideTextProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(SideTextProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="SideText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly SideTextProperty As DependencyProperty = DependencyProperty.Register("SideText", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnSideTextChanged))
    ''' <summary>Called when value of the property <see cref="SideText"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnSideTextChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnSideTextChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="SideText"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSideTextChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If SideText <> txtSideText.Text Then txtSideText.Text = SideText
    End Sub
    Private Sub txtSideText_TextChanged2(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtSideText.TextChanged
        If SideText <> txtSideText.Text Then SideText = txtSideText.Text
    End Sub
#End Region
#Region "BottomText"
    ''' <summary>Gets or sets text at bottom side of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property BottomText() As String
        <DebuggerStepThrough()> Get
            Return GetValue(BottomTextProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(BottomTextProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="BottomText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly BottomTextProperty As DependencyProperty = DependencyProperty.Register("BottomText", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnBottomTextChanged))
    ''' <summary>Called when value of the property <see cref="BottomText"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnBottomTextChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnBottomTextChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="BottomText"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnBottomTextChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If BottomText <> txtBottomText.Text Then txtBottomText.Text = BottomText
    End Sub
    Private Sub txtBottomText_TextChanged2(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtBottomText.TextChanged
        If BottomText <> txtBottomText.Text Then BottomText = txtBottomText.Text
    End Sub
#End Region
#Region "CapNote"
    ''' <summary>Gets or sets name of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapNote() As String
        <DebuggerStepThrough()> Get
            Return GetValue(CapNoteProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(CapNoteProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapNote"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapNoteProperty As DependencyProperty = DependencyProperty.Register("CapNote", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapNoteChanged))
    ''' <summary>Called when value of the property <see cref="CapNote"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapNoteChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapNoteChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapNote"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapNoteChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If CapNote <> txtNote.Text Then txtNote.Text = CapNote
    End Sub
    Private Sub txtNote_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtNote.TextChanged
        If CapNote <> txtNote.Text Then CapNote = txtNote.Text
    End Sub
#End Region
#Region "CapID"
    ''' <summary>Gets or sets ID of displayed cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapID() As Integer
        <DebuggerStepThrough()> Get
            Return GetValue(CapIDProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer)
            SetValue(CapIDProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapID"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapIDProperty As DependencyProperty = DependencyProperty.Register("CapID", GetType(Integer), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapIDChanged))
    ''' <summary>Called when value of the property <see cref="CapID"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapIDChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapIDChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapID"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapIDChanged(ByVal e As DependencyPropertyChangedEventArgs)
        lblID.Content = CapID.ToString
    End Sub
#End Region
#Region "Year"
    ''' <summary>Gets or sets year when cap was found</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Year() As Integer?
        <DebuggerStepThrough()> Get
            Return GetValue(YearProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Integer?)
            SetValue(YearProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Year"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly YearProperty As DependencyProperty = DependencyProperty.Register("Year", GetType(Integer?), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnYearChanged))
    ''' <summary>Called when value of the property <see cref="Year"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnYearChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnYearChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Year"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnYearChanged(ByVal e As DependencyPropertyChangedEventArgs)
        Dim yr As Integer = If(Year, 0)
        If If(Year, 0) <> nudYear.Value Then nudYear.Value = If(Year, 0)
    End Sub
    Private Sub txtYear_TextChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Decimal)) Handles nudYear.ValueChanged
        If If(Year, 0) <> nudYear.Value Then Year = If(nudYear.Value = 0, New Integer?, CInt(nudYear.Value))
    End Sub
#End Region
#Region "Country"
    ''' <summary>Gets or sets name of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Country() As String
        <DebuggerStepThrough()> Get
            Return GetValue(CountryProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(CountryProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Country"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CountryProperty As DependencyProperty = DependencyProperty.Register("Country", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCountryChanged, AddressOf CoerceCountryCode))
    ''' <summary>Coerces value of the <see cref="Country"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="BaseValueSource"/> is it is OK, otherwise an exception is throen</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> isnot <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither <see cref="String"/> not null.</exception>
    Private Shared Function CoerceCountryCode(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is String Then Throw New TypeMismatchException("baseValue", baseValue, GetType(String))
        'If baseValue IsNot Nothing AndAlso DirectCast(baseValue, String).Length <> 3 AndAlso DirectCast(baseValue, String) <> "" Then Throw New ArgumentException(My.Resources.ex_CountryCode3)
        Return baseValue
    End Function
    ''' <summary>Called when value of the property <see cref="Country"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCountryChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCountryChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Country"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCountryChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If Country <> txtCountryCode.Text Then txtCountryCode.Text = Country
    End Sub
    Private Sub txtCountry_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtCountryCode.TextChanged
        If Country <> txtCountryCode.Text Then Country = txtCountryCode.Text
    End Sub
#End Region
#Region "CountryOfOrigin"
    ''' <summary>Gets or sets name of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CountryOfOrigin() As String
        <DebuggerStepThrough()> Get
            Return GetValue(CountryOfOriginProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(CountryOfOriginProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CountryOfOrigin"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CountryOfOriginProperty As DependencyProperty = DependencyProperty.Register("CountryOfOrigin", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCountryOfOriginChanged, AddressOf CoerceCountryOfOrigin))
    ''' <summary>Coerces value of the <see cref="CountryOfOrigin"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="BaseValueSource"/> is it is OK, otherwise an exception is throen</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> isnot <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither <see cref="String"/> not null.</exception>
    Private Shared Function CoerceCountryOfOrigin(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is String Then Throw New TypeMismatchException("baseValue", baseValue, GetType(String))
        'If baseValue IsNot Nothing AndAlso DirectCast(baseValue, String).Length <> 3 AndAlso DirectCast(baseValue, String) <> "" Then Throw New ArgumentException(My.Resources.ex_CountryOfOrigin3)
        Return baseValue
    End Function
    ''' <summary>Called when value of the property <see cref="CountryOfOrigin"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCountryOfOriginChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCountryOfOriginChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CountryOfOrigin"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCountryOfOriginChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If CountryOfOrigin <> txtCountryOfOrigin.Text Then txtCountryOfOrigin.Text = CountryOfOrigin
    End Sub
    Private Sub txtCountryOfOrigin_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtCountryOfOrigin.TextChanged
        If CountryOfOrigin <> txtCountryOfOrigin.Text Then CountryOfOrigin = txtCountryOfOrigin.Text
    End Sub
#End Region
#Region "Storage"
    ''' <summary>Gets or sets cap Storage.</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Storage() As Storage
        <DebuggerStepThrough()> Get
            Return GetValue(StorageProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Storage)
            SetValue(StorageProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Storage"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly StorageProperty As DependencyProperty = DependencyProperty.Register("Storage", GetType(Storage), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnStorageChanged, AddressOf CoerceStorage))
    ''' <summary>Coerces value of the <see cref="Storage"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Storage"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="Storage"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Storage.StorageID"/> in combobox</exception>
    Private Shared Function CoerceStorage(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is Storage Then Throw New TypeMismatchException("baseValue", baseValue, GetType(Storage))
        Return DirectCast(d, CapEditor).CoerceStorage(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="Storage"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Storage"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Storage.StorageID"/> in combobox</exception>
    Protected Overridable Function CoerceStorage(ByVal baseValue As Storage) As Storage
        If baseValue Is Nothing Then cmbStorage.SelectedIndex = -1 : Return Nothing
        For Each item As Storage In cmbStorage.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As Storage In cmbStorage.Items
            If item.StorageID = baseValue.StorageID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownStorage)
    End Function
    ''' <summary>Called when value of the property <see cref="Storage"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnStorageChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnStorageChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Storage"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnStorageChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbStorage.SelectedItem = Storage
    End Sub
    Private Sub cmbStorage_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbStorage.SelectionChanged
        Storage = cmbStorage.SelectedItem
    End Sub
#End Region
#Region "HasBottom"
    ''' <summary>Gets or sets valkue indicating if cap has somethign interesting at bottom side</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property HasBottom() As Boolean
        <DebuggerStepThrough()> Get
            Return GetValue(HasBottomProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Boolean)
            SetValue(HasBottomProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="HasBottom"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly HasBottomProperty As DependencyProperty = DependencyProperty.Register("HasBottom", GetType(Boolean), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnHasBottomChanged))
    ''' <summary>Called when value of the property <see cref="HasBottom"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnHasBottomChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnHasBottomChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="HasBottom"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnHasBottomChanged(ByVal e As DependencyPropertyChangedEventArgs)
        chkHasBottom.IsChecked = HasBottom
    End Sub
    Private Sub txtHasBottom_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles chkHasBottom.Checked, chkHasBottom.Unchecked
        HasBottom = chkHasBottom.IsChecked
    End Sub
#End Region
#Region "HasSide"
    ''' <summary>Gets or sets valkue indicating if cap has somethign interesting at side</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property HasSide() As Boolean
        <DebuggerStepThrough()> Get
            Return GetValue(HasSideProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Boolean)
            SetValue(HasSideProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="HasSide"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly HasSideProperty As DependencyProperty = DependencyProperty.Register("HasSide", GetType(Boolean), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnHasSideChanged))
    ''' <summary>Called when value of the property <see cref="HasSide"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnHasSideChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnHasSideChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="HasSide"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnHasSideChanged(ByVal e As DependencyPropertyChangedEventArgs)
        chkHasSide.IsChecked = HasSide
    End Sub
    Private Sub txtHasSide_TextChanged(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles chkHasSide.Checked, chkHasSide.Unchecked
        HasSide = chkHasSide.IsChecked
    End Sub
#End Region
#Region "ProductSelection"
    ''' <summary>Gets or sets way in which tproduct is set</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property ProductSelection() As CreatableItemSelection
        <DebuggerStepThrough()> Get
            Return GetValue(ProductSelectionProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As CreatableItemSelection)
            SetValue(ProductSelectionProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="ProductSelection"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly ProductSelectionProperty As DependencyProperty = DependencyProperty.Register("ProductSelection", GetType(CreatableItemSelection), GetType(CapEditor), New FrameworkPropertyMetadata(CreatableItemSelection.AnonymousItem, AddressOf OnProductSelectionChanged, AddressOf CoerceProductSelection))
    ''' <summary>Coereces value of the <see cref="ProductSelection"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><paramref name="baseValue"/> if value is OK (otherwise exception is thrown).</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither <see cref="CreatableItemSelection"/> nor <see cref="String"/></exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is <see cref="String"/> but it is not name of one of <see cref="CreatableItemSelection"/> members</exception>
    ''' <exception cref="InvalidEnumArgumentException"><paramref name="baseValue"/> is <see cref="CreatableItemSelection"/> but it is not one of <see cref="CreatableItemSelection"/> enumerated constants.</exception>
    Private Shared Function CoerceProductSelection(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If TypeOf baseValue Is String Then
            Return [Enum].Parse(GetType(CreatableItemSelection), baseValue)
        ElseIf TypeOf baseValue Is CreatableItemSelection Then
            If Not DirectCast(baseValue, CreatableItemSelection).IsDefined Then Throw New InvalidEnumArgumentException("baseValue", baseValue, GetType(CreatableItemSelection))
        Else : Throw New TypeMismatchException("baseValue", baseValue, GetType(CreatableItemSelection), "Property {0} can be set only by types {1} and {2}.".f("ProductSelection", GetType(CreatableItemSelection), GetType(String)))
        End If
        Return baseValue
    End Function
    ''' <summary>Called when value of the property <see cref="ProductSelection"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnProductSelectionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnProductSelectionChanged(e)
    End Sub
    ''' <summary>True when <see cref="OnCapNameChanged"/> or <see cref="optProduct_Checked"/> is currently on callstach and thus should not proceed again</summary>
    Private Setting_ProductSelection As Boolean = False
    ''' <summary>Called when value of the <see cref="ProductSelection"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnProductSelectionChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If Setting_ProductSelection Then Exit Sub
        Setting_ProductSelection = True
        Try
            Select Case ProductSelection
                Case CreatableItemSelection.NewItem : optProductNew.IsChecked = True
                Case CreatableItemSelection.SelectedItem : optProductSelected.IsChecked = True
                Case Else : optProductAnonymous.IsChecked = True
            End Select
        Finally
            Setting_ProductSelection = False
        End Try
    End Sub
    Private Sub optProduct_Checked(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles optProductAnonymous.Checked, optProductNew.Checked, optProductSelected.Checked
        If Setting_ProductSelection Then Exit Sub
        Setting_ProductSelection = True
        Try
            If optProductSelected.IsChecked Then : ProductSelection = CreatableItemSelection.SelectedItem
            ElseIf optProductNew.IsChecked Then : ProductSelection = CreatableItemSelection.NewItem
            ElseIf optProductAnonymous.IsChecked Then : ProductSelection = CreatableItemSelection.AnonymousItem
            End If
        Finally
            Setting_ProductSelection = False
        End Try
    End Sub
#End Region
#Region "Product"
    ''' <summary>Gets or sets sepected product. Valid when <see cref="ProductSelection"/> is <see cref="CreatableItemSelection.SelectedItem"/></summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Product() As Product
        <DebuggerStepThrough()> Get
            Return GetValue(ProductProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Product)
            SetValue(ProductProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Product"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly ProductProperty As DependencyProperty = DependencyProperty.Register("Product", GetType(Product), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnProductChanged, AddressOf CoerceProduct))
    ''' <summary>Coerces value of the <see cref="Product"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Product"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="Product"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Product.ProductID"/> in combobox</exception>
    Private Shared Function CoerceProduct(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is Product Then Throw New TypeMismatchException("baseValue", baseValue, GetType(Product))
        Return DirectCast(d, CapEditor).CoerceProduct(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="Product"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="Product"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Product.ProductID"/> in combobox</exception>
    Protected Overridable Function CoerceProduct(ByVal baseValue As Product) As Product
        If baseValue Is Nothing Then cmbProduct.SelectedIndex = -1 : Return Nothing
        For Each item As Product In cmbProduct.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As Product In cmbProduct.Items
            If item.ProductID = baseValue.ProductID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownProduct)
    End Function
    ''' <summary>Called when value of the property <see cref="Product"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnProductChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnProductChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Product"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnProductChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbProduct.SelectedItem = Product
        If Product IsNot Nothing Then optProductSelected.IsChecked = True Else optProductAnonymous.IsChecked = True
    End Sub
    Private Sub cmbProduct_SelectionChanged2(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbProduct.SelectionChanged
        Product = cmbProduct.SelectedItem
    End Sub
#End Region
#Region "ProducteName"
    ''' <summary>Gets or sets name of product when <see cref="ProductSelection"/> is <see cref="CreatableItemSelection.NewItem"/></summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property ProductName() As String
        <DebuggerStepThrough()> Get
            Return GetValue(ProductNameProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(ProductNameProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="ProductName"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly ProductNameProperty As DependencyProperty = DependencyProperty.Register("ProductName", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnProductNameChanged))
    ''' <summary>Called when value of the property <see cref="ProductName"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnProductNameChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnProductNameChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="ProductName"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnProductNameChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If ProductName <> txtProductName.Text Then txtProductName.Text = ProductName
    End Sub
    Private Sub txtProductName_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtProductName.TextChanged
        If ProductName <> txtProductName.Text Then ProductName = txtProductName.Text
    End Sub
#End Region
#Region "ProductDescription"
    ''' <summary>Gets or sets description of cap type when <see cref="ProductSelection"/> is <see cref="CreatableItemSelection.NewItem"/></summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property ProductDescription() As String
        <DebuggerStepThrough()> Get
            Return GetValue(ProductDescriptionProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As String)
            SetValue(ProductDescriptionProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="ProductDescription"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly ProductDescriptionProperty As DependencyProperty = DependencyProperty.Register("ProductDescription", GetType(String), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnProductDescriptionChanged))
    ''' <summary>Called when value of the property <see cref="ProductDescription"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnProductDescriptionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnProductDescriptionChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="ProductDescription"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnProductDescriptionChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If ProductDescription <> txtProductDescription.Text Then txtProductDescription.Text = ProductDescription
    End Sub
    Private Sub txtProductDescription_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtProductDescription.TextChanged
        If ProductDescription <> txtProductDescription.Text Then ProductDescription = txtProductDescription.Text
    End Sub
#End Region
#Region "CapProductType"
    ''' <summary>Gets or sets main cap type.</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapProductType() As ProductType
        <DebuggerStepThrough()> Get
            Return GetValue(CapProductTypeProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As ProductType)
            SetValue(CapProductTypeProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapProductType"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapProductTypeProperty As DependencyProperty = DependencyProperty.Register("CapProductType", GetType(ProductType), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapProductTypeChanged, AddressOf CoerceCapProductType))
    ''' <summary>Coerces value of the <see cref="CapProductType"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapProductType"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="CapProductType"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="ProductType.ProductTypeID"/> in combobox</exception>
    Private Shared Function CoerceCapProductType(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is ProductType Then Throw New TypeMismatchException("baseValue", baseValue, GetType(ProductType))
        Return DirectCast(d, CapEditor).CoerceCapProductType(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="CapProductType"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapProductType"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="ProductType.ProductTypeID"/> in combobox</exception>
    Protected Overridable Function CoerceCapProductType(ByVal baseValue As ProductType) As ProductType
        If baseValue Is Nothing Then cmbProductType.SelectedIndex = -1 : Return Nothing
        For Each item As ProductType In cmbProductType.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As ProductType In cmbProductType.Items
            If item.ProductTypeID = baseValue.ProductTypeID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownCapProductType)
    End Function
    ''' <summary>Called when value of the property <see cref="CapProductType"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapProductTypeChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapProductTypeChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapProductType"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapProductTypeChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbProductType.SelectedItem = CapProductType
    End Sub
    Private Sub cmbCapProductType_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbProductType.SelectionChanged
        If cmbProductType.SelectedItem IsNot Nothing Then
            With DirectCast(cmbProductType.SelectedItem, ProductType)
                chkIsDrink.IsChecked = .IsDrink
                chkIsAlcoholic.IsChecked = .IsAlcoholic
            End With
        End If
        CapProductType = cmbProductType.SelectedItem
    End Sub
#End Region
#Region "CapProductType"
    ''' <summary>Gets or sets main cap type.</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property CapCompany() As Company
        <DebuggerStepThrough()> Get
            Return GetValue(CapCompanyProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As Company)
            SetValue(CapCompanyProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="CapCompany"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly CapCompanyProperty As DependencyProperty = DependencyProperty.Register("CapCompany", GetType(Company), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnCapCompanyChanged, AddressOf CoerceCapCompany))
    ''' <summary>Coerces value of the <see cref="CapCompany"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapCompany"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="CapCompany"/>.</exception>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Company.CompanyID"/> in combobox</exception>
    Private Shared Function CoerceCapCompany(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is Company Then Throw New TypeMismatchException("baseValue", baseValue, GetType(Company))
        Return DirectCast(d, CapEditor).CoerceCapCompany(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="CapCompany"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns><see cref="CapCompany"/> that is either <paramref name="baseValue"/> if it is in combo box or has same id as <paramref name="baseValue"/> if it is not in combo box. Null when <paramref name="baseValue"/> is null.</returns>
    ''' <exception cref="ArgumentException"><paramref name="baseValue"/> is not in combo box and there is no item with same <see cref="Company.CompanyID"/> in combobox</exception>
    Protected Overridable Function CoerceCapCompany(ByVal baseValue As Company) As Company
        If baseValue Is Nothing Then cmbCompany.SelectedIndex = -1 : Return Nothing
        For Each item As Company In cmbCompany.Items
            If item Is baseValue Then Return baseValue
        Next
        For Each item As Company In cmbCompany.Items
            If item.CompanyID = baseValue.CompanyID Then Return item
        Next
        Throw New ArgumentException(My.Resources.ex_SetUnknownCapCompany)
    End Function
    ''' <summary>Called when value of the property <see cref="CapCompany"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnCapCompanyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnCapCompanyChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="CapCompany"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnCapCompanyChanged(ByVal e As DependencyPropertyChangedEventArgs)
        cmbCompany.SelectedItem = CapCompany
    End Sub
    Private Sub cmbCapCompany_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs) Handles cmbCompany.SelectionChanged
        CapCompany = cmbCompany.SelectedItem
    End Sub
#End Region
#Region "SelectedCategories"
    ''' <summary>Gets or sets name of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property SelectedCategories() As IEnumerable(Of Category)
        <DebuggerStepThrough()> Get
            Return GetValue(SelectedCategoriesProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As IEnumerable(Of Category))
            SetValue(SelectedCategoriesProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="SelectedCategories"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly SelectedCategoriesProperty As DependencyProperty = DependencyProperty.Register("SelectedCategories", GetType(IEnumerable(Of Category)), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnSelectedCategoriesChanged, AddressOf CoerceSelectedCategories))
    ''' <summary>COerces value of the <see cref="SelectedCategories"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="IEnumerable(Of Category)"/> of <see cref="Category"/></exception> 
    ''' <returns>Those of categories in <paramref name="baseValue"/> which are known to this control</returns>
    Private Shared Function CoerceSelectedCategories(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is IEnumerable(Of Category) Then Throw New TypeMismatchException("baseValue", baseValue, GetType(IEnumerable(Of Category)))
        Return DirectCast(d, CapEditor).CoerceSelectedCategories(baseValue)
    End Function
    ''' <summary>COerces value of the <see cref="SelectedCategories"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns>Array of <see cref="Category"/>: Those of categories in <paramref name="baseValue"/> which are known to this control</returns>
    Private Function CoerceSelectedCategories(ByVal baseValue As IEnumerable(Of Category)) As IEnumerable(Of Category)
        If baseValue Is Nothing Then Return Nothing
        Return (From itm In baseValue Where (From cat As CategoryProxy In lstCategories.Items Select cat.Category.CategoryID).Contains(itm.CategoryID)).ToArray
    End Function
    ''' <summary>Called when value of the property <see cref="SelectedCategories"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnSelectedCategoriesChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnSelectedCategoriesChanged(e)
    End Sub
    ''' <summary>Indicates that <see cref="OnSelectedCategoriesChanged"/> is on call stack</summary>
    Private OnSelectedCategoriesChangedOnStack As Boolean = False
    ''' <summary>Called when value of the <see cref="SelectedCategories"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSelectedCategoriesChanged(ByVal e As DependencyPropertyChangedEventArgs)
        OnSelectedCategoriesChangedOnStack = True
        Try
            For Each item As CategoryProxy In lstCategories.ItemsSource
                item.Checked = SelectedCategories IsNot Nothing AndAlso (From cat In SelectedCategories Select cat.CategoryID).Contains(item.Category.CategoryID)
            Next
        Finally
            OnSelectedCategoriesChangedOnStack = False
        End Try
    End Sub
    Private Sub lstCategories_CheckedChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If OnSelectedCategoriesChangedOnStack Then Exit Sub
        SelectedCategories = (From item As CategoryProxy In lstCategories.Items Where item.Checked Select item.Category).ToArray
    End Sub
#End Region
#Region "Keywords"
    ''' <summary>Gets or sets name of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Keywords() As IEnumerable(Of String)
        <DebuggerStepThrough()> Get
            Return GetValue(KeywordsProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As IEnumerable(Of String))
            SetValue(KeywordsProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Keywords"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly KeywordsProperty As DependencyProperty = DependencyProperty.Register("Keywords", GetType(IEnumerable(Of String)), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnKeywordsChanged, AddressOf CoerceKeywords))
    ''' <summary>COerces value of the <see cref="Keywords"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="IEnumerable(Of Category)"/> of <see cref="Category"/></exception> 
    ''' <returns><paramref name="baseValue"/> as array</returns>
    Private Shared Function CoerceKeywords(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is IEnumerable(Of String) Then Throw New TypeMismatchException("baseValue", baseValue, GetType(IEnumerable(Of String)))
        If baseValue Is Nothing Then Return Nothing
        Return DirectCast(baseValue, IEnumerable(Of String)).ToArray
    End Function
    ''' <summary>Called when value of the property <see cref="Keywords"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnKeywordsChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnKeywordsChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Keywords"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnKeywordsChanged(ByVal e As DependencyPropertyChangedEventArgs)
        kweKeywords.KeyWords.Clear()
        kweKeywords.KeyWords.AddRange(Keywords)
    End Sub
    Private Sub kweKeywords_Changed(ByVal sender As Object, ByVal e As EventArgs) Handles kweKeywords.KeywordAdded, kweKeywords.KeyWordRemoved
        Keywords = kweKeywords.KeyWords
    End Sub
#End Region
#Region "Images"
    ''' <summary>Gets or sets name of cap</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property Images() As ListWithEvents(Of Image)
        <DebuggerStepThrough()> Get
            Return GetValue(ImagesProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As ListWithEvents(Of Image))
            SetValue(ImagesProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="Images"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly ImagesProperty As DependencyProperty = DependencyProperty.Register("Images", GetType(ListWithEvents(Of Image)), GetType(CapEditor), New FrameworkPropertyMetadata(New ListWithEvents(Of Image)(), AddressOf OnImagesChanged))

    ''' <summary>Called when value of the property <see cref="Images"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnImagesChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnImagesChanged(e)
    End Sub
    ''' <summary>Called when value of the <see cref="Images"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnImagesChanged(ByVal e As DependencyPropertyChangedEventArgs)
        lvwImages.ItemsSource = Images
    End Sub

#End Region

#Region "SelectedCapSigns"
    ''' <summary>Gets or sets selected cap signs</summary>
    <LCategory("Caps.Console.Resources.resources", "cat_CapProperties", GetType(CapEditor), "Cap properties")> _
    Public Property SelectedCapSigns() As IEnumerable(Of CapSign)
        <DebuggerStepThrough()> Get
            Return GetValue(SelectedCapSignsProperty)
        End Get
        <DebuggerStepThrough()> Set(ByVal value As IEnumerable(Of CapSign))
            SetValue(SelectedCapSignsProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the <see cref="SelectedCapSigns"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Shared ReadOnly SelectedCapSignsProperty As DependencyProperty = DependencyProperty.Register("SelectedCapSigns", GetType(IEnumerable(Of CapSign)), GetType(CapEditor), New FrameworkPropertyMetadata(AddressOf OnSelectedCapSignsChanged, AddressOf CoerceSelectedCapSigns))
    ''' <summary>COerces value of the <see cref="SelectedCapSigns"/> property</summary>
    ''' <param name="d">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/> -or- <paramref name="baseValue"/> is neither null nor <see cref="IEnumerable(Of CapSign)"/> of <see cref="CapSign"/></exception> 
    ''' <returns>Those of CapSigns in <paramref name="baseValue"/> which are known to this control</returns>
    Private Shared Function CoerceSelectedCapSigns(ByVal d As DependencyObject, ByVal baseValue As Object) As Object
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        If baseValue IsNot Nothing AndAlso Not TypeOf baseValue Is IEnumerable(Of CapSign) Then Throw New TypeMismatchException("baseValue", baseValue, GetType(IEnumerable(Of CapSign)))
        Return DirectCast(d, CapEditor).CoerceSelectedCapSigns(baseValue)
    End Function
    ''' <summary>Contains list of value which when set to <see cref="SelectedCapSigns"/> are accepted without any checks and manipulation</summary>
    Private SelectedCapSignsValuesNotToBeCoerced As New List(Of IEnumerable(Of CapSign))
    ''' <summary>Coerces value of the <see cref="SelectedCapSigns"/> property</summary>
    ''' <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
    ''' <returns>Array of <see cref="CapSign"/>: Those of CapSigns in <paramref name="baseValue"/> which are known to this control</returns>
    Private Function CoerceSelectedCapSigns(ByVal baseValue As IEnumerable(Of CapSign)) As IEnumerable(Of CapSign)
        If baseValue Is Nothing Then Return Nothing
        If SelectedCapSignsValuesNotToBeCoerced.Contains(baseValue) Then Return baseValue
        Return (From itm In baseValue Where (From sign In AllCapSigns Select sign.CapSignID).Contains(itm.CapSignID)).ToArray
    End Function
    ''' <summary>Called when value of the property <see cref="SelectedCapSigns"/> is changed</summary>
    ''' <param name="d">The <see cref="CapEditor"/> the change occured for</param>
    ''' <param name="e">Evcent arguments</param>
    ''' <exception cref="TypeMismatchException"><paramref name="d"/> is not <see cref="CapEditor"/></exception>
    Private Shared Sub OnSelectedCapSignsChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnSelectedCapSignsChanged(e)
    End Sub
    ''' <summary>Indicates that <see cref="OnSelectedCapSignsChanged"/> is on call stack</summary>
    Private OnSelectedCapSignsChangedOnStack As Boolean = False
    ''' <summary>Called when value of the <see cref="SelectedCapSigns"/> property changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnSelectedCapSignsChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If SelectedCapSignsValuesNotToBeCoerced.Contains(e.NewValue) Then
            SelectedCapSignsValuesNotToBeCoerced.Remove(e.NewValue)
            Exit Sub
        End If
        OnSelectedCapSignsChangedOnStack = True
        Try
            Dim Cap_Sign_Ints As ListWithEvents(Of Cap_CapSign_Int) = icSigns.ItemsSource
            Cap_Sign_Ints.Clear()
            Cap_Sign_Ints.AddRange(From itm In AllCapSigns
                                   Where (From base In DirectCast(e.NewValue, IEnumerable(Of CapSign)) Select base.CapSignID).Contains(itm.CapSignID)
                                   Select New Cap_CapSign_Int With {.CapSign = itm})
        Finally
            OnSelectedCapSignsChangedOnStack = False
        End Try
    End Sub
#End Region
#End Region

    ''' <summary>Possible ways how to specifiy certain cap properties</summary>
    Public Enum CreatableItemSelection
        ''' <summary>No parent object is set</summary>
        AnonymousItem
        ''' <summary>Parent object is selected from list of exisitng ones</summary>
        SelectedItem
        ''' <summary>New parent object is to be created</summary>
        NewItem
    End Enum


    Private Sub lvwImages_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles lvwImages.KeyDown
        If e.Key = Key.Delete AndAlso lvwImages.SelectedItems.Count <> 0 Then
            Dim todel As New List(Of Image)(From img As Image In lvwImages.SelectedItems)
            DirectCast(lvwImages.ItemsSource, ListWithEvents(Of Image)).RemoveAll(Function(img) todel.Contains(img))
            lvwImages.Items.Refresh()
        End If
    End Sub

    ''' <summary>Resets datacontext of this instance</summary>
    Public Function ResetContext(Optional ByRef Context As CapsDataDataContext = Nothing) As CapsDataDataContext
        Dim OldSelectedIds = New With { _
            .CapType = If(CapType IsNot Nothing, CapType.CapTypeID, New Integer?), _
            .MainType = If(CapMainType IsNot Nothing, CapMainType.MainTypeID, New Integer?), _
            .Shape = If(CapShape IsNot Nothing, CapShape.ShapeID, New Integer?), _
            .Material = If(Material IsNot Nothing, Material.MaterialID, New Integer?), _
            .Storage = If(Storage IsNot Nothing, Storage.StorageID, New Integer?), _
            .ProductType = If(CapProductType IsNot Nothing, CapProductType.ProductTypeID, New Integer?), _
            .Product = If(Product IsNot Nothing, Product.ProductID, New Integer?), _
            .Company = If(CapCompany IsNot Nothing, CapCompany.CompanyID, New Integer?), _
            .Categories = (From cat In If(SelectedCategories, New Category() {}) Select cat.CategoryID).ToArray,
            .CapSigns = (From sign In If(SelectedCapSigns, New CapSign() {}) Select sign.CapSignID).ToArray
        }
        If OriginalContext IsNot Nothing Then OriginalContext.Dispose()
        OriginalContext = Nothing
        If Context Is Nothing Then
            OriginalContext = New CapsDataDataContext(Main.Connection)
            Context = OriginalContext
        End If

        Me.Context = Context
        With OldSelectedIds
            'CapType
            cmbCapType.ItemsSource = New ListWithEvents(Of CapType)(From item In Context.CapTypes Order By item.TypeName)
            If .CapType.HasValue Then cmbCapType.SelectedItem = (From itm As CapType In cmbCapType.ItemsSource Where itm.CapTypeID = .CapType).FirstOrDefault
            'MainType
            cmbMainType.ItemsSource = New ListWithEvents(Of MainType)(From item In Context.MainTypes Order By item.TypeName)
            If .MainType.HasValue Then cmbMainType.SelectedItem = (From itm As MainType In cmbMainType.ItemsSource Where itm.MainTypeID = .MainType).FirstOrDefault
            'Shape
            cmbShape.ItemsSource = New ListWithEvents(Of Shape)(From item In Context.Shapes Order By item.Name)
            If .Shape.HasValue Then cmbShape.SelectedItem = (From itm As Shape In cmbShape.ItemsSource Where itm.ShapeID = .Shape).FirstOrDefault
            'Material
            cmbMaterial.ItemsSource = New ListWithEvents(Of Material)(From item In Context.Materials Order By item.Name)
            If .Material.HasValue Then cmbMaterial.SelectedItem = (From itm As Material In cmbMaterial.ItemsSource Where itm.MaterialID = .Material).FirstOrDefault
            'Storage
            cmbStorage.ItemsSource = New ListWithEvents(Of Storage)(From item In Context.Storages Order By item.StorageNumber)
            If .Storage.HasValue Then cmbStorage.SelectedItem = (From itm As Storage In cmbStorage.ItemsSource Where itm.StorageID = .Storage).FirstOrDefault
            'Product
            cmbProduct.ItemsSource = New ListWithEvents(Of Product)(From item In Context.Products Order By item.ProductName)
            If .Product.HasValue Then cmbProduct.SelectedItem = (From itm As Product In cmbProduct.ItemsSource Where itm.ProductID = .Product).FirstOrDefault
            'ProductType
            Dim ProductTypesList As ListWithEvents(Of ProductType) = New ListWithEvents(Of ProductType)(From item In Context.ProductTypes Order By item.ProductTypeName)
            ProductTypesList.Add(Nothing)
            cmbProductType.ItemsSource = ProductTypesList
            If .ProductType.HasValue Then cmbProductType.SelectedItem = (From itm As ProductType In cmbProductType.ItemsSource Where itm.ProductTypeID = .ProductType).FirstOrDefault Else cmbProductType.SelectedItem = Nothing
            'Company
            Dim CompaniesList As ListWithEvents(Of Company) = New ListWithEvents(Of Company)(From item In Context.Companies Order By item.CompanyName)
            CompaniesList.Add(Nothing)
            cmbCompany.ItemsSource = CompaniesList
            If .Company.HasValue Then cmbCompany.SelectedItem = (From itm As Company In cmbCompany.ItemsSource Where itm.CompanyID = .Company).FirstOrDefault Else cmbCompany.SelectedItem = Nothing
            'Categories
            lstCategories.ItemsSource = New ListWithEvents(Of CategoryProxy)(From item In Context.Categories Order By item.CategoryName Select New CategoryProxy(item, .Categories.Contains(item.CategoryID)))
            lstCategories_CheckedChanged(Nothing, Nothing)
            'CapSigns
            AllCapSigns.Clear()
            AllCapSigns.AddRange(Context.CapSigns)
            SelectedCapSigns = From cs In AllCapSigns Where .CapSigns.Contains(cs.CapSignID)
        End With
        With DirectCast(lvwImages.ItemsSource, ListWithEvents(Of Image))
            .RemoveAll(Function(img) Not TypeOf img Is NewImage)
            Dim i As Integer = 0
            For Each img In From imgx In Context.Images Where imgx.CapID = Me.CapID
                .Insert(i, img)
                i += 1
            Next
        End With
        Return Context
    End Function


#Region "Services"

    ''' <summary>Does tests of values</summary>
    Public Function Tests() As Boolean
        Tests = False
        If cmbMainType.SelectedItem Is Nothing Then mBox.Modal_PTIW(My.Resources.msg_MainTypeMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : cmbMainType.Focus() : Exit Function
        If cmbShape.SelectedItem Is Nothing Then mBox.Modal_PTIW(My.Resources.msg_ShapeMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : cmbShape.Focus() : Exit Function
        If cmbMaterial.SelectedItem Is Nothing Then mBox.Modal_PTIW(My.Resources.msg_MaterialMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : cmbMaterial.Focus() : Exit Function
        If cmbStorage.SelectedItem Is Nothing Then mBox.Modal_PTIW(My.Resources.msg_StorageMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : cmbStorage.Focus() : Exit Function
        If txtCapName.Text = "" Then mBox.Modal_PTIW(My.Resources.msg_CapNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : txtCapName.Focus() : Exit Function
        If DirectCast(lvwImages.ItemsSource, ListWithEvents(Of Image)).Count = 0 Then mBox.Modal_PTIW(My.Resources.msg_AtLeastOneImageMustBeSelected, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : btnAddImage.Focus() : Exit Function
        If txtSideText.Text <> "" AndAlso Not chkHasSide.IsChecked Then mBox.Modal_PTIW(My.Resources.msg_SideText_HasSide, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : chkHasSide.Focus() : Exit Function
        If txtBottomText.Text <> "" AndAlso Not chkHasBottom.IsChecked Then mBox.Modal_PTIW(My.Resources.msg_BottomText_HasBottom, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : chkHasBottom.Focus() : Exit Function
        If txtMainPicture.Text <> "" AndAlso (cmbPictureType.SelectedItem Is cmiImageNo OrElse cmbPictureType.SelectedItem Is Nothing) Then mBox.Modal_PTIW(My.Resources.msg_MainPicture_PictureType, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : cmbPictureType.Focus() : Exit Function
        If txtAnotherPictures.Text <> "" AndAlso txtMainPicture.Text = "" Then mBox.Modal_PTIW(My.Resources.msg_AnotherPictures_MainPicture, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : txtMainPicture.Focus() : Exit Function
        If copForeground2.Color.HasValue AndAlso Not copForeground.Color.HasValue Then mBox.Modal_PTIW(My.Resources.msg_ForeColor_ForeColor2, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : copForeground.Focus() : Exit Function
        If optProductSelected.IsChecked AndAlso cmbProduct.SelectedItem Is Nothing Then mBox.Modal_PTIW(My.Resources.msg_NoProductSelected, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : cmbProduct.Focus() : Exit Function
        If optCapTypeSelect.IsChecked AndAlso cmbCapType.SelectedItem Is Nothing Then mBox.Modal_PTIW(My.Resources.msg_NoCapTypeSelected, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : cmbCapType.Focus() : Exit Function
        If txtCountryCode.Text <> "" AndAlso (txtCountryCode.Text.Length <> 2 OrElse Not txtCountryCode.Text Like "[A-Z][A-Z]") Then mBox.Modal_PTIW(My.Resources.txt_InvalidCountryCode, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : txtCountryCode.Focus() : Exit Function
        If txtCountryOfOrigin.Text <> "" AndAlso (txtCountryOfOrigin.Text.Length <> 2 OrElse Not txtCountryOfOrigin.Text Like "[A-Z][A-Z]") Then mBox.Modal_PTIW(My.Resources.txt_InvalidCountryOfOriginCode, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : txtCountryOfOrigin.Focus() : Exit Function
        If (From CapSign In SelectedCapSigns Select (From CapSign2 In SelectedCapSigns Where CapSign.CapSignID = CapSign2.CapSignID).Count).Any(Function(count) count > 1) Then mBox.Modal_PTIW(My.Resources.txt_ToManyCapSigns, My.Resources.txt_InvalidInput, mBox.MessageBoxIcons.Exclamation, Me) : icSigns.Focus() : Exit Function
        Tests = True
    End Function

    ''' <summary>Attempts to copy image of cap type to images directory</summary>
    ''' <param name="NewType">Newly created cap type</param>
    Public Sub CopyTypeImage(ByVal NewType As CapType)
        If IO.File.Exists(CapTypeImagePath) Then
            Dim CapTypeDir = IO.Path.Combine(My.Settings.ImageRoot, "CapType")
            If Not IO.Directory.Exists(CapTypeDir) Then
                Try
                    IO.Directory.CreateDirectory(CapTypeDir)
                Catch ex As Exception
                    mBox.MsgBox(My.Resources.err_CreatingDirectoryCapType.f(CapTypeDir, vbCrLf, ex.Message), MsgBoxStyle.Exclamation, My.Resources.txt_CopyFile, Me)
                    Exit Sub
                End Try
            End If
            Dim targpath = IO.Path.Combine(CapTypeDir, NewType.CapTypeID.ToString(System.Globalization.CultureInfo.InvariantCulture) & ".png")
            If Not IO.File.Exists(targpath) OrElse mBox.MsgBox(My.Resources.msg_CapImageExistsOverwrite.f(CapTypeImagePath), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, My.Resources.txt_OwervriteFile, Me) = MsgBoxResult.Yes Then
                Try
                    IO.File.Copy(CapTypeImagePath, targpath)
                Catch ex As Exception
                    mBox.MsgBox(My.Resources.err_CopyCapTypeImageFailed.f(vbCrLf, ex.Message), MsgBoxStyle.Exclamation, My.Resources.txt_CopyFile, Me)
                End Try
            End If : End If
    End Sub
    ''' <summary>Tests if newwly introduced cap type is OK</summary>
    ''' <remarks>True if it is OK or user is OK with it not being OK</remarks>
    Public Function TestNewCapType() As Boolean
        If Not IO.File.Exists(CapTypeImagePath) Then
            Select Case mBox.ModalF_PTWBIa(My.Resources.msg_FileNotExists_ContinueWOImage, My.Resources.txt_CapTypeImage, Me, mBox.MessageBoxButton.Buttons.Yes Or mBox.MessageBoxButton.Buttons.No, mBox.GetIcon(mBox.MessageBoxIcons.Question), CapTypeImagePath)
                Case Forms.DialogResult.Yes
                Case Else : Return False
            End Select
        ElseIf IO.Path.GetExtension(CapTypeImagePath).ToLower <> ".png" Then
            mBox.Modal_PTIW(My.Resources.msg_OnlyPNG, My.Resources.txt_CapTypeImage, mBox.MessageBoxIcons.Exclamation, Me)
            Return False
        End If
        If CapTypeName = "" Then mBox.Modal_PTIW(My.Resources.msg_CapTypeNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : txtCapTypeName.Focus() : Return False
        If (From CapType In Context.CapTypes Where CapType.TypeName = CapTypeName).Any Then _
            mBox.Modal_PTIW(My.Resources.msg_CapTypeAreadyExists, My.Resources.txt_DuplicateEntry, mBox.MessageBoxIcons.Exclamation, Me) : txtCapTypeName.SelectAll() : txtCapName.Focus() : Return False
        Return True
    End Function
    ''' <summary>Tests newly introduced product</summary>
    ''' <returns>True if it is OK</returns>
    Public Function TestNewProduct() As Boolean
        If ProductName = "" Then mBox.Modal_PTIW(My.Resources.msg_ProductNameMustBeEntered, My.Resources.txt_IncompleteEntry, mBox.MessageBoxIcons.Exclamation, Me) : txtProductName.Focus() : Return False
        If (From Product In Context.Products Where Product.ProductName = txtProductName.Text).Any Then _
            mBox.Modal_PTIW(My.Resources.msg_ProductWithAreadyExists, My.Resources.txt_DuplicateEntry, mBox.MessageBoxIcons.Exclamation, Me) : txtProductName.SelectAll() : txtProductName.Focus() : Return False
        Return True
    End Function
#Region "Copy images"
    ''' <summary>Copies images from <see cref="CapEditor.Images">Images</see> to image directory and creates resized images</summary>
    ''' <returns>List of copied images; null on error</returns>
    Public Function CopyImages() As List(Of Image)
        Dim Imgs = New List(Of Image)
        CopyImages = Imgs
        If Images Is Nothing Then Exit Function
        Dim folOrig = IO.Path.Combine(My.Settings.ImageRoot, "original")
        Dim fol64 = IO.Path.Combine(My.Settings.ImageRoot, "64_64")
        Dim fol256 = IO.Path.Combine(My.Settings.ImageRoot, "256_256")
        Dim FolTBC = folOrig
TryCreateDirsIfNotExist: Try
            If Not IO.Directory.Exists(folOrig) Then IO.Directory.CreateDirectory(folOrig)
            FolTBC = fol64
            If Not IO.Directory.Exists(fol64) Then IO.Directory.CreateDirectory(fol64)
            FolTBC = fol256
            If Not IO.Directory.Exists(fol256) Then IO.Directory.CreateDirectory(fol256)
        Catch ex As Exception
            If mBox.Error_XPTIBWO(ex, "Error creating folder {0}".f(FolTBC), ex.GetType.Name, , mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Abort, Me) = Forms.DialogResult.Retry Then
                GoTo TryCreateDirsIfNotExist
            Else
                Return Nothing
            End If
        End Try
        Dim CreatedFiles As New List(Of String)
        Dim Exception As Exception = Nothing
        Try
            For Each Item As NewImage In Images.OfType(Of NewImage)()
                'Copy original size file
CopyFile:       Dim newName = IO.Path.GetFileName(Item.RelativePath)
                Dim newName1 = newName
                Dim i As Integer = 0
                While IO.File.Exists(IO.Path.Combine(folOrig, newName)) OrElse IO.File.Exists(IO.Path.Combine(fol64, newName)) OrElse IO.File.Exists(IO.Path.Combine(fol256, newName))
                    i += 1
                    newName = String.Format(System.Globalization.CultureInfo.InvariantCulture, _
                                            "{0}_{1}{2}", IO.Path.GetFileNameWithoutExtension(newName1), i, IO.Path.GetExtension(newName1))
                End While
                Dim OrigFilePath As String = IO.Path.Combine(folOrig, newName)
                Try
                    IO.File.Copy(Item.RelativePath, OrigFilePath)
                Catch ex As Exception
                    If mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCopyingFile, My.Resources.txt_ImageCopyError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Abort, Me) = Forms.DialogResult.Retry Then GoTo CopyFile
                    Exception = ex
                    Return Nothing
                End Try
                Dim OrigFileInfo As New IO.FileInfo(OrigFilePath)
                With OrigFileInfo 'If file is readonly, make it RW
                    If (OrigFileInfo.Attributes And IO.FileAttributes.ReadOnly) = IO.FileAttributes.ReadOnly Then _
                                        .Attributes = .Attributes And Not IO.FileAttributes.ReadOnly
                End With

                CreatedFiles.Add(OrigFilePath)
                'Write metadata
                Dim IPTC As Tools.MetadataT.IptcT.Iptc = Nothing
                Try
                    If IO.Path.GetExtension(OrigFilePath).ToLower = ".jpg" OrElse IO.Path.GetExtension(OrigFilePath).ToLower = ".jpeg" Then
                        Using JPEG As New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(OrigFilePath, False)
                            If JPEG.ContainsIptc Then
                                IPTC = New Tools.MetadataT.IptcT.Iptc(JPEG)
                            Else
                                IPTC = New Tools.MetadataT.IptcT.Iptc
                            End If
                        End Using
                        Dim keywords = New List(Of String)(If(IPTC.Keywords, New String() {}))
                        For Each NewKw In My.Resources.CapKeywords.Split(","c)
                            If Not keywords.Contains(NewKw) Then keywords.Add(NewKw)
                        Next
                        For Each NewKw In keywords
                            If Not keywords.Contains(NewKw) Then keywords.Add(NewKw)
                        Next
                        If SelectedCategories IsNot Nothing Then
                            For Each cat As Category In SelectedCategories
                                keywords.Add(cat.CategoryName)
                            Next
                        End If
                        IPTC.Keywords = keywords.ToArray
                        If Country <> "" Then
                            Dim cox As New CapsDataDataContext(Main.Connection)
                            Dim Code = (From c In cox.ISO_3166_1s Where c.Alpha_2 = Country Select c.Alpha_3).FirstOrDefault
                            If Code IsNot Nothing Then
                                IPTC.CountryPrimaryLocationCode = Code
                            End If
                        End If
                        IPTC.ObjectName = CapName
                        If MainText <> "" Then IPTC.Headline = MainText
                        If TopText <> "" OrElse SideText <> "" OrElse BottomText <> "" Then
                            Dim strlist As New List(Of String)
                            If TopText <> "" Then strlist.Add(TopText)
                            If SideText <> "" Then strlist.Add(SideText)
                            If BottomText <> "" Then strlist.Add(BottomText)
                            IPTC.CaptionAbstract = strlist.Join(vbCrLf)
                        End If
                        If Year.HasValue > 0 Then IPTC.DateCreated = New Tools.MetadataT.IptcT.IptcDataTypes.OmmitableDate(Year)
                        IPTC.ReleaseDate = Now.ToUniversalTime.Date
                        IPTC.ReleaseTime = New Tools.MetadataT.IptcT.IptcDataTypes.Time(Now.ToUniversalTime.Date.TimeOfDay)
                        If CapNote <> "" Then IPTC.SpecialInstructions = CapNote.Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ")
                        Using JPEG As New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(OrigFilePath, True)
                            JPEG.IPTCEmbed(IPTC.GetBytes)
                        End Using
                    End If
                Catch ex As Exception
                    If mBox.Error_XPTIBWO(ex, My.Resources.msg_IPTCError, My.Resources.txt_IPTC, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Ignore, Me) <> Forms.DialogResult.Ignore Then
                        Exception = ex
                        Return Nothing
                    End If
                End Try
                'Resize file to 64px
                Dim File64 As String = IO.Path.Combine(fol64, newName)
Resize64:       Try
                    SaveResizedImage(OrigFilePath, File64, 64, CreatedFiles, IPTC)
                Catch ex As Exception
                    Dim result As System.Windows.Forms.DialogResult = mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCreatingResizedFile.f(64), My.Resources.txt_ImageResizeError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Ignore, Me)
                    If result = Forms.DialogResult.Retry OrElse result = Forms.DialogResult.Ignore Then
                        Try
                            IO.File.Delete(File64)
                        Catch ex2 As Exception
                            mBox.Error_XPTIBWO(ex2, My.Resources.msg_CreatedFileWasNotDeleted.f(File64), My.Resources.txt_ErrorRemovingFile, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Ignore, Me)
                        End Try
                        CreatedFiles.Remove(File64)
                    End If
                    Select Case result
                        Case Forms.DialogResult.Retry : GoTo Resize64
                        Case Forms.DialogResult.Ignore 'Do nothing
                        Case Else : Exception = ex : Return Nothing
                    End Select
                End Try
                'Resize file to 256px
                Dim File256 As String = IO.Path.Combine(fol256, newName)
Resize256:      Try
                    SaveResizedImage(OrigFilePath, File256, 256, CreatedFiles, IPTC)
                Catch ex As Exception
                    Dim result As System.Windows.Forms.DialogResult = mBox.Error_XPTIBWO(ex, My.Resources.msg_ErrorCreatingResizedFile.f(256), My.Resources.txt_ImageResizeError, mBox.MessageBoxIcons.Error, mBox.MessageBoxButton.Buttons.Abort Or mBox.MessageBoxButton.Buttons.Retry Or mBox.MessageBoxButton.Buttons.Ignore, Me)
                    If result = Forms.DialogResult.Retry OrElse result = Forms.DialogResult.Ignore Then
                        Try
                            IO.File.Delete(File256)
                        Catch ex2 As Exception
                            mBox.Error_XPTIBWO(ex2, My.Resources.msg_CreatedFileWasNotDeleted.f(File256), My.Resources.txt_ErrorRemovingFile, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Ignore, Me)
                        End Try
                        CreatedFiles.Remove(File256)
                    End If
                    Select Case result
                        Case Forms.DialogResult.Retry : GoTo Resize256
                        Case Forms.DialogResult.Ignore 'Do nothing
                        Case Else : Exception = ex : Return Nothing
                    End Select
                End Try
                Imgs.Add(New Image With {.RelativePath = newName, .IsMain = Item.IsMain})
            Next
        Finally
            If Exception IsNot Nothing Then
                For Each file In CreatedFiles
                    Try
                        IO.File.Delete(file)
                    Catch ex As Exception
                        mBox.Error_XPTIBWO(ex, My.Resources.msg_CreatedFileWasNotDeleted.f(file), My.Resources.txt_ErrorRemovingFile, mBox.MessageBoxIcons.Exclamation, mBox.MessageBoxButton.Buttons.Ignore, Me)
                    End Try
                Next
            End If
        End Try
    End Function
    ''' <summary>Resizes image and saves it under given name</summary>
    ''' <param name="OrigFilePath">Path of image to be resized</param>
    ''' <param name="TargetFilePath">Path to save resized image to</param>
    ''' <param name="Size">Desired maximum square size</param>
    ''' <param name="CreatedFiles">Procedure adds path to any file it creates to this list</param>
    Private Shared Sub SaveResizedImage(ByVal OrigFilePath As String, ByVal TargetFilePath$, ByVal Size%, ByVal CreatedFiles As List(Of String), ByVal IPTC As Tools.MetadataT.IptcT.Iptc)
        Dim Extension As String = IO.Path.GetExtension(OrigFilePath).ToLower
        If Extension <> ".wmp" AndAlso Extension <> ".hdp" Then
            Using bmp As New System.Drawing.Bitmap(OrigFilePath)
                Using small = bmp.GetThumbnail(New System.Drawing.Size(Size, Size))
                    Dim format As System.Drawing.Imaging.ImageFormat
                    Select Case Extension
                        Case ".jpg", ".jpeg" : format = System.Drawing.Imaging.ImageFormat.Jpeg
                        Case ".bmp", ".dib" : format = System.Drawing.Imaging.ImageFormat.Bmp
                        Case ".tif", ".tiff" : format = System.Drawing.Imaging.ImageFormat.Tiff
                        Case ".gif" : format = System.Drawing.Imaging.ImageFormat.Gif
                        Case ".png" : format = System.Drawing.Imaging.ImageFormat.Png
                            'Case ".wmp", ".hdp" 
                            'Case ".ico" : encoder = New IconBitmapEncoder
                        Case Else : Throw New InvalidOperationException(My.Resources.err_UnknownImageExtension.f(IO.Path.GetExtension(OrigFilePath)))
                    End Select
                    small.Save(TargetFilePath, format)
                    CreatedFiles.Add(TargetFilePath)
                End Using
                If IPTC IsNot Nothing AndAlso (Extension = ".jpg" OrElse Extension = ".jpeg") Then
                    Using smalljpeg = New Tools.DrawingT.DrawingIOt.JPEG.JPEGReader(TargetFilePath, True)
                        smalljpeg.IPTCEmbed(IPTC.GetBytes)
                    End Using
                End If
            End Using
        Else
            Using OrigFile = IO.File.Open(OrigFilePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                Dim imgA As New BitmapImage
                imgA.BeginInit()
                imgA.StreamSource = OrigFile
                imgA.CreateOptions = BitmapCreateOptions.PreservePixelFormat
                imgA.CacheOption = BitmapCacheOption.None
                imgA.EndInit()
                Dim img = New BitmapImage
                img.BeginInit()
                img.StreamSource = OrigFile
                img.CreateOptions = BitmapCreateOptions.PreservePixelFormat
                img.CacheOption = BitmapCacheOption.None
                If imgA.Height > imgA.Width Then img.DecodePixelHeight = Size _
                Else img.DecodePixelWidth = Size
                OrigFile.Position = 0
                img.EndInit()
                Dim encoder As BitmapEncoder
                Select Case Extension
                    'Case ".jpg", ".jpeg" : encoder = New JpegBitmapEncoder()
                    'Case ".bmp", ".dib" : encoder = New BmpBitmapEncoder
                    'Case ".tif", ".tiff" : encoder = New TiffBitmapEncoder
                    'Case ".gif" : encoder = New GifBitmapEncoder()
                    'Case ".png" : encoder = New PngBitmapEncoder()
                    Case ".wmp", ".hdp" : encoder = New WmpBitmapEncoder()
                        'Case ".ico" : encoder = New IconBitmapEncoder
                    Case Else : Throw New InvalidOperationException(My.Resources.err_UnknownImageExtension.f(IO.Path.GetExtension(OrigFilePath)))
                End Select
                encoder.Frames.Add(BitmapFrame.Create(img))
                Using ostream = IO.File.Open(TargetFilePath, IO.FileMode.Create, IO.FileAccess.ReadWrite)
                    CreatedFiles.Add(TargetFilePath)
                    encoder.Save(ostream)
                End Using
            End Using
        End If
    End Sub
    ''' <summary>Undos copying images</summary>
    ''' <param name="IntroducedImages">Images returned by <see cref="CopyImages"/></param>
    Public Sub UndoCopyImages(ByVal IntroducedImages As IEnumerable(Of String))
        Dim FaildedDeletes As New System.Text.StringBuilder
        For Each img In IntroducedImages
            For Each folder In New String() {"original", "64_64", "256_256"}
                Dim imgpath = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, folder), img)
                If IO.File.Exists(imgpath) Then
                    Try
                        IO.File.Delete(imgpath)
                    Catch delex As Exception
                        FaildedDeletes.AppendLine(String.Format("{0}{1}{2}{1}({3})", img, vbTab, delex.Message, imgpath))
                    End Try
                End If
            Next
        Next
        If FaildedDeletes.Length > 0 Then
            mBox.Modal_PTIW(My.Resources.err_DeleteingOfSomeImagesFailed & vbCrLf & FaildedDeletes.ToString & vbCrLf & My.Resources.msg_ThereAreImagesThatDoNotBelongToAnyCap, My.Resources.txt_Error, mBox.MessageBoxIcons.Error, Me)
        End If
    End Sub
#End Region
#End Region

    Public Property AllowSearch() As Boolean
        Get
            Return GetValue(AllowSearchProperty)
        End Get

        Set(ByVal value As Boolean)
            SetValue(AllowSearchProperty, value)
        End Set
    End Property

    Public Shared ReadOnly AllowSearchProperty As DependencyProperty = _
                           DependencyProperty.Register("AllowSearch", _
                           GetType(Boolean), GetType(CapEditor), _
                           New FrameworkPropertyMetadata(True, AddressOf OnAllowSearchChanged))
    Private Shared Sub OnAllowSearchChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        If Not TypeOf d Is CapEditor Then Throw New TypeMismatchException("d", d, GetType(CapEditor))
        DirectCast(d, CapEditor).OnAllowSearchChanged(e)
    End Sub
    Protected Overridable Sub OnAllowSearchChanged(ByVal e As DependencyPropertyChangedEventArgs)
        btnSearch.IsEnabled = AllowSearch
    End Sub

    Private Sub btnNewTarget_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNewTarget.Click
        Dim win As New winNewSimple(winNewSimple.SimpleTypes.Target, Context)
        If win.ShowDialog Then
            DirectCast(cmbTarget.ItemsSource, ListWithEvents(Of Target)).Add(DirectCast(win.NewObject, Target))
            cmbTarget.Items.Refresh()
            cmbTarget.SelectedItem = win.NewObject
        End If
    End Sub
    ''' <summary>Actualizes lists which are changed when cap is saved</summary>
    Public Sub ActualizeChangedLists()
        kweKeywords.AutoCompleteStable = New ListWithEvents(Of String)(From item In Context.Keywords Order By item.Keyword Select item.Keyword)

        cmbCapType.ItemsSource = New ListWithEvents(Of CapType)(From item In Context.CapTypes Order By item.TypeName)
        cmbProduct.ItemsSource = New ListWithEvents(Of Product)(From item In Context.Products Order By item.ProductName)
        DirectCast(cmbProduct.ItemsSource, ListWithEvents(Of Product)).Add(Nothing)
    End Sub
    ''' <summary>Sets focus to first item in UI</summary>
    Public Sub InitFocus()
        txtCapName.Focus()
    End Sub

    ''' <summary>Resets values of editor</summary>
    Public Sub Reset()
        txtCapName.Text = ""
        txtMainText.Text = ""
        txtSubTitle.Text = ""
        txtMainPicture.Text = ""
        txtAnotherPictures.Text = ""
        PictureType = Nothing

        cmbCapType.SelectedIndex = -1
        optCapTypeSelect.IsChecked = True
        txtCapTypeName.Text = ""
        txtCapTypeDesc.Text = ""
        txtCapTypeImagePath.Text = ""
        cmbMainType.SelectedIndex = -1
        cmbShape.SelectedIndex = -1
        nudSize1.Value = 0
        nudSize2.Value = 0
        nudHeight.Value = 0
        cmbMaterial.SelectedIndex = -1
        cmbTarget.SelectedIndex = -1

        copBackground.Color = Colors.Transparent
        copSecondaryBackground.Color = Nothing
        copForeground.Color = Nothing
        copForeground2.Color = Nothing
        chk3D.IsChecked = False
        optMatting.IsChecked = True

        txtTopText.Text = ""
        txtSideText.Text = ""
        txtBottomText.Text = ""
        txtNote.Text = ""

        lblID.Content = ""
        nudYear.Value = 0
        txtCountryCode.Text = ""
        txtCountryOfOrigin.Text = ""
        cmbStorage.SelectedIndex = -1
        chkHasBottom.IsChecked = False
        chkHasSide.IsChecked = False
        nudCapState.Value = 1

        optProductAnonymous.IsChecked = True
        cmbProduct.SelectedIndex = -1
        txtProductName.Text = ""
        txtProductDescription.Text = ""
        cmbProductType.SelectedIndex = -1
        cmbCompany.SelectedIndex = -1
        chkIsDrink.IsChecked = Nothing
        chkIsAlcoholic.IsChecked = Nothing

        SelectedCategories = New Category() {}
        Keywords = New String() {}
        SelectedCapSigns = New CapSign() {}
        Images.Clear()
        ActualizeChangedLists()

        InitFocus()

        ShowCount()
    End Sub

    Private Sub Image_MouseDown(ByVal sender As FrameworkElement, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If e.ClickCount = 2 AndAlso e.ChangedButton = MouseButton.Left Then
            e.Handled = True
            Dim item As Image = sender.DataContext
            Dim path$
            If IO.Path.IsPathRooted(item.RelativePath) Then
                path = item.RelativePath
            Else
                path = IO.Path.Combine(IO.Path.Combine(My.Settings.ImageRoot, "original"), item.RelativePath)
            End If
            Try
                Process.Start(path)
            Catch ex As Exception
                mBox.Error_XTW(ex, ex.GetType.Name, Me)
            End Try
        End If

    End Sub


    Private LastCountry As TextBox
    Private Sub btnCCCountry_Click(ByVal sender As Button, ByVal e As System.Windows.RoutedEventArgs) Handles btnCCCountry.Click, btnCCCountryOfOrigin.Click
        e.Handled = True
        With DirectCast(Me.Resources("cmnCC"), ContextMenu)
            sender.ContextMenu = .self
            .IsOpen = True
            If sender Is btnCCCountry Then : LastCountry = txtCountryCode
            ElseIf sender Is btnCCCountryOfOrigin Then : LastCountry = txtCountryOfOrigin
            Else : LastCountry = Nothing
            End If
        End With
    End Sub

    Private Sub mniCountry_Click(ByVal sender As MenuItem, ByVal e As System.Windows.RoutedEventArgs)
        If LastCountry IsNot Nothing Then LastCountry.Text = DirectCast(sender.DataContext, Country).Code2
    End Sub

#Region "TypeSuggestor"
    Private Sub tysSuggestor_ApplyExistingType(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles tysSuggestor.ApplyExistingType
        'Preserve values
        Dim OldSize1 = Size1
        Dim OldSize2 = Size2
        Dim OldHeigh = CapHeight
        Dim OldTarget = Target
        Dim OldMaterial = Material
        'Make selection
        Dim exType = tysSuggestor.SelectedExistingType
        CapTypeSelection = CreatableItemSelection.SelectedItem
        CapType = exType
        'Preserved values
        Size1 = OldSize1
        Size2 = OldSize2
        CapHeight = OldHeigh
        Material = OldMaterial
        Target = OldTarget
    End Sub

    Private Sub tysSuggestor_ApplyNewType(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles tysSuggestor.ApplyNewType
        'Preserve values
        Dim OldSize1 = Size1
        Dim OldSize2 = Size2
        Dim OldHeigh = CapHeight
        Dim OldTarget = Target
        Dim OldMaterial = Material
        'Show dialog
        Dim win As New winCreateNewType(Me.Context)
        win.DataContext = tysSuggestor.SelectedNewType
        win.ShowDialog(Me)
        If win.DialogResult Then
            'Apply new type
            DirectCast(cmbCapType.ItemsSource, IList(Of CapType)).Add(win.CreatedType)
            cmbCapType.Items.Refresh() 'TODO: THis is workaround. Source shall refresh automatically
            CapTypeSelection = CreatableItemSelection.SelectedItem
            Me.CapType = win.CreatedType
            'Preserved values
            Size1 = OldSize1
            Size2 = OldSize2
            CapHeight = OldHeigh
            Material = OldMaterial
            Target = OldTarget
        End If
    End Sub
#End Region
#Region "txtFavoriteCharacters"
    Private Sub txtFavoriteCharacters_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles txtFavoriteCharacters.LostFocus
        txtFavoriteCharacters.IsReadOnly = True
    End Sub


    Private Sub txtFavoriteCharacters_PreviewMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles txtFavoriteCharacters.PreviewMouseDown
        If e.ChangedButton = MouseButton.Left AndAlso e.ClickCount = 2 Then
            txtFavoriteCharacters.IsReadOnly = Not txtFavoriteCharacters.IsReadOnly
            e.Handled = True
        End If
    End Sub

    Private Sub txtFavoriteCharacters_PreviewDragEnter(ByVal sender As Object, ByVal e As System.Windows.DragEventArgs) Handles txtFavoriteCharacters.PreviewDragEnter
        txtFavoriteCharacters.IsReadOnly = False
    End Sub
#End Region

    ''' <summary>Shows total count of caps</summary>
    Private Sub ShowCount()
        If Context IsNot Nothing AndAlso Not Context.IsDisposed Then _
            lblCapsCount.Content = Context.Caps.Count
    End Sub


    Private Sub cmbSign_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)

    End Sub
End Class

''' <summary>Allows to distinguish image already in database and a new image</summary>
''' <remarks><see cref="NewImage.RelativePath"/> does not store relative path, but absolute path.</remarks>
Public Class NewImage : Inherits Image
    ''' <summary>CTor with path</summary>
    ''' <param name="Path">Path to image</param>
    Public Sub New(ByVal Path As String)
        MyBase.New()
        Me.RelativePath = Path
    End Sub
End Class

