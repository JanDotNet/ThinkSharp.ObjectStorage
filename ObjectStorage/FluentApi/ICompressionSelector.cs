using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface ICompressionSelector<TData> where TData : class
    {
        ICompEncryptionLocation<TData> Zipped();
        ICompEncryptionLocation<TData> GZipped();
    }
}
