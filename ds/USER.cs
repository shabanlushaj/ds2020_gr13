using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace ds
{
    class USER
    {
        private static  RsaEncryptor rsa;
        private static readonly DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");

        public static void Create_user(string command2)
        {
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < command2.Length; i++)
            {
                if ((command2[i] >= '0' && command2[i] <= '9') || (command2[i] >= 'A' && command2[i] <= 'Z') || (command2[i] >= 'a' && command2[i] <= 'z') || command2[i] == '_')
                {
                    sb.Append(command2[i].ToString());
                }
                else
                {
                    Console.WriteLine("Emrat duhet të përmbajnë vetëm simbolet A-Z, a-z, 0-9,dhe _");
                    return;
                }
            }
            command2 = sb.ToString();
            rsa = new RsaEncryptor();
            string privateKey = rsa.GetPrivateKey();
            string publicKey = rsa.GetPublicKey();
            string privkey = di + command2 + ".xml";
            string pubkey = di + command2 + ".pub.xml";
            if (File.Exists(privkey) && File.Exists(pubkey))
            {
                Console.WriteLine("Gabim: Celesi '" + command2 + "' ekziston paraprakisht.");
                return;
            }
            File.WriteAllText(di + command2 + ".xml", privateKey);
            File.WriteAllText(di + command2 + ".pub.xml", publicKey);

            Console.WriteLine("Eshte krijuar celsi privat: 'keys/{0}.xml'", command2);
            Console.WriteLine("Eshte krijuar celsi publik: 'keys/{0}.pub.xml'", command2);
        }

        public static void Delete_user(string command2)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < command2.Length; i++)
            {
                if ((command2[i] >= '0' && command2[i] <= '9') || (command2[i] >= 'A' && command2[i] <= 'Z') || (command2[i] >= 'a' && command2[i] <= 'z') || command2[i] == '_')
                {
                    sb.Append(command2[i].ToString());
                }
                else
                {
                    Console.WriteLine("Emrat duhet të përmbajnë vetëm simbolet A-Z, a-z, 0-9,dhe _");
                    return;
                }
            }
            command2 = sb.ToString();

            string privkey = di + command2 + ".xml";
            string pubkey = di + command2 + ".pub.xml";
            if (File.Exists(privkey) && File.Exists(pubkey))
            {
                File.Delete(privkey);
                File.Delete(pubkey);
                Console.WriteLine("Eshte larguar celsi privat: 'keys/{0}.xml'", command2);
                Console.WriteLine("Eshte larguar celsi publik: 'keys/{0}.pub.xml'", command2);
            }
            else
            {
                Console.WriteLine("Gabim: Celesi '" + command2 + "' nuk ekziston.");
            }
        }
    }
}