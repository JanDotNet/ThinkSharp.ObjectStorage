using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.ObjectStorage.Location
{
    internal class StorageEndpoint<TData>
    {
        public StorageEndpoint(IStorageLocation storageLocation)
        {
            Location = storageLocation;
        }
        public bool IsReadonly { get; set; }
        public TData DefaultValue { get; set; }
        public IStorageLocation Location { get; }
    }
}
