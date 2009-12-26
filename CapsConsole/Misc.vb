Imports System.Runtime.CompilerServices, Tools.ExtensionsT
Imports System.ComponentModel, Tools

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
            currobj = LogicalTreeHelper.GetParent(obj)
            If currobj Is Nothing Then Return Nothing
            If TypeOf currobj Is TAncestor Then Return currobj
        Loop
        Return Nothing
    End Function

    ''' <summary>Undos all INSERTs performed with <see cref="System.Data.Linq.Table(Of TEntity)"/></summary>
    ''' <param name="Table">Table to delete all inserted not commited items of coresponding type from</param>
    ''' <typeparam name="TEntity">Type of items in table</typeparam>
    ''' <remarks>This methods delets all items in <paramref name="Table"/>.<see cref="System.Data.Linq.Table.Context">Context</see>.<see cref="System.Data.Linq.DataContext.GetChangeSet">GetChangeSet</see>.<see cref="System.Data.Linq.ChangeSet.Inserts">Inserts</see> which are of type <typeparamref name="TEntity"/>.</remarks>
    ''' <exception cref="ArgumentNullException"><paramref name="Table"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="Table"/>.<see cref="System.Data.Linq.Table.Context">Context</see> is null</exception>
    <Extension()> Public Sub DeleteAllNew(Of TEntity As Class)(ByVal Table As System.Data.Linq.Table(Of TEntity))
        If Table Is Nothing Then Throw New ArgumentNullException("Table")
        If Table.Context Is Nothing Then Throw New ArgumentException(My.Resources.ex_MustNotBeNull.f("Table.Context"))
        Table.DeleteAllOnSubmit(Table.Context.GetChangeSet.Inserts.OfType(Of TEntity))
    End Sub
    ''' <summary>Undos all DELETEs performed with <see cref="System.Data.Linq.Table(Of TEntity)"/></summary>
    ''' <param name="Table">Table to insert (undelete) all deleted not commited items of coresponding type to</param>
    ''' <typeparam name="TEntity">Type of items in table</typeparam>
    ''' <remarks>This methods re-inserts all items in <paramref name="Table"/>.<see cref="System.Data.Linq.Table.Context">Context</see>.<see cref="System.Data.Linq.DataContext.GetChangeSet">GetChangeSet</see>.<see cref="System.Data.Linq.ChangeSet.Deletes">Deletes</see> which are of type <typeparamref name="TEntity"/>.</remarks>
    ''' <exception cref="ArgumentNullException"><paramref name="Table"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="Table"/>.<see cref="System.Data.Linq.Table.Context">Context</see> is null</exception>
    <Extension()> Public Sub RecoverAllDeleted(Of TEntity As Class)(ByVal Table As System.Data.Linq.Table(Of TEntity))
        If Table Is Nothing Then Throw New ArgumentNullException("Table")
        If Table.Context Is Nothing Then Throw New ArgumentException(My.Resources.ex_MustNotBeNull.f("Table.Context"))
        Table.InsertAllOnSubmit(Table.Context.GetChangeSet.Deletes.OfType(Of TEntity))
    End Sub
    ''' <summary>Sets windows position and size. Prevents window from leaking out of screen.</summary>
    ''' <param name="Window">Window to set position of</param>
    ''' <param name="Position">Proposed position and size of <paramref name="Window"/> in window client coordinates</param>
    ''' <exception cref="ArgumentNullException"><paramref name="Window"/> is null</exception>
    ''' <remarks>If <paramref name="Position"/> <see cref="System.Drawing.Rectangle.IsEmpty">is empty</see>, neither window position nor size is set, but off-screen prevention alghoritm is run for window current position.</remarks>
    <Extension()> Public Sub SetWindowPosition(ByVal Window As Window, ByVal Position As System.Drawing.Rectangle)
        If Window Is Nothing Then Throw New ArgumentNullException("Window")
        If Not Position.IsEmpty Then
            Window.Left = Position.Left
            Window.Top = Position.Top
            Window.Height = Position.Height
            Window.Width = Position.Width
        End If



        Dim winTopLeft = PresentationSource.FromVisual(Window).CompositionTarget.TransformToDevice.Transform(New Point(Window.Left, Window.Top))
        Dim winBottomRight = PresentationSource.FromVisual(Window).CompositionTarget.TransformToDevice.Transform(New Point(Window.Left + Window.ActualWidth, Window.Top + Window.ActualHeight))
        Dim winRect = System.Drawing.Rectangle.FromLTRB(winTopLeft.X, winTopLeft.Y, winBottomRight.X, winBottomRight.Y)

        Dim winScreen = Forms.Screen.FromRectangle(winRect)

        If winScreen.WorkingArea.Contains(winRect) Then Exit Sub

        Dim Left = winScreen.GetNeighbourScreen(Direction.Left)
        Dim Right = winScreen.GetNeighbourScreen(Direction.Right)
        Dim Top = winScreen.GetNeighbourScreen(Direction.Top)
        Dim Bottom = winScreen.GetNeighbourScreen(Direction.Bottom)

        Dim MoveRight = 0%
        Dim MoveDown = 0%

        If winRect.Left < winScreen.WorkingArea.Left AndAlso Left Is Nothing Then MoveRight = winScreen.WorkingArea.Left - winRect.Left
        If winRect.Top < winScreen.WorkingArea.Top AndAlso Top Is Nothing Then MoveDown = winScreen.WorkingArea.Top - winRect.Top
        If winRect.Bottom > winScreen.WorkingArea.Bottom AndAlso Bottom Is Nothing Then MoveDown = winScreen.WorkingArea.Bottom - winRect.Bottom
        If winRect.Right > winScreen.WorkingArea.Right AndAlso Right Is Nothing Then MoveRight = winScreen.WorkingArea.Right - winRect.Right

        If MoveRight <> 0 OrElse MoveDown <> 0 Then
            Dim newloc = PresentationSource.FromVisual(Window).CompositionTarget.TransformFromDevice.Transform(New Point(winRect.Left + MoveRight, winRect.Top + MoveDown))
            Window.Left = newloc.X
            Window.Top = newloc.Y
        End If
    End Sub
    ''' <summary>Gtes size and location of the <see cref="Window"/></summary>
    ''' <param name="Window">A <see cref="Window"/> to get size and location of</param>
    ''' <returns>Rectangle of <paramref name="Window"/> in window coordinates</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="Window"/> is null</exception>
    <Extension()> Public Function GetWindowPosition(ByVal Window As Window) As System.Drawing.Rectangle
        If Window Is Nothing Then Throw New ArgumentNullException("Window")
        Return New System.Drawing.Rectangle(Window.Left, Window.Top, Window.ActualWidth, Window.ActualHeight)
    End Function

    ''' <summary>Gets neighbouring screen to given screen in given direction</summary>
    ''' <param name="Screen">Screen to get neighbour of</param>
    ''' <param name="Direction">Direction in which (on which side) get neighbour</param>
    ''' <returns>Screen that directly neighbours with <paramref name="Screen"/> in given <paramref name="Direction"/>.</returns>
    ''' <exception cref="ArgumentNullException"><paramref name="Screen"/> is null</exception>
    ''' <exception cref="InvalidEnumArgumentException"><paramref name="Direction"/> is not one of <see cref="Direction"/> values</exception>
    ''' <remarks>Maximal allowed gap or overlap between screens is 5px. For screen (B) to be considered neighbour with <paramref name="Screen"/> (A) screen edges must either share at least 25% of appropriate edge of <paramref name="Screen"/> (A) or screen (B) must share 100% of it's appropriate edge with <paramref name="Screen"/> (A).</remarks>
    <Extension()> Public Function GetNeighbourScreen(ByVal Screen As Forms.Screen, ByVal Direction As Direction) As Forms.Screen
        If Screen Is Nothing Then Throw New ArgumentNullException("Screen")
        For Each scr In Forms.Screen.AllScreens
            If Screen Is scr Then Continue For
            Dim vDistance = If(scr.Bounds.Top < Screen.Bounds.Top, scr.Bounds.Bottom - scr.Bounds.Top, Screen.Bounds.Bottom - scr.Bounds.Top)
            Dim hDistance = If(scr.Bounds.Left < Screen.Bounds.Left, scr.Bounds.Right - Screen.Bounds.Left, Screen.Bounds.Right - scr.Bounds.Left)
            Select Case Direction
                Case Misc.Direction.Left
                    If Math.Abs(scr.Bounds.Right - Screen.Bounds.Left) < 5 AndAlso vDistance >= Screen.Bounds.Height / 4 Then Return scr
                Case Misc.Direction.Right
                    If Math.Abs(scr.Bounds.Left - Screen.Bounds.Right) < 5 AndAlso vDistance >= Screen.Bounds.Height / 4 Then Return scr
                Case Misc.Direction.Top
                    If Math.Abs(scr.Bounds.Bottom - Screen.Bounds.Top) < 5 AndAlso hDistance >= Screen.Bounds.Width / 4 Then Return scr
                Case Misc.Direction.Bottom
                    If Math.Abs(scr.Bounds.Top - Screen.Bounds.Bottom) < 5 AndAlso hDistance >= Screen.Bounds.Width / 4 Then Return scr
                Case Else : Throw New InvalidEnumArgumentException("Direction", Direction, Direction.GetType)
            End Select
        Next
        Return Nothing
    End Function
    ''' <summary>Neighbourhood directions</summary>
    Public Enum Direction
        ''' <summary>Left neighbour</summary>
        Left
        ''' <summary>Right neighbour</summary>
        Right
        ''' <summary>Top (front) neighbour</summary>
        Top
        ''' <summary>Bottom (back) neighbour</summary>
        Bottom
    End Enum
    ''' <summary>Gets value indicating if <see cref="CapEditor.SaveMode"/> indicates save action</summary>
    ''' <param name="this">Value to test</param>
    ''' <returns>True if <paramref name="this"/> indicates save action; false if it does not</returns>
    <Extension()> Function IsSave(ByVal this As CapEditor.SaveMode) As Boolean
        Select Case this
            Case CapEditor.SaveMode.SaveAndClose, CapEditor.SaveMode.SaveAndNew, CapEditor.SaveMode.SaveAndNext, CapEditor.SaveMode.SaveAndNextNoClean, CapEditor.SaveMode.SaveAndPrevious : Return True
            Case Else : Return False
        End Select
    End Function
    ''' <summary>Pareses command line arguments from string array to key-values dictionary</summary>
    ''' <param name="arguments">Application arguments</param>
    ''' <param name="ignore1st">True to indicate that 1st (index 0) arggument in <paramref name="arguments"/> contains application executable name and should be ignored by this method</param>
    ''' <returns>Dictionary containing values in front of = from each parameter as keys aand remainders of parameters as values.</returns>
    ''' <remarks>Each commmand line ergument is expected to have form <c>key</c> or <c>key=value</c></remarks>
    Public Function ParseParameters(ByVal arguments As IEnumerable(Of String), Optional ByVal ignore1st As Boolean = False) As Dictionary(Of String, List(Of String))
        Dim i% = 0
        Dim ret As New Dictionary(Of String, List(Of String))
        For Each arg In arguments
            Try
                If i = 0 AndAlso ignore1st Then Continue For
                Dim name$, value$
                If arg.Contains("="c) Then
                    name = arg.Substring(0, arg.IndexOf("="c))
                    value = arg.Substring(arg.IndexOf("="c) + 1)
                Else
                    name = arg
                    value = Nothing
                End If
                If ret.ContainsKey(name) AndAlso value <> "" Then
                    ret(name).Add(value)
                Else
                    ret.Add(name, New List(Of String))
                    If value <> "" Then ret(name).Add(value)
                End If
            Finally
                i += 1
            End Try
        Next
        Return ret
    End Function

End Module
