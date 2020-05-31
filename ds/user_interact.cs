using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ds
{
    class User_interact
    {
        public static void Userinfo(string uname, string password)
        {
            string connectionstring = @"server=localhost;userid=root;password=7834;database=ds";
            MySqlConnection con = new MySqlConnection(connectionstring);

            MySqlCommand query1 = new MySqlCommand("select * from users where uname ='" + uname + "'", con);
            con.Open();
            Int32 count = Convert.ToInt32(query1.ExecuteScalar());
            if (count > 0)
            {
                Console.WriteLine("Useri ekziston paraprakisht!");
            }

            else 
            {
                byte[] randomBytes = new byte[32];
                RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
                rngCsp.GetBytes(randomBytes);
                string randString = Convert.ToBase64String(randomBytes);
                string mysqlStatement = "insert into users(uname,salt, password) values('" + uname + "' ,  '" + randString + "','" + Hash(password, randString) + "')";
                MySqlCommand commands = new MySqlCommand(mysqlStatement, con);
                try
                {
                    int AffectedRows = commands.ExecuteNonQuery();
                    if (AffectedRows == 1)
                    {
                        Console.WriteLine("Eshte krijuar shfrytezuesi: '{0}'", uname);
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
            
        }
        /*
        static string RandomA()
        {
            int Rand = new Random().Next(1000000, 10000000);
            string randString = Rand.ToString();
            return randString;
        }*/

        static string Hash(string plain, string randString)
        {
            SHA512CryptoServiceProvider objHash = new SHA512CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < plain.Length; i++)
            {
                sb.Append(string.Concat(plain[i], randString[i]));
            }
            string salt_password = sb.ToString();
            byte[] hashGet = Encoding.UTF8.GetBytes(salt_password);

            byte[] computeHash = objHash.ComputeHash(hashGet);
            string hash = Convert.ToBase64String(computeHash);
            return hash;

        }
    
    }
}
