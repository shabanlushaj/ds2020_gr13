using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

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

        // user -> emri i user
        // password -> salt + hash
    }
}
