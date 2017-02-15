using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.ObjectStorage.Locations;
using ThinkSharp.ObjectStorage.Serializers;
using ThinkSharp.ObjectStorage.StreamTransformations;

namespace ThinkSharp.ObjectStorage
{
    public interface IConfigureSerializer<TData> where TData : class
    {
        /// <summary>
        /// Sets the passed serializer as serializer for the storage.
        /// </summary>
        /// <param name="serializer"></param>
        /// <returns></returns>
        IConfigureLocation<TData> SetSerializer(ISerializer<TData> serializer);
    }

    public interface IConfigureLocation<TData> : INamed where TData : class
    {
        /// <summary>
        /// Adds a custom <see cref="IStorageLocation"/> to use for storing / restoring an object.
        /// If multiple locations are added, the storage writes objects to all available locations and loads the object from the first available location.
        /// </summary>
        /// <param name="location">The <see cref="IStorageLocation"/> object to add.</param>
        IConfigureLocationOrLocationOptions<TData> AddLocation(IStorageLocation location);
    }

    public interface IConfigureLocationOrLocationOptions<TData> : IConfigureLocation<TData>, IBuilder<TData> where TData : class
    {
        /// <summary>
        /// Adds EndPoint as read-only endpoint. That ensures, that the stores tries not to write to the endpoint.
        /// </summary>
        /// <returns></returns>
        IConfigureLocationOrLocationOptions<TData> AsReadonly();

        /// <summary>
        /// Ads a default value for the end point. If the endpoint is not available, clone of the default value will be returned.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        IConfigureLocationOrLocationOptions<TData> WithDefault(TData defaultValue);

        /// <summary>
        /// Adds a custom <see cref="IStreamTransformation"/> to use for transforming the serialization data stream.
        /// </summary>
        /// <param name="transformation"></param>
        IConfigureLocationOrLocationOptions<TData> AddTransformation(IStreamTransformation transformation);
    }

    public interface INamed
    {
        string Name { get; }
    }

    public interface IBuilder<TData> : INamed where TData : class
    {
        IStorage<TData> Build();
    }
}
