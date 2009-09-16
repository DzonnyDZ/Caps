Imports System.ComponentModel
Imports System.Data.SqlClient

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
            OnPropertyChanged("RequiresFileNameVisibility")
            OnPropertyChanged("DatabaseTypeDesc")
        End Set
    End Property
#Region "Database type helpers"
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property DatabaseTypeIsAttachFile() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.AttachFile
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.AttachFile
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property DatabaseTypeIsServerDatabase() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.ServerDatabase
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.ServerDatabase
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property DatabaseTypeIsUserInstance() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.UserInstance
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.UserInstance
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property FileSelectionVisibility() As Visibility
        Get
            Return If(DatabaseTypeIsAttachFile OrElse DatabaseTypeIsUserInstance, Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
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
            OnPropertyChanged("ConnectionTypeDesc")
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
            OnPropertyChanged("ConnectionTypeDesc")
        End Set
    End Property
#Region "Connection type helpers"
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsFileConnectionTypeNew() As Boolean
        Get
            Return FileConnectionType = Console.FileConnectionType.New
        End Get
        Set(ByVal value As Boolean)
            If value Then FileConnectionType = Console.FileConnectionType.New
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsFileConnectionTypeExisting() As Boolean
        Get
            Return FileConnectionType = Console.FileConnectionType.Existing
        End Get
        Set(ByVal value As Boolean)
            If value Then FileConnectionType = Console.FileConnectionType.Existing
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsDatabaseConnectionTypeNew() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.New
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.New
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsDatabaseConnectionTypeExisting() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.Existing
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.Existing
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsDatabaseConnectionTypeEmpty() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.Empty
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.Empty
        End Set
    End Property
#End Region
#Region "Step 3 helpers"
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property RequiresFileNameVisibility() As Visibility
        Get
            Return If(DatabaseType = Console.DatabaseType.ServerDatabase, Visibility.Collapsed, Visibility.Visible)
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property RequiresDatabaseNameVisibility() As Visibility
        Get
            Return If(DatabaseType = Console.DatabaseType.ServerDatabase, Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property UseSqlServerAuth() As Boolean
        Get
            Return Not UseIntegratedSecurity
        End Get
    End Property
#End Region
#Region "Step 4 helpers"
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property DatabseTypeDesc$()
        Get
            Select Case DatabaseType
                Case Console.DatabaseType.AttachFile : Return "Attach datbase file to server. File with given path must exist on server!"
                Case Console.DatabaseType.ServerDatabase : Return "Connect to database server"
                Case Console.DatabaseType.UserInstance : Return "Use SQL Server Express User Instances"
                Case Else : Return "unknown"
            End Select
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property ConnectionTypeDesc$()
        Get
            Select Case DatabaseType
                Case Console.DatabaseType.AttachFile, Console.DatabaseType.UserInstance
                    Select Case FileConnectionType
                        Case Console.FileConnectionType.Existing : Return "Use existing database file"
                        Case Console.FileConnectionType.New : Return "Create new database file"
                    End Select
                Case Console.DatabaseType.ServerDatabase
                    Select Case DatabaseConnectionType
                        Case Console.DatabaseConnectionType.Existing : Return "Use existing datbase"
                        Case Console.DatabaseConnectionType.Empty : Return "Connect to existing empty database and create required structures there"
                        Case Console.DatabaseConnectionType.New : Return "Create a new database"
                    End Select
            End Select
            Return "unknown"
        End Get
    End Property
#End Region
    Private _UseIntegratedSecurity As Boolean = True
    <DefaultValue(True)> _
    Public Property UseIntegratedSecurity() As Boolean
        Get
            Return _UseIntegratedSecurity
        End Get
        Set(ByVal value As Boolean)
            _UseIntegratedSecurity = value
            OnPropertyChanged("UseIntegratedSecurity")
            OnPropertyChanged("UseSqlServerAuth")
        End Set
    End Property
    Private _FilePath$
    Private _ServerName$
    Private _UserName$
    Private _Password$
    Private _DatabaseName$

    Public Property FilePath$()
        Get
            Return _FilePath
        End Get
        Set(ByVal value$)
            _FilePath = value
            OnPropertyChanged("FilePath")
        End Set
    End Property
    Public Property ServerName$()
        Get
            Return _ServerName
        End Get
        Set(ByVal value$)
            _ServerName = value
            OnPropertyChanged("ServerName")
        End Set
    End Property
    Public Property UserName$()
        Get
            Return _UserName
        End Get
        Set(ByVal value$)
            _UserName = value
            OnPropertyChanged("UserName")
        End Set
    End Property
    Public Property Password$()
        Get
            Return _Password
        End Get
        Set(ByVal value$)
            _Password = value
            OnPropertyChanged("Password")
        End Set
    End Property
    Public Property DatabaseName$()
        Get
            Return _DatabaseName
        End Get
        Set(ByVal value$)
            _DatabaseName = value
            OnPropertyChanged("DatabaseName")
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(ByVal PropertyName$)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub

    Public Function GetFinalConnectionString() As SqlConnectionStringBuilder
        Dim b As New SqlConnectionStringBuilder
        With b
            b.DataSource = ServerName
            If UseIntegratedSecurity Then
                b.IntegratedSecurity = True
            Else
                b.IntegratedSecurity = False
                b.UserID = UserName
                b.Password = Password
            End If
            Select Case Me.DatabaseType
                Case Console.DatabaseType.ServerDatabase
                    b.InitialCatalog = DatabaseName
                Case Console.DatabaseType.AttachFile
                    b.AttachDBFilename = FilePath
                    b.InitialCatalog = FilePath
                Case Console.DatabaseType.UserInstance
                    b.UserInstance = True
                    b.AttachDBFilename = FilePath
                    b.InitialCatalog = FilePath
            End Select
        End With
        Return b
    End Function
End Class