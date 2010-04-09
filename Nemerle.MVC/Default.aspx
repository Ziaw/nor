<%@ Page Language="C#" %>

<script runat="server">

    protected override void OnLoad(EventArgs e) {
        base.OnLoad(e);

        string originalPath = Request.Path;
        HttpContext.Current.RewritePath(Request.ApplicationPath, false);
        IHttpHandler httpHandler = new MvcHttpHandler();
        httpHandler.ProcessRequest(HttpContext.Current);
        HttpContext.Current.RewritePath(originalPath, false);
    }

</script>
