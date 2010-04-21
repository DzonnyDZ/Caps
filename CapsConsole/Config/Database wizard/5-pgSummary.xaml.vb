Imports System, mBox = Tools.WindowsT.IndependentT.MessageBox
Imports System.Windows
Imports System.Windows.Controls
Imports System.Data.SqlClient, Caps.Data
Imports Microsoft.Data.Schema.Build
Imports Microsoft.Data.Schema.SchemaModel
Imports Microsoft.SqlServer.Management.Common
Imports Microsoft.Data.Schema
Imports Microsoft.Build.Evaluation
Imports Microsoft.Data.Schema.Extensibility
Imports Tools, Tools.ExtensionsT
Imports System.Xml
Imports System.IO.MemoryMappedFiles, Tools.IOt
Imports System.Reflection

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
        Dim testOnly As Boolean = False
        Try
            Select Case data.DatabaseType
                Case DatabaseType.AttachFile
                    Select Case data.FileConnectionType
                        Case FileConnectionType.Existing 'Test connrection to file
                            testOnly = True
                            Using Connection As SqlConnection = New SqlConnection(b.ToString)
                                Connection.Open()
                                VerifyDatabaseVersionWithUpgrade(Connection, Me.FindAncestor(Of Window))
                            End Using
                        Case FileConnectionType.New 'Create new database file to attach
                            'TODO: Create file and create database in there
                    End Select
                Case DatabaseType.ServerDatabase
                    Select Case data.DatabaseConnectionType
                        Case DatabaseConnectionType.New 'Create a new database
                            'TODO: Create database and create database in threre
                        Case DatabaseConnectionType.Empty  'Create structures in exisitng database
                            testOnly = True
                            Using Connection As SqlConnection = New SqlConnection(b.ToString)
                                Connection.Open()
                            End Using
                            testOnly = False
                            InitDatabase(b.ToString)
                            Using Connection As SqlConnection = New SqlConnection(b.ToString)
                                Connection.Open()
                                VerifyDatabaseVersionWithUpgrade(Connection, Me.FindAncestor(Of Window))
                            End Using
                        Case DatabaseConnectionType.Existing  'Test connection to database
                            testOnly = True
                            Using Connection As SqlConnection = New SqlConnection(b.ToString)
                                Connection.Open()
                                VerifyDatabaseVersionWithUpgrade(Connection, Me.FindAncestor(Of Window))
                            End Using
                    End Select
                Case DatabaseType.UserInstance
                    Select Case data.FileConnectionType
                        Case FileConnectionType.New  'Create a new databse file using User Instance
                            'TODO: Create file and create database there
                        Case FileConnectionType.Existing 'Test connection to user instance
                            testOnly = True
                            Using Connection As SqlConnection = New SqlConnection(b.ToString)
                                Connection.Open()
                                VerifyDatabaseVersionWithUpgrade(Connection, Me.FindAncestor(Of Window))
                            End Using
                    End Select
            End Select
        Catch ex As Exception
            If testOnly Then
                mBox.MsgBox(String.Format(My.Resources.err_TestConnection, vbCr, ex.Message), MsgBoxStyle.Critical, My.Resources.txt_DatabaseError, Me)
            Else
                mBox.MsgBox(String.Format(My.Resources.err_SetupDatabase, vbCr, ex.Message), MsgBoxStyle.Critical, My.Resources.txt_DatabaseError, Me)
            End If
            Exit Sub
        End Try
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(True))
    End Sub

    ''' <summary>Creates tables etc. in database</summary>
    ''' <param name="connectionString">Connection string to a database</param>
    Private Shared Sub InitDatabase(ByVal connectionString As String)
        Dim errors As New Text.StringBuilder
        Dim directory = ExtractDatabaseSchema()
        Dim manifestFilePath = IO.Path.Combine(directory, "CapsData.deploymanifest")
        Try
            Dim handler As EventHandler(Of DeploymentContributorEventArgs) = Nothing
            Dim manager As New ErrorManager
            Dim errorCount As Integer = 0
            AddHandler manager.ErrorsChanged,
                Sub(sender, args)
                    Dim [error] As DataSchemaError
                    For Each [error] In args.ErrorsAdded
                        If Not [error].GetType.FullName = "Microsoft.Data.Schema.SchemaModel.ExternalElementResolutionError" Then
                            OutputDataSchemaError(errors, [error])
                            errorCount = IIf(([error].Severity = ErrorSeverity.Error), (errorCount + 1), errorCount)
                        End If
                    Next
                End Sub
            Dim engine As SchemaDeployment = Nothing
            Try
                'Dim [set] As HashSet(Of String)
                Dim manifest As Project = Nothing
                Dim manifestFile As IO.FileInfo = Nothing
                'If Not String.IsNullOrEmpty(parsedArgs.ManifestFile) Then
                manifestFile = New IO.FileInfo(manifestFilePath)
                manifest = LoadManifest(manifestFile)
                'End If
                Dim dbSchemaFile As IO.FileInfo = Nothing
                Dim str As String = Nothing
                Dim propertyValue As String = manifest.GetPropertyValue("SourceModel")
                If Not String.IsNullOrEmpty(propertyValue) Then
                    dbSchemaFile = New IO.FileInfo(IO.Path.Combine(manifestFile.DirectoryName, propertyValue))
                    If Not dbSchemaFile.Exists Then
                        Throw New IO.FileNotFoundException(String.Format("The database schema file {0} does not exist", dbSchemaFile.FullName))
                        dbSchemaFile = Nothing
                    End If
                End If
                Dim em As ExtensionManager = LoadExtensionManagerFromDBSchema(dbSchemaFile)
                Dim cmdServices As VSDBCmdServices = GetCmdServices(em)
                Dim serviceConstructor As SchemaDeploymentConstructor = em.DatabaseSchemaProvider.GetServiceConstructor(Of SchemaDeploymentConstructor)()
                serviceConstructor.Errors = manager
                serviceConstructor.TargetDatabaseName = str
                'If (parsedArgs.TargetSource = TargetModelOriginator.Model) Then
                '    Dim sourceSchemaFile As FileInfo = dbSchemaFile
                '    Dim targetSchemaFile As New FileInfo(parsedArgs.TargetModelFile)
                '    serviceConstructor.Setup(sourceSchemaFile, targetSchemaFile)
                'Else
                serviceConstructor.Setup(dbSchemaFile, connectionString)
                'End If
                If (Not cmdServices Is Nothing) Then
                    cmdServices.GetType.GetMethod("InitializeConstructor", BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, {GetType(SchemaDeploymentConstructor)}, Nothing).Invoke(cmdServices, {serviceConstructor})
                    'cmdServices.InitializeConstructor(serviceConstructor)
                End If
                engine = serviceConstructor.ConstructService
                If (Not cmdServices Is Nothing) Then
                    cmdServices.GetType.GetMethod("InitializeSchemaDeploymentOptions", BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, {GetType(SchemaDeploymentOptions)}, Nothing).Invoke(cmdServices, {engine.Options})
                    'cmdServices.InitializeSchemaDeploymentOptions(engine.Options)
                End If
                If (handler Is Nothing) Then
                    handler = Sub(sender, args)
                                  OutputDataSchemaError(errors, args.Message)
                                  errorCount = If((args.Message.Severity = ErrorSeverity.Error), (errorCount + 1), errorCount)
                              End Sub
                End If
                AddHandler engine.ContributorMessage, handler
                'If (Not manifest Is Nothing) Then
                engine.Configure(manifest, manifestFile.Directory)
                'If Not String.IsNullOrEmpty(parsedArgs.DeploymentScriptFile) Then
                '    engine.SetDeployToScript(True, parsedArgs.DeploymentScriptFile)
                'End If
                'Else
                'Dim deploymentScriptFile As String = parsedArgs.DeploymentScriptFile
                'If String.IsNullOrEmpty(deploymentScriptFile) Then
                '    deploymentScriptFile = (Path.GetFileNameWithoutExtension(Path.GetFileName(parsedArgs.ModelFile)) & ".txt")
                'End If
                'engine.SetDeployToScript(True, deploymentScriptFile)
                'If (Not cmdServices Is Nothing) Then
                '    Dim referenceDBSchema As IList(Of CustomSchemaData) = cmdServices.GetReferenceDBSchema(Me._installPath)
                '    If ((Not referenceDBSchema Is Nothing) AndAlso (referenceDBSchema.Count > 0)) Then
                '        Dim data As CustomSchemaData
                '        For Each data In referenceDBSchema
                '            Dim metadata As String = data.GetMetadata("LogicalName")
                '            Dim str9 As String = data.GetMetadata("FileName")
                '            If (String.IsNullOrEmpty(metadata) OrElse String.IsNullOrEmpty(str9)) Then
                '                TSDTrace.Source.TraceEvent(TraceEventType.Error, &H8000, "Failed to load custom schema data.  Either logical name or file path was not specified")
                '                Continue For
                '            End If
                '            engine.AddReferencedFile(metadata, str9)
                '        Next
                '    End If
                'End If
                'End If
                Dim availableProperties As IDictionary(Of String, PropertyInfo) = Nothing
                If (Not cmdServices Is Nothing) Then
                    availableProperties = cmdServices.GetSetableDeployProperties(engine.Options.GetType)
                Else
                    availableProperties = New Dictionary(Of String, PropertyInfo)
                End If

                'If (Not cmdServices Is Nothing) Then
                '    cmdServices.SetAdditionalDeploymentOptions(engine, parsedArgs.PropertyLookup)
                'End If
                'If Not VSDBCmdServices.SetProperties(availableProperties, parsedArgs.PropertyLookup, engine.Options, [set]) Then
                '    Me.WriteUnsetPropertyErrors([set])
                '    Return False
                'End If
                'If ((Not parsedArgs.ExtensionArguments Is Nothing) AndAlso (parsedArgs.ExtensionArguments.Length > 0)) Then
                '    Dim errors As List(Of String) = Nothing
                '    Dim source As Dictionary(Of String, String) = BuildHelperUtils.SplitNameValuePairs(parsedArgs.ExtensionArguments, False, errors)
                '    If ((Not errors Is Nothing) AndAlso (errors.Count > 0)) Then
                'Dim error As New DataSchemaError(VSDBCmdResources.ErrorsParsingExtArgs, ErrorSeverity.Error)
                '        Me.OutputDataSchemaError([error])
                '        Dim str10 As String
                '        For Each str10 In errors
                '            Dim error2 As New DataSchemaError(str10, ErrorSeverity.Error)
                '            Me.OutputDataSchemaError(error2)
                '        Next
                '        Return False
                '    End If
                '    BuildHelperUtils.CopyDictionary(Of String, String)(source, engine.ContributorArguments)
                'End If

                'If parsedArgs.DeployToDatabase.HasValue Then
                engine.SetDeployToDatabase(True) 'parsedArgs.DeployToDatabase.Value)
                'End If
                'If Not String.IsNullOrEmpty(connectionString) Then
                engine.Options.TargetConnectionString = connectionString
                'End If
                'If Not Me.ValidateForDeployment(parsedArgs, engine) Then
                '    Return False
                'End If
                engine.Execute()
                'Catch exception As DeploymentFailedException
                '    Me.WriteError(exception.Message)
                '    errorCount += 1
            Finally
                If (Not engine Is Nothing) Then
                    engine.Dispose()
                End If
            End Try
            Dim obj2 As Object
            For Each obj2 In manager.GetAllCategories
                Dim error3 As DataSchemaError
                For Each error3 In manager.GetAllErrors(obj2)
                    If error3.GetType.FullName = "Microsoft.Data.Schema.SchemaModel.ExternalElementResolutionError" Then
                        OutputDataSchemaError(errors, error3)
                    End If
                Next
            Next
            If errorCount > 0 Then
                Throw New DataSchemaException("An error occured while deploying the database:" & Environment.NewLine & errors.ToString)
            End If
        Finally
            IO.Directory.Delete(directory, True)
        End Try
    End Sub

    Private Shared Sub OutputDataSchemaError(ByVal b As Text.StringBuilder, ByVal [error] As DataSchemaError)
        Dim msg As String = Nothing
        Dim prefix As String = [error].Prefix
        If [error].ErrorCode <> 0 Then
            prefix = [error].BuildErrorCode
        End If
        If Not String.IsNullOrEmpty([error].Document) Then
            msg = String.Format("{0}" & ChrW(9) & "{1}" & ChrW(9) & "({2},{3})" & ChrW(9) & "{4}", New Object() {prefix, [error].Document, [error].Line, [error].Column, [error].Message})
        Else
            msg = String.Format("{0}" & ChrW(9) & "{1}", New Object() {prefix, [error].Message})
        End If
        If ([error].Severity = ErrorSeverity.Error) Then
            b.AppendLine(msg)
        Else
            b.AppendLine(msg)
        End If
    End Sub

    Private Shared Function LoadExtensionManagerFromDBSchema(ByVal input As IO.FileInfo) As Extensibility.ExtensionManager
        Dim manager2 As ExtensionManager
        Try
            Dim header As DataSchemaModelHeader = DataSchemaModel.ReadDataSchemaModelHeader(input.FullName, True)
            Dim manager As New ExtensionManager(header.DatabaseSchemaProviderName)
            If Not manager.DatabaseSchemaProvider.SchemaVersionSupported(header.SchemaVersion) Then
                Throw New DeploymentFailedException("Database schema provider {0} does not support schema file version '{1}'.".f(manager.DatabaseSchemaProvider.GetType.Name, header.SchemaVersion)) 'ModelSchema_NotSupportedDbSchemaVersionError
            End If
            manager2 = manager
        Catch exception As XmlException
            Throw New DeploymentFailedException(exception.Message, exception)
        Catch exception2 As ModelSerializationException
            Throw New DeploymentFailedException(exception2.Message, exception2)
        Catch exception3 As ExtensibilityException
            Throw New DeploymentFailedException(exception3.Message, exception3)
        End Try
        Return manager2
    End Function

    Private Shared Function LoadManifest(ByVal manifestFile As IO.FileInfo) As Project
        Dim project As Project = Nothing
        Try
            project = New Project(manifestFile.FullName, Nothing, "4.0", New ProjectCollection)
        Catch exception As Exception
            Throw New DeploymentFailedException(exception.Message, exception)
        End Try
        Return project
    End Function



    ''' <summary>Extracts database schema files from assembly</summary>
    ''' <returns>Name of temporary folder files were extracted to</returns>
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

    Private Shared Function GetCmdServices(ByVal em As ExtensionManager, ByVal connectionString$) As VSDBCmdServices
        Dim cmdServices As VSDBCmdServices = GetCmdServices(em)
        If (Not cmdServices Is Nothing) Then
            Dim prp As PropertyInfo = cmdServices.GetType.GetProperty("ConnectionString").GetSetMethod(True).Invoke(cmdServices, {connectionString})
            'cmdServices.ConnectionString = connectionString
        End If
        Return cmdServices
    End Function

    Private Shared Function GetCmdServices(ByVal em As ExtensionManager) As VSDBCmdServices
        Dim services As VSDBCmdServices = Nothing
        Dim handle As ExtensionHandle(Of VSDBCmdServices) = Nothing
        If em.TryGetSingleExtension(Of VSDBCmdServices)(handle) Then
            services = handle.Instantiate
        End If
        Return services
    End Function





End Class

