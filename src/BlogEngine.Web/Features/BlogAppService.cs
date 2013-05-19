using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class BlogAppService
        : AppServiceBase<BlogAggregate>, IBlogApplicationService
    {
        private readonly ITimeProvider _time;
        private readonly IDomainSender _send;

        public BlogAppService(
            IEventStore eventStore, ITimeProvider time, IDomainSender send)
            : base(eventStore)
        {
            _time = time;
            _send = send;
        }

        public void When(StartBlog c)
        {
            ChangeAggregate(c.Id, x => x.Start(c.Id, c.Name, _time));
        }

        public void When(SubmitStory c)
        {
            ChangeAggregate(c.Id, x => x.SubmitStory(c.Id, c.Author, c.Title, c.Body, _send));
        }
    }
}