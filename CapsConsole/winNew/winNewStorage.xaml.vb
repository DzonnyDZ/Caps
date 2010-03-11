Imports Tools, Tools.TypeTools, Tools.ExtensionsT
Imports System.ComponentModel
Imports mBox = Tools.WindowsT.IndependentT.MessageBox
Imports Tools.CollectionsT.GenericT
Imports Caps.Data

Partial Public Class winNewStorage
    Implements IDisposable
    ''' <summary>CTor</summary>
    ''' <exception cref="ArgumentNullException"><paramref name="Context"/> is null</exception>
    Public Sub New()
        InitializeComponent()
        Me.Context = New CapsDataContext(Main.EntityConnection)
    End Sub
    ''' <summary>Data context</summary>
    Private Context As CapsDataContext
    Private _NewObject As Storage
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Try
            _NewObject = New Storage() With {.StorageNumber = txtNumber.Text, .Description = txtDescription.Text, .StorageTypeID = cmbStorageType.SelectedValue}
            Context.Storages.AddObject(_NewObject)
        Catch ex As Exception
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Try
            Context.SaveChanges()
        Catch ex As Exception
            'Context.Storages.DeleteAllNew()
            mBox.Error_XTW(ex, ex.GetType.Name, Me)
            Exit Sub
        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    Public ReadOnly Property NewObject() As Storage
        Get
            Return _NewObject
        End Get
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        Me.DialogResult = False
        Me.Close()
    End Sub



    Private Sub cmdNewType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdNewType.Click
        Using win As New winNewSimple(winNewSimple.SimpleTypes.StorageType)
            If win.ShowDialog Then
                Context.Attach(win.NewObject)
                DirectCast(cmbStorageType.ItemsSource, ListWithEvents(Of StorageType)).Add(DirectCast(win.NewObject, StorageType))
                cmbStorageType.Items.Refresh()
                cmbStorageType.SelectedItem = win.NewObject
            End If
        End Using
    End Sub

    Private Sub winNewStorage_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cmbStorageType.ItemsSource = New ListWithEvents(Of StorageType)(From item In Context.StorageTypes Order By item.Name)
    End Sub

#Region "IDisposable Support"
    ''' <summary>To detect redundant calls</summary>
    Private disposedValue As Boolean

    ''' <summary>Implements <see cref="IDisposable.Dispose"/></summary>
    ''' <param name="disposing">Trie whan called from <see cref="Dispose"/></param>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If Context IsNot Nothing Then Context.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub


    ''' <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    ''' <filterpriority>2</filterpriority>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
