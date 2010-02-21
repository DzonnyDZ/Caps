Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Caps.Data

Partial Public Class winNewSimple

    Private Type As SimpleTypes
    Private Context As CapsDataDataContext
    Private UnderConstruction As Boolean = True
    ''' <summary>CTor</summary>
    ''' <param name="Type">Type of item to add</param>
    ''' <param name="Context">Data context</param>
    ''' <exception cref="InvalidEnumArgumentException"><paramref name="Type"/> is not member of <see cref="SimpleTypes"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New(ByVal Type As SimpleTypes, ByVal Context As CapsDataDataContext)
        If Not Type.IsDefined Then Throw New InvalidEnumArgumentException("Type", Type, Type.GetType)
        If Context Is Nothing Then Throw New ArgumentNullException("Context")
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
        Me.Context = Context
        UnderConstruction = False
        chkIsDrink_Checked(chkIsDrink, New RoutedEventArgs)
    End Sub
    Private _NewObject As Object
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Dim tbl As System.Data.Linq.ITable
        Try
            Select Case Type
                Case SimpleTypes.Material
                    _NewObject = New Material() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    tbl = Context.Materials
                Case SimpleTypes.Category
                    _NewObject = New Category() With {.CategoryName = txtName.Text, .Description = txtDescription.Text}
                    tbl = Context.Categories
                Case SimpleTypes.Company
                    _NewObject = New Company() With {.CompanyName = txtName.Text, .Description = txtDescription.Text}
                    tbl = Context.Companies
                Case SimpleTypes.ProductType
                    _NewObject = New ProductType() With {.ProductTypeName = txtName.Text, .Description = txtDescription.Text, .IsDrink = chkIsDrink.IsChecked, .IsAlcoholic = chkIsAlcoholic.IsChecked}
                    tbl = Context.ProductTypes
                Case SimpleTypes.StorageType
                    _NewObject = New StorageType() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    tbl = Context.StorageTypes
                Case SimpleTypes.Target
                    _NewObject = New Target() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    tbl = Context.Targets
                Case Else
                    Throw New InvalidOperationException(My.Resources.err_UnknownSimpleObject.f(Type))
            End Select
            tbl.InsertOnSubmit(_NewObject)
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            tbl.DeleteOnSubmit(_NewObject)
            Exit Sub
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    Public ReadOnly Property NewObject() As Object
        Get
            Return _NewObject
        End Get
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub


    Public Enum SimpleTypes
        Material
        Category
        Company
        ProductType
        StorageType
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
End Class
