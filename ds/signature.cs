using Org.BouncyCastle.Bcpg.Sig;
using Renci.SshNet.Messages;
using Renci.SshNet.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ds
{
    public sealed class Signature
    {
        private  DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");
        public string Compute_hash(string plaintext)
        {
            SHA256CryptoServiceProvider sObj = new SHA256CryptoServiceProvider();
            byte[] pByte = Encoding.UTF8.GetBytes(plaintext);
            byte[] hByte = sObj.ComputeHash(pByte);
            string hValue = Convert.ToBase64String(hByte);
            return hValue;
        }

        public string Sign_data(string plain,string input)
        {
            byte[] hashOfDataToSign = Convert.FromBase64String(plain);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                string user = File.ReadAllText(di+input+".xml");
               // Console.WriteLine(user);
                rsa.FromXmlString(user);
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                byte[] rf = rsaFormatter.CreateSignature(hashOfDataToSign);
                string sdata = Convert.ToBase64String(rf);
                return sdata;
            }
        }
        public bool Verify_sign(string plain, string data, string input)
        {
            byte[] hashOfDataToSign = Convert.FromBase64String(plain);
            byte[] signature = Convert.FromBase64String(data);
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                string user = File.ReadAllText(di + input + ".pub.xml");
                rsa.FromXmlString(user);
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
        public string Sender(string token,string message, string user ,string key, string iv)
        {
            if (isExpired(token))
            {

                string msg_des = WR.des_Encrypt(message, key, iv);
                string hash = Compute_hash(msg_des);
                string msg_sign = Sign_data(hash,user);
                string msg_b64 = WR.Base64Encode(msg_sign);
                return msg_b64;
            }
            else
            {
                return null;
            }
        }
        public string Ver_msg(string cipher, string message, string user1, string key, string iv)
        {
            
            string msg_des = WR.des_Encrypt(message, key, iv);
            string hash = Compute_hash(msg_des);
            string msg_sign = Sign_data(hash, user1);
            string msg_b64 = WR.Base64Encode(msg_sign);
            if (cipher.Equals(msg_b64))
            {
                //Console.WriteLine("Perputhen");
                bool verify = Verify_sign(hash, msg_sign,user1);
                if (verify)
                {
                    return "Nenshkrimi eshte valid.";
                }
                else
                {
                    return "Nenshkrimi nuk eshte valid!";
                }
            }
            else
            {
                return "Mesazhi eshte i ndryshuar";
            }
        }

        //net-informations.com
        private static bool isExpired(string token)
        {
            int indexOfFirstPoint = token.IndexOf('.') + 1;
            string toDecode = token.Substring(indexOfFirstPoint, token.LastIndexOf('.') - indexOfFirstPoint);
            while (toDecode.Length % 4 != 0)
            {
                toDecode += '=';
            }
            string decodedString = Encoding.ASCII.GetString(Convert.FromBase64String(toDecode));
            string beginning = "\" Skadimi \":\"";
            int startPosition = decodedString.LastIndexOf(beginning) + beginning.Length;
            decodedString = decodedString.Substring(startPosition);
            int endPosition = decodedString.IndexOf("\"");
            decodedString = decodedString.Substring(0, endPosition);
            DateTime tokenDate = DateTime.ParseExact(decodedString, "yyyy-MM-dd HH:mm tt", null);
            DateTime compareTo = DateTime.Now;

            int result = DateTime.Compare(tokenDate, compareTo);

            if (result>0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public string GetSender(string token)
        {

            int indexOfFirstPoint = token.IndexOf('.') + 1;
            string toDecode = token.Substring(indexOfFirstPoint, token.LastIndexOf('.') - indexOfFirstPoint);
            while (toDecode.Length % 4 != 0)
            {
                toDecode += '=';
            }
            string decodedString = Encoding.ASCII.GetString(Convert.FromBase64String(toDecode));
            string beginning = "\" User \":\"";
            int startPosition = decodedString.LastIndexOf(beginning) + beginning.Length;
            decodedString = decodedString.Substring(startPosition);
            int endPosition = decodedString.IndexOf("\"");
            decodedString = decodedString.Substring(0, endPosition);
            string sender = WR.Base64Encode(decodedString);
            return sender;
        }
    }
}