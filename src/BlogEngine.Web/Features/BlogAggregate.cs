using BlogEngine.CoreDomain;
using Fjord.Mesa;
using Fjord.Mesa.Domain;

namespace BlogEngine.Web.Features
{
    public class BlogAggregate : AggregateRoot<BlogState>
    {
        public BlogAggregate(BlogState state) : base(state) { }

        public void Start(BlogId blogId, string name, ISystemClock time)
        {
            Ensure(State).IsNew().WithDomainError("blog started", "Blog has already been started");
            var timeUtc = time.GetUtcNow();

            ApplyChange(new BlogStarted(blogId, name, timeUtc));
        }

        public void SubmitStory(BlogId blogId, string author, string title, string body, IDomainSender send)
        {
            Ensure(State).HasBeenCreated().WithDomainError("blog not started", "Blog has not been started");

            ApplyChange(new StorySubmitted(blogId, author, title, body));
            send.ToStory(new CreateStoryFromBlog(new StoryId(1), blogId, author, title, body));
        }
    }

    public interface IDomainSender
    {
        void ToStory(Command command);
    }

    public class DomainSender : IDomainSender
    {
        private readonly TinyIoCContainer _container;

        public DomainSender(TinyIoCContainer container)
        {
            _container = container;
        }

        public void ToStory(Command command)
        {
            var storyAppService = _container.Resolve<StoryAppService>();
            storyAppService.Execute(command);
        }
    }
}