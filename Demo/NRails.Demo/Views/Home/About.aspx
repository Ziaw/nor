<%@ Page Language="Nemerle" MasterPageFile="~/Views/Shared/Site.Master" Inherits="(System.Web.Mvc.ViewPage[ViewModels.Home.About])" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About Us
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About</h2>
    <p>
        <%= Html.Encode(Model.description) %>
        Now: <%= Html.Encode(Model.time) %>
    </p>
</asp:Content>
