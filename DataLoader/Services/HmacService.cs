using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Services
{
    public class HmacService
    {

        /// <summary>
        /// Computes RFC 2104-compliant HMAC signature
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="algorithm"></param>
        /// <returns>Signature</returns>
        public String Sign(string data, string key, KeyedHashAlgorithm algorithm)
        {
            Encoding encoding = new UTF8Encoding();
            algorithm.Key = encoding.GetBytes(key);
            return Convert.ToBase64String(algorithm.ComputeHash(
            encoding.GetBytes(data.ToCharArray())));
        }

        /// <summary>
        /// Computes RFC 2104-compliant HMAC signature using SHA1
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns>Signature</returns>
        public String Sign1(String data, String key)
        {
            KeyedHashAlgorithm algorithm = new HMACSHA1();
            return Sign(data, key, algorithm);
        }

        /// <summary>
        /// Computes RFC 2104-compliant HMAC signature using SHA256
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns>Signature</returns>
        public String Sign256(String data, String key)
        {
            KeyedHashAlgorithm algorithm = new HMACSHA256();
            return Sign(data, key, algorithm);
        }

        /// <summary>
        /// Computes RFC 2104-compliant HMAC signature using specified signature method
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="SignatureMethod"></param>
        /// <returns>Signature</returns>
        public String SignUsing(String data, String key, String SignatureMethod)
        {
            KeyedHashAlgorithm algorithm = KeyedHashAlgorithm.Create(SignatureMethod.ToUpper());
            return Sign(data, key, algorithm);
        }


        public string ComputeNonce()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[20];
            rng.GetBytes(data);
            int value = Math.Abs(BitConverter.ToInt32(data, 0));
            return value.ToString();
        }



        private long computeTimestamp()
        {
            return ((long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

    }
}
//Warning: For security reasons, this is the only time that the Client Credentials values are displayed. After you leave this page, they cannot be retrieved from the system. If you lose or forget these credentials, you will need to reset them to obtain new values.
//Treat the values for Client Credentials as you would a password.Never share these credentials with unauthorized individuals and never send them by email.
//CONSUMER KEY / CLIENT ID
//9be628e1cf623f3e6808dc806ba728ab46e42721bc0308b6c01ddac41818d666
//CONSUMER SECRET / CLIENT SECRET
//5167c7b438db3644822124d7ad6c0e061f9a7f7b545ecbc9bd7818c0bf03d168


//Warning: For security reasons, this is the only time that the Token ID and Token Secret values are displayed. After you leave this page, they cannot be retrieved from the system. If you lose or forget these credentials, you will need to reset them to obtain new values.
//Treat the values for Token ID and Token Secret as you would a password.Never share these credentials with unauthorized individuals and never send them by email.
//TOKEN ID
//52e613ff372d4df021f4a2ed6cc796c69214bdb02e7243187f3820b57076c204
//TOKEN SECRET
//50a652633d85512388a3a84708d9871a901a7c411f0aa8e26a86ae1cde74f2b5