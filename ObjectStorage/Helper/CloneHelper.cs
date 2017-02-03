using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ThinkSharp.ObjectStorage.Helper
{
    internal static class CloneHelper
    {
        public static TData Clone<TData>(this TData data, ISerializer<TData> serializer) where TData : class
        {
            using (var stream = serializer.Serialize(data))
            {
                stream.Position = 0;
                return serializer.Deserialize(stream);
            }
        }
    }
}
