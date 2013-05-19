using System;
using Nancy.TinyIoc;
using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class BlogAggregate : AggregateRoot<BlogState>
    {
        public BlogAggregate(BlogState state) : base(state) { }

        public void Start(BlogId blogId, string name, ITimeProvider time)
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
        void ToStory(IStoryCommand command);
    }

    public class DomainSender : IDomainSender
    {
        private readonly TinyIoCContainer _container;

        public DomainSender(TinyIoCContainer container)
        {
            _container = container;
        }

        public void ToStory(IStoryCommand command)
        {
            var storyAppService = _container.Resolve<StoryAppService>();
            storyAppService.Execute(command);
        }
    }
}