using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace ds
{
    class Login_user
    {
        public static void Login(string username, string password)
        {
            string connectionstring = @"server=localhost;userid=root;password=7834;database=ds";
            MySqlConnection con = new MySqlConnection(connectionstring);
            string mysqlStatement = "select * from users where uname = '" + username + "'";
            MySqlCommand commands = new MySqlCommand(mysqlStatement, con);
            commands.Prepare();
            DataSet ds = new DataSet();

            MySqlDataAdapter objAdapter = new MySqlDataAdapter(commands);
            try
            {
                objAdapter.Fill(ds);
                if (ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0)
                {
                    string DbPassword = ds.Tables[0].Rows[0]["password"].ToString();
                    string DbSalt = ds.Tables[0].Rows[0]["salt"].ToString();

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < password.Length; i++)
                    {
                        sb.Append(string.Concat(password[i], DbSalt[(DbSalt.Length/3)+i]));
                    }
                    string SaltPassword = sb.ToString();
                    byte[] byteSaltPassword = Encoding.UTF8.GetBytes(SaltPassword);

                    SHA512CryptoServiceProvider objHash = new SHA512CryptoServiceProvider();
                    byte[] byteSaltedHashPassword = objHash.ComputeHash(byteSaltPassword);

                    string saltedHashPassword = Convert.ToBase64String(byteSaltedHashPassword);

                    if (DbPassword.Equals(saltedHashPassword))
                    {
                        Console.WriteLine("Login SUCCESS!");
                    }
                    else
                    {
                        Console.WriteLine("Username or password is error!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e);
            }
        }
    }
}
