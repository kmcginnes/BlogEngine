using System;
using System.Linq;
using Fjord.Mesa;
using KellermanSoftware.CompareNetObjects;
using Ploeh.AutoFixture;
using Xunit;

namespace BlogEngine.Web.Tests
{
    public class SerializationSpecs
    {
        [Fact]
        public void messages_serialize_and_deserialize()
        {
            var messageTypes = typeof(Message).Assembly.ExportedTypes
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => typeof(Message).IsAssignableFrom(t))
                .ToArray();

            var fixture = new Fixture();
            fixture.Register(() => DateTime.UtcNow.Date);

            foreach (var messageType in messageTypes)
            {
                var instance = fixture.CreateFromType(messageType);
                // Concrete message types do not have [Serializable] attribute
                //SerializeDeserializeCompare<BinarySerializer>(instance);
                SerializeDeserializeCompare<JsonSerializer>(instance);
                SerializeDeserializeCompare<JsonDotNetSerializer>(instance);
                SerializeDeserializeCompare<ServiceStackJsonSerializer>(instance);
            }
        }

        private static void SerializeDeserializeCompare<T>(object identityInstance)
            where T : ISerializer, new()
        {
            var serializer = new T();
            serializer.Serialize(identityInstance);
            var deserializedIdentity = serializer.Deserialize();

            var compareObjects = new CompareObjects();
            if (!compareObjects.Compare(identityInstance, deserializedIdentity))
            {
                Assert.True(false, compareObjects.DifferencesString);
            }
        }
    }
}
