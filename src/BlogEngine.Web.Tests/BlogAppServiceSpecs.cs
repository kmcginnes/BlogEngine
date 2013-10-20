using System;
using BlogEngine.CoreDomain;
using BlogEngine.Web.Features;
using Fjord.Mesa;
using NSubstitute;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace BlogEngine.Web.Tests
{
    public class BlogAppServiceSpecs : AppServiceSpecs<BlogAppService>
    {
        private readonly BlogStarted _blogStarted;
        private readonly DateTime _currentDate;
        private readonly BlogId _blogId;

        public BlogAppServiceSpecs()
        {
            _blogId = new BlogId(1);
            _currentDate = DateTime.UtcNow.Date;
            Fixture.Freeze<ISystemClock>().GetUtcNow().Returns(_currentDate);
            _blogStarted = new BlogStarted(_blogId, "TestName", _currentDate);
        }

        [Theory, AutoMockData]
        public void blog_started_successfully(string name)
        {
            Given();
            When(new StartBlog(_blogId, name));
            Expect(new BlogStarted(_blogId, name, _currentDate));
        }

        [Theory, AutoMockData]
        public void story_posts_successfully(string author, string title, string body)
        {
            Given(_blogStarted);
            When(new SubmitStory(_blogId, author, title, body));
            Expect(new StorySubmitted(_blogId, author, title, body));
        }
    }
}