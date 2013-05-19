using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class BlogAppService
        : AppServiceBase<BlogAggregate>, IBlogApplicationService
    {
        private readonly ITimeProvider _time;

        public BlogAppService(IEventStore eventStore, ITimeProvider time)
            : base(eventStore)
        {
            _time = time;
        }

        public void When(StartBlog c)
        {
            ChangeAggregate(c.Id, x => x.Start(c.Id, c.Name, _time));
        }

        public void When(PostStory c)
        {
            ChangeAggregate(c.Id, x => x.PostStory(c.Id, c.Author, c.Title, c.Body, _time));
        }
    }
}