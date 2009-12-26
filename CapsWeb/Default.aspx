<?xml version="1.0" encoding="utf-8"?>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Caps.Web.Default" ContentType="application/xhtml+xml" %>
<%@ Import Namespace="Caps.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= LocalizedSettings.GetValue("AppTitle")%></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1><%= LocalizedSettings.GetValue("AppTitle")%></h1>
        <%= LocalizedSettings.GetBodyHtml("Welcome.htm")%>
    </form>
</body>
</html>
