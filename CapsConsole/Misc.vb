﻿Imports System.Runtime.CompilerServices
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

    ''' <summary>Searches for ancestor of given WPF object of given type</summary>
    ''' <param name="obj">Object to find ancestor of</param>
    ''' <typeparam name="TAncestor">Type of ancestor to find. This type must be or derive from <see cref="DependencyObject"/>.</typeparam>
    ''' <returns>The closest ancestor of object <paramref name="obj"/> which's type is <typeparamref name="TAncestor"/>. Null where there is no such ancestor.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> is null</exception>
    ''' <remarks>This function uses <see cref="ContentOperations.GetParent"/> to walk visual tree upwards from <paramref name="obj"/>.</remarks>
    <Extension()> Public Function FindAncestor(Of TAncestor As DependencyObject)(ByVal obj As DependencyObject) As TAncestor
        If obj Is Nothing Then Throw New ArgumentNullException("obj")
        Dim currobj As DependencyObject = obj
        Do
            currobj = ContentOperations.GetParent(currobj)
            If currobj Is Nothing Then Return Nothing
            If TypeOf currobj Is TAncestor Then Return currobj
        Loop
        Return Nothing
    End Function
End Module
