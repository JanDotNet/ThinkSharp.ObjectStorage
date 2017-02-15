using System.IO;
using System.Xml.Serialization;

namespace ThinkSharp.ObjectStorage.Serializers
{
    internal class XmlSerializer<TData> : ISerializer<TData> where TData : class
    {
        private static readonly XmlSerializer theSerializer = new XmlSerializer(typeof(TData));
        public TData Deserialize(Stream stream) => theSerializer.Deserialize(stream) as TData;

        public Stream Serialize(TData data)
        {
            var stream = new MemoryStream();
            theSerializer.Serialize(stream, data);
            return stream;
        } 
    }
}
