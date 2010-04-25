Imports System.Data.SqlClient
Imports Caps.Data

Partial Public Class winEditors
    Private CapsContext As New CapsDataContext(Main.EntityConnection)

    Private Sub winEditors_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        My.Settings.winEditorsLoc = Me.GetWindowPosition
        CapsContext.Dispose()
    End Sub
    Private Sub winEditors_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.SetWindowPosition(My.Settings.winEditorsLoc)
        dgrCompanies.ItemsSource = CapsContext.Companies
        dgrCategories.ItemsSource = CapsContext.Categories
        dgrMainTypes.ItemsSource = CapsContext.MainTypes
        dgrMaterials.ItemsSource = CapsContext.Materials
        dgrProducts.ItemsSource = CapsContext.Products
        dgrProductTypes.ItemsSource = CapsContext.ProductTypes
        dgrShapes.ItemsSource = CapsContext.Shapes
        dgrTypes.ItemsSource = CapsContext.CapTypes
        dgrStorages.ItemsSource = CapsContext.Storages
        dgrStorageTypes.ItemsSource = CapsContext.StorageTypes
        dgrTargets.ItemsSource = CapsContext.Targets
        dgrSigns.ItemsSource = CapsContext.CapSigns

        cmcTypes_Shape.ItemsSource = CapsContext.Shapes
        cmcTypes_Material.ItemsSource = CapsContext.Materials
        cmcTypes_MainType.ItemsSource = CapsContext.MainTypes
        cmcProduct_Company.ItemsSource = CapsContext.Companies
        cmcProduct_ProductType.ItemsSource = CapsContext.ProductTypes
        cmcStorage_StorageType.ItemsSource = CapsContext.StorageTypes
        cmcTypes_Target.ItemsSource = CapsContext.Targets
        cmcStorage_Parent.ItemsSource = CapsContext.Storages
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            CapsContext.SaveChanges()
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnApply.Click
        Try
            CapsContext.SaveChanges()
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
        End Try
    End Sub

End Class
