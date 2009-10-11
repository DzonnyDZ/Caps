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

Public Class Country
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
    Public ReadOnly Property Name$()
        Get
            Return My.Resources.Countries.ResourceManager.GetString("cc_" & Code2, My.Resources.Countries.Culture)
        End Get
    End Property
    Public ReadOnly Property ImageSource() As String
        Get
            Return String.Format("/CapsConsole;component/Resources/Flags/{0}.png", Code2)
        End Get
    End Property
End Class