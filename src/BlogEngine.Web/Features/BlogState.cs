using BlogEngine.CoreDomain;
using Fjord.Mesa.Domain;

namespace BlogEngine.Web.Features
{
    public class BlogState : AggregateState, IBlogState
    {
        private BlogId _id;

        public void When(BlogStarted e)
        {
            _id = e.Id;
        }

        public void When(StorySubmitted e)
        {
        }
    }
}