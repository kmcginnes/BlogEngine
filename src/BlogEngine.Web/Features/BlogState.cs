using BlogEngine.PublishedLanguage;

namespace BlogEngine.Web.Features
{
    public class BlogState : AggregateState, IBlogState
    {
        private BlogId _id;

        public void When(BlogStarted e)
        {
            _id = e.Id;
        }

        public void When(StoryPosted e)
        {
            throw new System.NotImplementedException();
        }
    }
}