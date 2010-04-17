Imports System, mBox = Tools.WindowsT.IndependentT.MessageBox
Imports System.Windows
Imports System.Windows.Controls
Imports System.Data.SqlClient, Caps.Data

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
                                If Not VerifyDatabaseVersion(Connection) Then
                                    mBox.MsgBox(My.Resources.err_IncorrectDatabaseVersion, MsgBoxStyle.Critical, My.Resources.txt_DatabaseError, Me)
                                    'TODO: Upgrade?
                                    Exit Sub
                                End If
                            End Using
                        Case FileConnectionType.New 'Create new database file to attach
                            'TODO: Create file and create database in there
                    End Select
                Case DatabaseType.ServerDatabase
                    Select Case data.DatabaseConnectionType
                        Case DatabaseConnectionType.New 'Create a new database
                            'TODO: Create database and create database in threre
                        Case DatabaseConnectionType.Empty  'Create structures in exisitng database
                            'TODO: Create database inthere
                        Case DatabaseConnectionType.Existing  'Test connection to database
                            testOnly = True
                            Using Connection As SqlConnection = New SqlConnection(b.ToString)
                                Connection.Open()
                                If Not VerifyDatabaseVersion(Connection) Then
                                    mBox.MsgBox(My.Resources.err_IncorrectDatabaseVersion, MsgBoxStyle.Critical, My.Resources.txt_DatabaseError, Me)
                                    'TODO: Upgrade?
                                    Exit Sub
                                End If
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
                                If Not VerifyDatabaseVersion(Connection) Then
                                    mBox.MsgBox(My.Resources.err_IncorrectDatabaseVersion, MsgBoxStyle.Critical, My.Resources.txt_DatabaseError, Me)
                                    'TODO: Upgrade?
                                    Exit Sub
                                End If
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

End Class

