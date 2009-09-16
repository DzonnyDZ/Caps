Imports System, mBox = Tools.WindowsT.IndependentT.MessageBox
Imports System.Windows
Imports System.Windows.Controls
Imports System.Data.SqlClient

Public Class pgSummary
    Inherits PageFunction(Of Boolean)
    Private data As WizardData

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
                            Using New SqlConnection(b.ToString)
                                Connection.Open()
                            End Using
                        Case FileConnectionType.New 'Create new database file to attach
                    End Select
                Case DatabaseType.ServerDatabase
                    Select Case data.DatabaseConnectionType
                        Case DatabaseConnectionType.New 'Create a new database
                        Case DatabaseConnectionType.Empty  'Create structures in exisitng database
                        Case DatabaseConnectionType.Existing  'Test connection to database
                            testOnly = True
                            Using New SqlConnection(b.ToString)
                                Connection.Open()
                            End Using
                    End Select
                Case DatabaseType.UserInstance
                    Select Case data.FileConnectionType
                        Case FileConnectionType.New  'Create a new databse file using User Instance
                        Case FileConnectionType.Existing 'Test connection to user instance
                            testOnly = True
                            Using New SqlConnection(b.ToString)
                                Connection.Open()
                            End Using
                    End Select
            End Select
        Catch ex As Exception
            If testOnly Then
                mBox.MsgBox(String.Format("Error while testing connection to database:{0}{1}{0}Please verify connection settings and try again.", vbCr, ex.Message), MsgBoxStyle.Critical, "Databse error")
            Else
                mBox.MsgBox(String.Format("Error setting up database:{0}{1}{0}Please verify connection settings and try again.", vbCr, ex.Message), MsgBoxStyle.Critical, "Databse error")
            End If
            Exit Sub
        End Try
        Me.OnReturn(New ReturnEventArgs(Of Boolean)(True))
    End Sub

End Class

