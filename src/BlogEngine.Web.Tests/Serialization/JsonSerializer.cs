using System.IO;
using System.Runtime.Serialization.Json;

namespace BlogEngine.Web.Tests
{
    public class JsonSerializer : ISerializer
    {
        private byte[] _data;
        private DataContractJsonSerializer _serializer;

        public void Serialize(object instance)
        {
            _serializer = new DataContractJsonSerializer(instance.GetType());
            using (var stream = new MemoryStream())
            {
                _serializer.WriteObject(stream, instance);
                _data = stream.ToArray();
            }
        }

        public object Deserialize()
        {
            using (var stream = new MemoryStream(_data))
            {
                return _serializer.ReadObject(stream);
            }
        }
    }
}