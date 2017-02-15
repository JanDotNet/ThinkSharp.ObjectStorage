using System.IO;
using System.Runtime.Serialization;
using ThinkSharp.ObjectStorage.Helper;

namespace ThinkSharp.ObjectStorage.Serializers
{
    internal abstract class XmlObjectSerializer<TData> : ISerializer<TData> where TData : class
    {
        private readonly XmlObjectSerializer mySerializer;
        protected XmlObjectSerializer(XmlObjectSerializer serializer)
        {
            serializer.Ensure("serializer").IsNotNull();

            mySerializer = serializer;
        }
        public TData Deserialize(Stream stream) => mySerializer.ReadObject(stream) as TData;

        public Stream Serialize(TData data)
        {
            var stream = new MemoryStream();
            mySerializer.WriteObject(stream, data);
            return stream;
        }
    }
}
