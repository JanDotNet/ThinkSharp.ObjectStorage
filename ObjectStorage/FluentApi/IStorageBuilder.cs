using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.ObjectStorage.Location;
using ThinkSharp.ObjectStorage.StreamTransformations;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IStorageBuilder<TData> where TData : class
    {
        /// <summary>
        /// Adds a custom <see cref="IStreamTransformation"/> to use for transforming the serialization data stream.
        /// </summary>
        /// <param name="transformation"></param>
        void AddTransformation(IStreamTransformation transformation);

        /// <summary>
        /// Adds a custom <see cref="IStorageLocation"/> to use for storing / restoring an object.
        /// If multiple locations are added, the storage writes objects to all available locations and loads the object from the first available location.
        /// </summary>
        /// <param name="location">The <see cref="IStorageLocation"/> object to add.</param>
        void AddLocation(IStorageLocation location);

        /// <summary>
        /// Set the serializer implementation to use.
        /// </summary>
        /// <param name="serializer"></param>
        void SetSerializer(ISerializer<TData> serializer);
    }
}
