

Partial Class Cap_Category_Int
    Public Sub New(ByVal Cap As Cap, ByVal Category As Category)
        Me.New()
        Me.Cap = Cap
        Me.Category = Category
    End Sub


End Class

Partial Class Cap_Keyword_Int
    Public Sub New(ByVal Cap As Cap, ByVal Keyword As Keyword)
        Me.New()
        Me.Cap = Cap
        Me.Keyword = Keyword
    End Sub
End Class

Partial Class Keyword
    Public Sub New(ByVal Keyword$)
        Me.New()
        Me._Keyword = Keyword
    End Sub
End Class

