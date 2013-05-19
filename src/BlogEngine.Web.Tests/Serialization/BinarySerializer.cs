using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlogEngine.Web.Tests
{
    class BinarySerializer : ISerializer
    {
        private byte[] _data;

        public void Serialize(object instance)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, instance);
                _data = stream.ToArray();
            }
        }

        public object Deserialize()
        {
            using (var stream = new MemoryStream(_data))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }
    }
}