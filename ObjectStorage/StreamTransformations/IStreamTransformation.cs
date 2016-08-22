using System.IO;

namespace ThinkSharp.ObjectStorage.StreamTransformations
{
    public interface IStreamTransformation
    {
        Stream TransformSerialization(Stream stream);
        Stream TransformDeserialization(Stream stream);
    }
}
