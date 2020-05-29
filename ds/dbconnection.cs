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
            string connectionstring = @"server=localhost;userid=root;password=7834;database=ds";
            var con = new MySqlConnection(connectionstring);

            string banti = "Shaban";
            string mysqlStatement = "insert into users(uname,salt, password) values('" + banti + "' ,  '" + Random() + "','" + Hash(banti) + "');";
            MySqlCommand commands = new MySqlCommand(mysqlStatement, con);
            try
            {
                con.Open();
                int fillRows = commands.ExecuteNonQuery();
                if (fillRows == 1)
                {
                   Console.WriteLine("Success!");
                }
                else
                {
                    Console.WriteLine("Error!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ka ndodhur nje gabim" + e);
            }
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
    }
}
