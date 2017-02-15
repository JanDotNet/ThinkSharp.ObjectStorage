using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using ThinkSharp.ObjectStorage.Helper;
using ThinkSharp.ObjectStorage.Locations;
using ThinkSharp.ObjectStorage.Serializers;
using ThinkSharp.ObjectStorage.StreamTransformations;

namespace ThinkSharp.ObjectStorage
{
    public static class StorageBuilder
    {
        public static IConfigureSerializer<TDataObj> ForType<TDataObj>() where TDataObj : class => new StorageBuilder<TDataObj>();
        public static IConfigureSerializer<TDataObj> ForType<TDataObj>(string name) where TDataObj : class => new StorageBuilder<TDataObj>(name);
    }

    internal class StorageBuilder<TData> :
        IConfigureSerializer<TData>,
        IConfigureLocationOrLocationOptions<TData>
        where TData : class
    {
        private readonly string myName;
        private readonly IList<StorageEndpoint<TData>> myStorageLocations = new List<StorageEndpoint<TData>>();

        private ISerializer<TData> mySerializer;

        internal StorageBuilder()
            : this(typeof(TData).Name)
        { }

        internal StorageBuilder(string name)
        {
            myName = name;
        }
        
        #region IBuilder

        string INamed.Name => myName;

        IStorage<TData> IBuilder<TData>.Build()
        {
            return new Storage<TData>(
                myName,
                mySerializer,
                myStorageLocations.ToArray());
        }

        #endregion

        #region IConfigureSerializer

        IConfigureLocation<TData> IConfigureSerializer<TData>.SetSerializer(ISerializer<TData> serializer)
        {
            serializer.Ensure(nameof(serializer)).IsNotNull();
            mySerializer = serializer;
            return this;
        }

        #endregion

        #region IConfigureLocation

        IConfigureLocationOrLocationOptions<TData> IConfigureLocation<TData>.AddLocation(IStorageLocation location)
        {
            location.Ensure(nameof(location)).IsNotNull();
            myStorageLocations.Add(new StorageEndpoint<TData>(location));
            return this;
        }

        #endregion

        #region IConfigureLocationOrLocationOptions

        IConfigureLocationOrLocationOptions<TData> IConfigureLocationOrLocationOptions<TData>.AsReadonly()
        {
            var endPoint = myStorageLocations.LastOrDefault();
            if (endPoint == null)
                throw new InvalidOperationException("Unable to set 'ReadOnly' if no storage location is defined.");
            endPoint.IsReadonly = true;
            return this;
        }

        IConfigureLocationOrLocationOptions<TData> IConfigureLocationOrLocationOptions<TData>.WithDefault(TData defaultValue)
        {
            var endPoint = myStorageLocations.LastOrDefault();
            if (endPoint == null)
                throw new InvalidOperationException("Unable to set 'Default' if no storage location is defined.");
            if (mySerializer == null && defaultValue != null)
                throw new InvalidOperationException("Unable to set 'Default' value because serializer is not available yet.");
            endPoint.DefaultValue = defaultValue?.Clone(mySerializer);
            return this;
        }

        IConfigureLocationOrLocationOptions<TData> IConfigureLocationOrLocationOptions<TData>.AddTransformation(IStreamTransformation transformation)
        {
            transformation.Ensure(nameof(transformation)).IsNotNull();
            var endPoint = myStorageLocations.LastOrDefault();
            if (endPoint == null)
                throw new InvalidOperationException("Unable to set 'Default' if no storage location is defined.");
            endPoint.StreamTransformations.Add(transformation);
            return this;
        }

        #endregion
    }
}
