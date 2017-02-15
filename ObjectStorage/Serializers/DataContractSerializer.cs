using System.Runtime.Serialization;

namespace ThinkSharp.ObjectStorage.Serializers
{
    internal class DataContractSerializer<TData> : XmlObjectSerializer<TData> where TData : class
    {
        internal DataContractSerializer() : base(new DataContractSerializer(typeof(TData)))
        { }
    }
}
