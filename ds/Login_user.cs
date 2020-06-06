using System;
using System.Data;
using System.Text;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MySql.Data.MySqlClient;



namespace ds
{
    class Login_user
    {
        public static void Login(string username, string password)
        {
            string connectionstring = "server=localhost;user=root;database=csharp;port=3306;password=";
            MySqlConnection con = new MySqlConnection(connectionstring);
            string mysqlStatement = "select * from db where User = '" + username + "'";
            MySqlCommand commands = new MySqlCommand(mysqlStatement, con);

            DataSet ds = new DataSet();

            MySqlDataAdapter objAdapter = new MySqlDataAdapter(commands);
            try
            {
                objAdapter.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string DbPassword = ds.Tables[0].Rows[0]["Password"].ToString();
                    string DbSalt = ds.Tables[0].Rows[0]["Salt"].ToString();

                    StringBuilder sb = new StringBuilder();
                    byte[] DbSaltt = Convert.FromBase64String(DbSalt);
                    // same method as at create-user to get the passHashSalt
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password,
                                                                                salt: DbSaltt,
                                                                                prf: KeyDerivationPrf.HMACSHA1,
                                                                                iterationCount: 10000,
                                                                                numBytesRequested: 256 / 8));

                    if (DbPassword.Equals(hashed))
                    {

                        var now = DateTime.UtcNow;
                        //DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");
                        //string publicKey = File.ReadAllText(di + username + ".pub.xml");
                        string[] secret = new string[1] { /*publicKey*/ "Sun" };
                                             
                        var token = new JwtBuilder()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(secret)
                        .AddClaim(" User ", username)
                        .AddClaim(" Skadimi ", DateTime.Now.AddMinutes(20).ToString("MM/dd/yyyy HH:mm:ss"))
                        //.ExpirationTime(now.AddMinutes(20).ToLocalTime())
                        .Encode();
                        string tokenn = token;
                        Console.WriteLine("Token: " + tokenn);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e);
            }
        }
        public static void Status(string token)
        {
            try
            {
                var now = DateTime.UtcNow;

                //string[] secret = new string[1] { "Sun" };
                Console.WriteLine("");
                //var token = new JwtBuilder()
                string[] secret = new string[1] { "Sun" };
                var json = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm()) // 
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode(token);
                string t = json;
                string r = t.Trim(',', '{', '}');
                String[] strlist = r.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (String s in strlist)
                {
                    Console.WriteLine(s);
                }
            }
            catch
            {
                Console.WriteLine("Tokeni nuk eshte valid!");
            }
        }
    }
}