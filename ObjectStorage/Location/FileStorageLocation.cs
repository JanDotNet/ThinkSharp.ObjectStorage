using System;
using System.Collections.Generic;
using System.IO;
using ThinkSharp.ObjectStorage.Helper;

namespace ThinkSharp.ObjectStorage.Location
{
    public class FileStorageLocation : IStorageLocation
    {
        private readonly string myFile;

        public FileStorageLocation(string file)
        {
            file.Ensure("file").IsNotNullOrEmpty();

            myFile = file;
        }
        public Stream Open() => File.Exists(myFile) ? File.Open(myFile, FileMode.Open) : null;

        public void Write(Stream stream)
        {
            using (var fileStream = File.Open(myFile, FileMode.Create))
                stream.CopyTo(fileStream);
        }
    }
}
