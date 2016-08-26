using System;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IStorageLocationSelector<TData> where TData : class
    {
        ICompLocationOptions<TData> AddFileLocation(string fileName);
        ICompLocationOptions<TData> AddEmbeddedResource(string path);
        ICompLocationOptions<TData> AddEmbeddedResource<TResourceLocation>(string path);
        ICompLocationOptions<TData> AddInMemoryLocation(string key);
        IStorage<TData> Build();
    }
}
