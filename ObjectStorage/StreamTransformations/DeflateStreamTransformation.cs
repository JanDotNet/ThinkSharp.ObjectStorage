using System;
using System.IO;
using System.IO.Compression;

namespace ThinkSharp.ObjectStorage.StreamTransformations
{
    internal class DeflateStreamTransformation : IStreamTransformation
    {
        public Stream TransformDeserialization(Stream stream)
        {
            return new DeflateStream(stream, CompressionMode.Decompress, true);
        }

        public Stream TransformSerialization(Stream stream)
        {
            var memoryStream = new MemoryStream();
            using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
                stream.CopyTo(deflateStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}