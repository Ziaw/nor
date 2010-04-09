<%@ Application Language="C#" %>


<script runat="server">

    public static void RegisterRoutes(RouteCollection routes) {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute(
            "Default",                                              // Route name
            "{controller}/{action}/{id}",                           // URL with parameters
            new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
        );
    }

    void Application_Start() {
        RegisterRoutes(RouteTable.Routes);
    }

</script>