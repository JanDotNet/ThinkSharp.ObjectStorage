using System.IO;

namespace ThinkSharp.ObjectStorage.Serializers
{
    public interface ISerializer<TData> where TData : class
    {
        TData Deserialize(Stream stream);
        Stream Serialize(TData data);
    }
}
