Imports System.Data

Module Main
    Public DatabaseConnection As System.Data.SqlClient.SqlConnection
    Public Workspace As New System.Data.Metadata.Edm.MetadataWorkspace( ) with { }
    Public EntityConnection As EntityClient.EntityConnection
End Module
