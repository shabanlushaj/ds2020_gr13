using MySql.Data.MySqlClient;
using System;
using System.Text;
using System.Security.Cryptography;

namespace ds
{
    class dbconnection
    {
        public static void Conn()
        {
            string cs = @"server=localhost;userid=root;password=;database=ds";
            var con = new MySqlConnection(cs);
            con.Open();
        }

        static string Random()
        {
            int Rand = new Random().Next(1000000, 10000000);
            string randString = Rand.ToString();
            return randString;
        }
        static string Hash(string plain)
        {
            SHA256CryptoServiceProvider objHash = new SHA256CryptoServiceProvider();

            string randString = Random();

            string salt_password = plain + randString;
            byte[] hashGet = Encoding.UTF8.GetBytes(salt_password);

            byte[] computeHash = objHash.ComputeHash(hashGet);
            string hash = Convert.ToBase64String(computeHash);
            return hash;
        }
        
        // tbd > mysql commands to save in db
    }
}
