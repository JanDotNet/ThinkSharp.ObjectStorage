using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface ISerializerSelector<TData> where TData : class
    {
        /// <summary>
        /// Configures the storage to use the <see cref="XmlSerializer"/> for serialization.
        /// </summary>
        /// <returns></returns>
        ICompEncryptionCompressionLocation<TData> UsingXmlSerializer();
        /// <summary>
        /// Configures the storage to use the <see cref="DataContractSerializer"/> for serialization.
        /// </summary>
        /// <returns></returns>
        ICompEncryptionCompressionLocation<TData> UsingDataContractSerializer();
        /// <summary>
        /// Configures the storage to use the <see cref="DataContractJsonSerializer"/> for serialization.
        /// </summary>
        /// <returns></returns>
        ICompEncryptionCompressionLocation<TData> UsingDataContractJsonSerializer();
        /// <summary>
        /// Configures the storage to use a custom serialization implementation.
        /// </summary>
        /// <returns></returns>
        ICompEncryptionCompressionLocation<TData> UsingCustonSerializer(ISerializer<TData> customSerializer);
    }
}
