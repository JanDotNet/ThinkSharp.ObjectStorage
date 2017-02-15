using System.IO;

namespace ThinkSharp.ObjectStorage.Locations
{
    public interface IStorageLocation
    {
        /// <summary>
        /// Returns a an opend stream to the underlying data.
        /// </summary>
        /// <returns></returns>
        Stream Open();

        /// <summary>
        /// Writes the content of the passed stream to the storage location.
        /// </summary>
        /// <param name="stream"></param>
        void Write(Stream stream);

        /// <summary>
        /// Clears the storage location.
        /// </summary>
        void Clear();
    }
}
