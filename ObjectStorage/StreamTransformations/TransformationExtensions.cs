using System.IO.Compression;
using System.Security.Cryptography;
using ThinkSharp.ObjectStorage.StreamTransformations;

// ReSharper disable once CheckNamespace
namespace ThinkSharp.ObjectStorage
{
    public static class TransformationExtensions
    {
        /// <summary>
        /// Configures the storage to encrypt the serialized content using the <see cref="RijndaelManaged"/> algorithm.
        /// </summary>
        /// <returns></returns>
        public static IConfigureLocationOrLocationOptions<TData> Encrypted<TData>(this IConfigureLocationOrLocationOptions<TData> obj,
            string password) where TData : class
        {
            obj.AddTransformation(new EncryptionStreamTransformation<RijndaelManaged>(password));
            return obj;
        }

        public static IConfigureLocationOrLocationOptions<TData> Encrypted<TData, TEncryptionAlgorithm>(
            this IConfigureLocationOrLocationOptions<TData> obj, string password) 
                where TEncryptionAlgorithm : SymmetricAlgorithm, new()
                where TData : class
        {
            obj.AddTransformation(new EncryptionStreamTransformation<TEncryptionAlgorithm>(password));
            return obj;
        }

        /// <summary>
        /// Configures the storage to compress the serialized content using the <see cref="DeflateStream"/>.
        /// </summary>
        /// <returns></returns>
        public static IConfigureLocationOrLocationOptions<TData> Zipped<TData>(this IConfigureLocationOrLocationOptions<TData> obj)
            where TData : class
        {
            obj.AddTransformation(new DeflateStreamTransformation());
            return obj;
        }

        /// /// <summary>
        /// Configures the storage to compress the serialized content using the <see cref="GZipStream"/>.
        /// </summary>
        /// <returns></returns>
        public static IConfigureLocationOrLocationOptions<TData> GZipped<TData>(this IConfigureLocationOrLocationOptions<TData> obj)
            where TData : class
        {
            obj.AddTransformation(new GZipStreamTransformation());
            return obj;
        }
    }
}
