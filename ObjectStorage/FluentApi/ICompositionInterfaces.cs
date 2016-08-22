﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.ObjectStorage.FluentApi
{
    public interface ICompEncryptionCompressionLocation<TData> : 
        IEncryptionSelector<TData> ,
        ICompressionSelector<TData>,
        IStorageLocationSelector<TData>
        where TData : class { }

    public interface ICompEncryptionLocation<TData> :
        IEncryptionSelector<TData>,
        IStorageLocationSelector<TData>
        where TData : class
    { }

    public interface ICompLocationOptions<TData> :
        IStorageLocationSelector<TData> ,
        IStorageLocationOptionsSelector<TData> 
        where TData : class
    { }
}
