using Org.BouncyCastle.Bcpg.Sig;
using Renci.SshNet.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ds
{
    public sealed class Signature
    {
        public string Compute_hash(string plaintext)
        {
            SHA256CryptoServiceProvider sObj = new SHA256CryptoServiceProvider();
            byte[] pByte = Encoding.UTF8.GetBytes(plaintext);
            byte[] hByte = sObj.ComputeHash(pByte);
            string hValue = Convert.ToBase64String(hByte);
            return hValue;
        }

        public string Sign_data(string plain)
        {
            byte[] hashOfDataToSign = Convert.FromBase64String(plain);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                string user = File.ReadAllText(@"C:\Users\Admin\Desktop\edon.xml");//only for example
                rsa.FromXmlString(user);
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                byte[] rf = rsaFormatter.CreateSignature(hashOfDataToSign);
                string sdata = Convert.ToBase64String(rf);
                return sdata;
            }
        }
        public bool Verify_sign(string plain, string data)
        {
            byte[] hashOfDataToSign = Convert.FromBase64String(plain);
            byte[] signature = Convert.FromBase64String(data);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                string user = File.ReadAllText(@"C:\Users\Admin\Desktop\edon.pub.xml");//only for example
                rsa.FromXmlString(user);
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
        public string Sender(string message,string key, string iv)
        {
            string msg_des = WR.des_Encrypt(message, key, iv);
            string msg_sign = Sign_data(msg_des);
            string msg_b64 = WR.Base64Encode(msg_sign);
            return msg_b64;
        }
        
    }
}