Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports System.Globalization
Imports System.Reflection
Imports Tools.ReflectionT, Tools.ExtensionsT, Tools

Partial Public Class winSettings

    Private Sub winSettings_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        txtImageRoot.Text = My.Settings.ImageRoot

        Dim selectedCulture As Object = Nothing
        Dim List As New List(Of Object)
        List.Add(New With {.NativeName = My.Resources.DefaultLanguage, .DisplayName = My.Resources.DefaultLanguage, .EnglishName = "Default"})
        Dim neutralResourcesLanguageAttr = GetType(winSettings).Assembly.GetAttribute(Of System.Resources.NeutralResourcesLanguageAttribute)()
        If My.Settings.Language = "" Then selectedCulture = List(0)
        Try
            If neutralResourcesLanguageAttr IsNot Nothing Then
                Dim NeutralCulture = CultureInfo.GetCultureInfo(neutralResourcesLanguageAttr.CultureName)
                List.Add(NeutralCulture)
                If NeutralCulture.Name = My.Settings.Language Then selectedCulture = NeutralCulture
            End If
        Catch : End Try
        Dim dir As String = IO.Path.GetDirectoryName(GetType(winSettings).Assembly.Location)
        For Each subDir In IO.Directory.EnumerateDirectories(dir)
            Try
                Dim ci = CultureInfo.GetCultureInfo(IO.Path.GetFileName(subDir))
                If ci Is Nothing Then Continue For
                Dim sa = GetType(winSettings).Assembly.GetSatelliteAssembly(ci)
                List.Add(ci)
                If IO.Path.GetFileName(subDir).ToLowerInvariant = My.Settings.Language.ToLowerInvariant Then selectedCulture = ci
            Catch : End Try
        Next
        If selectedCulture Is Nothing Then
            Try
                Dim missingCulture = CultureInfo.GetCultureInfo(My.Settings.Language)
                List.Add(missingCulture)
                selectedCulture = missingCulture
            Catch :End Try
        End If
        cmbLanguage.ItemsSource = List
        If selectedCulture IsNot Nothing Then cmbLanguage.SelectedItem = selectedCulture
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        If Not IO.Directory.Exists(txtImageRoot.Text) Then
            mBox.Modal_PTIW(My.Resources.msg_ImageRootPathError, My.Resources.txt_ImageRoot, Tools.WindowsT.IndependentT.MessageBox.MessageBoxIcons.Exclamation, Me)
            Exit Sub
        End If
        My.Settings.ImageRoot = txtImageRoot.Text

        If TypeOf cmbLanguage.SelectedItem Is CultureInfo Then
            My.Settings.Language = DirectCast(cmbLanguage.SelectedItem, CultureInfo).Name
        ElseIf cmbLanguage.SelectedItem IsNot Nothing Then
            My.Settings.Language = Nothing
        End If

        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub btnImageRoot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnImageRoot.Click
        Dim dlg As New Forms.FolderBrowserDialog
        Try
            dlg.SelectedPath = txtImageRoot.Text
        Catch : End Try
        If dlg.ShowDialog = Forms.DialogResult.OK Then
            txtImageRoot.Text = dlg.SelectedPath
        End If
    End Sub
End Class
