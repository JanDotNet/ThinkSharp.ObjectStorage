using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface IStorageLocationOptionsSelector<TData> 
        where TData : class
    {
        ICompLocationOptions<TData> AsReadonly();
        ICompLocationOptions<TData> WithDefault(TData defaultValue);
    }
}
