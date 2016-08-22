using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface ISerializerSelector<TData> where TData : class
    {
        ICompEncryptionCompressionLocation<TData> UsingXmlSerializer();
        ICompEncryptionCompressionLocation<TData> UsingDataContractSerializer();
        ICompEncryptionCompressionLocation<TData> UsingDataContractJsonSerializer();
        ICompEncryptionCompressionLocation<TData> UsingCustonSerializer(ISerializer<TData> customSerializer);
    }
}
