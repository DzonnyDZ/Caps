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
End Module
