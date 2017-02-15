﻿using System;
using System.IO;

namespace ThinkSharp.ObjectStorage.Locations
{
    internal class EmbeddedResourceLocation<TType> : IStorageLocation
    {
        private readonly string myPath;

        public EmbeddedResourceLocation(string path)
        {
            myPath = path;
        }

        public void Clear()
        {
            // ignore - embedded storage can not be cleared
        }

        public Stream Open()
        {
            var stream = typeof(TType).Assembly.GetManifestResourceStream(myPath);
            if (stream == null)
                throw new InvalidOperationException($"Could not found embedded resource '{myPath}'.");
            return stream;
        }

        public void Write(Stream stream)
        {
            throw new NotSupportedException($"Writing is not supported for '{this.GetType().Name}'.");
        }
    }
}
