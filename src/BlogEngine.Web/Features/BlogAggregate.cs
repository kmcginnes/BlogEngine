using System;
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

        public void PostStory(BlogId blogId, string author, string title, string body, ITimeProvider time)
        {
            Ensure(State).HasBeenCreated().WithDomainError("blog not started", "Blog has not been started");
            var timeUtc = time.GetUtcNow();

            ApplyChange(new StoryPosted(blogId, author, timeUtc, title, body));
        }
    }
}