Imports System.Data, System.Reflection
Imports System.Data.SqlClient

Namespace Data
    Public Module DALProperties
        ''' <summary>Expected GUID of database returned by <c>dbo.GetDatabaseVersion</c></summary>
        Public ReadOnly DatabaseGuid As New Guid("{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}")
        ''' <summary>Expected version of database returned by <c>dbo.GetDatabaseVersion</c></summary>
        ''' <remarks><see cref="Version.Revision"/> part is ignored</remarks>
        Public ReadOnly DatabaseVersion As New Version(0, 1, 5, 0)
        Private VersionRegEx As New System.Text.RegularExpressions.Regex("^(?<Guid>{[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}})(?<Version>[0-9]{1,2}(\.[0-9]{1,2}){2,3})$", Text.RegularExpressions.RegexOptions.Compiled Or Text.RegularExpressions.RegexOptions.CultureInvariant Or Text.RegularExpressions.RegexOptions.ExplicitCapture)
        ''' <summary>Verifies if database with given connection is caps database of expected version</summary>
        ''' <param name="Connection">Connection to the database</param>
        ''' <returns>Database version verification result</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="Connection"/> is null</exception>
        Public Function VerifyDatabaseVersion(ByVal Connection As SqlConnection) As DatabaseVerificationResult
            If Connection Is Nothing Then Throw New ArgumentNullException("Connection")
            Dim cmd = Connection.CreateCommand
            cmd.CommandText = "SELECT dbo.GetDatabaseVersion()"
            cmd.CommandType = CommandType.Text
            Try
                Dim result$ = cmd.ExecuteScalar
                If result Is Nothing Then Return New DatabaseVerificationResult
                Dim match = VersionRegEx.Match(result)
                If Not match.Success Then Return New DatabaseVerificationResult
                Return New DatabaseVerificationResult(New Guid(match.Groups!Guid.Value), New Version(match.Groups!Version.Value))
            Catch ex As Exception
                Return New DatabaseVerificationResult
            End Try
        End Function

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
End Namespace