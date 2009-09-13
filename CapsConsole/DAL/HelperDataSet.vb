Imports System.Data.SqlClient

Namespace HelperDataSetTableAdapters
    Partial Class GetSimilarCapsTableAdapter
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
                    ByVal Keywords As HelperDataSet.VarCharTableDataTable) As SqlDataReader
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
            End With
            Return cmd.ExecuteReader()
        End Function
    End Class
End Namespace
Partial Class HelperDataSet
End Class
