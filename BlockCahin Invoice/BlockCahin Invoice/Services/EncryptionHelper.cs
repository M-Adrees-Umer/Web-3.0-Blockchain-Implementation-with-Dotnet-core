using System;
using System.Security.Cryptography;
using System.Text;
namespace BlockCahin_Invoice.Services
{
    public class EncryptionHelper
    {
        public static string Encrypt(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Generate a random IV (Initialization Vector)
                aes.GenerateIV();
                byte[] iv = aes.IV;

                // Encrypt the data
                byte[] encrypted;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                    encrypted = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
                }

                // Combine the IV and encrypted data
                byte[] combinedBytes = new byte[iv.Length + encrypted.Length];
                Array.Copy(iv, 0, combinedBytes, 0, iv.Length);
                Array.Copy(encrypted, 0, combinedBytes, iv.Length, encrypted.Length);

                // Convert the combined data to a base64 string for storage
                return Convert.ToBase64String(combinedBytes);
            }
        }

        public static string Decrypt(string encryptedText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extract the IV from the encrypted text
                byte[] combinedBytes = Convert.FromBase64String(encryptedText);
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] encryptedData = new byte[combinedBytes.Length - iv.Length];

                Array.Copy(combinedBytes, 0, iv, 0, iv.Length);
                Array.Copy(combinedBytes, iv.Length, encryptedData, 0, encryptedData.Length);

                aes.IV = iv;

                // Decrypt the data
                byte[] decrypted;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    decrypted = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                }

                return Encoding.UTF8.GetString(decrypted);
            }
        }
        public static string GenerateRandomKey()
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                return Convert.ToBase64String(aes.Key);
            }

        }
    }
}
