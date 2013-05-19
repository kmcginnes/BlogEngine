using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class StoryAppService : AppServiceBase<StoryAggregate>, IStoryApplicationService
    {
        private readonly ITimeProvider _time;

        public StoryAppService(IEventStore eventStore, ITimeProvider time) : base(eventStore)
        {
            _time = time;
        }

        public void When(CreateStoryFromBlog c)
        {
            ChangeAggregate(c.Id, x => x.CreateFromBlog(c.Id, c.BlogId, c.Author, c.Title, c.Body, _time));
        }
    }

    public class StoryAggregate : AggregateRoot<StoryState>
    {
        public StoryAggregate(StoryState state) : base(state) { }

        public void CreateFromBlog(StoryId id, BlogId blogId, string author, string title, string body, ITimeProvider time)
        {
            Ensure(State).IsNew().WithDomainError("story already created", "Story has already been created");
            
            var timeUtc = time.GetUtcNow();
            ApplyChange(new StoryCreatedFromBlog(id, blogId, author, timeUtc, title, body));
        }
    }

    public class StoryState : AggregateState, IStoryState
    {
        public void When(StoryCreatedFromBlog e)
        {
        }
    }
}