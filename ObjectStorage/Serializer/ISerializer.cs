using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using ThinkSharp.ObjectStorage.Helper;

namespace ThinkSharp.ObjectStorage
{
    public interface ISerializer<TData> where TData : class
    {
        TData Deserialize(Stream stream);
        Stream Serialize(TData data);
    }
}
