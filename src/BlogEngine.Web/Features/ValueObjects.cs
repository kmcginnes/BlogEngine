using System;
using System.Runtime.Serialization;

namespace BlogEngine.PublishedLanguage
{
    [Serializable]
    public abstract class AbstractLongIdentity : AbstractIdentity<long>
    {
        protected AbstractLongIdentity(long id, string tagValue)
        {
            Id = id;
            TagValue = tagValue;
        }

        public override string GetTag()
        {
            return TagValue;
        }

        [DataMember(Order = 1)] public override sealed long Id { get; protected set; }
        [DataMember(Order = 2)] public string TagValue { get; private set; }
    }

    [Serializable]
    public sealed class BlogId : AbstractLongIdentity
    {
        public BlogId(long id) : base(id, "blog") { }
    }

    public interface IBlogCommand : Command<BlogId> { }
    public interface IBlogEvent : Event<BlogId> { }

    [Serializable]
    public sealed class StoryId : AbstractLongIdentity
    {
        public StoryId(long id) : base(id, "story") { }
    }

    public interface IStoryCommand : Command<StoryId> { }
    public interface IStoryEvent : Event<StoryId> { }
}