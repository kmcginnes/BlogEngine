using System;
using BlogEngine.PublishedLanguage;
using BlogEngine.Web.Features;
using Xunit;

namespace BlogEngine.Web.Tests
{
    public class BlogApplicationServiceSpecs : ApplicationServiceSpecsBase<BlogApplicationService>
    {
        [Fact]
        public void Test()
        {
            Given();
            When(new StartBlog(new BlogId(1), "TestName", DateTime.UtcNow.Date));
            Then(new BlogStarted(new BlogId(1), "TestName", DateTime.UtcNow.Date));
        }
    }
}