using System;
using System.IO;
using System.IO.Compression;

namespace ThinkSharp.ObjectStorage.StreamTransformations
{
    internal class GZipStreamTransformation : IStreamTransformation
    {
        public Stream TransformDeserialization(Stream stream) => new GZipStream(stream, CompressionMode.Decompress);
        public Stream TransformSerialization(Stream stream)
        {
            var memoryStream = new MemoryStream();
            using (var deflateStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                stream.CopyTo(deflateStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}