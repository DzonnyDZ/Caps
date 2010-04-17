Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows.Navigation
Imports System.ComponentModel

''' <summary>Delegate of event returning <see cref="WizardData"/></summary>
Public Delegate Sub WizardReturnEventHandler(ByVal sender As Object, ByVal e As WizardReturnEventArgs)

''' <summary>Whindo that shows database wizard</summary>
Public Class winDatabaseWizard
    Inherits NavigationWindow
    ''' <summary>CTor -  creates a new instance of the <see cref="winDatabaseWizard"/> class</summary>
    Public Sub New()
        InitializeComponent()
        Dim launcher As New WizardLauncher
        AddHandler launcher.WizardReturn, New WizardReturnEventHandler(AddressOf Me.wizardLauncher_WizardReturn)
        Navigate(launcher)
    End Sub

    Private Sub wizardLauncher_WizardReturn(ByVal sender As Object, ByVal e As WizardReturnEventArgs)
        Me._wizardData = TryCast(e.Data, WizardData)
        If Not MyBase.DialogResult.HasValue Then
            MyBase.DialogResult = New Nullable(Of Boolean)((e.Result = True))
        End If
    End Sub

    ''' <summary>Gets wizard data used by the wizard</summary>
    Public ReadOnly Property WizardData() As WizardData
        Get
            Return Me._wizardData
        End Get
    End Property
    ''' <summary>Contains value of the <see cref="WizardData"/> property</summary>
    Private _wizardData As WizardData
End Class




