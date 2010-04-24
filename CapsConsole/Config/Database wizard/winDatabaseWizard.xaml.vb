Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows.Navigation
Imports System.ComponentModel
Imports System.Data.SqlClient

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
    Private WithEvents _wizardData As WizardData

    Private _FinalConnectionString As SqlConnectionStringBuilder
    Private _ImageRoot As String
    ''' <summary>Once wizard is successfully finished gets connection string to connect to database selected or created in wizard</summary>
    Public ReadOnly Property FinalConnectionString() As SqlConnectionStringBuilder
        Get
            Return _FinalConnectionString
        End Get
    End Property
    ''' <summary>Once wizard is successfully finished gets image root directory (only for newly created databases when images are stored in file system)</summary>
    Public ReadOnly Property ImageRoot() As String
        Get
            Return _ImageRoot
        End Get
    End Property
    ''' <summary>Handles the <see cref="WizardData"/>.<see cref="WizardData.Finished">Finished</see> event</summary>
    ''' <param name="connectionString">Connection string to connect to database selected or created in wizard</param>
    ''' <param name="imageRoot">gets image root directory (only for newly created databases when images are stored in file system)</param>
    ''' <exception cref="ArgumentNullException"><paramref name="connectionString"/> is null</exception>
    Private Sub OnFinished(ByVal connectionString As SqlConnectionStringBuilder, ByVal imageRoot$) Handles _wizardData.Finished
        If connectionString Is Nothing Then Throw New ArgumentNullException("connectionString")
        _FinalConnectionString = connectionString
        _ImageRoot = imageRoot
    End Sub
End Class




