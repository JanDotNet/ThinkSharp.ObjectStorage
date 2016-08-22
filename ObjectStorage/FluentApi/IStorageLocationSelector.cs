using System;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IStorageLocationSelector<TData> where TData : class
    {
        ICompLocationOptions<TData> AddFileLocation(string fileName);
        IStorage<TData> Build();
    }
}
