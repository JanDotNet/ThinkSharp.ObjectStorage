using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IVersionSelector<TData> where TData : class
    {
        IStorageLocationSelector<TData> WithVersion(int version);
        IStorageLocationSelector<TData> AddMigration<TType>(int version, Func<TType, TData> convert);
    }
}
