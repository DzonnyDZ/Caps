Imports Tools.TypeTools
Imports System.ComponentModel
Imports Caps.Data
Imports System.Data.Objects.DataClasses

''' <summary>Base class of generic dialogs used to create a new instance of classes implementing <see cref="ISimpleObject"/> interface</summary>
<EditorBrowsable(ComponentModel.EditorBrowsableState.Advanced)>
Partial Public MustInherit Class winNewSimpleBase
    Implements IDisposable
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
    ''' <summary>When overriden in derived class gets enumerated value representing type of object cxurrently edited by derived class instance</summary>
    Public MustOverride ReadOnly Property Type As SimpleTypes
    ''' <summary>CTor - creates new instance of the <see cref="winNewSimpleBase"/></summary>
    Public Sub New()
        InitializeComponent()
        Select Case Type
            Case SimpleTypes.Category : Me.Title = My.Resources.txt_NewCategory
            Case SimpleTypes.Company : Me.Title = My.Resources.txt_NewCompany
            Case SimpleTypes.Material : Me.Title = My.Resources.txt_NewMaterial
            Case SimpleTypes.ProductType : Me.Title = My.Resources.txt_NewProductType : chkIsDrink.Visibility = Windows.Visibility.Visible : chkIsAlcoholic.Visibility = Windows.Visibility.Visible
            Case SimpleTypes.StorageType : Me.Title = My.Resources.txt_NewStorageType
            Case SimpleTypes.Target : Me.Title = My.Resources.txt_NewTarget
        End Select
        UnderConstruction = False
        chkIsDrink_Checked(chkIsDrink, New RoutedEventArgs)
    End Sub
    ''' <summary>True when windows is being constructed (CTor is on call stack)</summary>
    Protected ReadOnly UnderConstruction As Boolean = True
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

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

''' <summary>Allows creation of several simple objects</summary>
Public Class winNewSimple(Of T As {ISimpleObject, EntityObject, New})
    Inherits winNewSimpleBase

    ''' <summary>Type of object to be created</summary>
    Public Overrides ReadOnly Property Type As winNewSimpleBase.SimpleTypes
        Get
            If GetType(T).IsAssignableFrom(GetType(Category)) Then : Return SimpleTypes.Category
            ElseIf GetType(T).IsAssignableFrom(GetType(Company)) Then : Return SimpleTypes.Company
            ElseIf GetType(T).IsAssignableFrom(GetType(Material)) Then : Return SimpleTypes.Material
            ElseIf GetType(T).IsAssignableFrom(GetType(ProductType)) Then : Return SimpleTypes.ProductType
            ElseIf GetType(T).IsAssignableFrom(GetType(StorageType)) Then : Return SimpleTypes.StorageType
            ElseIf GetType(T).IsAssignableFrom(GetType(Target)) Then : Return SimpleTypes.Target
            Else : Return Integer.MinValue
            End If
        End Get
    End Property

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            NewObject = New T() With {.Name = txtName.Text, .Description = txtDescription.Text}
            If TypeOf NewObject Is ProductType Then
                With DirectCast(CObj(NewObject), ProductType)
                    .IsDrink = chkIsDrink.IsChecked
                    .IsAlcoholic = chkIsAlcoholic.IsChecked
                End With
            End If
            Context.AddObject(NewObject)
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

End Class
