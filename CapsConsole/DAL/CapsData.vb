<DebuggerDisplay("Image {RelativePath} ({ImageID})")> _
Partial Class Image

End Class
<DebuggerDisplay("Material {Name} ({MaterialID})")> _
Partial Class Material

End Class

<DebuggerDisplay("CapType {TypeName} ({CapTypeID})")> _
Partial Class CapType

End Class

<DebuggerDisplay("Cap {CapName} ({CapID})")> _
Partial Class Cap

End Class
<DebuggerDisplay("Shape {Name} ({ShapeID})")> _
Partial Class Shape

End Class

<DebuggerDisplay("MainType {TypeName} ({MainTypeID})")> _
Partial Class MainType

End Class


<DebuggerDisplay("Category {CategoryName} ({CategoryID})")> _
Partial Class Category

End Class


<DebuggerDisplay("Product {ProductName} ({ProductID})")> _
Partial Class Product

End Class

<DebuggerDisplay("Company {CompanyName} ({CompanyID})")> _
Partial Class Company

End Class
<DebuggerDisplay("ProductType {ProducTypeName} ({ProductTypeID})")> _
Partial Class ProductType

End Class
<DebuggerDisplay("Storage {StorageNumber} ({StorageID})")> _
Partial Class Storage

End Class
<DebuggerDisplay("StorageType {Name} ({StorageTypeID})")> _
Partial Class StorageType

End Class

<DebuggerDisplay("Cap {CapID} <-> Category {CategoryID}")> _
Partial Class Cap_Category_Int
    Public Sub New(ByVal Cap As Cap, ByVal Category As Category)
        Me.New()
        Me.Cap = Cap
        Me.Category = Category
    End Sub


End Class
<DebuggerDisplay("Cap {CapID} <-> Keyword {KeywordID}")> _
Partial Class Cap_Keyword_Int
    Public Sub New(ByVal Cap As Cap, ByVal Keyword As Keyword)
        Me.New()
        Me.Cap = Cap
        Me.Keyword = Keyword
    End Sub
End Class

<DebuggerDisplay("Keyword {Keyword} ({KeywordID})")> _
Partial Class Keyword
    Public Sub New(ByVal Keyword$)
        Me.New()
        Me._Keyword = Keyword
    End Sub
End Class

