using System;
using System.IO;

namespace BlogEngine.Web.Tests
{
    public class ServiceStackJsonSerializer : ISerializer
    {
        private byte[] _data;
        private Type _type;

        public void Serialize(object instance)
        {
            using (var stream = new MemoryStream())
            {
                _type = instance.GetType();
                ServiceStack.Text.JsonSerializer.SerializeToStream(instance, _type, stream);
                _data = stream.ToArray();
            }
        }

        public object Deserialize()
        {
            using (var stream = new MemoryStream(_data))
            {
                return ServiceStack.Text.JsonSerializer.DeserializeFromStream(_type, stream);
            }
        }
    }
}