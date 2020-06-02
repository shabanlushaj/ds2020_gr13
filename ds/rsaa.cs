using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

/*
 * C:\Users\HP\source\repos\rsaa\rsaa\bin\Debug\netcoreapp3.1>rsaa create-user Fiek.1
Argumenti i dyte duhet te jete A-Z,a-z,0-9.
C:\Users\HP\source\repos\rsaa\rsaa\bin\Debug\netcoreapp3.1>rsaa create-user Fiek1
Eshte krijua celsi privat 'keys/Fiek1.xml'
Eshte krijua celse publik 'keys/Fiek1.pub.xml'
C:\Users\HP\source\repos\rsaa\rsaa\bin\Debug\netcoreapp3.1>rsaa create-user Fiek1
Gabim: Celesi 'Fiek1' ekziston paraprakisht.
C:\Users\HP\source\repos\rsaa\rsaa\bin\Debug\netcoreapp3.1>rsaa delete-user Fiek1
Eshte larguar celsi privat 'keys/Fiek1.xml'
Eshte larguar celse publik 'keys/Fiek1.pub.xml'
C:\Users\HP\source\repos\rsaa\rsaa\bin\Debug\netcoreapp3.1>rsaa delete-user Fiek1
Gabim: Celesi 'Fiek1' ekziston paraprakisht.
*/

/*
C:\Users\HP\source\repos\rsaa\rsaa\bin\Debug\netcoreapp3.1>rsaa create-user fiek
Jepni fjalekalimin:Fiek.123
Perserite fjalekalimin:Fiek.123
Eshte krijua shfrytezuesi 'fiek'
Eshte krijua celsi privat 'keys/fiek.xml'
Eshte krijua celse publik 'keys/fiek.pub.xml'

C:\Users\HP\source\repos\rsaa\rsaa\bin\Debug\netcoreapp3.1>rsaa delete-user fiek
Eshte larguar celsi privat 'keys/fiek.xml'
Eshte larguar celse publik 'keys/fiek.pub.xml'
Eshte fshire shfrytezuesi 'fiek'
*/
namespace RsaSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            void VerifyLength(int length)
            {
                if (args.Length < length)
                {
                    throw new Exception("Mungojne argumentet.");
                }
            }
            VerifyLength(1);

            string command = args[0];

            RsaEncryptor rsa;

            if (command == "create-user")
            {
                try
                {
                    if (args[1].Length != 0)
                    {
                        string command2 = args[1];
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
                        string privkey = "keys/" + command2 + ".xml";
                        string pubkey = "keys/" + command2 + ".pub.xml";

                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (File.Exists(privkey) && File.Exists(pubkey))
                        {
                            Console.WriteLine("Gabim: Celesi '" + command2 + "' ekziston paraprakisht.");
                            return;
                        }
                        Console.Write("Jepni fjalekalimin:");
                        string password = Console.ReadLine();
                        Match match = Regex.Match(password, @"[0-9]+");
                        Match match1 = Regex.Match(password, @"[A-Z]+");
                        Match match2 = Regex.Match(password, @".{6,}");
                        Match match3 = Regex.Match(password, @"[a-z]+");
                        Match match4 = Regex.Match(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
                        if (!match.Success)
                        {
                            Console.WriteLine("Fjalekalimi duhet te kete se paku nje numer.");
                            return;
                        }
                        else if (!match1.Success)
                        {
                            Console.WriteLine("Fjalekalimi duhet te kete se paku nje shkronje te madhe.");
                            return;
                        }
                        else if (!match2.Success)
                        {
                            Console.WriteLine("Fjalekalimi duhet te kete se paku 6 karaktere.");
                            return;
                        }
                        else if (!match3.Success)
                        {
                            Console.WriteLine("Fjalekalimi duhet te kete se paku nje shkronje te vogel.");
                            return;
                        }
                        else if (!match4.Success)
                        {
                            Console.WriteLine("Fjalekalimi duhet te kete se paku nje simbol.");
                            return;
                        }
                        Console.Write("Perserite fjalekalimin:");
                        string cpassword = Console.ReadLine();
                        if (cpassword != password)
                        {
                            Console.WriteLine("Fjalekalimet nuk perputhen.");
                            return;
                        }

                        //////////////////////////////////
                        byte[] salt = new byte[128 / 8];
                        using (var rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(salt);
                        }
                        //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                        // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));
                       // Console.WriteLine($"Hashed: {hashed}");
                        ///////////////////////////////////////
                        
                        string connStr = "server=localhost;user=root;database=csharp;port=3306;password=";
                        MySqlConnection conn = new MySqlConnection(connStr);
                        try
                        {
                            Console.Write("");
                            string instr = "INSERT INTO db (User,Password) VALUES('" + command2 + "','" + hashed+ "')";
                            conn.Open();
                            MySqlCommand Command = new MySqlCommand(instr, conn);
                            // Perform database operations
                            if (Command.ExecuteNonQuery() == 1)
                            {
                                Console.Write("");
                            }
                            else
                            {
                                Console.Write("Failed");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            return;
                        }
                        conn.Close();
                        Console.Write("");

                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        File.WriteAllText("keys/" + command2 + ".xml", privateKey);
                        File.WriteAllText("keys/" + command2 + ".pub.xml", publicKey);

                        Console.WriteLine("Eshte krijua shfrytezuesi '" + command2 + "'");
                        Console.WriteLine("Eshte krijua celsi privat '" + privkey + "'");
                        Console.WriteLine("Eshte krijua celse publik '" + pubkey + "'");

                    }
                }
                catch
                {
                    Console.WriteLine("Kerkesa duhet te jete: create-user <emri>");
                    return;
                }
            }
            else if (command == "delete-user")
            {
                try
                {
                    if (args[1].Length != 0)
                    {
                        string command2 = args[1];
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
                        string privkey = "keys/" + command2 + ".xml";
                        string pubkey = "keys/" + command2 + ".pub.xml";
                        if (File.Exists(privkey) && File.Exists(pubkey))
                        {
                            File.Delete(privkey);
                            File.Delete(pubkey);
                            Console.WriteLine("Eshte larguar celsi privat '" + privkey + "'");
                            Console.WriteLine("Eshte larguar celse publik '" + pubkey + "'");
                        }
                        else
                        {
                            Console.WriteLine("Gabim: Celesi '" + command2 + "' nuk ekziston paraprakisht.");
                            return;
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        string connStr = "server=localhost;user=root;database=csharp;port=3306;password=";
                        MySqlConnection conn = new MySqlConnection(connStr);
                        try
                        {
                            Console.Write("");
                            string instr = "DELETE FROM db WHERE User='" + command2 + "'";
                            conn.Open();
                            MySqlCommand Command = new MySqlCommand(instr, conn);
                            // Perform database operations
                            if (Command.ExecuteNonQuery() == 1)
                            {
                                Console.Write("");
                            }
                            else
                            {
                                Console.Write("Failed");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            return;
                        }
                        conn.Close();
                        Console.Write("");
                        Console.WriteLine("Eshte fshire shfrytezuesi '" + command2 + "'");
                    }
                }//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                catch
                {
                    Console.WriteLine("Kerkesa duhet te jete: delete-user <emri>");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Kerkesa duhet te jete: create-user/delete-user <emri>");
                return;
            }
        }
    }

    class RsaEncryptor
    {
        private readonly RSACryptoServiceProvider rsacsp;

        public RsaEncryptor()
        {
            rsacsp = new RSACryptoServiceProvider();
        }

        public RsaEncryptor(string privateKey)
        {
            rsacsp = new RSACryptoServiceProvider();
            rsacsp.FromXmlString(privateKey);
        }

        public string GetPublicKey()
        {
            return rsacsp.ToXmlString(false);
        }

        public string GetPrivateKey()
        {
            return rsacsp.ToXmlString(true);
        }

        public string EncryptTextFor(string plaintext, string publicKey)
        {
            var otherParty = new RSACryptoServiceProvider();
            otherParty.FromXmlString(publicKey);

            return EncryptText(plaintext, otherParty);
        }

        public string EncryptText(string plaintext)
        {
            return EncryptText(plaintext, this.rsacsp);
        }

        public string DecryptText(string ciphertext)
        {
            // Decryption

            // ciphertext (base64) -> ciphertext (byte[]) - Convert.FromBase64String
            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            // ciphertext (byte[]) -> plaintext (byte[])  - rsa.Decrypt
            byte[] plaintextBytes = rsacsp.Decrypt(ciphertextBytes, true);

            // plaintext (byte[])  -> plaintext (string)  - Encoding.UTF8.GetString
            string plaintext = Encoding.UTF8.GetString(plaintextBytes);

            return plaintext;
        }

        private static string EncryptText(string plaintext, RSACryptoServiceProvider rsa)
        {
            // Encryption

            // plaintext (string)  -> plaintext (byte[])  - Encoding.UTF8.GetBytes
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            // plaintext (byte[])  -> ciphertext (byte[]) - rsa.Encrypt
            byte[] ciphertextBytes = rsa.Encrypt(plaintextBytes, true);

            // ciphertext (byte[]) -> ciphertext (base64) - Convert.ToBase64String
            string ciphertextString = Convert.ToBase64String(ciphertextBytes);

            return ciphertextString;
        }
    }
}
