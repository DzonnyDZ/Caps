Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows.Navigation
Imports System.ComponentModel

Public Delegate Sub WizardReturnEventHandler(ByVal sender As Object, ByVal e As WizardReturnEventArgs)
Public Class winDatabaseWizard
    Inherits NavigationWindow
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

    Public ReadOnly Property WizardData() As WizardData
        Get
            Return Me._wizardData
        End Get
    End Property

    Private _wizardData As WizardData
End Class


Public Class WizardLauncher
    Inherits PageFunction(Of Boolean)

    Public Event WizardReturn As WizardReturnEventHandler

    Public Sub New()
        Me.wizardData = New WizardData
    End Sub

    Protected Overrides Sub Start()
        MyBase.Start()
        MyBase.KeepAlive = True
        Dim firstPage As New pgServerOrFile(Me.wizardData)
        AddHandler firstPage.Return, New ReturnEventHandler(Of Boolean)(AddressOf Me.wizardPage_Return)
        MyBase.NavigationService.Navigate(firstPage)
    End Sub

    Public Sub wizardPage_Return(ByVal sender As Object, ByVal e As ReturnEventArgs(Of Boolean))
        RaiseEvent WizardReturn(Me, New WizardReturnEventArgs(e.Result, Me.wizardData))
        Me.OnReturn(Nothing)
    End Sub

    Private wizardData As WizardData

End Class



Public Class WizardReturnEventArgs

    Public Sub New(ByVal result As Boolean, ByVal data As Object)
        Me._result = result
        Me._data = data
    End Sub

    Public ReadOnly Property Data() As Object
        Get
            Return Me._data
        End Get
    End Property

    Public ReadOnly Property Result() As Boolean
        Get
            Return Me._result
        End Get
    End Property

    Private _data As Object
    Private _result As Boolean

End Class



Public Enum DatabaseType
    UserInstance
    AttachFile
    ServerDatabase
End Enum

Public Enum FileConnectionType
    [New]
    Existing
End Enum

Public Enum DatabaseConnectionType
    [New]
    Existing
    Empty
End Enum

Public Class WizardData
    Implements INotifyPropertyChanged





    Private _DatabaseType As DatabaseType = Console.DatabaseType.UserInstance
    Public Property DatabaseType() As DatabaseType
        Get
            Return _DatabaseType
        End Get
        Set(ByVal value As DatabaseType)
            _DatabaseType = value
            OnPropertyChanged("DatabaseType")
            OnPropertyChanged("DatabaseTypeIsAttachFile")
            OnPropertyChanged("DatabaseTypeIsServerDatabase")
            OnPropertyChanged("DatabaseTypeIsUserInstance")
            OnPropertyChanged("FileSelectionVisibility")
            OnPropertyChanged("DatabaseSelectionVisibility")
        End Set
    End Property
#Region "Database type helpers"
    Public Property DatabaseTypeIsAttachFile() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.AttachFile
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.AttachFile
        End Set
    End Property
    Public Property DatabaseTypeIsServerDatabase() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.ServerDatabase
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.ServerDatabase
        End Set
    End Property
    Public Property DatabaseTypeIsUserInstance() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.UserInstance
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.UserInstance
        End Set
    End Property

    Public ReadOnly Property FileSelectionVisibility() As Visibility
        Get
            Return If(DatabaseTypeIsAttachFile OrElse DatabaseTypeIsUserInstance, Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
    Public ReadOnly Property DatabaseSelectionVisibility() As Visibility
        Get
            Return If(DatabaseTypeIsServerDatabase, Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
#End Region
    Private _FileConnectionType As FileConnectionType = Console.FileConnectionType.Existing
    Private _DatabaseConnectionType As DatabaseConnectionType = Console.DatabaseConnectionType.Existing
    Public Property FileConnectionType() As FileConnectionType
        Get
            Return _FileConnectionType
        End Get
        Set(ByVal value As FileConnectionType)
            _FileConnectionType = value
            OnPropertyChanged("FileConnectionType")
            OnPropertyChanged("IsFileConnectionTypeExisting")
            OnPropertyChanged("IsFileConnectionTypeNew")
        End Set
    End Property
    Public Property DatabaseConnectionType() As DatabaseConnectionType
        Get
            Return _DatabaseConnectionType
        End Get
        Set(ByVal value As DatabaseConnectionType)
            _DatabaseConnectionType = value
            OnPropertyChanged("DatabaseConnectionType")
            OnPropertyChanged("IsDatabaseConnectionTypeExisting")
            OnPropertyChanged("IsDatabaseConnectionTypeEmpty")
            OnPropertyChanged("IsDatabaseConnectionTypeNew")
        End Set
    End Property
#Region "Connection type helpers"
    Public Property IsFileConnectionTypeNew() As Boolean
        Get
            Return FileConnectionType = Console.FileConnectionType.New
        End Get
        Set(ByVal value As Boolean)
            If value Then FileConnectionType = Console.FileConnectionType.New
        End Set
    End Property
    Public Property IsFileConnectionTypeExisting() As Boolean
        Get
            Return FileConnectionType = Console.FileConnectionType.Existing
        End Get
        Set(ByVal value As Boolean)
            If value Then FileConnectionType = Console.FileConnectionType.Existing
        End Set
    End Property
    Public Property IsDatabaseConnectionTypeNew() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.New
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.New
        End Set
    End Property
    Public Property IsDatabaseConnectionTypeExisting() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.Existing
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.Existing
        End Set
    End Property
    Public Property IsDatabaseConnectionTypeEmpty() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.Empty
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.Empty
        End Set
    End Property
#End Region
    Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(ByVal PropertyName$)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub
End Class


