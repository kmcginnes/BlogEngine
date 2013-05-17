using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class BlogApplicationService : AppServiceBase, IBlogApplicationService
    {
        public BlogApplicationService(IEventStore eventStore) : base(eventStore) { }

        public void When(StartBlog c)
        {
            ChangeAggregate<BlogAggregate>(c.Id, x => x.Start(c.Id, c.Name, c.TimeUtc));
        }
    }
}