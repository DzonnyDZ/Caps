Class Application

    Private Sub Application_Exit(ByVal sender As Object, ByVal e As System.Windows.ExitEventArgs) Handles Me.Exit
        My.Settings.Save()
        If EntityConnection IsNot Nothing Then EntityConnection.Close()
    End Sub

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As System.Windows.StartupEventArgs) Handles Me.Startup
        System.Windows.Forms.Application.EnableVisualStyles()
        Tools.WindowsT.IndependentT.MessageBox.DefaultImplementation = GetType(Tools.WindowsT.WPF.DialogsT.MessageBox)
        If My.Settings.Language <> "" Then
            Try
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(My.Settings.Language)
            Catch : End Try
        End If
    End Sub

End Class
