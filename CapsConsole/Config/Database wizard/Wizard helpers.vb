Imports System.ComponentModel
Imports System.Data.SqlClient

''' <summary>Launches Database wizard</summary>
Public Class WizardLauncher
    Inherits PageFunction(Of Boolean)

    ''' <summary>Raised once wizard is completed</summary>
    Public Event WizardReturn As WizardReturnEventHandler

    ''' <summary>CTor - creates a new instance of the <see cref="WizardLauncher"/> class</summary>
    Public Sub New()
        Me.wizardData = New WizardData
    End Sub

    ''' <summary>Initializes a <see cref="T:System.Windows.Navigation.PageFunction`1" /> when it is navigated to for the first time.</summary>
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

    ''' <summary>A wizard data to be filled by the wizard</summary>
    Private wizardData As WizardData
End Class

''' <summary>Event arguments of the <see cref="WizardLauncher.WizardReturn"/> event</summary>
Public Class WizardReturnEventArgs
    ''' <summary>CTor - creates a new instance of the <see cref="WizardReturnEventArgs"/> class</summary>
    ''' <param name="result">Indicates if wizard was cancelled (false) or not (true)</param>
    ''' <param name="data">Wizard data</param>
    Public Sub New(ByVal result As Boolean, ByVal data As WizardData)
        Me._result = result
        Me._data = data
    End Sub

    ''' <summary>Gets a wizard data</summary>
    Public ReadOnly Property Data() As WizardData
        Get
            Return Me._data
        End Get
    End Property

    ''' <summary>Gets result of the wizard</summary>
    ''' <returns>True if a wizard was completed successfully, false whan it was cancelled by user</returns>
    Public ReadOnly Property Result() As Boolean
        Get
            Return Me._result
        End Get
    End Property
    ''' <summary>Contains value of the <see cref="Data"/> property</summary>
    Private _data As WizardData
    ''' <summary>Contains value of the <see cref="Result"/> property</summary>
    Private _result As Boolean
End Class

''' <summary>Types of database storage</summary>
Public Enum DatabaseType
    ''' <summary>SQL Server 2008 Express User instance</summary>
    UserInstance
    ''' <summary>Attach database file to SQL Server 2008</summary>
    AttachFile
    ''' <summary>SQL Server 2008 database</summary>
    ServerDatabase
End Enum

''' <summary>Types of database file</summary>
Public Enum FileConnectionType
    ''' <summary>Create a new database file</summary>
    [New]
    ''' <summary>Use an existing database file</summary>
    Existing
End Enum

''' <summary>Types of server database</summary>
Public Enum DatabaseConnectionType
    ''' <summary>Create a new server database</summary>
    [New]
    ''' <summary>Use existing server database</summary>
    Existing
    ''' <summary>Connect to existing server database and create tables etc. there</summary>
    Empty
End Enum

''' <summary>Data of Database wizard</summary>
Public Class WizardData
    Implements INotifyPropertyChanged
    ''' <summary>COntains value of the <see cref="DatabaseType"/> property</summary>
    Private _DatabaseType As DatabaseType = Console.DatabaseType.UserInstance
    ''' <summary>Gets or sets type of database to connect to (User instance, attach db file, server database)</summary>
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
            OnPropertyChanged("RequiresDatabaseNameVisibility")
            OnPropertyChanged("RequiresExistingDatabaseNameVisibility")
            OnPropertyChanged("RequiresFileNameVisibility")
            OnPropertyChanged("DatabaseTypeDesc")
            OnPropertyChanged("ImageStorageSettingsVisible")
        End Set
    End Property
#Region "Database type helpers"
    ''' <summary>Gets or sets value indicating if database file will be attached to database server</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property DatabaseTypeIsAttachFile() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.AttachFile
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.AttachFile
        End Set
    End Property
    ''' <summary>Gets or sets value indicating if server-side tabase will be used</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property DatabaseTypeIsServerDatabase() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.ServerDatabase
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.ServerDatabase
        End Set
    End Property
    ''' <summary>Gets or sets value indicating if User instances will be used</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property DatabaseTypeIsUserInstance() As Boolean
        Get
            Return DatabaseType = Console.DatabaseType.UserInstance
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseType = Console.DatabaseType.UserInstance
        End Set
    End Property
    ''' <summary>Gets value indicating if database type requires file selection</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property FileSelectionVisibility() As Visibility
        Get
            Return If(DatabaseTypeIsAttachFile OrElse DatabaseTypeIsUserInstance, Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
    ''' <summary>gets value indicating if database type requires database selection</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property DatabaseSelectionVisibility() As Visibility
        Get
            Return If(DatabaseTypeIsServerDatabase, Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
#End Region
    ''' <summary>Contains value of the <see cref="FileConnectionType"/> property</summary>
    Private _FileConnectionType As FileConnectionType = Console.FileConnectionType.Existing
    ''' <summary>Contains value of the <see cref="DatabaseConnectionType"/> property</summary>
    Private _DatabaseConnectionType As DatabaseConnectionType = Console.DatabaseConnectionType.Existing
    ''' <summary>Gets or sets type of database file (new/existing)</summary>
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
            OnPropertyChanged("ImageStorageSettingsVisible")
        End Set
    End Property
    ''' <summary>Gets or sets type of database (new/existing/empty)</summary>
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
            OnPropertyChanged("ImageStorageSettingsVisible")
            OnPropertyChanged("RequiresExistingDatabaseNameVisibility")
        End Set
    End Property
#Region "Connection type helpers"
    ''' <summary>Gets or sets value indicating if a new database file will be created.</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsFileConnectionTypeNew() As Boolean
        Get
            Return FileConnectionType = Console.FileConnectionType.New
        End Get
        Set(ByVal value As Boolean)
            If value Then FileConnectionType = Console.FileConnectionType.New
        End Set
    End Property
    ''' <summary>Gets or sets value indicating if existing database file will be used</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsFileConnectionTypeExisting() As Boolean
        Get
            Return FileConnectionType = Console.FileConnectionType.Existing
        End Get
        Set(ByVal value As Boolean)
            If value Then FileConnectionType = Console.FileConnectionType.Existing
        End Set
    End Property
    ''' <summary>Gets or sets value indicating if a new database will be created on server</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsDatabaseConnectionTypeNew() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.New
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.New
        End Set
    End Property
    ''' <summary>Gets or sets value indicating if existing database will be used from sever</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property IsDatabaseConnectionTypeExisting() As Boolean
        Get
            Return DatabaseConnectionType = Console.DatabaseConnectionType.Existing
        End Get
        Set(ByVal value As Boolean)
            If value Then DatabaseConnectionType = Console.DatabaseConnectionType.Existing
        End Set
    End Property
    ''' <summary>gets or sets value indicating if an empty database will be used from server and tables etc. will be created in that database</summary>
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
    ''' <summary>Gets value indicating if current database type requires file name to be selected</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property RequiresFileNameVisibility() As Visibility
        Get
            Return If(DatabaseType = Console.DatabaseType.ServerDatabase, Visibility.Collapsed, Visibility.Visible)
        End Get
    End Property
    ''' <summary>Gets value indicating if current database type requires database name to be selected</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property RequiresDatabaseNameVisibility() As Visibility
        Get
            Return If(DatabaseType = Console.DatabaseType.ServerDatabase, Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
    ''' <summary>Gets value indicating if current database type requires database name to be selected from existing databases</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property RequiresExistingDatabaseNameVisibility() As Visibility
        Get
            Return If(DatabaseType = Console.DatabaseType.ServerDatabase AndAlso
                      (DatabaseConnectionType = Console.DatabaseConnectionType.Empty OrElse DatabaseConnectionType = Console.DatabaseConnectionType.Existing),
                      Visibility.Visible, Visibility.Collapsed)
        End Get
    End Property
    ''' <summary>Gets value indicating if SQL Server authentication is used</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public Property UseSqlServerAuth() As Boolean
        Get
            Return Not UseIntegratedSecurity
        End Get
        Set(ByVal value As Boolean)
            UseIntegratedSecurity = Not value
        End Set
    End Property
#End Region
#Region "Step 4 helpers"
    ''' <summary>Gets value indicating if image store selecdtion controls are visible</summary>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public ReadOnly Property ImageStorageSettingsVisible As Boolean
        Get
            Return ((DatabaseType = Console.DatabaseType.AttachFile OrElse DatabaseType = Console.DatabaseType.UserInstance) AndAlso FileConnectionType = Console.FileConnectionType.New) OrElse
                (DatabaseType = Console.DatabaseType.ServerDatabase AndAlso (DatabaseConnectionType = Console.DatabaseConnectionType.New OrElse DatabaseConnectionType = Console.DatabaseConnectionType.Empty))
        End Get
    End Property
    ''' <summary>it is necessary to specify image root folder</summary>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public ReadOnly Property ImageRootRequired As Boolean
        Get
            Return Not CapImagesInDb OrElse Not OtherImagesInDb
        End Get
    End Property
#End Region
#Region "Step 5 helpers"
    ''' <summary>Gets human-readable description of <see cref="DatabaseType"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property DatabseTypeDesc$()
        Get
            Select Case DatabaseType
                Case Console.DatabaseType.AttachFile : Return My.Resources.wiz_txt_AttachDatabaseToServer
                Case Console.DatabaseType.ServerDatabase : Return My.Resources.wiz_txt_ConnectToDatabaseServer
                Case Console.DatabaseType.UserInstance : Return My.Resources.wiz_txt_UseUserInstances
                Case Else : Return My.Resources.txt_Unknown
            End Select
        End Get
    End Property
    ''' <summary>gets human-readable description of connection type</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property ConnectionTypeDesc$()
        Get
            Select Case DatabaseType
                Case Console.DatabaseType.AttachFile, Console.DatabaseType.UserInstance
                    Select Case FileConnectionType
                        Case Console.FileConnectionType.Existing : Return My.Resources.wiz_txt_UseExistingDatabaseFile
                        Case Console.FileConnectionType.New : Return My.Resources.wiz_txt_NewDatabaseFile
                    End Select
                Case Console.DatabaseType.ServerDatabase
                    Select Case DatabaseConnectionType
                        Case Console.DatabaseConnectionType.Existing : Return My.Resources.wiz_txt_UseExistingDatbase
                        Case Console.DatabaseConnectionType.Empty : Return My.Resources.wiz_txt_ConnectToEmptyDatabase
                        Case Console.DatabaseConnectionType.New : Return My.Resources.wiz_txt_NewDatabase
                    End Select
            End Select
            Return My.Resources.txt_Unknown
        End Get
    End Property
#End Region
    ''' <summary>Contains value of the <see cref="UseIntegratedSecurity"/> property</summary>
    Private _UseIntegratedSecurity As Boolean = True
    ''' <summary>Gets or sets value indicating if Windows authetication (integrated security) is used to connect to database</summary>
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
    ''' <summary>Contains value of the <see cref="FilePath"/> property</summary>
    Private _FilePath$
    ''' <summary>Contains value of the <see cref="ServerName"/> property</summary>
    Private _ServerName$
    ''' <summary>Contains value of the <see cref="UserName"/> property</summary>
    Private _UserName$
    ''' <summary>Contains value of the <see cref="Password"/> property</summary>
    Private _Password$
    ''' <summary>Contains value of the <see cref="DatabaseName"/> property</summary>
    Private _DatabaseName$
    ''' <summary>Contains value of the <see cref="ImageRoot"/> property</summary>
    Private _ImageRoot$
    ''' <summary>Contains value of the <see cref="CapImagesInDb"/> property</summary>
    Private _CapImagesInDb As Boolean
    ''' <summary>Contains value of the <see cref="OtherImagesInDb"/> property</summary>
    Private _OtherImagesInDb As Boolean

    ''' <summary>Gets or sets path of database file</summary>
    Public Property FilePath$()
        Get
            Return _FilePath
        End Get
        Set(ByVal value$)
            _FilePath = value
            OnPropertyChanged("FilePath")
        End Set
    End Property
    ''' <summary>Gets or sets name (i.e. address and instance name, domain name or IP address) of database server</summary>
    Public Property ServerName$()
        Get
            Return _ServerName
        End Get
        Set(ByVal value$)
            _ServerName = value
            OnPropertyChanged("ServerName")
        End Set
    End Property
    ''' <summary>Gets ore sets user name used to connect to database</summary>
    Public Property UserName$()
        Get
            Return _UserName
        End Get
        Set(ByVal value$)
            _UserName = value
            OnPropertyChanged("UserName")
        End Set
    End Property
    ''' <summary>Gats or sets password used to connect to database</summary>
    Public Property Password$()
        Get
            Return _Password
        End Get
        Set(ByVal value$)
            _Password = value
            OnPropertyChanged("Password")
        End Set
    End Property
    ''' <summary>Gets or sets name of server database</summary>
    Public Property DatabaseName$()
        Get
            Return _DatabaseName
        End Get
        Set(ByVal value$)
            _DatabaseName = value
            OnPropertyChanged("DatabaseName")
        End Set
    End Property

    ''' <summary>Gets or sets path of folder to store images in file system in</summary>
    Public Property ImageRoot$
        Get
            Return _ImageRoot
        End Get
        Set(ByVal value$)
            _ImageRoot = value
            OnPropertyChanged("ImageRoot")
        End Set
    End Property

    ''' <summary>Gets or sets value indicating if images of caps will be stored in database</summary>
    Public Property CapImagesInDb As Boolean
        Get
            Return _CapImagesInDb
        End Get
        Set(ByVal value As Boolean)
            _CapImagesInDb = value
            OnPropertyChanged("CapImagesInDb")
            OnPropertyChanged("ImageRootRequired")
        End Set
    End Property

    ''' <summary>gets or sets value indicating if other images will be store din database</summary>
    Public Property OtherImagesInDb As Boolean
        Get
            Return _OtherImagesInDb
        End Get
        Set(ByVal value As Boolean)
            _OtherImagesInDb = value
            OnPropertyChanged("OtherImagesInDb")
            OnPropertyChanged("ImageRootRequired")
        End Set
    End Property

    ''' <summary>Occurs when a property value changes.</summary>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Private _FinalConnectionString As SqlConnectionStringBuilder
    ''' <summary>Gets or sets final connection string established once wizard has finished</summary>
    Property FinalConnectionString As SqlConnectionStringBuilder
        Get
            Return _FinalConnectionString
        End Get
        Set(ByVal value As SqlConnectionStringBuilder)
            _FinalConnectionString = value
            OnPropertyChanged("FinalConnectionString")
        End Set
    End Property
    Private _FinalImageRoot$
    ''' <summary>Gets or sets final image root path</summary>
    Property FinalImageRoot As String
        Get
            Return _FinalImageRoot
        End Get
        Set(ByVal value As String)
            _FinalImageRoot = value
            OnPropertyChanged("FinalImageRoot")
        End Set
    End Property

    ''' <summary>Raises the <see cref="PropertyChanged"/> event</summary>
    Protected Overridable Sub OnPropertyChanged(ByVal PropertyName$)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
    End Sub

    ''' <summary>Gets file connection string</summary>
    ''' <returns>A connection string to connect to caps database</returns>
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