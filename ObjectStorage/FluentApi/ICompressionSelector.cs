using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface ICompressionSelector<TData> where TData : class
    {
        /// <summary>
        /// Configures the storage to compress the serialized content using the <see cref="DeflateStream"/>.
        /// </summary>
        /// <returns></returns>
        ICompEncryptionLocation<TData> Zipped();

        /// /// <summary>
        /// Configures the storage to compress the serialized content using the <see cref="GZipStream"/>.
        /// </summary>
        /// <returns></returns>
        ICompEncryptionLocation<TData> GZipped();
    }
}
