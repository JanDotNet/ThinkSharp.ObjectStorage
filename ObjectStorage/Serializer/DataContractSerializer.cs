using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ThinkSharp.ObjectStorage.Serializer
{
    internal class DataContractSerializer<TData> : XmlObjectSerializer<TData> where TData : class
    {
        internal DataContractSerializer() : base(new DataContractSerializer(typeof(TData)))
        { }
    }
}
