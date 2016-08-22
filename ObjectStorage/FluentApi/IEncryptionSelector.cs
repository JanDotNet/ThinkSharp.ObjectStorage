using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IEncryptionSelector<TData> where TData : class
    {
        IStorageLocationSelector<TData> Encrypted(string password);
        IStorageLocationSelector<TData> Encrypted<TEncryptionAlgorithm>(string password)
            where TEncryptionAlgorithm : SymmetricAlgorithm, new();
    }
}
