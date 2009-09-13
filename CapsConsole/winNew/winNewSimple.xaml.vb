Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel, Caps.Console.DataAccess
Imports mBox = Tools.WindowsT.IndependentT.MessageBox

Partial Public Class winNewSimple

    Private Type As SimpleTypes
    Private Context As DataAccess.Entities
    ''' <summary>CTor</summary>
    ''' <param name="Type">Type of item to add</param>
    ''' <param name="Context">Data context</param>
    ''' <exception cref="InvalidEnumArgumentException"><paramref name="Type"/> is not member of <see cref="SimpleTypes"/></exception>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New(ByVal Type As SimpleTypes, ByVal Context As DataAccess.Entities)
        If Not Type.IsDefined Then Throw New InvalidEnumArgumentException("Type", Type, Type.GetType)
        If Context Is Nothing Then Throw New ArgumentNullException("Context")
        Me.Type = Type
        InitializeComponent()
        Select Case Type
            Case SimpleTypes.Category : Me.Title = My.Resources.txt_NewCategory
            Case SimpleTypes.Company : Me.Title = My.Resources.txt_NewCompany
            Case SimpleTypes.Material : Me.Title = My.Resources.txt_NewMaterial
            Case SimpleTypes.ProductType : Me.Title = My.Resources.txt_NewProductType
            Case SimpleTypes.StorageType : Me.Title = My.Resources.txt_NewStorageType
        End Select
        Me.Context = Context
    End Sub
    Private _NewObject As Global.System.Data.Objects.DataClasses.EntityObject
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        'Dim tbl As System.Data.Linq.ITable
        Try
            Select Case Type
                Case SimpleTypes.Material
                    _NewObject = New Material() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    Context.AddToMaterials(_NewObject)
                Case SimpleTypes.Category
                    _NewObject = New Category() With {.CategoryName = txtName.Text, .Description = txtDescription.Text}
                    Context.AddToCategories(_NewObject)
                Case SimpleTypes.Company
                    _NewObject = New Company() With {.CompanyName = txtName.Text, .Description = txtDescription.Text}
                    Context.AddToCompanies(_NewObject)
                Case SimpleTypes.ProductType
                    _NewObject = New ProductType() With {.ProductTypeName = txtName.Text, .Description = txtDescription.Text}
                    Context.AddToProductTypes(_NewObject)
                Case SimpleTypes.StorageType
                    _NewObject = New StorageType() With {.Name = txtName.Text, .Description = txtDescription.Text}
                    Context.AddToStorages(_NewObject)
                Case Else
                    Throw New InvalidOperationException(My.Resources.err_UnknownSimpleObject.f(Type))
            End Select
            'tbl.InsertOnSubmit(_NewObject)
        Catch ex As Exception
            mBox.Error_X(ex)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            mBox.Error_X(ex)
            Context.Detach(_NewObject)
            Exit Sub
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    Public ReadOnly Property NewObject() As Global.System.Data.Objects.DataClasses.EntityObject
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
    End Enum
End Class
