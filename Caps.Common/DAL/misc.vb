''' <summary>Possible storages of iamges</summary>
<Flags()>
Public Enum ImageSources
    ''' <summary>File system storage</summary>
    FileSystem = 1
    ''' <summary>Database storage</summary>
    Database = 2
    ''' <summary>Any known storage</summary>
    Any = FileSystem Or Database
End Enum