using MySql.Data.MySqlClient;
using System;
using System.Text;
using System.Security.Cryptography;

namespace ds
{
    class userinfo
    {
        public static void Conn(string uname, string password)
        {
            string connectionstring = @"server=localhost;userid=root;password=7834;database=ds";
            MySqlConnection con = new MySqlConnection(connectionstring);
            
            int Rand = new Random().Next(1000000, 10000000);
            string randString = Rand.ToString();
            
            string mysqlStatement = "insert into users(uname,salt, password) values('" + uname + "' ,  '" + randString + "','" + Hash(password, randString) + "');";
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

                Console.WriteLine("Error: " + e);
            }
        }

        static string Hash(string plain, string randString)
        {
            SHA256CryptoServiceProvider objHash = new SHA256CryptoServiceProvider();

            string salt_password = plain + randString;
            byte[] hashGet = Encoding.UTF8.GetBytes(salt_password);

            byte[] computeHash = objHash.ComputeHash(hashGet);
            string hash = Convert.ToBase64String(computeHash);
            return hash;
        }
    }
}
