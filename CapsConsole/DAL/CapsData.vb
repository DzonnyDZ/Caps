
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
    ''' <summary>Gets the <see cref="Images"/> collection ordered</summary>
    ''' <returns>The <see cref="Images"/> collection ordered by <see cref="Image.IsMain"/> descending first and then by <see cref="Image.RelativePath"/> ascending</returns>
    Public ReadOnly Property ImagesOrdered() As IOrderedEnumerable(Of Image)
        Get
            Return From img In Images Order By img.IsMain Descending, img.RelativePath Ascending
        End Get
    End Property
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

Partial Class CapsDataDataContext
#If DEBUG Then
    Private Sub OnCreated()
        MyBase.Log = New DebugLog
    End Sub
#End If
End Class
#If DEBUG Then
''' <summary><see cref="IO.TextWriter"/> over <see cref="Debug"/></summary>
Friend Class DebugLog
    Inherits IO.TextWriter
    ''' <summary>Contains value of the <see cref="Enabled"/> property</summary>
    Private Shared _Enabled As Boolean = True
    ''' <summary>Gets or sets value indicating if logging is enabled</summary>
    ''' <value>Ture to enable logging (default); false to ignore all logging attempts</value>
    Public Shared Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property
    ''' <summary>Gtes value indicating if debuger is attached to this proces and logging is enabled</summary>
    ''' <returns>True when <see cref="Enabled"/> is true and <see cref="Debugger.IsAttached"/> is true</returns>
    Private ReadOnly Property EnabledInternal() As Boolean
        Get
            Return Enabled AndAlso Debugger.IsAttached
        End Get
    End Property

    ''' <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
    Public Overrides Sub Flush()
        If EnabledInternal Then Debug.Flush()
    End Sub
    ''' <summary>When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"></see> in which the output is written.</summary>
    ''' <returns>The Encoding in which the output is written.</returns>
    Public Overrides ReadOnly Property Encoding() As System.Text.Encoding
        Get
            Return System.Text.Encoding.UTF8
        End Get
    End Property
    ''' <summary>Writes a subarray of characters to the text stream.</summary>
    ''' <param name="count">The number of characters to write. </param>
    ''' <param name="buffer">The character array to write data from. </param>
    ''' <param name="index">Starting index in the buffer. </param>
    ''' <exception cref="T:System.ArgumentOutOfRangeException">index or count is negative. </exception>
    ''' <exception cref="T:System.ArgumentException">The buffer length minus index is less than count. </exception>
    ''' <exception cref="T:System.ArgumentNullException">The buffer parameter is null. </exception>
    Public Overrides Sub Write(ByVal buffer() As Char, ByVal index As Integer, ByVal count As Integer)
        If EnabledInternal Then Write(New String(buffer, index, count))
    End Sub
    ''' <summary>Writes a string followed by a line terminator to the text stream.</summary>
    ''' <param name="value">The string to write. If value is null, only the line termination characters are written. </param>
    Public Overrides Sub Write(ByVal value As String)
        If EnabledInternal Then Debug.Write(value)
    End Sub
End Class
#End If