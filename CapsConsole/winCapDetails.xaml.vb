Partial Public Class winCapDetails
    Public Sub New(ByVal Caps As IEnumerable(Of Cap))
        InitializeComponent()
        lstCaps.ItemsSource = Caps
    End Sub
End Class
