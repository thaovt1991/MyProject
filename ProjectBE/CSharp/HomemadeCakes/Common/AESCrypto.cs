using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace HomemadeCakes.Common
{
    public class AESCrypto
    {
        public const string DefaultPassword = "!vothao.tin@gmail.com!";
        #region Mã Hóa
        public static string Encrypt(string plainText, string pw = null)
        {
            if (string.IsNullOrEmpty(pw))
            {
                pw = "!vothao.tin@gmail.com!";
            }

            byte[] bytes = Encoding.UTF8.GetBytes(pw);
            byte[] bytes2 = Encoding.UTF8.GetBytes(pw);
            return Convert.ToBase64String(EncryptStringToBytes(plainText, bytes, bytes2));
        }
        #endregion
        #region Giải mã
        public static string Decrypt(string encryptedValue, string pw = null)
        {
            if (string.IsNullOrEmpty(pw))
            {
                pw = "!vothao.tin@gmail.com!";
            }

            byte[] bytes = Encoding.UTF8.GetBytes(pw);
            byte[] bytes2 = Encoding.UTF8.GetBytes(pw);
            return DecryptStringFromBytes(Convert.FromBase64String(encryptedValue), bytes, bytes2);
        }
        #endregion
        #region Mã hóa chuỗi thành byte
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }

            if (key == null || key.Length == 0)
            {
                throw new ArgumentNullException("key");
            }

            if (iv == null || iv.Length == 0)
            {
                throw new ArgumentNullException("key");
            }

            using RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.FeedbackSize = 128;
            rijndaelManaged.Key = key;
            rijndaelManaged.IV = iv;
            ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(plainText);
            }

            return memoryStream.ToArray();
        }
        #endregion
        #region Giãi mã chuỗi thành Byte
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length == 0)
            {
                throw new ArgumentNullException("cipherText");
            }

            if (key == null || key.Length == 0)
            {
                throw new ArgumentNullException("key");
            }

            if (iv == null || iv.Length == 0)
            {
                throw new ArgumentNullException("key");
            }

            string text = null;
            using RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.FeedbackSize = 128;
            rijndaelManaged.Key = key;
            rijndaelManaged.IV = iv;
            ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            using MemoryStream stream = new MemoryStream(cipherText);
            using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(stream2);
            return streamReader.ReadToEnd();
        }
        #endregion
        public static string Encode(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string Decode(string encoded)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }
    }
}
