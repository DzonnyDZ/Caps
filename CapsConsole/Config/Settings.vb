﻿'This class allows you to handle specific events on the settings class:
' The SettingChanging event is raised before a setting's value is changed.
' The PropertyChanged event is raised after a setting's value is changed.
' The SettingsLoaded event is raised after the setting values are loaded.
' The SettingsSaving event is raised before the setting values are saved.
Partial Public NotInheritable Class MySettings

    Private Sub MySettings_SettingsLoaded(ByVal sender As Object, ByVal e As System.Configuration.SettingsLoadedEventArgs) Handles Me.SettingsLoaded
        If My.Settings.Version <> My.Application.Info.Version.ToString Then
            Try
                My.Settings.Upgrade()
            Catch : End Try
            My.Settings.Version = My.Application.Info.Version.ToString
            My.Settings.Save()
        End If
    End Sub
End Class
