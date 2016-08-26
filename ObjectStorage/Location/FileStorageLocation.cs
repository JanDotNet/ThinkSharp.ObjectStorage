using System;
using System.Collections.Generic;
using System.IO;
using ThinkSharp.ObjectStorage.Helper;

namespace ThinkSharp.ObjectStorage.Location
{
    internal class FileStorageLocation : IStorageLocation
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
            EnsureDirectoryExists();
            using (var fileStream = File.Open(myFile, FileMode.Create))
                stream.CopyTo(fileStream);
        }

        private void EnsureDirectoryExists()
        {
            var fullPath = Path.GetFullPath(myFile);
            var directory = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directory);
        }

        public void Clear()
        {
            File.Delete(myFile);
        }
    }
}
