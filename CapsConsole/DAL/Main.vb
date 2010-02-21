Imports System.Data, System.Reflection, Tools.ExtensionsT
Imports System.Data.SqlClient
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Microsoft.SqlServer.Management.Common
Imports Caps.Data

Friend Module Main
    ''' <summary>Current con nection to database</summary>
    Public Connection As System.Data.SqlClient.SqlConnection

    ''' <summary>Gets change script to alter database represent by connection to newer one</summary>
    ''' <param name="Connection">Connection to database to alter</param>
    ''' <returns>Change script wthich alters database to newer version. Null if script is not available or change is not necessary.</returns>
    ''' <remarks>There is no guarantee that script will alter database to newest version (<see cref="DatabaseVersion"/>). It may be necessary to obtain another script for resulting database.</remarks>
    ''' <exception cref="ArgumentNullException"><paramref name="Connection"/> is null</exception>
    Public Function GetUpdateScript(ByVal Connection As SqlConnection) As String
        Dim result = VerifyDatabaseVersion(Connection)
        If result.IsOK OrElse result.DatabaseVersion Is Nothing OrElse result.DatabaseGuid <> DatabaseGuid Then Return Nothing
        For Each resname In GetType(Main).Assembly.GetManifestResourceNames
            If resname.StartsWith("ChangeScripts/{0}-to-".f(result.DatabaseVersion)) Then
                Using resstream = GetType(Main).Assembly.GetManifestResourceStream(resname)
                    Using r As New IO.StreamReader(resstream, System.Text.Encoding.UTF8)
                        Return r.ReadToEnd
                    End Using
                End Using
            End If
        Next
        Return Nothing
    End Function
    ''' <summary>True when <see cref="VerifyDatabaseVersionWithUpgrade"/> is currently on callstack</summary>
    Private VerifyDatabaseVersionWithUpgradeOnStack As Boolean = False
    ''' <summary>Verifies if database with given connection is caps database of expected version</summary>
    ''' <param name="Connection">Connection to the database</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Connection"/> is null</exception>
    ''' <exception cref="ApplicationException">Database version does not match <see cref="DatabaseVersion"/> and update script is not available or user opted not to upgrade.</exception>
    ''' <exception cref="SqlException">Upgrade script failed</exception>
    ''' <remarks>If database version matches, procedure just returns.
    ''' <para>This procedure emits messageboxes to user</para></remarks>
    Public Sub VerifyDatabaseVersionWithUpgrade(ByVal Connection As SqlConnection, ByVal owner As Window)
        Dim checkResult = VerifyDatabaseVersion(Connection)
        If checkResult.IsOK Then Return
        Dim Script = GetUpdateScript(Connection)
        If Script Is Nothing Then Throw New ApplicationException(My.Resources.err_IncorrectDatabaseVersion)
        If VerifyDatabaseVersionWithUpgradeOnStack OrElse _
            mBox.MsgBox(My.Resources.msg_DatabaseVersionUpgrade.f( _
                             checkResult.DatabaseVersion, My.Application.Info.Title, My.Application.Info.Version, DatabaseVersion, vbCrLf), _
                             MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.txt_UpgradeDatabase, owner) = MsgBoxResult.Yes Then
            Dim cmd = Connection.CreateCommand
            Dim Server = New Microsoft.SqlServer.Management.Smo.Server(New ServerConnection(Connection))
            Server.ConnectionContext.ExecuteNonQuery(Script)
            Dim WasOnStack = VerifyDatabaseVersionWithUpgradeOnStack
            VerifyDatabaseVersionWithUpgradeOnStack = True
            Try
                VerifyDatabaseVersion(Connection)
            Finally
                VerifyDatabaseVersionWithUpgradeOnStack = WasOnStack
            End Try
            If Not VerifyDatabaseVersionWithUpgradeOnStack Then mBox.MsgBox(My.Resources.msg_DatabaseChangedSuccessfully, MsgBoxStyle.OkCancel Or MsgBoxStyle.Information, My.Resources.txt_UpgradeDatabase, owner)
        Else
            Throw New ApplicationException(My.Resources.err_IncorrectDatabaseVersion)
        End If
    End Sub
End Module
''' <summary>Result of <see cref="VerifyDatabaseVersion"/></summary>
Public Structure DatabaseVerificationResult
    ''' <summary>Contains value of the <see cref="DatabaseGuid"/> property</summary>
    Public ReadOnly _DatabaseGuid As Guid?
    ''' <summary>Contains value of the <see cref="DatabaseVersion"/> property</summary>
    Public ReadOnly _DatabaseVersion As Version
    ''' <summary>CTor from database guid and version</summary>
    ''' <param name="DatabaseGuid">Dababase guid</param>
    ''' <param name="DatabaseVersion">Database version</param>
    Friend Sub New(ByVal DatabaseGuid As Guid, ByVal DatabaseVersion As Version)
        _DatabaseGuid = DatabaseGuid
        _DatabaseVersion = DatabaseVersion
    End Sub
    ''' <summary>Gets database Guid (if database provides it)</summary>
    ''' <returns>A dababase guid; or null if the database does not provide it's Guid</returns>
    Public ReadOnly Property DatabaseGuid() As Guid?
        Get
            Return _DatabaseGuid
        End Get
    End Property
    ''' <summary>Gets database version (if database provides it)</summary>
    ''' <returns>Database version or null if database doesn't provide it</returns>
    Public ReadOnly Property DatabaseVersion() As Version
        Get
            Return _DatabaseVersion
        End Get
    End Property
    ''' <summary>Gets value indicating if this application can work with database being verified</summary>
    ''' <returns>True if database guid matches expected guid and database version martches expected database version (with exception of <see cref="Version.Revision"/> part)</returns>
    Public ReadOnly Property IsOK() As Boolean
        Get
            Return DatabaseGuid IsNot Nothing AndAlso DatabaseVersion IsNot Nothing AndAlso DatabaseGuid = DALProperties.DatabaseGuid _
                   AndAlso DatabaseVersion IsNot Nothing AndAlso _
                    (DatabaseVersion.Major = DALProperties.DatabaseVersion.Major AndAlso DatabaseVersion.Minor = DALProperties.DatabaseVersion.Minor AndAlso DatabaseVersion.Build = DALProperties.DatabaseVersion.Build)
        End Get
    End Property
    ''' <summary>Gets value indicationg if given <see cref="DatabaseVerificationResult"/> shall be interpreted as success</summary>
    ''' <param name="a">A <see cref="DatabaseVerificationResult"/> to interpret</param>
    ''' <returns>True if <paramref name="a"/>.<see cref="DatabaseVerificationResult.IsOK">IsOK</see> is true; false otherwise</returns>
    Public Shared Operator IsTrue(ByVal a As DatabaseVerificationResult) As Boolean
        Return a.IsOK
    End Operator
    ''' <summary>Gets value indicationg if given <see cref="DatabaseVerificationResult"/> shall be interpreted as non-success</summary>
    ''' <param name="a">A <see cref="DatabaseVerificationResult"/> to interpret</param>
    ''' <returns>False if <paramref name="a"/>.<see cref="DatabaseVerificationResult.IsOK">IsOK</see> is true; true otherwise</returns>
    Public Shared Operator IsFalse(ByVal a As DatabaseVerificationResult) As Boolean
        Return Not a.IsOK
    End Operator
    ''' <summary>Converts given <see cref="DatabaseVerificationResult"/> to <see cref="Boolean"/></summary>
    ''' <param name="a">A <see cref="DatabaseVerificationResult"/> to convert</param>
    ''' <returns><see cref="Boolean"/> - <paramref name="a"/>.<see cref="IsOK">IsOK</see></returns>
    Public Shared Widening Operator CType(ByVal a As DatabaseVerificationResult) As Boolean
        Return a.IsOK
    End Operator
    ''' <summary>Converts given <see cref="DatabaseVerificationResult"/> to <see cref="Boolean"/> and negates it</summary>
    ''' <param name="a">A <see cref="DatabaseVerificationResult"/></param>
    ''' <returns>Not <paramref name="a"/>.<see cref="IsOK">IsOK</see></returns>
    Public Shared Operator Not(ByVal a As DatabaseVerificationResult) As Boolean
        Return Not a.IsOK
    End Operator
End Structure
