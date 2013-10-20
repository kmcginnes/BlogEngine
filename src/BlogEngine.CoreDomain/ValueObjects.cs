using System;
using System.Runtime.Serialization;
using Fjord.Mesa.Domain;

namespace BlogEngine.CoreDomain
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

    public interface IBlogCommand { BlogId Id { get; } }
    public interface IBlogEvent { BlogId Id { get; } }

    [Serializable]
    public sealed class StoryId : AbstractLongIdentity
    {
        public StoryId(long id) : base(id, "story") { }
    }

    public interface IStoryCommand { StoryId Id { get; } }
    public interface IStoryEvent { StoryId Id { get; } }
}