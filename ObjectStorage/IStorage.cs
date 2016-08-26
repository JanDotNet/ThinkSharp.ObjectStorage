using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThinkSharp.ObjectStorage
{
    public interface IStorage<TData> where TData : class
    {
        /// <summary>
        /// Load the data object <see cref="TData"/> from the first location that
        /// is available.
        /// </summary>
        /// <returns></returns>
        TData Load();

        /// <summary>
        /// Writes the passed object to all writable locations added to the storage.
        /// </summary>
        /// <param name="data"></param>
        void Save(TData data);

        /// <summary>
        /// Gets the name of the storage.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Clears all writable locations.
        /// </summary>
        void Clear();
    }
}
