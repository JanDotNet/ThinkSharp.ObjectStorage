using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.ObjectStorage.FluentApi;

namespace ThinkSharp.ObjectStorage
{ 
    public static class StorageBuilder
    {
        public static ISerializerSelector<TData> ForType<TData>() where TData : class => new StorageBuilder<TData>();
        public static ISerializerSelector<TData> ForType<TData>(string name) where TData : class => new StorageBuilder<TData>(name);
    }
}
