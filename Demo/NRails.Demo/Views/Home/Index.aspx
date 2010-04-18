<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    
    <p>Taxonomies list</p>
    <ul>
    <%
        var taxonomies = ViewData["Taxonomies"] as IEnumerable<string>;
        foreach (var tax in taxonomies)
        { %>
        <li><%=tax%></li> 
    <%  }%>
    </ul>
    
</asp:Content>
