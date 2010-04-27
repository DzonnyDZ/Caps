Imports System.Runtime.CompilerServices
Imports System.ComponentModel, Tools
Imports System.Data.Objects
Imports System.Data.Objects.DataClasses
Imports System.Reflection

'TODO: Move to ĐTool

''' <summary>Miscelaneous functions</summary>
Friend Module Misc
    ''' <summary>Gets value indicating if <see cref="CapEditor.SaveMode"/> indicates save action</summary>
    ''' <param name="this">Value to test</param>
    ''' <returns>True if <paramref name="this"/> indicates save action; false if it does not</returns>
    <Extension()> Function IsSave(ByVal this As CapEditor.SaveMode) As Boolean
        Select Case this
            Case CapEditor.SaveMode.SaveAndClose, CapEditor.SaveMode.SaveAndNew, CapEditor.SaveMode.SaveAndNext, CapEditor.SaveMode.SaveAndNextNoClean, CapEditor.SaveMode.SaveAndPrevious : Return True
            Case Else : Return False
        End Select
    End Function
End Module


''' <summary>Flags defining various propertires of <see cref="CheckBox"/></summary>
<Flags()>
Public Enum CheckBoxState
    ''' <summary><see cref="CheckBox.IsChecked"/></summary>
    Checked = 1
    ''' <summary><see cref="CheckBox.IsEnabled"/></summary>
    Enabled = 2
    ''' <summary><see cref="CheckBox.Visibility"/></summary>
    Visible = 4
End Enum