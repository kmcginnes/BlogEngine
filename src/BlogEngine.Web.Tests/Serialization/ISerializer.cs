namespace BlogEngine.Web.Tests
{
    public interface ISerializer
    {
        void Serialize(object instance);
        object Deserialize();
    }
}