using System.Collections.Generic;
using ThinkSharp.ObjectStorage.StreamTransformations;

namespace ThinkSharp.ObjectStorage.Locations
{
    internal class StorageEndpoint<TData>
    {
        public StorageEndpoint(IStorageLocation storageLocation)
        {
            Location = storageLocation;
            StreamTransformations = new List<IStreamTransformation>();
        }
        public bool IsReadonly { get; set; }
        public TData DefaultValue { get; set; }
        public IStorageLocation Location { get; }
        public IList<IStreamTransformation> StreamTransformations { get; }
    }
}
