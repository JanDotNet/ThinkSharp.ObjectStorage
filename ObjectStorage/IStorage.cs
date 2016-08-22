using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.ObjectStorage
{
    public interface IStorage<TData> where TData : class
    {
        TData Load();
        bool Save(TData data);
    }
}
