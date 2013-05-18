using System;
using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class BlogAggregate : AggregateRoot<BlogState>
    {
        public BlogAggregate(BlogState state) : base(state) { }

        public void Start(BlogId blogId, string name, DateTime timeUtc)
        {
            ApplyChange(new BlogStarted(blogId, name, timeUtc));
        }

        public void PostStory(BlogId blogId, string author, DateTime timeUtc, string title, string body)
        {
            throw new NotImplementedException();
        }
    }
}