using System.Collections.Generic;
using System.IO;

namespace ThinkSharp.ObjectStorage.Locations
{
    internal class InMemoryLocation<TType> : IStorageLocation
    {
        private readonly string myKey;
        private static readonly Dictionary<string, byte[]> theInMemoryData = new Dictionary<string, byte[]>();

        public InMemoryLocation(string key)
        {
            myKey = key;
        }

        public Stream Open()
        {
            lock (theInMemoryData)
            {
                byte[] bytes;
                if (!theInMemoryData.TryGetValue(FullKey, out bytes))
                    return null;
                return new MemoryStream(bytes);
            }
        }

        public void Write(Stream stream)
        {
            lock (theInMemoryData)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    theInMemoryData[FullKey] = ms.ToArray();
                }
            }
        }

        public void Clear()
        {
            theInMemoryData.Remove(FullKey);
        }

        private string FullKey => typeof(TType).FullName + "_" + myKey;
    }
}
