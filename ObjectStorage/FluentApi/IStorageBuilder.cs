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
        void AddTransformation(IStreamTransformation transformation);
        void AddLocation(IStorageLocation location);
        void SetSerializer(ISerializer<TData> serializer);
    }
}
