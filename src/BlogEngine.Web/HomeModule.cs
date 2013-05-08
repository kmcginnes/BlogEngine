using Nancy;

namespace BlogEngine.Web
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["Index"];
        }
    }
}