using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ThinkSharp.ObjectStorage.Helper;

namespace ThinkSharp.ObjectStorage.StreamTransformations
{
    internal class EncryptionStreamTransformation<TEncryptionAlgorithm>
        : IStreamTransformation where TEncryptionAlgorithm : SymmetricAlgorithm, new()
    {
        private readonly string myPassword;

        public EncryptionStreamTransformation(string password)
        {
            password.Ensure("password").IsNotNullOrEmpty();

            myPassword = password;
        }

        private TEncryptionAlgorithm CreateAlgorithm()
        {
            var salt = Encoding.ASCII.GetBytes("1fkdMs4M643MsdmcIW");
            var key = new Rfc2898DeriveBytes(myPassword, salt);

            var algorithm = new TEncryptionAlgorithm();
            algorithm.IV = Convert.FromBase64String("GAeqQ1Kr9jnz9OTm4eqf2Q==");
            algorithm.Key = key.GetBytes(algorithm.KeySize/8);
            return algorithm;
        }

        public Stream TransformDeserialization(Stream stream)
        {
            using (var algorithm = CreateAlgorithm())
            {
                var cryptoTransform = algorithm.CreateDecryptor();
                var memoryStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    stream.CopyTo(cryptoStream);
                    cryptoStream.FlushFinalBlock();
                    return new MemoryStream(memoryStream.ToArray());
                }
            }
        }

        public Stream TransformSerialization(Stream stream)
        {
            var algorithm = CreateAlgorithm();
            var cryptoTransform = algorithm.CreateEncryptor();
            return new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Read);
        }
    }
}