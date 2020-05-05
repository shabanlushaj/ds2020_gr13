using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ds
{
    class WR
    {
        public static string des_Encrypt(string plaintext, string key, string iv)
        {
            byte[] bptext = Encoding.UTF8.GetBytes(plaintext);
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();

            objDES.Key = Convert.FromBase64String(key);
            objDES.IV = Convert.FromBase64String(iv);
            objDES.Padding = PaddingMode.Zeros;
            objDES.Mode = CipherMode.ECB;


            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, objDES.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(bptext, 0, bptext.Length);
            cs.Close();
            byte[] bciphertext = ms.ToArray();

            return Convert.ToBase64String(bciphertext);
        }

        //valid key^iv size 
        public static string des_Decrypt(string ciphertext, string key, string iv)
        {
            byte[] bcptext = Convert.FromBase64String(ciphertext);
            
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            objDES.Key = Convert.FromBase64String(key);
            objDES.IV = Convert.FromBase64String(iv);
            objDES.Padding = PaddingMode.Zeros;
            objDES.Mode = CipherMode.ECB;

            MemoryStream ms = new MemoryStream(bcptext);
            byte[] bdecrypted = new byte[ms.Length];

            CryptoStream cs = new CryptoStream(ms, objDES.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(bdecrypted, 0, bdecrypted.Length);
            cs.Close();

            return Encoding.UTF8.GetString(bdecrypted);
        }

        public static string rsa_Encrypt(string textToEncrypt, string publicKeyString)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
            var rsa = new RSACryptoServiceProvider();

            try
            {
                rsa.FromXmlString(publicKeyString.ToString());
                byte[] encryptedData = rsa.Encrypt(bytesToEncrypt, true);
                string base64Encrypted = Convert.ToBase64String(encryptedData);
                return base64Encrypted;
            }

            finally
            {
                rsa.PersistKeyInCsp = false;
            }
            
        }
        public static string rsa_Decrypt(string textToDecrypt, string privateKeyString)
        {
            byte[] bytesToDecrypt = Encoding.UTF8.GetBytes(textToDecrypt);
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(privateKeyString);
                    byte[] resultBytes = Convert.FromBase64String(textToDecrypt);
                    byte[] decryptedBytes = rsa.Decrypt(resultBytes, true); 
                    string decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        public static string Base64Encode(string plaintext)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            return Convert.ToBase64String(plaintextBytes);
        }
        public static string Base64Decode(string plaintext)
        {
            var plaintextBytes = Convert.FromBase64String(plaintext);
            return Encoding.UTF8.GetString(plaintextBytes);
        }

        public static bool Check_Base64(string check)
        {
            check = check.Trim();
            return (check.Length % 4 == 0) && Regex.IsMatch(check, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }
    }
}
