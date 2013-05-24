using System;
using Newtonsoft.Json;

namespace BlogEngine.Web.Tests
{
    public class JsonDotNetSerializer : ISerializer
    {
        private string _data;
        private Type _type;

        public void Serialize(object instance)
        {
            _type = instance.GetType();
            _data = JsonConvert.SerializeObject(instance);
        }

        public object Deserialize()
        {
            return JsonConvert.DeserializeObject(_data, _type);
        }
    }
}