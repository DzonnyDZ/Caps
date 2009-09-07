Imports Tools.CollectionsT.GenericT, Tools.ExtensionsT

''' <summary>Picks common colors, allows to pick any color</summary>
Partial Public Class ColorPicker
    ''' <summary>CTor</summary>
    Public Sub New()
        InitializeComponent()
        OldSelectedIndex = cmbColors.SelectedIndex
        For Each item As ComboBoxItem In cmbColors.Items
            If item Is cmiMoreColors Then Continue For
            item.Foreground = New SolidColorBrush(System.Windows.Media.Color.FromRgb( _
                              Not DirectCast(item.Background, SolidColorBrush).Color.R, _
                              Not DirectCast(item.Background, SolidColorBrush).Color.G, _
                              Not DirectCast(item.Background, SolidColorBrush).Color.B))
        Next
        cmbColors.SelectedItem = cmiNull
    End Sub
    ''' <summary>Index of previuously selected color</summary>
    Private OldSelectedIndex As Integer
    ''' <summary>Currently selected color</summary>
    Public Property Color() As Color?
        Get
            Return GetValue(ColorProperty)
        End Get
        Set(ByVal value As Color?)
            SetValue(ColorProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the  <see cref="Color"/> property</summary>
    Public Shared ReadOnly ColorProperty As DependencyProperty = DependencyProperty.Register("Color", GetType(Color?), GetType(ColorPicker), _
                           New FrameworkPropertyMetadata(New Color?(), AddressOf OnColorChanged, AddressOf CoerceColorValue))
    ''' <summary>Coerces value of the <see cref="Color"/> property</summary>
    Private Shared Function CoerceColorValue(ByVal d As System.Windows.DependencyObject, ByVal baseValue As Object) As Object
        With DirectCast(d, ColorPicker)
            If TypeOf baseValue Is Color Then Return New Color?(baseValue)
            If TypeOf baseValue Is Color? Then
                If Not .AllowNull AndAlso Not DirectCast(baseValue, Color?).HasValue Then Throw New ArgumentException("Null color is not allowed")
                Return baseValue
            End If
            If baseValue Is Nothing Then
                If Not .AllowNull Then Throw New ArgumentException("Null color is not allowed")
                Return New Color?
            End If
            Throw New Tools.TypeMismatchException("baseValue", baseValue, GetType(Color?), "Unexpected type: Expected {0} or {1}".f(GetType(Color).FullName, GetType(Color?).FullName))
        End With
    End Function


    ''' <summary>Gets or sets value indicating if Null can be selected</summary>
    Public Property AllowNull() As Boolean
        Get
            Return GetValue(AllowNullProperty)
        End Get

        Set(ByVal value As Boolean)
            SetValue(AllowNullProperty, value)
        End Set
    End Property
    ''' <summary>Metadata of the  <see cref="AllowNull"/> property</summary>
    Public Shared ReadOnly AllowNullProperty As DependencyProperty = DependencyProperty.Register("AllowNull", GetType(Boolean), GetType(ColorPicker), _
                           New FrameworkPropertyMetadata(True, AddressOf OnAllowNullChanged))
    ''' <summary>Caled when <see cref="Color"/> changes</summary>
    ''' <param name="d">A <see cref="ColorPicker"/> for which the color has changed</param>
    ''' <param name="e">Event arguments</param>
    Private Shared Sub OnAllowNullChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        DirectCast(d, ColorPicker).OnAllowNullChanged(e)
    End Sub
    ''' <summary>Called when <see cref="Color"/> changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnAllowNullChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If Not AllowNull Then
            Dim Null = cmbColors.SelectedItem Is cmiNull
            If cmbColors.Items.Contains(cmiNull) Then cmbColors.Items.Remove(cmiNull)
            If Null Then Color = Colors.Transparent
        Else
            If Not cmbColors.Items.Contains(cmiNull) Then cmbColors.Items.Add(cmiNull)
        End If
    End Sub


    '''' <summary>Contains value of the <see cref="AllowNull"/> property</summary>
    'Private _AllowNull As Boolean

    'Public Property AllowNull() As Boolean
    '    Get
    '        Return _AllowNull
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _AllowNull = value
    '        If AllowNull Then
    '            Dim Null = cmbColors.SelectedItem Is cmiNull
    '            If Not cmbColors.Items.Contains(cmiNull) Then cmbColors.Items.Add(cmiNull)
    '            If Null Then Color = Colors.Transparent
    '        Else
    '            If cmbColors.Items.Contains(cmiNull) Then cmbColors.Items.Remove(cmiNull)
    '        End If
    '    End Set
    'End Property
    ''' <summary>Caled when <see cref="Color"/> changes</summary>
    ''' <param name="d">A <see cref="ColorPicker"/> for which the color has changed</param>
    ''' <param name="e">Event arguments</param>
    Private Shared Sub OnColorChanged(ByVal d As System.Windows.DependencyObject, ByVal e As System.Windows.DependencyPropertyChangedEventArgs)
        DirectCast(d, ColorPicker).OnColorChanged(e)
    End Sub
    ''' <summary>Called when <see cref="Color"/> changes</summary>
    ''' <param name="e">Event arguments</param>
    Protected Overridable Sub OnColorChanged(ByVal e As DependencyPropertyChangedEventArgs)
        If Not Color.HasValue Then
            If AllowNull Then
                cmbColors.SelectedItem = cmiNull
            End If
        Else
            If cmbColors.SelectedItem IsNot Nothing AndAlso DirectCast(DirectCast(cmbColors.SelectedItem, ComboBoxItem).Background, SolidColorBrush).Color = Color Then Exit Sub
            For i As Integer = 0 To cmbColors.Items.Count - 1
                If cmbColors.Items(i) Is cmiMoreColors Then Continue For
                If DirectCast(DirectCast(cmbColors.Items(i), ComboBoxItem).Background, SolidColorBrush).Color = Color Then
                    cmbColors.SelectedIndex = i
                    Exit Sub
                End If
            Next
            cmbColors.Items.Insert(cmbColors.Items.IndexOf(cmiMoreColors), New ComboBoxItem With { _
                                          .Background = New SolidColorBrush(Color), _
                                          .Content = Color.ToString, _
                                          .Foreground = New SolidColorBrush(System.Windows.Media.Color.FromRgb( _
                                                        Not DirectCast(.Background, SolidColorBrush).Color.R, _
                                                        Not DirectCast(.Background, SolidColorBrush).Color.G, _
                                                        Not DirectCast(.Background, SolidColorBrush).Color.B))})
        End If
    End Sub

    Private Sub cmbColors_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles cmbColors.KeyDown
        If e.Key = Key.Delete AndAlso AllowNull Then Color = Nothing
    End Sub

    Private Sub cmbColors_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbColors.SelectionChanged
        If cmbColors.SelectedIndex < 0 Then Exit Sub
        If cmbColors.SelectedItem Is cmiNull Then
            OldSelectedIndex = cmbColors.SelectedIndex
            Color = Nothing
        ElseIf cmbColors.SelectedItem IsNot cmiMoreColors Then
            OldSelectedIndex = cmbColors.SelectedIndex
            Color = DirectCast(DirectCast(cmbColors.SelectedItem, ComboBoxItem).Background, SolidColorBrush).Color
        Else
            'Select color via dialog
            Dim cdl As New Forms.ColorDialog
            If cdl.ShowDialog = Forms.DialogResult.OK Then
                cmbColors.Items.Insert(cmbColors.Items.IndexOf(cmiMoreColors), New ComboBoxItem With { _
                                       .Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(cdl.Color.A, cdl.Color.R, cdl.Color.G, cdl.Color.B)), _
                                       .Content = DirectCast(.Background, SolidColorBrush).Color.ToString, _
                                       .Foreground = New SolidColorBrush(System.Windows.Media.Color.FromRgb( _
                                                     Not DirectCast(.Background, SolidColorBrush).Color.R, _
                                                     Not DirectCast(.Background, SolidColorBrush).Color.G, _
                                                     Not DirectCast(.Background, SolidColorBrush).Color.B))})
                cmbColors.SelectedIndex = cmbColors.Items.IndexOf(cmiMoreColors) - 1
            Else
                cmbColors.SelectedIndex = OldSelectedIndex
            End If
        End If
    End Sub
End Class
