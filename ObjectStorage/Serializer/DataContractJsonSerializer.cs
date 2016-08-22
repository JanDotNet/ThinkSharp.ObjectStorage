using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ThinkSharp.ObjectStorage.Serializer
{
    internal class DataContractJsonSerializer<TData> : XmlObjectSerializer<TData> where TData : class
    {
        internal DataContractJsonSerializer() : base(new DataContractJsonSerializer(typeof(TData)))
        { }
    }
}
