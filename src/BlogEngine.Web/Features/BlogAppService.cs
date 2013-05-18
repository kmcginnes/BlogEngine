using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class BlogAppService
        : AppServiceBase<BlogAggregate>, IBlogApplicationService
    {
        public BlogAppService(IEventStore eventStore) : base(eventStore) { }

        public void When(StartBlog c)
        {
            ChangeAggregate(c.Id, x => x.Start(c.Id, c.Name, c.TimeUtc));
        }

        public void When(PostStory c)
        {
            ChangeAggregate(c.Id, x => x.PostStory(c.Id, c.Author, c.TimeUtc, c.Title, c.Body));
        }
    }
}