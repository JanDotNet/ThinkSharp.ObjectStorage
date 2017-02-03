using System;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IStorageLocationSelector<TData> where TData : class
    {
        /// <summary>
        /// Adds a file location for reading / writing objects.
        /// </summary>
        /// <param name="fileName">
        /// The path to the file.
        /// </param>
        /// <returns></returns>
        ICompLocationOptions<TData> AddFileLocation(string fileName);

        /// <summary>
        /// Adds an embedded resource as read-only location.
        /// Note that the embedded resource have to be located in the same assembly as the object to restore.
        /// </summary>
        /// <param name="path">
        /// The path to the embeded resource.
        /// </param>
        /// <returns></returns>
        ICompLocationOptions<TData> AddEmbeddedResource(string path);

        /// <summary>
        /// Adds an embedded resource as read-only location.
        /// </summary>
        /// <param name="path">
        /// The path to the embeded resource.
        /// </param>
        /// <returns></returns>
        ICompLocationOptions<TData> AddEmbeddedResource<TResourceLocation>(string path);

        /// <summary>
        /// Adds an in-memory location that used to read / write objects to the memory.
        /// Useful to cache objects and avoid unnecessary reading of other locations like files.
        /// </summary>
        /// <returns></returns>
        ICompLocationOptions<TData> AddInMemoryLocation();
        [Obsolete("'key' is not used anymore.")]
        ICompLocationOptions<TData> AddInMemoryLocation(string key);
        IStorage<TData> Build();
    }
}
