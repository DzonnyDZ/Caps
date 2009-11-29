Namespace My.Resources
    Partial Class Countries
        Public Shared ReadOnly Property Countries() As IEnumerable(Of Country)
            Get
                Return From item As DictionaryEntry In My.Resources.Countries.ResourceManager.GetResourceSet(If(Culture, System.Globalization.CultureInfo.CurrentUICulture), True, True) _
                       Select c = New Country(DirectCast(item.Key, String).Split("_"c)(1)) _
                       Order By c.Name
            End Get
        End Property
    End Class
End Namespace
''' <summary>Provides basic information about contry and country name and flag</summary>
Public Class Country
    ''' <summary>For given 2-letters country code gets localized country name</summary>
    ''' <param name="Code2">2-letters country code</param>
    ''' <returns>Localized country name</returns>
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)> _
    Public Shared Function GetCountryNameFromCode(ByVal Code2 As String) As String
        Return My.Resources.Countries.ResourceManager.GetString("cc_" & Code2, System.Threading.Thread.CurrentThread.CurrentUICulture)
    End Function
    Public Sub New(ByVal Code As String)
        _Code2 = Code
    End Sub
    ''' <summary>Contains value of the <see cref="Code2"/> property</summary>
    Private _Code2 As String
    ''' <summary>Get 2-letters country code</summary>
    Public ReadOnly Property Code2() As String
        Get
            Return _Code2
        End Get
    End Property
    ''' <summary>gets localized name of country</summary>
    Public ReadOnly Property Name$()
        Get
            Return My.Resources.Countries.ResourceManager.GetString("cc_" & Code2, System.Threading.Thread.CurrentThread.CurrentUICulture)
        End Get
    End Property
    ''' <summary>gets source of country flag image</summary>
    Public ReadOnly Property ImageSource() As String
        Get
            Return String.Format("/CapsConsole;component/Resources/Flags/{0}.png", Code2)
        End Get
    End Property
End Class