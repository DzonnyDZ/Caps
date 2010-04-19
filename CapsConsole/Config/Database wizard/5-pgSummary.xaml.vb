Imports System, mBox = Tools.WindowsT.IndependentT.MessageBox
Imports System.Windows
Imports System.Windows.Controls
Imports System.Data.SqlClient, Caps.Data
Imports Microsoft.Data.Schema.Build
Imports Microsoft.Data.Schema.SchemaModel
Imports Microsoft.SqlServer.Management.Common

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
                        Case FileConnectionType.Existing 'Test connection to file
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
    Private Sub InitDatabase(ByVal connectionString As String)
        'Deploy database
        Dim deployCTor As New SchemaDeploymentConstructor
        Dim sr As New IO.StreamReader(GetType(pgSummary).Assembly.GetManifestResourceStream("Deploy/CapsData.dbschema"))
        Dim em As New Microsoft.Data.Schema.ErrorManager
        Dim model As DataSchemaModel = DataSchemaModel.Deserialize(sr, em, "Deploy/CapsData.dbschema")
        deployCTor.Setup(model, connectionString)
        deployCTor.Errors = em
        Dim csb As New SqlConnectionStringBuilder(connectionString)
        deployCTor.TargetDatabaseName = csb.InitialCatalog
        Dim deploy As SchemaDeployment = deployCTor.ConstructService
        deploy.Execute()

        'Post-deploy script
        Dim PostDeploy As String
        Using resstream = GetType(pgSummary).Assembly.GetManifestResourceStream("Deploy/CapsData_Post-deploy.sql")
            Using r As New IO.StreamReader(resstream, System.Text.Encoding.UTF8)
                PostDeploy = r.ReadToEnd
            End Using
        End Using
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim Server = New Microsoft.SqlServer.Management.Smo.Server(New ServerConnection(connection))
            Server.ConnectionContext.ExecuteNonQuery(PostDeploy)
        End Using
    End Sub

End Class

