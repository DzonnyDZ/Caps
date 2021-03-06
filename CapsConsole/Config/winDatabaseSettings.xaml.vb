﻿Imports Caps.Data
''' <summary>Edits settings stored in database</summary>
Public Class winDatabaseSettings : Implements IDisposable
    ''' <summary>Data context</summary>
    Private context As CapsDataContext
    ''' <summary>Indicates if <see cref="Content"/> will be disposed on window disposal</summary>
    Private ReadOnly disposeContext As Boolean
    ''' <summary>CTor - creates a new instance of the <see cref="winDatabaseSettings"/> class (using default data context)</summary>
    Public Sub New()
        InitializeComponent()
        Me.context = New CapsDataContext(Main.EntityConnection)
        disposeCOntext = True
    End Sub
    ''' <summary>CTor - creates a new instance of the <see cref="winDatabaseSettings"/> class using given data context</summary>
    ''' <param name="context">A <see cref="CapsDataContext"/> to be used by this window</param>
    ''' <exception cref="ArgumentNullException"><paramref name="context"/> is null</exception>
    Public Sub New(ByVal context As CapsDataContext)
        If context Is Nothing Then Throw New ArgumentNullException("context")
        InitializeComponent()
        Me.context = context
        disposeContext = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

#Region "IDisposable Support"
    ''' <summary>To detect redundant calls</summary>
    Private disposedValue As Boolean

    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">True when disposing</param>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If context IsNot Nothing AndAlso disposeContext Then context.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            context.SaveChanges()
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Return
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        dgrData.ItemsSource = context.Settings
    End Sub
End Class
