using System;
using System.Runtime.Serialization;

namespace BlogEngine.PublishedLanguage
{
    [Serializable]
    public sealed class BlogId : AbstractIdentity<long>
    {
        public const string TagValue = "blog";

        public BlogId(long id)
        {
            //Ensure.Requires(id > 0);
            Id = id;
        }

        public override string GetTag()
        {
            return TagValue;
        }

        [DataMember(Order = 1)]
        public override long Id { get; protected set; }

        public BlogId() { }
    }

    public interface IBlogCommand : Command<BlogId> { }
    public interface IBlogEvent : Event<BlogId> { }
}