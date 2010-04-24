Imports System, mBox = Tools.WindowsT.IndependentT.MessageBox
Imports System.Windows
Imports System.Windows.Controls
Imports System.Data.SqlClient, Caps.Data
Imports Tools, Tools.ExtensionsT, Tools.IOt
Imports System.Data.EntityClient
Imports Microsoft.Data.Schema
Imports Tools.ComponentModelT
Imports Tools.DataT.SchemaT.DeployT

''' <summary>This wizard step shows summary information and when confirmed performs necessary tasks to apply settings chosen in wizard</summary>
Public Class pgSummary
    Inherits PageFunction(Of Boolean)
    ''' <summary>Wizard data filled by previosu steps</summary>
    Private data As WizardData

    ''' <summary>CTor - creates a new instance of the <see cref="pgSummary"/> class</summary>
    ''' <param name="wizardData">Wizard data partially initialized by previous steps</param>
    Public Sub New(ByVal wizardData As WizardData)
        Me.InitializeComponent()
        MyBase.DataContext = wizardData
        data = wizardData
    End Sub

    Private Sub backButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        MyBase.NavigationService.GoBack()
    End Sub

    Private Sub cancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(False))
    End Sub

    Private Sub finishButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim b = data.GetFinalConnectionString
        Dim databaseCreated As String = Nothing
        Dim createDatabaseConnectionString = b
        Dim testOnly As Boolean = False
        Dim DatabaseDeployed As Boolean = False
        Dim imageRoot As String = Nothing
        Try
            Dim dbSetup As Boolean = False
            Select Case data.DatabaseType
                Case DatabaseType.AttachFile
                    Select Case data.FileConnectionType
                        Case FileConnectionType.Existing
                            'Attach existing database file
                            testOnly = True
                            VerifyDatabase(b.ToString)
                        Case FileConnectionType.New
                            'Create a new database file and attach it
                            createDatabaseConnectionString = New SqlConnectionStringBuilder(b.ToString)
                            createDatabaseConnectionString.AttachDBFilename = ""
                            createDatabaseConnectionString.InitialCatalog = ""
                            databaseCreated = CreateDatabase(createDatabaseConnectionString.ToString, b.AttachDBFilename, b.AttachDBFilename)
                            'Creatreate structures there
                            b.AttachDBFilename = ""
                            InitDatabase(b.ToString, b.InitialCatalog)
                            DatabaseDeployed = True
                            VerifyDatabase(b.ToString)
                            dbSetup = True
                    End Select
                Case DatabaseType.ServerDatabase
                    Select Case data.DatabaseConnectionType
                        Case DatabaseConnectionType.New 'Create a new database
                            'Create a new server database
                            createDatabaseConnectionString = New SqlConnectionStringBuilder(b.ToString)
                            createDatabaseConnectionString.InitialCatalog = ""
                            databaseCreated = CreateDatabase(createDatabaseConnectionString.ToString, b.InitialCatalog)
                            'Creatreate structures there
                            InitDatabase(b.ToString, b.InitialCatalog)
                            DatabaseDeployed = True
                            VerifyDatabase(b.ToString)
                            dbSetup = True
                        Case DatabaseConnectionType.Empty
                            'Create structures in exisitng database
                            InitDatabase(b.ToString, b.InitialCatalog)
                            DatabaseDeployed = True
                            VerifyDatabase(b.ToString)
                            dbSetup = True
                        Case DatabaseConnectionType.Existing
                            'Connect to existing database
                            testOnly = True
                            VerifyDatabase(b.ToString)
                    End Select
                Case DatabaseType.UserInstance
                    Select Case data.FileConnectionType
                        Case FileConnectionType.New
                            'Create a new databse file using User Instance
                            createDatabaseConnectionString = New SqlConnectionStringBuilder(b.ToString)
                            createDatabaseConnectionString.AttachDBFilename = ""
                            createDatabaseConnectionString.InitialCatalog = ""
                            databaseCreated = CreateDatabase(createDatabaseConnectionString.ToString, b.InitialCatalog, b.AttachDBFilename)
                            'Creatreate structures there
                            b.AttachDBFilename = ""
                            InitDatabase(b.ToString, b.InitialCatalog)
                            DatabaseDeployed = True
                            VerifyDatabase(b.ToString)
                            dbSetup = True
                        Case FileConnectionType.Existing
                            'Connect to user instance database file
                            testOnly = True
                            VerifyDatabase(b.ToString)
                    End Select
            End Select
            'Set database settings
            If dbSetup Then
                Using context As New CapsDataContext(New System.Data.EntityClient.EntityConnection(CapsDataContext.DefaultMetadataWorkspace, New SqlConnection(b.ToString))),
                      settings As New ConfigNodeProvider(context)
                    If data.CapImagesInDb Then
                        settings.Images.CapsInDatabase = {256, 64, 0}
                        settings.Images.CapsInFileSystem = Nothing
                    Else
                        settings.Images.CapsInFileSystem = {256, 64, 0}
                        settings.Images.CapsInDatabase = Nothing
                    End If
                    settings.Images.SetOtherImagesStorage(If(data.OtherImagesInDb, ConfigNodeProvider.ImagesProvider.Storage.Database, ConfigNodeProvider.ImagesProvider.Storage.FileSystem))
                    If data.OtherImagesInDb OrElse data.CapImagesInDb Then imageRoot = data.ImageRoot
                End Using
            End If
        Catch ex As Exception
            If databaseCreated Then
                'Database was created but deploy, post-deplyment script, test or confuguration failed
                Using dropConn As New SqlConnection(createDatabaseConnectionString.ToString)
                    Try
                        dropConn.Open()
                        Dim drop As New SqlCommand(String.Format("DROP DATABASE [{0}]", databaseCreated.Replace("]", "]]")))
                        drop.Connection = dropConn
                        drop.ExecuteNonQuery()
                    Catch ex2 As Exception
                        If TypeOf ex Is HandledException Then
                            mBox.MsgBoxFW(My.Resources.wiz_msg_DropErrorSimple & vbCrLf & ex.Message, MsgBoxStyle.Critical, My.Resources.wiz_txt_DeployDatabase, Me, databaseCreated)
                        Else
                            mBox.MsgBox(My.Resources.wiz_msg_DropError.f(databaseCreated, ex.Message, ex2.Message), MsgBoxStyle.Critical, My.Resources.wiz_txt_DeployDatabase, Me)
                        End If
                        Exit Sub
                    End Try
                End Using
            ElseIf DatabaseDeployed Then
                'Deploy was done to existing database but test or configuration failed
                mBox.MsgBoxFW(My.Resources.wiz_msg_DeployErrorExistingDb & vbCrLf & ex.Message, MsgBoxStyle.Critical, My.Resources.wiz_txt_DeployDatabase, Me, b.InitialCatalog)
            End If
            If testOnly Then 'Connection test failed
                mBox.MsgBoxFW(My.Resources.wiz_err_TestConnection, MsgBoxStyle.Critical, My.Resources.txt_DatabaseError, Me, ex.Message)
            ElseIf Not TypeOf ex Is HandledException Then 'Connection, creation or some other initialization failed
                mBox.MsgBoxFW(My.Resources.wiz_err_SetupDatabase, MsgBoxStyle.Critical, My.Resources.txt_DatabaseError, Me, ex.Message)
            End If
            Exit Sub
        End Try
        'Done
        data.OnFinished(b, imageRoot)
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(True))
    End Sub

#Region "DB helper methods"
    ''' <summary>Creates a dabase</summary>
    ''' <param name="connectionString">Connection string to database server to use to create a database</param>
    ''' <param name="databaseName">Name of newly created database</param>
    ''' <param name="fileName">Path of database file to create database in. If null or empty database is created in default location.</param>
    ''' <returns><paramref name="databaseName"/></returns>
    ''' <exception cref="ArgumentNullException"><paramref name="connectionString"/> or <paramref name="databaseName"/> is null</exception>
    ''' <exception cref="System.InvalidOperationException"><paramref name="connectionString"/> is invalid: It does not specify a data source or server</exception>
    ''' <exception cref="System.Data.SqlClient.SqlException">A connection-level error occurred while opening the connection.</exception>
    ''' <exception cref="System.ArgumentException"><paramref name="connectionString"/> is invalid: A Connection Plan is specified together with one of the following:FailOverPartner, AttachDbFileName, UserInstance=true, or contextConnection=true.</exception>
    Private Shared Function CreateDatabase(ByVal connectionString$, ByVal databaseName$, Optional ByVal fileName$ = Nothing) As String
        If connectionString Is Nothing Then Throw New ArgumentNullException("connectionString")
        If databaseName Is Nothing Then Throw New ArgumentNullException("databaseName")
        If fileName Is Nothing Then fileName = String.Empty
        Using Connection As New SqlConnection(connectionString)
            Connection.Open()
            Dim cmd As New SqlCommand(If(fileName = "",
                                         "CREATE DATABASE [{0}]",
                                         "CREATE DATABASE [{0}] ON (NAME=[{1}], FILENAME = '{2}')").
                                     f(databaseName.Replace("]", "]]"), fileName.Replace("]", "]]"), fileName.Replace("'", "''")))
            cmd.Connection = Connection
            cmd.ExecuteNonQuery()
            Return databaseName
        End Using
    End Function

    ''' <summary>Verifies if database with given connection is caps database of expected version</summary>
    ''' <param name="connectionString">Connection stringto the database</param>
    ''' <exception cref="ArgumentNullException"><paramref name="connectionString"/> is null</exception>
    ''' <exception cref="ApplicationException">Database version does not match <see cref="DatabaseVersion"/> and update script is not available or user opted not to upgrade.</exception>
    ''' <exception cref="SqlException">Upgrade script failed -or- Connection-level occured while oppenign connection to the database</exception>
    ''' <exception cref="InvalidOperationException"><paramref name="connectionString"/> is invalid - it's missing data source or server</exception>
    ''' <exception cref="ArgumentException"><paramref name="connectionString"/> is invalid: A Connection Plan is specified together with one of the following:FailOverPartner, AttachDbFileName, UserInstance=true, or contextConnection=true.</exception>
    ''' <remarks>If database version matches, procedure just returns.
    ''' <para>This procedure emits messageboxes to user</para></remarks>
    ''' <seelaso cref="VerifyDatabaseVersionWithUpgrade"/><seelaso cref="SqlConnection.Open"/>
    Private Sub VerifyDatabase(ByVal connectionString$)
        If connectionString Is Nothing Then Throw New ArgumentNullException("connectionString")
        Using Connection As SqlConnection = New SqlConnection(connectionString) 'Test connection to file
            Connection.Open()
            VerifyDatabaseVersionWithUpgrade(Connection, Me.FindAncestor(Of Window))
        End Using
    End Sub

    ''' <summary>Creates tables etc. in database</summary>
    ''' <param name="connectionString">Connection string to a database</param>
    ''' <param name="databaseName">Name of database do be initialized.</param>
    ''' <exception cref="HandledException">An exception occured during database deployment and was reported to user. See <see cref="HandledException.InnerException"/> for details.</exception>
    ''' <exception cref="UnauthorizedAccessException">The caller does not have the required permission for writing to temporary directory.</exception>
    ''' <exception cref="IO.IOException">An I/O error occured while writing schema files to temporary directory</exception>
    Private Shared Sub InitDatabase(ByVal connectionString As String, ByVal databaseName$)
        Dim errors As New Text.StringBuilder
        Dim directory = ExtractDatabaseSchema()
        Dim handler As EventHandler(Of Tools.ConsoleT.ConsoleClosingEventArgs) = Sub(sender, e) e.Cancel = True
        Try
            Tools.ConsoleT.AllocateConsole()
            System.Console.Title = My.Resources.wiz_txt_CreatingDatbase
            Tools.ConsoleT.Icon = My.Resources.ico_Database
            Tools.ConsoleT.PreventClose()
            AddHandler Tools.ConsoleT.Closing, handler
            Dim manifestFilePath = IO.Path.Combine(directory, "CapsData.deploymanifest")
            Using engine As New DatabaseDeployment(connectionString, databaseName, manifestFilePath)
                AddHandler engine.ErrorOccured, Sub(sender, e)
                                                    Dim oldCC = System.Console.ForegroundColor
                                                    Try
                                                        Select Case e.error.severity
                                                            Case ErrorSeverity.Error
                                                                System.Console.ForegroundColor = ConsoleColor.Red
                                                            Case ErrorSeverity.Warning
                                                                System.Console.ForegroundColor = ConsoleColor.Yellow
                                                        End Select
                                                        System.Console.WriteLine(DatabaseDeployment.FormatDataSchemaError(e))
                                                    Finally
                                                        System.Console.ForegroundColor = oldCC
                                                    End Try
                                                End Sub
                Try
                    engine.Deploy()
                Catch ex As Exception
                    mBox.MsgBox(My.Resources.wiz_msg_ErrorInConsole & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical, My.Resources.wiz_txt_DeployDatabase)
                    Throw New HandledException(ex)
                End Try
            End Using
        Finally
            RemoveHandler Tools.ConsoleT.Closing, handler
            Try
                IO.Directory.Delete(directory, True)
            Catch : End Try
            Tools.ConsoleT.DetachConsole()
        End Try
    End Sub

    ''' <summary>Extracts database schema files from assembly</summary>
    ''' <returns>Name of temporary folder files were extracted to</returns>
    ''' <exception cref="UnauthorizedAccessException">The caller does not have the required permission for writing to temporary directory.</exception>
    ''' <exception cref="IO.IOException">An I/O error occured while writing schema files to temporary directory</exception>
    Private Shared Function ExtractDatabaseSchema() As String
        Dim folder As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, Guid.NewGuid.ToString)
        Dim assembly As System.Reflection.Assembly = GetType(pgSummary).Assembly
        IO.Directory.CreateDirectory(folder)
        For Each rn In assembly.GetManifestResourceNames()
            If rn.StartsWith("Deploy/") Then
                Using file = IO.File.Create(IO.Path.Combine(folder, rn.Substring("Deploy/".Length))),
                      resstrr = assembly.GetManifestResourceStream(rn)
                    file.Write(resstrr)
                End Using
            End If
        Next
        Return folder
    End Function
#End Region
End Class