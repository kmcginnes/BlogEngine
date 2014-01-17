using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlogEngine.CoreDomain;
using Fjord.Mesa;
using Fjord.Mesa.Bus;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace BlogEngine.Web
{
    [Route("/Blogs")]
    public class ShowBlogs { }

    public class ShowBlogsResponse
    {
        public IEnumerable<BlogView> Blogs { get; set; }
    }

    public class StoryView
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string CreateDate { get; set; }
        public string PublishDate { get; set; }
        public string Body { get; set; }
    }

    [Route("/Blogs")]
    public class CreateBlog
    {
        public string Name { get; set; }
    }
    public class CreateBlogResponse { }

    public static class BlogSystem
    {
        public static IList<BlogView> Blogs = new List<BlogView>();
        public static IList<StoryView> Stories = new List<StoryView>();
    }

    public class BlogView
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Creator { get; set; }
    }

    public class BlogService : Service
    {
        private readonly IBus _bus;

        public BlogService(IBus bus)
        {
            _bus = bus;
        }

        public object Get(ShowBlogs request)
        {
            return new ShowBlogsResponse {Blogs = BlogSystem.Blogs.OrderBy(x => x.CreatedDate)};
        }

        public object Put(CreateBlog request)
        {
            var requestId = new BlogId(Guid.NewGuid());
            _bus.Publish(new StartBlog(requestId, request.Name, "Kris McGinnes"));

            return new CreateBlogResponse();
        }
    }
}