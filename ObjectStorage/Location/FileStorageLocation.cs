using System;
using System.Collections.Generic;
using System.IO;
using ThinkSharp.ObjectStorage.Helper;

namespace ThinkSharp.ObjectStorage.Location
{
    internal class FileStorageLocation : IStorageLocation
    {
        private readonly FileInfo myFile;

        public FileStorageLocation(string file)
        {
            file.Ensure("file").IsNotNullOrEmpty();

            myFile = new FileInfo(file);
        }

        public Stream Open()
        {
            myFile.Refresh();
            return myFile.Exists ? myFile.Open(FileMode.Open) : null;
        }

        public void Clear() => myFile.Delete();

        public void Write(Stream stream)
        {
            // ensure that the directory exists
            myFile.Directory.Create();
            using (var fileStream = myFile.Open(FileMode.Create))
                stream.CopyTo(fileStream);
        }

    }
}
