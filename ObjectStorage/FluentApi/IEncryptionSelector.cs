using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IEncryptionSelector<TData> where TData : class
    {
        /// <summary>
        /// Configures the storage to encrypt the serialized content using the <see cref="RijndaelManaged"/> algorithm.
        /// </summary>
        /// <returns></returns>
        IStorageLocationSelector<TData> Encrypted(string password);

        /// <summary>
        /// Configures the storage to encrypt the serialized content using a custom <see cref="SymmetricAlgorithm"/> algorithm.
        /// </summary>
        /// <returns></returns>
        IStorageLocationSelector<TData> Encrypted<TEncryptionAlgorithm>(string password)
            where TEncryptionAlgorithm : SymmetricAlgorithm, new();
    }
}
