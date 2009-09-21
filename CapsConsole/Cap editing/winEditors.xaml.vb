﻿Imports System.Data.SqlClient
Imports mBox = Tools.WindowsT.IndependentT.MessageBox

Partial Public Class winEditors
    Private CapsContext As New CapsDataDataContext(Main.Connection)

    Private Sub winEditors_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        CapsContext.Dispose()
    End Sub
    Private Sub winEditors_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
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

        cmcTypes_Shape.ItemsSource = CapsContext.Shapes
        cmcTypes_Material.ItemsSource = CapsContext.Materials
        cmcTypes_MainType.ItemsSource = CapsContext.MainTypes
        cmcProduct_Company.ItemsSource = CapsContext.Companies
        cmcProduct_ProductType.ItemsSource = CapsContext.ProductTypes
        cmcStorage_StorageType.ItemsSource = CapsContext.StorageTypes
        cmcTypes_Target.ItemsSource = CapsContext.Targets
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            CapsContext.SubmitChanges()
        Catch ex As Exception
            mBox.Error_X(ex)
            Exit Sub
        End Try
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnApply.Click
        Try
            CapsContext.SubmitChanges()
        Catch ex As Exception
            mBox.Error_X(ex)
        End Try
    End Sub

End Class