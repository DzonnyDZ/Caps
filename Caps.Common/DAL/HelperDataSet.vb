Imports System.Data.SqlClient

Namespace Data

    Partial Class HelperDataSet
        Partial Class VarCharTableDataTable
            ''' <summary>Converts array of <see cref="String"/> to <see cref="VarCharTableDataTable"/></summary>
            ''' <param name="a">An array to be converted</param>
            ''' <returns>A new <see cref="VarCharTableDataTable"/> filled with values from <paramref name="a"/>; null when <paramref name="a"/> is null</returns>
            Public Shared Widening Operator CType(ByVal a As String()) As VarCharTableDataTable
                If a Is Nothing Then Return Nothing
                Dim ret As New VarCharTableDataTable
                For Each value In a
                    ret.AddVarCharTableRow(value)
                Next
                Return ret
            End Operator
            ''' <summary>Converts <see cref="VarCharTableDataTable"/> to array of <see cref="String"/></summary>
            ''' <param name="a">A <see cref="VarCharTableDataTable"/> to be converted</param>
            ''' <returns>An array containing <see cref="VarCharTableRow.Value">Values</see> from <paramref name="a"/>; null when <paramref name="a"/> is null.</returns>
            Public Shared Widening Operator CType(ByVal a As VarCharTableDataTable) As String()
                If a Is Nothing Then Return Nothing
                Dim ret(a.Rows.Count - 1) As String
                Dim i As Integer = 0
                For Each row In a
                    ret(i) = row.Value
                    i += 1
                Next
                Return ret
            End Operator
        End Class
        Partial Class IntTableDataTable
            ''' <summary>Converts array of <see cref="Integer"/> to <see cref="IntTableDataTable"/></summary>
            ''' <param name="a">An array to be converted</param>
            ''' <returns>A new <see cref="IntTableDataTable"/> filled with values from <paramref name="a"/>; null when <paramref name="a"/> is null</returns>
            Public Shared Widening Operator CType(ByVal a As Integer()) As IntTableDataTable
                If a Is Nothing Then Return Nothing
                Dim ret As New IntTableDataTable
                For Each value In a
                    ret.AddIntTableRow(value)
                Next
                Return ret
            End Operator
            ''' <summary>Converts <see cref="IntTableDataTable"/> to array of <see cref="Integer"/></summary>
            ''' <param name="a">A <see cref="IntTableDataTable"/> to be converted</param>
            ''' <returns>An array containing <see cref="IntTableRow.Value">Values</see> from <paramref name="a"/>; null when <paramref name="a"/> is null.</returns>
            Public Shared Widening Operator CType(ByVal a As IntTableDataTable) As Integer()
                If a Is Nothing Then Return Nothing
                Dim ret(a.Rows.Count - 1) As Integer
                Dim i As Integer = 0
                For Each row In a
                    ret(i) = row.Value
                    i += 1
                Next
                Return ret
            End Operator
        End Class

    End Class

End Namespace

Namespace Data.HelperDataSetTableAdapters
    Partial Class GetSimilarCapsTableAdapter
        ''' <summary>Opens reader to read <see cref="Cap">Caps</see> similar to cap with given properties</summary>
        ''' <param name="CapTypeID">ID of <see cref="CapType"/></param>
        ''' <param name="MainTypeID">ID of <see cref="MainType"/></param>
        ''' <param name="ShapeID">ID of <see cref="Shape"/></param>
        ''' <param name="CapName">Name of cap</param>
        ''' <param name="MainText">Main text of cap</param>
        ''' <param name="SubTitle">Sub-main text of cap</param>
        ''' <param name="BackColor1">Primary background color of cap (ARGB value)</param>
        ''' <param name="BackColor2">Secondary background color of cap (ARGB value)</param>
        ''' <param name="ForeColor">Primary fore color of cap (ARGB value)</param>
        ''' <param name="MainPicture">Description of main picture fo cap</param>
        ''' <param name="TopText">Full text of cap top side</param>
        ''' <param name="SideText">Full text of cap side side</param>
        ''' <param name="BottomText">Full text of cap bottom side</param>
        ''' <param name="MaterialID">ID of <see cref="Material"/></param>
        ''' <param name="Surface">Identifies cap surface - 'G' or 'M'</param>
        ''' <param name="Size">Primary size of cap (in mms)</param>
        ''' <param name="Size2">Secondary size of cap (in mms)</param>
        ''' <param name="Height">Height of cap (in mms)</param>
        ''' <param name="Is3D">Idicates if cap surface is three-dimensional or not</param>
        ''' <param name="Year">Yer whan cap was found</param>
        ''' <param name="CountryCode">Code (ISO-3) of country where the cap was found</param>
        ''' <param name="Note">Note on cap</param>
        ''' <param name="CompanyID">ID of <see cref="Company"/></param>
        ''' <param name="ProductID">ID of <see cref="Product"/></param>
        ''' <param name="ProductTypeID">ID of <see cref="ProductType"/></param>
        ''' <param name="StorageID">ID of <see cref="Storage"/></param>
        ''' <param name="ForeColor2">Secondary foreground color of cap (ARGB value)</param>
        ''' <param name="PictureType">Type of picture of cap - 'P', 'D', 'L' or 'G'</param>
        ''' <param name="HasBottom">Value indicating if cap has significant bottom side</param>
        ''' <param name="HasSide">Value indicating if cap has significant side side</param>
        ''' <param name="AnotherPictures">Describes all but main pictures of cap</param>
        ''' <param name="CategoryIDs">IDs of <see cref="Category">Categories</see></param>
        ''' <param name="Keywords">Keywords</param>
        ''' <param name="CountryOfOrigin">Code (ISO-3) of country cap originates from</param>
        ''' <param name="IsDrink">Indicates if product is dring or not</param>
        ''' <param name="State">Cap state 1-5 (1-best, 5-worst)</param>
        ''' <param name="TargetID">Id of <see cref="Target"/></param>
        ''' <param name="IsAlcoholic">Indicates if cap product is alcoholic</param>
        ''' <param name="CapSignIDs">IDs of <see cref="CapSign">CapSigns</see></param>
        ''' <returns>Open <see cref="SqlDataReader"/> to read <see cref="Cap">Caps</see> from</returns>
        ''' <exception cref="SqlException">SQL server error occured</exception>
        ''' <exception cref="InvalidOperationException">The current state of <see cref="Connection"/> is closed.</exception>
        Public Function ReadSimilarCaps( _
                    ByVal CapTypeID As Global.System.Nullable(Of Integer), _
                    ByVal MainTypeID As Global.System.Nullable(Of Integer), _
                    ByVal ShapeID As Global.System.Nullable(Of Integer), _
                    ByVal CapName As String, _
                    ByVal MainText As String, _
                    ByVal SubTitle As String, _
                    ByVal BackColor1 As Global.System.Nullable(Of Integer), _
                    ByVal BackColor2 As Global.System.Nullable(Of Integer), _
                    ByVal ForeColor As Global.System.Nullable(Of Integer), _
                    ByVal MainPicture As String, _
                    ByVal TopText As String, _
                    ByVal SideText As String, _
                    ByVal BottomText As String, _
                    ByVal MaterialID As Global.System.Nullable(Of Integer), _
                    ByVal Surface As String, _
                    ByVal Size As Global.System.Nullable(Of Integer), _
                    ByVal Size2 As Global.System.Nullable(Of Integer), _
                    ByVal Height As Global.System.Nullable(Of Integer), _
                    ByVal Is3D As Global.System.Nullable(Of Boolean), _
                    ByVal Year As Global.System.Nullable(Of Integer), _
                    ByVal CountryCode As String, _
                    ByVal Note As String, _
                    ByVal CompanyID As Global.System.Nullable(Of Integer), _
                    ByVal ProductID As Global.System.Nullable(Of Integer), _
                    ByVal ProductTypeID As Global.System.Nullable(Of Integer), _
                    ByVal StorageID As Global.System.Nullable(Of Integer), _
                    ByVal ForeColor2 As Global.System.Nullable(Of Integer), _
                    ByVal PictureType As String, _
                    ByVal HasBottom As Global.System.Nullable(Of Boolean), _
                    ByVal HasSide As Global.System.Nullable(Of Boolean), _
                    ByVal AnotherPictures As String, _
                    ByVal CategoryIDs As HelperDataSet.IntTableDataTable, _
                    ByVal Keywords As HelperDataSet.VarCharTableDataTable, _
                    ByVal CountryOfOrigin As String, _
                    ByVal IsDrink As Global.System.Nullable(Of Boolean), _
                    ByVal State As Global.System.Nullable(Of Short), _
                    ByVal TargetID As Global.System.Nullable(Of Integer), _
                    ByVal IsAlcoholic As Global.System.Nullable(Of Boolean),
                    ByVal CapSignIDs As HelperDataSet.IntTableDataTable) As SqlDataReader
            Dim cmd = Me.CommandCollection(0)
            With cmd
                If (CapTypeID.HasValue = True) Then
                    .Parameters(1).Value = CType(CapTypeID.Value, Integer)
                Else
                    .Parameters(1).Value = Global.System.DBNull.Value
                End If
                If (MainTypeID.HasValue = True) Then
                    .Parameters(2).Value = CType(MainTypeID.Value, Integer)
                Else
                    .Parameters(2).Value = Global.System.DBNull.Value
                End If
                If (ShapeID.HasValue = True) Then
                    .Parameters(3).Value = CType(ShapeID.Value, Integer)
                Else
                    .Parameters(3).Value = Global.System.DBNull.Value
                End If
                If (CapName Is Nothing) Then
                    .Parameters(4).Value = Global.System.DBNull.Value
                Else
                    .Parameters(4).Value = CType(CapName, String)
                End If
                If (MainText Is Nothing) Then
                    .Parameters(5).Value = Global.System.DBNull.Value
                Else
                    .Parameters(5).Value = CType(MainText, String)
                End If
                If (SubTitle Is Nothing) Then
                    .Parameters(6).Value = Global.System.DBNull.Value
                Else
                    .Parameters(6).Value = CType(SubTitle, String)
                End If
                If (BackColor1.HasValue = True) Then
                    .Parameters(7).Value = CType(BackColor1.Value, Integer)
                Else
                    .Parameters(7).Value = Global.System.DBNull.Value
                End If
                If (BackColor2.HasValue = True) Then
                    .Parameters(8).Value = CType(BackColor2.Value, Integer)
                Else
                    .Parameters(8).Value = Global.System.DBNull.Value
                End If
                If (ForeColor.HasValue = True) Then
                    .Parameters(9).Value = CType(ForeColor.Value, Integer)
                Else
                    .Parameters(9).Value = Global.System.DBNull.Value
                End If
                If (MainPicture Is Nothing) Then
                    .Parameters(10).Value = Global.System.DBNull.Value
                Else
                    .Parameters(10).Value = CType(MainPicture, String)
                End If
                If (TopText Is Nothing) Then
                    .Parameters(11).Value = Global.System.DBNull.Value
                Else
                    .Parameters(11).Value = CType(TopText, String)
                End If
                If (SideText Is Nothing) Then
                    .Parameters(12).Value = Global.System.DBNull.Value
                Else
                    .Parameters(12).Value = CType(SideText, String)
                End If
                If (BottomText Is Nothing) Then
                    .Parameters(13).Value = Global.System.DBNull.Value
                Else
                    .Parameters(13).Value = CType(BottomText, String)
                End If
                If (MaterialID.HasValue = True) Then
                    .Parameters(14).Value = CType(MaterialID.Value, Integer)
                Else
                    .Parameters(14).Value = Global.System.DBNull.Value
                End If
                If (Surface Is Nothing) Then
                    .Parameters(15).Value = Global.System.DBNull.Value
                Else
                    .Parameters(15).Value = CType(Surface, String)
                End If
                If (Size.HasValue = True) Then
                    .Parameters(16).Value = CType(Size.Value, Integer)
                Else
                    .Parameters(16).Value = Global.System.DBNull.Value
                End If
                If (Size2.HasValue = True) Then
                    .Parameters(17).Value = CType(Size2.Value, Integer)
                Else
                    .Parameters(17).Value = Global.System.DBNull.Value
                End If
                If (Height.HasValue = True) Then
                    .Parameters(18).Value = CType(Height.Value, Integer)
                Else
                    .Parameters(18).Value = Global.System.DBNull.Value
                End If
                If (Is3D.HasValue = True) Then
                    .Parameters(19).Value = CType(Is3D.Value, Boolean)
                Else
                    .Parameters(19).Value = Global.System.DBNull.Value
                End If
                If (Year.HasValue = True) Then
                    .Parameters(20).Value = CType(Year.Value, Integer)
                Else
                    .Parameters(20).Value = Global.System.DBNull.Value
                End If
                If (CountryCode Is Nothing) Then
                    .Parameters(21).Value = Global.System.DBNull.Value
                Else
                    .Parameters(21).Value = CType(CountryCode, String)
                End If
                If (Note Is Nothing) Then
                    .Parameters(22).Value = Global.System.DBNull.Value
                Else
                    .Parameters(22).Value = CType(Note, String)
                End If
                If (CompanyID.HasValue = True) Then
                    .Parameters(23).Value = CType(CompanyID.Value, Integer)
                Else
                    .Parameters(23).Value = Global.System.DBNull.Value
                End If
                If (ProductID.HasValue = True) Then
                    .Parameters(24).Value = CType(ProductID.Value, Integer)
                Else
                    .Parameters(24).Value = Global.System.DBNull.Value
                End If
                If (ProductTypeID.HasValue = True) Then
                    .Parameters(25).Value = CType(ProductTypeID.Value, Integer)
                Else
                    .Parameters(25).Value = Global.System.DBNull.Value
                End If
                If (StorageID.HasValue = True) Then
                    .Parameters(26).Value = CType(StorageID.Value, Integer)
                Else
                    .Parameters(26).Value = Global.System.DBNull.Value
                End If
                If (ForeColor2.HasValue = True) Then
                    .Parameters(27).Value = CType(ForeColor2.Value, Integer)
                Else
                    .Parameters(27).Value = Global.System.DBNull.Value
                End If
                If (PictureType Is Nothing) Then
                    .Parameters(28).Value = Global.System.DBNull.Value
                Else
                    .Parameters(28).Value = CType(PictureType, String)
                End If
                If (HasBottom.HasValue = True) Then
                    .Parameters(29).Value = CType(HasBottom.Value, Boolean)
                Else
                    .Parameters(29).Value = Global.System.DBNull.Value
                End If
                If (HasSide.HasValue = True) Then
                    .Parameters(30).Value = CType(HasSide.Value, Boolean)
                Else
                    .Parameters(30).Value = Global.System.DBNull.Value
                End If
                If (AnotherPictures Is Nothing) Then
                    .Parameters(31).Value = Global.System.DBNull.Value
                Else
                    .Parameters(31).Value = CType(AnotherPictures, String)
                End If
                If (CategoryIDs Is Nothing) Then
                    .Parameters(32).Value = Global.System.DBNull.Value
                Else
                    .Parameters(32).Value = CType(CategoryIDs, Object)
                End If
                If (Keywords Is Nothing) Then
                    .Parameters(33).Value = Global.System.DBNull.Value
                Else
                    .Parameters(33).Value = CType(Keywords, Object)
                End If
                If (CountryOfOrigin Is Nothing) Then
                    .Parameters(34).Value = Global.System.DBNull.Value
                Else
                    .Parameters(34).Value = CType(CountryOfOrigin, String)
                End If
                If (IsDrink.HasValue = True) Then
                    .Parameters(35).Value = CType(IsDrink.Value, Boolean)
                Else
                    .Parameters(35).Value = Global.System.DBNull.Value
                End If
                If (State.HasValue = True) Then
                    .Parameters(36).Value = CType(State.Value, Short)
                Else
                    .Parameters(36).Value = Global.System.DBNull.Value
                End If
                If (TargetID.HasValue = True) Then
                    .Parameters(37).Value = CType(TargetID.Value, Integer)
                Else
                    .Parameters(37).Value = Global.System.DBNull.Value
                End If
                If (IsAlcoholic.HasValue = True) Then
                    .Parameters(38).Value = CType(IsAlcoholic.Value, Boolean)
                Else
                    .Parameters(38).Value = Global.System.DBNull.Value
                End If
                If (CapSignIDs Is Nothing) Then
                    .Parameters(39).Value = Global.System.DBNull.Value
                Else
                    .Parameters(39).Value = CType(CapSignIDs, Object)
                End If
            End With
            Return cmd.ExecuteReader()
        End Function
    End Class

    Partial Class TranslateCapTableAdapter
        ''' <summary>Opens reader reading translation of <see cref="Cap"/></summary>
        ''' <param name="CapID">Id of <see cref="Cap"/> to get translation of</param>
        ''' <param name="CultureNames">Names of cultures to get translation for. Cultures are evaluated in given order.</param>
        ''' <returns><see cref="SqlDataReader"/> to read data for <see cref="CapFullTranslation"/> from</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="CultureNames"/> is null</exception>
        ''' <exception cref="SqlException">SQL server error occured</exception>
        ''' <exception cref="InvalidOperationException">The current state of <see cref="Connection"/> is closed.</exception>
        Public Overridable Overloads Function Read(ByVal CapID As Integer, ByVal CultureNames As HelperDataSet.VarCharTableDataTable) As SqlDataReader
            Dim cmd = Me.CommandCollection(0)
            With cmd
                .Parameters(1).Value = CType(CapID, Integer)
                If (CultureNames Is Nothing) Then
                    Throw New Global.System.ArgumentNullException("CultureNames")
                Else
                    .Parameters(2).Value = CType(CultureNames, Object)
                End If
            End With
            Return cmd.ExecuteReader
        End Function
    End Class

    Partial Class TranslateSimpleObjectTableAdapter
        ''' <summary>Opens reader reading translation of siplme object</summary>
        ''' <param name="ObjectType">Name of table in database representing object to translate. This must be table <see cref="SimpleTranslation"/> has relation with</param>
        ''' <param name="ObjectID">Id of object to get translation of</param>
        ''' <param name="CultureNames">Names of cultures to get translation for. Cultures are evaluated in given order.</param>
        ''' <returns><see cref="SqlDataReader"/> to read data for <see cref="SimpleFullTranslation"/> from</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="ObjectType"/> or <paramref name="CultureNames"/> is null</exception>
        ''' <exception cref="SqlException">SQL server error occured</exception>
        ''' <exception cref="InvalidOperationException">The current state of <see cref="Connection"/> is closed.</exception>
        Public Overridable Overloads Function Read(ByVal ObjectType$, ByVal ObjectID As Integer, ByVal CultureNames As HelperDataSet.VarCharTableDataTable) As SqlDataReader
            Dim cmd = Me.CommandCollection(0)
            With cmd
                If (ObjectType Is Nothing) Then
                    Throw New Global.System.ArgumentNullException("ObjectType")
                Else
                    .Parameters(1).Value = CType(ObjectType, String)
                End If
                .Parameters(2).Value = CType(ObjectID, Integer)
                If (CultureNames Is Nothing) Then
                    Throw New Global.System.ArgumentNullException("CultureNames")
                Else
                    .Parameters(3).Value = CType(CultureNames, Object)
                End If
            End With
            Return cmd.ExecuteReader
        End Function
    End Class

    Partial Class TranslateShapeTableAdapter
        ''' <summary>Opens reader reading translation of <see cref="Shape"/></summary>
        ''' <param name="ShapeID">Id of <see cref="Shape"/> to get translation of</param>
        ''' <param name="CultureNames">Names of cultures to get translation for. Cultures are evaluated in given order.</param>
        ''' <returns><see cref="SqlDataReader"/> to read data for <see cref="ShapeFullTranslation"/> from</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="CultureNames"/> is null</exception>
        ''' <exception cref="SqlException">SQL server error occured</exception>
        ''' <exception cref="InvalidOperationException">The current state of <see cref="Connection"/> is closed.</exception>
        Public Overridable Overloads Function Read(ByVal ShapeID As Integer, ByVal CultureNames As HelperDataSet.VarCharTableDataTable) As SqlDataReader
            Dim cmd = Me.CommandCollection(0)
            With cmd
                .Parameters(1).Value = CType(ShapeID, Integer)
                If (CultureNames Is Nothing) Then
                    Throw New Global.System.ArgumentNullException("CultureNames")
                Else
                    .Parameters(2).Value = CType(CultureNames, Object)
                End If
            End With
            Return cmd.ExecuteReader
        End Function
    End Class
End Namespace

