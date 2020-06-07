using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MySql.Data.MySqlClient;



namespace ds
{
    class Login_user
    {
        private static RSACryptoServiceProvider rsaObj = new RSACryptoServiceProvider();
        private static DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");
        public static void Login(string username, string password)
        {
            string connectionstring = "server=localhost;user=root;database=ds;port=3306;password=7834";
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
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password,
                                                                               salt: DbSaltt,
                                                                               prf: KeyDerivationPrf.HMACSHA1,
                                                                               iterationCount: 10000,
                                                                               numBytesRequested: 256 / 8));
                    if (DbPassword.Equals(hashed))
                    {

                        var now = DateTime.UtcNow;
                        string privateKey = File.ReadAllText(di + username + ".xml");
                        rsaObj.FromXmlString(privateKey);
                        var token = new JwtBuilder()
                        .WithAlgorithm(new RS256Algorithm(rsaObj,rsaObj))
                        .AddClaim(" User ", username)
                        .AddClaim(" Skadimi ", DateTime.Now.AddMinutes(20).ToString("yyyy-MM-dd HH:mm tt"))
                        .Encode();
                        string tokenn = token;
                        Console.WriteLine("Token: " + tokenn);
                    }
                    else 
                    {
                        Console.WriteLine("Gabim: " + "Fjalekalimi i gabuar.");
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
            Signature sign = new Signature();
            string uname = sign.GetSender(token);
            string username = WR.Base64Decode(uname);
            Console.WriteLine(username);
            try
            {
                var now = DateTime.UtcNow;

                
                string[] secret = new string[1] { "Sun" };
                string pubKey = File.ReadAllText(di + username + ".pub.xml");
                rsaObj.FromXmlString(pubKey);
                var json = new JwtBuilder()
                .WithAlgorithm(new RS256Algorithm(rsaObj,rsaObj)) // 
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
