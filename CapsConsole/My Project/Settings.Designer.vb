﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.21205.1
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"),  _
 Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
Partial Public NotInheritable Class MySettings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
    
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
    
    Public Shared ReadOnly Property [Default]() As MySettings
        Get
            
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.ConnectionString),  _
     Global.System.Configuration.DefaultSettingValueAttribute("Data Source=.\SQLEXPRESS;AttachDbFilename=""C:\Documents and Settings\Honza\Dokume"& _ 
        "nty\Programy\Caps\CapsData.mdf"";Integrated Security=True;Connect Timeout=30;User"& _ 
        " Instance=True")>  _
    Public ReadOnly Property CapsDataConnectionString() As String
        Get
            Return CType(Me("CapsDataConnectionString"),String)
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property ImageRoot() As String
        Get
            Return CType(Me("ImageRoot"),String)
        End Get
        Set
            Me("ImageRoot") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property LastImageName() As String
        Get
            Return CType(Me("LastImageName"),String)
        End Get
        Set
            Me("LastImageName") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property Version() As String
        Get
            Return CType(Me("Version"),String)
        End Get
        Set
            Me("Version") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property UserConnectionString() As String
        Get
            Return CType(Me("UserConnectionString"),String)
        End Get
        Set
            Me("UserConnectionString") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0, 0, 0, 0")>  _
    Public Property winMainLoc() As Global.System.Drawing.Rectangle
        Get
            Return CType(Me("winMainLoc"),Global.System.Drawing.Rectangle)
        End Get
        Set
            Me("winMainLoc") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0, 0, 0, 0")>  _
    Public Property winNewCapLoc() As Global.System.Drawing.Rectangle
        Get
            Return CType(Me("winNewCapLoc"),Global.System.Drawing.Rectangle)
        End Get
        Set
            Me("winNewCapLoc") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0, 0, 0, 0")>  _
    Public Property winCapDetailsLoc() As Global.System.Drawing.Rectangle
        Get
            Return CType(Me("winCapDetailsLoc"),Global.System.Drawing.Rectangle)
        End Get
        Set
            Me("winCapDetailsLoc") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0, 0, 0, 0")>  _
    Public Property winCapEditorLoc() As Global.System.Drawing.Rectangle
        Get
            Return CType(Me("winCapEditorLoc"),Global.System.Drawing.Rectangle)
        End Get
        Set
            Me("winCapEditorLoc") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0, 0, 0, 0")>  _
    Public Property winEditorsLoc() As Global.System.Drawing.Rectangle
        Get
            Return CType(Me("winEditorsLoc"),Global.System.Drawing.Rectangle)
        End Get
        Set
            Me("winEditorsLoc") = value
        End Set
    End Property
    
    <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.ConnectionString),  _
     Global.System.Configuration.DefaultSettingValueAttribute("Data Source=.\SQLEXPRESS;AttachDbFilename=""C:\Documents and Settings\Honza\Dokume"& _ 
        "nty\Programy\Caps\CapsData.mdf"";Initial Catalog=CapsDev;Integrated Security=True"& _ 
        ";User Instance=True")>  _
    Public ReadOnly Property CapsDevConnectionString() As String
        Get
            Return CType(Me("CapsDevConnectionString"),String)
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("®©№■•‰½→←↺↻▲▼▶◀★")>  _
    Public Property FavoriteCharacters() As String
        Get
            Return CType(Me("FavoriteCharacters"),String)
        End Get
        Set
            Me("FavoriteCharacters") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property Language() As String
        Get
            Return CType(Me("Language"),String)
        End Get
        Set
            Me("Language") = value
        End Set
    End Property
End Class

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.Caps.Console.MySettings
            Get
                Return Global.Caps.Console.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
