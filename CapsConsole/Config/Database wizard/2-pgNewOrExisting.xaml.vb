Imports System
Imports System.Windows
Imports System.Windows.Controls

''' <summary>A wizard page used to let usere chose whether to use existing or create a new database</summary>
Public Class pgNewOrExisting
    Inherits PageFunction(Of Boolean)

    ''' <summary>CTor - creates a new instance of the <see cref="pgNewOrExisting"/> class</summary>
    ''' <param name="wizardData">Wizard data partially initialized by previous steps</param>
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

    Private Sub nextButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim nextPage As New pgCredentials(DirectCast(MyBase.DataContext, WizardData))
        AddHandler nextPage.Return, New ReturnEventHandler(Of Boolean)(AddressOf Me.wizardPage_Return)
        MyBase.NavigationService.Navigate(nextPage)
    End Sub

    Public Sub wizardPage_Return(ByVal sender As Object, ByVal e As ReturnEventArgs(Of Boolean))
        Me.OnReturn(e)
    End Sub

End Class

