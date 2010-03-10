Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Caps.Data

''' <summary>Allows creation of several simple objects</summary>
Partial Public Class winNewSimple
    Implements IDisposable
    ''' <summary>Type of object to be created</summary>
    Private ReadOnly Type As SimpleTypes
    ''' <summary>Data context</summary>
    Private ReadOnly Context As CapsDataContext
    ''' <summary>True when windows is being constructed (CTor is on call stack)</summary>
    Private ReadOnly UnderConstruction As Boolean = True
    ''' <summary>CTor</summary>
    ''' <param name="Type">Type of item to add</param>
    ''' <exception cref="InvalidEnumArgumentException"><paramref name="Type"/> is not member of <see cref="SimpleTypes"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New(ByVal Type As SimpleTypes)
        If Not Type.IsDefined Then Throw New InvalidEnumArgumentException("Type", Type, Type.GetType)
        Me.Type = Type
        InitializeComponent()
        Select Case Type
            Case SimpleTypes.Category : Me.Title = My.Resources.txt_NewCategory
            Case SimpleTypes.Company : Me.Title = My.Resources.txt_NewCompany
            Case SimpleTypes.Material : Me.Title = My.Resources.txt_NewMaterial
            Case SimpleTypes.ProductType : Me.Title = My.Resources.txt_NewProductType : chkIsDrink.Visibility = Windows.Visibility.Visible : chkIsAlcoholic.Visibility = Windows.Visibility.Visible
            Case SimpleTypes.StorageType : Me.Title = My.Resources.txt_NewStorageType
            Case SimpleTypes.Target : Me.Title = My.Resources.txt_NewTarget
        End Select
        Me.Context = New CapsDataContext(Main.Connection)
        UnderConstruction = False
        chkIsDrink_Checked(chkIsDrink, New RoutedEventArgs)
    End Sub
    ''' <summary>Contains value of the <see cref="NewObject"/> property</summary>
    Private _NewObject As ISimpleObject
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Dim DeleteOnSubmit As Action(Of Object)
        Try
            Select Case Type
                Case SimpleTypes.Material
                    _NewObject = New Material() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    Context.Materials.AddObject(_NewObject)
                    DeleteOnSubmit = AddressOf Context.Materials.DeleteObject
                Case SimpleTypes.Category
                    _NewObject = New Category() With {.CategoryName = txtName.Text, .Description = txtDescription.Text}
                    Context.Categories.AddObject(_NewObject)
                    DeleteOnSubmit = AddressOf Context.Categories.DeleteObject
                Case SimpleTypes.Company
                    _NewObject = New Company() With {.CompanyName = txtName.Text, .Description = txtDescription.Text}
                    Context.Companies.AddObject(_NewObject)
                    DeleteOnSubmit = AddressOf Context.Companies.DeleteObject
                Case SimpleTypes.ProductType
                    _NewObject = New ProductType() With {.ProductTypeName = txtName.Text, .Description = txtDescription.Text, .IsDrink = chkIsDrink.IsChecked, .IsAlcoholic = chkIsAlcoholic.IsChecked}
                    Context.ProductTypes.AddObject(_NewObject)
                    DeleteOnSubmit = AddressOf Context.ProductTypes.DeleteObject
                Case SimpleTypes.StorageType
                    _NewObject = New StorageType() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    Context.StorageTypes.AddObject(_NewObject)
                    DeleteOnSubmit = AddressOf Context.StorageTypes.DeleteObject
                Case SimpleTypes.Target
                    _NewObject = New Target() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    Context.Targets.AddObject(_NewObject)
                    DeleteOnSubmit = AddressOf Context.Targets.DeleteObject
                Case Else
                    Throw New InvalidOperationException(My.Resources.err_UnknownSimpleObject.f(Type))
            End Select
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            If DeleteOnSubmit IsNot Nothing Then DeleteOnSubmit(_NewObject)
            Exit Sub
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    ''' <summary>Gets the object created by this instance</summary>
    Public ReadOnly Property NewObject() As ISimpleObject
        Get
            Return _NewObject
        End Get
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    ''' <summary>Types that can be created by <see cref="winNewSimple"/>.</summary>
    Public Enum SimpleTypes
        ''' <summary><see cref="Data.Material"/></summary>
        Material
        ''' <summary><see cref="Data.Category"/></summary>
        Category
        ''' <summary><see cref="Data.Company"/></summary>
        Company
        ''' <summary><see cref="Data.ProductType"/></summary>
        ProductType
        ''' <summary><see cref="Data.StorageType"/></summary>
        StorageType
        ''' <summary><see cref="Target"/></summary>
        Target
    End Enum

    Private Sub chkIsDrink_Checked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles chkIsDrink.Checked, chkIsDrink.Unchecked, chkIsDrink.Indeterminate
        If UnderConstruction Then Exit Sub
        If chkIsDrink.IsChecked.HasValue AndAlso chkIsDrink.IsChecked.Value Then
            chkIsAlcoholic.IsEnabled = True
        ElseIf chkIsDrink.IsChecked.HasValue Then
            chkIsAlcoholic.IsEnabled = False
            chkIsAlcoholic.IsChecked = False
        Else
            chkIsAlcoholic.IsEnabled = False
            chkIsAlcoholic.IsChecked = New Boolean?
        End If
    End Sub
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
