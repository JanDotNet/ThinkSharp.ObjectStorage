using ThinkSharp.ObjectStorage.Locations;

// ReSharper disable once CheckNamespace
namespace ThinkSharp.ObjectStorage
{
    public static class LocationExtensions
    {
        /// <summary>
        /// Adds a file location for reading / writing objects.
        /// </summary>
        /// <param name="fileName">
        /// The path to the file.
        /// </param>
        /// <returns></returns>
        public static IConfigureLocationOrLocationOptions<TData> AddFileLocation<TData>(
            this IConfigureLocation<TData> obj, string fileName)
            where TData : class
            => obj.AddLocation(new FileStorageLocation(fileName));

        /// <summary>
        /// Adds an embedded resource as read-only location.
        /// Note that the embedded resource have to be located in the same assembly as the object to restore.
        /// </summary>
        /// <param name="path">
        /// The path to the embeded resource.
        /// </param>
        /// <returns></returns>
        public static IConfigureLocationOrLocationOptions<TData> AddEmbeddedResource<TData>(
            this IConfigureLocation<TData> obj, string path)
            where TData : class
            => obj.AddLocation(new EmbeddedResourceLocation<TData>(path)).AsReadonly();

        /// <summary>
        /// Adds an embedded resource as read-only location.
        /// </summary>
        /// <param name="path">
        /// The path to the embeded resource.
        /// </param>
        /// <returns></returns>
        public static IConfigureLocationOrLocationOptions<TData> AddEmbeddedResource<TData, TResourceLocation>(
            this IConfigureLocation<TData> obj, string path)
            where TData : class
            => obj.AddLocation(new EmbeddedResourceLocation<TResourceLocation>(path)).AsReadonly();

        /// <summary>
        /// Adds an in-memory location that used to read / write objects to the memory.
        /// Useful to cache objects and avoid unnecessary reading of other locations like files.
        /// </summary>
        /// <returns></returns>
        public static IConfigureLocationOrLocationOptions<TData> AddInMemoryLocation<TData>(
            this IConfigureLocation<TData> obj)
            where TData : class
            => obj.AddLocation(new InMemoryLocation<TData>(obj.Name));
    }
}
