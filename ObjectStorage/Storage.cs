using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThinkSharp.ObjectStorage.Helper;
using ThinkSharp.ObjectStorage.Location;
using ThinkSharp.ObjectStorage.StreamTransformations;

namespace ThinkSharp.ObjectStorage
{
    internal class Storage<TData> : IStorage<TData> where TData : class
    {
        private readonly ISerializer<TData> mySerializer;
        private readonly IStreamTransformation[] myStreamTransformations;
        private readonly StorageEndpoint<TData>[] myStorageEndPoints;

        public Storage(
            string name,
            ISerializer<TData> serializer,
            IStreamTransformation[] streamTransformations,
            StorageEndpoint<TData>[] storageEndPoints)
        {
            serializer.Ensure("serializer").IsNotNull();
            storageEndPoints.Ensure("storageEndPoints").IsNotNull();

            if (storageEndPoints.Length == 0)
                throw new InvalidOperationException("No storage locations defines");

            Name = name;
            mySerializer = serializer;
            myStreamTransformations = streamTransformations ?? new IStreamTransformation[0];
            myStorageEndPoints = storageEndPoints;
        }

        public TData Load()
        {
            foreach (var location in myStorageEndPoints)
            {
                using (var stream = location.Location.Open())
                {
                    if (stream == null && location.DefaultValue != null)
                        return location.DefaultValue;
                    if (stream == null)
                        continue;

                    var streamsToClose = new List<Stream>();
                    var transformedStream = stream;

                    try
                    {
                        foreach (var streamTransformation in myStreamTransformations.Reverse())
                        {
                            transformedStream = streamTransformation.TransformDeserialization(transformedStream);
                            streamsToClose.Add(transformedStream);
                        }

                        return mySerializer.Deserialize(transformedStream);
                    }
                    finally
                    {
                        streamsToClose.Reverse();
                        foreach (var streamToDispose in streamsToClose)
                            streamToDispose.Close();
                    }
                }
            }
            return null;
        }

        public void Save(TData data)
        {
            data.Ensure("data").IsNotNull();

            foreach (var endPoint in myStorageEndPoints.Where(e => !e.IsReadonly))
            {
                var streamsToClose = new List<Stream>();
                using (var stream = mySerializer.Serialize(data))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    try
                    {
                        var transformedStream = stream;
                        foreach (var streamTransformation in myStreamTransformations)
                        {
                            transformedStream = streamTransformation.TransformSerialization(transformedStream);
                            streamsToClose.Add(transformedStream);
                        }

                        endPoint.Location.Write(transformedStream);
                    }
                    finally
                    {
                        streamsToClose.Reverse();
                        foreach (var streamToDispose in streamsToClose)
                            streamToDispose.Close();
                    }
                }
            }
        }

        public void Clear()
        {
            foreach (var storageEndpoint in myStorageEndPoints)
                storageEndpoint.Location.Clear();
        }

        public string Name { get; }
    }
}
