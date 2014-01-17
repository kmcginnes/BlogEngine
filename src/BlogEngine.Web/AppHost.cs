using BlogEngine.CoreDomain;
using BlogEngine.Web.Features;
using Funq;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.Razor;
using ServiceStack.WebHost.Endpoints;

namespace BlogEngine.Web
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Blog Engine", typeof(AppHost).Assembly) { }
        
        public override void Configure(Container container)
        {
            LogManager.LogFactory = new ConsoleLogFactory();

            Plugins.Add(new RazorFormat());

            container.RegisterAutoWiredAs<BlogAppService, IBlogApplicationService>();
            container.RegisterAutoWiredAs<StoryAppService, IStoryApplicationService>();

            SetConfig(new EndpointHostConfig()
            {
                DefaultRedirectPath = "/Blogs",
                AllowFileExtensions = {{"eot"}, {"svg"}, {"ttf"}, {"woff"}},
            });
        }
    }
}