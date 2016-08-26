using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ThinkSharp.ObjectStorage.Helper;
using ThinkSharp.ObjectStorage.Location;
using ThinkSharp.ObjectStorage.Serializer;
using ThinkSharp.ObjectStorage.StreamTransformations;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    internal class StorageBuilder<TData> :
        ISerializerSelector<TData>,
        ICompEncryptionCompressionLocation<TData>,
        ICompEncryptionLocation<TData>,
        ICompLocationOptions<TData>,
        IStorageBuilder<TData>
        where TData : class
    {
        private readonly string myName;
        private readonly IList<IStreamTransformation> myStreamTransformation = new List<IStreamTransformation>();
        private readonly IList<StorageEndpoint<TData>> myStorageLocations = new List<StorageEndpoint<TData>>();

        private ISerializer<TData> mySerializer;

        public StorageBuilder() 
            : this(typeof(TData).Name) { }
        public StorageBuilder(string name)
        {
            myName = name;
        }

        #region Implementation of IStorageLocationSelector<TData>
        ICompLocationOptions<TData> IStorageLocationSelector<TData>.AddFileLocation(string fileName)
        {
            myStorageLocations.Add(new StorageEndpoint<TData>(new FileStorageLocation(fileName)));
            return this;
        }

        ICompLocationOptions<TData> IStorageLocationSelector<TData>.AddEmbeddedResource(string path)
        {
            AddLocation(new EmbeddedResourceLocation<TData>(path));
            return AsReadonly();
        }

        ICompLocationOptions<TData> IStorageLocationSelector<TData>.AddEmbeddedResource<TResourceLocation>(string path)
        {
            AddLocation(new EmbeddedResourceLocation<TResourceLocation>(path));
            return AsReadonly();
        }

        ICompLocationOptions<TData> IStorageLocationSelector<TData>.AddInMemoryLocation(string key)
        {
            AddLocation(new InMemoryLocation<TData>($"{myName}_{key}"));
            return this;
        }

        IStorage<TData> IStorageLocationSelector<TData>.Build()
        {
            return new Storage<TData>(
                myName,
                mySerializer, 
                myStreamTransformation.ToArray(), 
                myStorageLocations.ToArray());
        }

        #endregion

        #region Implementation of ISerializerSelector<TData>

        public ICompEncryptionCompressionLocation<TData> UsingXmlSerializer()
        {
            mySerializer = new XmlSerializer<TData>();
            return this;
        }

        public ICompEncryptionCompressionLocation<TData> UsingDataContractSerializer()
        {
            mySerializer = new DataContractSerializer<TData>();
            return this;
        }

        public ICompEncryptionCompressionLocation<TData> UsingDataContractJsonSerializer()
        {
            mySerializer = new DataContractJsonSerializer<TData>();
            return this;
        }

        public ICompEncryptionCompressionLocation<TData> UsingCustonSerializer(ISerializer<TData> customSerializer)
        {
            mySerializer = customSerializer;
            return this;
        }

        #endregion

        #region Implementation of IEncryptionSelector<TData>

        public IStorageLocationSelector<TData> Encrypted(string password)
        {
            myStreamTransformation.Add(new EncryptionStreamTransformation<RijndaelManaged>(password));
            return this;
        }

        public IStorageLocationSelector<TData> Encrypted<TEncryptionAlgorithm>(string password) where TEncryptionAlgorithm : SymmetricAlgorithm, new()
        {
            myStreamTransformation.Add(new EncryptionStreamTransformation<TEncryptionAlgorithm>(password));
            return this;
        }

        #endregion

        #region Implementation of ICompressionSelector<TData>

        public ICompEncryptionLocation<TData> Zipped()
        {
            myStreamTransformation.Add(new DeflateStreamTransformation());
            return this;
        }

        public ICompEncryptionLocation<TData> GZipped()
        {
            myStreamTransformation.Add(new GZipStreamTransformation());
            return this;
        }

        #endregion

        #region Implementation of IStorageLocationOptionsSelector<TData>

        public ICompLocationOptions<TData> AsReadonly()
        {
            var endPoint = myStorageLocations.LastOrDefault();
            if (endPoint == null)
                throw new InvalidOperationException("Unable to set 'ReadOnly' if no storage location is defined.");
            endPoint.IsReadonly = true;
            return this;
        }

        public ICompLocationOptions<TData> WithDefault(TData defaultValue)
        {
            var endPoint = myStorageLocations.LastOrDefault();
            if (endPoint == null)
                throw new InvalidOperationException("Unable to set 'Default' if no storage location is defined.");
            endPoint.DefaultValue = defaultValue;
            return this;
        }

        #endregion

        #region Implementation of IStorageBuilder<TData>
        public void AddTransformation(IStreamTransformation transformation)
        {
            transformation.Ensure("transformation").IsNotNull();
            myStreamTransformation.Add(transformation);
        }

        public void AddLocation(IStorageLocation location)
        {
            location.Ensure("location").IsNotNull();
            myStorageLocations.Add(new StorageEndpoint<TData>(location));
        }

        public void SetSerializer(ISerializer<TData> serializer)
        {
            serializer.Ensure("serializer").IsNotNull();
            mySerializer = serializer;
        }

        #endregion
    }
}
