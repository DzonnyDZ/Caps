Imports System.Runtime.CompilerServices
''' <summary>Miscelaneous functions</summary>
Friend Module Misc
    ''' <summary>Gets the 32-bit ARGB value of given <see cref="Color"/> structure.</summary>
    ''' <param name="Color">Color to get ARGB value of</param>
    ''' <returns>The 32-bit ARGB value of <paramref name="Color"/>.</returns>
    <Extension()> _
    Public Function ToArgb(ByVal Color As Color) As Integer
        Return System.Drawing.Color.FromArgb(Color.A, Color.R, Color.G, Color.B).ToArgb
    End Function
    ''' <summary>Gets the 32-bit ARGB value of given <see cref="Color"/> structure.</summary>
    ''' <param name="Color">COlor to get ARGB value of or null</param>
    ''' <returns>The 32-bit ARGB value of <paramref name="Color"/>, or null when <paramref name="Color"/> is null.</returns>
    <Extension()> _
  Public Function ToArgb(ByVal Color As Color?) As Integer?
        If Color.HasValue Then
            Return Color.Value.ToArgb
        Else
            Return Nothing
        End If
    End Function
    ''' <summary>Gets <see cref="System.Drawing.Color"/> equivalent to given <see cref="Windows.Media.Color"/></summary>
    ''' <param name="Color"><see cref="Windows.Media.Color"/> to get <see cref="System.Drawing.Color"/> for</param>
    ''' <returns><see cref="System.Drawing.Color"/> initialized to same ARGB as <paramref name="Color"/></returns>
    <Extension()> Function ToColor(ByVal Color As Windows.Media.Color) As System.Drawing.Color
        Return System.Drawing.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)
    End Function
    ''' <summary>Gets <see cref="System.Drawing.Color"/> equivalent to given <see cref="Windows.Media.Color"/></summary>
    ''' <param name="Color"><see cref="Windows.Media.Color"/> to get <see cref="System.Drawing.Color"/> for</param>
    ''' <returns><see cref="System.Drawing.Color"/> initialized to same ARGB as <paramref name="Color"/>; null when <paramref name="Color"/> is null</returns>
    <Extension()> Function ToColor(ByVal Color As Windows.Media.Color?) As System.Drawing.Color?
        If Color Is Nothing Then Return Nothing
        Return System.Drawing.Color.FromArgb(Color.Value.A, Color.Value.R, Color.Value.G, Color.Value.B)
    End Function
    ''' <summary>Gets <see cref="Windows.Media.Color"/> equivalent to given <see cref="System.Drawing.Color"/></summary>
    ''' <param name="Color"><see cref="System.Drawing.Color"/> to get <see cref="Windows.Media.Color"/> for</param>
    ''' <returns><see cref="Windows.Media.Color"/> initialized to same ARGB as <paramref name="Color"/></returns>
    <Extension()> Function ToColor(ByVal Color As System.Drawing.Color) As Windows.Media.Color
        Return Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)
    End Function
    ''' <summary>Gets <see cref="Windows.Media.Color"/> equivalent to given <see cref="System.Drawing.Color"/></summary>
    ''' <param name="Color"><see cref="System.Drawing.Color"/> to get <see cref="Windows.Media.Color"/> for</param>
    ''' <returns><see cref="Windows.Media.Color"/> initialized to same ARGB as <paramref name="Color"/>; null when <paramref name="Color"/> is null</returns>
    <Extension()> Function ToColor(ByVal Color As System.Drawing.Color?) As Windows.Media.Color?
        If Color Is Nothing Then Return Nothing
        Return Windows.Media.Color.FromArgb(Color.Value.A, Color.Value.R, Color.Value.G, Color.Value.B)
    End Function

End Module
