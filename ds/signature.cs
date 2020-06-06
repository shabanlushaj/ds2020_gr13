using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ds
{
    class signature
    {

        private static readonly RSACryptoServiceProvider objRsa = new RSACryptoServiceProvider();


        private string Sign_data(string plain)
        {

            byte[] bytePlaintexti = Encoding.UTF8.GetBytes(plain);
            byte[] byteSignedText = objRsa.SignData(bytePlaintexti, new SHA1CryptoServiceProvider());

            string signature = Convert.ToBase64String(byteSignedText);
            return signature;
        }


        private void Verify_sign(string plain, string data)
        {
            byte[] bsd = Convert.FromBase64String(data);
            byte[] bytePlaintexti = Encoding.UTF8.GetBytes(plain);

            bool Verified = objRsa.VerifyData(bytePlaintexti, new SHA1CryptoServiceProvider(), bsd);

            if (Verified)
                Console.WriteLine("Nenshkrimi eshte valid!");
            else
                Console.WriteLine("Nenshkrimi NUK eshte valid!");
        }

       
    }
}
