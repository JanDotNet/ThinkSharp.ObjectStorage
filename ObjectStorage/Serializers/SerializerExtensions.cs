using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.ObjectStorage.Serializers;

// ReSharper disable once CheckNamespace
namespace ThinkSharp.ObjectStorage
{
    public static class SerializerExtensions
    {
        public static IConfigureLocation<TData> UsingXmlSerializer<TData>(this IConfigureSerializer<TData> builder)
            where TData : class
            => builder.SetSerializer(new XmlSerializer<TData>());

        public static IConfigureLocation<TData> UsingDataContractSerializer<TData>(this IConfigureSerializer<TData> builder)
            where TData : class
            => builder.SetSerializer(new DataContractSerializer<TData>());

        public static IConfigureLocation<TData> UsingDataContractJsonSerializer<TData>(this IConfigureSerializer<TData> builder)
            where TData : class
            => builder.SetSerializer(new DataContractJsonSerializer<TData>());

        public static IConfigureLocation<TData> UsingCustonSerializer<TData>(this IConfigureSerializer<TData> builder, ISerializer<TData> customSerializer)
            where TData : class
            => builder.SetSerializer(customSerializer);
    }
}
