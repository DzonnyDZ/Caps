Imports Caps.Data

''' <summary>Window used to migrate images between database and file system</summary>
Public Class winSyncImages
    Implements IDisposable
    Private context As CapsDataContext

#Region "IDisposable Support"
    ''' <summary>To detect redundant calls</summary>
    Private disposedValue As Boolean

    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                context.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
