using System.Runtime.Serialization.Json;

namespace ThinkSharp.ObjectStorage.Serializers
{
    internal class DataContractJsonSerializer<TData> : XmlObjectSerializer<TData> where TData : class
    {
        internal DataContractJsonSerializer() : base(new DataContractJsonSerializer(typeof(TData)))
        { }
    }
}
