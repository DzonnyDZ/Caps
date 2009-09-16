Imports System
Imports System.Windows
Imports System.Windows.Controls

Public Class pgCredentials
    Inherits PageFunction(Of Boolean)

    Public Sub New(ByVal wizardData As WizardData)
        Me.InitializeComponent()
        MyBase.DataContext = wizardData
    End Sub

    Private Sub backButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        MyBase.NavigationService.GoBack()
    End Sub

    Private Sub cancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(False))
    End Sub

    Private Sub finishButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(True))
    End Sub

    Private Sub nextButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles nextButton.Click
        Dim nextPage As New pgSummary(DirectCast(MyBase.DataContext, WizardData))
        AddHandler nextPage.Return, New ReturnEventHandler(Of Boolean)(AddressOf Me.wizardPage_Return)
        MyBase.NavigationService.Navigate(nextPage)
    End Sub
    Public Sub wizardPage_Return(ByVal sender As Object, ByVal e As ReturnEventArgs(Of Boolean))
        Me.OnReturn(e)
    End Sub
End Class

