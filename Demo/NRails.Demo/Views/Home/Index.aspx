<%@ Page Language="Nemerle" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(Model().message) %></h2>
    <p>Taxonomies list</p>
    <ul>
    <% foreach (tax in Model().taxonomies) { %>
        <li><%= Html.Encode(tax) %></li> 
    <%  } %>
    </ul>

</asp:Content>
