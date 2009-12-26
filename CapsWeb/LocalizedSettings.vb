Imports <xmlns:html="http://www.w3.org/1999/xhtml">

''' <summary>Provides access to localizable application settings</summary>
Public Module LocalizedSettings
    ''' <summary>Gets value of localizable settings key</summary>
    ''' <param name="key">Key to get value of (unlocalized name)</param>
    ''' <returns>Value of the key in current UI culture, one of it's parent cultures, or unlocalized default value</returns>
    Public Function GetValue(ByVal key As String) As String
        Dim Culture = Threading.Thread.CurrentThread.CurrentUICulture
        While Culture IsNot Nothing AndAlso Not Culture.Equals(System.Globalization.CultureInfo.InvariantCulture)
            Dim val = ConfigurationManager.AppSettings(String.Format("{0}_{1}", key, Culture.Name))
            If val IsNot Nothing Then Return val
            Culture = Culture.Parent
        End While
        Return ConfigurationManager.AppSettings(key)
    End Function
    ''' <summary>Gets content of localizable content file</summary>
    ''' <param name="name">Name (withouth path, with extension, without localization part) of file to get content of</param>
    ''' <returns>Text of file in current UI culture, one of it's parent cultures, or unlocalized default value; null when there is neither unlocalized file.</returns>
    Public Function GetContent(ByVal name As String) As String
        Dim filename = GetFilePath(name)
        If filename Is Nothing Then Return Nothing
        Return My.Computer.FileSystem.ReadAllText(filename)
    End Function

    ''' <summary>Gets path of localizable content file</summary>
    ''' <param name="name">Name (withouth path, with extension, without localization part) of file to get content of</param>
    ''' <returns>Path of file in current UI culture, one of it's parent cultures, or unlocalized default value; null when there is neither unlocalized file.</returns>
    Public Function GetFilePath(ByVal name As String) As String
        Dim Culture = Threading.Thread.CurrentThread.CurrentUICulture
        Dim filename$ = Nothing
        While Culture IsNot Nothing AndAlso Not Culture.Equals(System.Globalization.CultureInfo.InvariantCulture)
            Dim path = IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,
                                       String.Format("Content\{0}.{1}{2}",
                                                     IO.Path.GetFileNameWithoutExtension(name),
                                                     Culture.Name,
                                                     IO.Path.GetExtension(name)))
            If IO.File.Exists(path) Then
                filename = path
                Exit While
            End If
            Culture = Culture.Parent
        End While
        If filename Is Nothing Then filename = IO.Path.Combine(HttpContext.Current.Request.ApplicationPath,
                                       String.Format("Content\{0}", name))
        If Not IO.File.Exists(filename) Then Return Nothing
        Return filename
    End Function
    ''' <summary>Gets &lt;body> of localizable content html file</summary>
    ''' <param name="name">Name (withouth path, with extension, without localization part) of file to get content of</param>
    ''' <returns>&lt;body> element of file in current UI culture, one of it's parent cultures, or unlocalized default value; null when there is neither unlocalized file.</returns>
    Public Function GetBody(ByVal name As String) As XElement
        Dim filename = GetFilePath(name)
        If filename Is Nothing Then Return Nothing
        Dim doc = XDocument.Load(filename)
        Return doc.<html:html>.<html:body>.FirstOrDefault
    End Function
    ''' <summary>Gets inner XML of &lt;body> of localizable content html file</summary>
    ''' <param name="name">Name (withouth path, with extension, without localization part) of file to get content of</param>
    ''' <returns>Inner XML of &lt;body> element of file in current UI culture, one of it's parent cultures, or unlocalized default value; null when there is neither unlocalized file.</returns>
    Public Function GetBodyHtml(ByVal name As String) As String
        Dim el = GetBody(name)
        If el Is Nothing Then Return Nothing
        Dim ret As New StringBuilder
        For Each nd In el.Nodes
            ret.Append(nd.ToString)
        Next
        Return ret.ToString
    End Function
End Module
