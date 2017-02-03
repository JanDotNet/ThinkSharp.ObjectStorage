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
        /// <summary>
        /// Adds EndPoint as read-only endpoint. That ensures, that the stores tries not to write to the endpoint.
        /// </summary>
        /// <returns></returns>
        ICompLocationOptions<TData> AsReadonly();

        /// <summary>
        /// Ads a default value for the end point. If the endpoint is not available, clone of the default value will be returned.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        ICompLocationOptions<TData> WithDefault(TData defaultValue);
    }
}
