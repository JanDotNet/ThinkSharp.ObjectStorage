using System.IO;

namespace ThinkSharp.ObjectStorage.Location
{
    public interface IStorageLocation
    {
        Stream Open();
        void Write(Stream stream);
    }
}
