using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ds
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string command = args[0];

                RsaEncryptor rsa;
                DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");

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
                            string privkey = di + command2 + ".xml";
                            string pubkey = di + command2 + ".pub.xml";
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
                            //////////////////////////////////

                            byte[] salt = new byte[128 / 8];
                            using (var rng = RandomNumberGenerator.Create())
                            {
                                rng.GetBytes(salt);
                            }
                            string Salt = Convert.ToBase64String(salt);
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
                            ///////////////////////////////////////

                            string connStr = "server=localhost;user=root;database=csharp;port=3306;password=";
                            MySqlConnection conn = new MySqlConnection(connStr);
                            try
                            {
                                Console.Write("");
                                string instr = "INSERT INTO db (User,Password,Salt) VALUES('" + command2 + "','" + hashed + "','" + Salt + "')";
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





                            File.WriteAllText(di + command2 + ".xml", privateKey);
                            File.WriteAllText(di + command2 + ".pub.xml", publicKey);

                            Console.WriteLine("Eshte krijuar shfrytezuesi '" + command2 + "'");
                            Console.WriteLine("Eshte krijuar celsi privat: 'keys/{0}.xml'", command2);
                            Console.WriteLine("Eshte krijuar celsi publik: 'keys/{0}.pub.xml'", command2);

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



                    }

                    catch
                    {
                        Console.WriteLine("Kerkesa duhet te jete: delete-user <emri>");
                        return;

                    }
        }
                else if (command == "export-key")
                {
                    try
                    {
                        if (args[1].Length != 0 && args[2].Length != 0)
                        {
                            string type = args[1];
        string name = args[2];
        string privKeysDir = di + name + ".xml";
        string pubKeysDir = di + name + ".pub.xml";
                            if (File.Exists(pubKeysDir) && File.Exists(privKeysDir))
                            {
                                if (args.Length == 3)
                                {
                                    if (type == "public")
                                    {
                                        string publicKey = File.ReadAllText(di + name + ".pub.xml");
        publicKey = publicKey.Replace(">", ">" + System.Environment.NewLine);
                                        Console.WriteLine("\n" + publicKey + "\n");

                                    }
                                    else if (type == "private")
                                    {
                                        string privateKey = File.ReadAllText(di + name + ".xml");
    privateKey = privateKey.Replace(">", ">" + System.Environment.NewLine);
                                        Console.WriteLine("\n" + privateKey + "\n");
                                    }
                                }
                                else if (args.Length == 4)
                                {
                                    string expFile = args[3];
DirectoryInfo expDir = Directory.CreateDirectory("exported/");
string publicKey = File.ReadAllText(di + name + ".pub.xml");
string privateKey = File.ReadAllText(di + name + ".xml");
                                    if (type == "private")
                                    {
                                        File.WriteAllText(expDir + expFile + ".xml", privateKey);
                                        Console.WriteLine("Celesi privat u ruajt ne fajllin " + expFile + ".xml");
                                    }
                                    else if (type == "public")
                                    {
                                        string expFilep = expFile + ".pub";
File.WriteAllText(expDir + expFilep + ".xml", publicKey);
                                        Console.WriteLine("Celesi publik u ruajt ne fajllin " + expFile + ".pub.xml");
                                    }
                                }
                            }
                            else if (File.Exists(pubKeysDir) && !File.Exists(privKeysDir))
                            {

                                if (args.Length == 3)
                                {
                                    if (type == "public")
                                    {
                                        string publicKey = File.ReadAllText(di + name + ".pub.xml");
Console.WriteLine("\n" + publicKey + "\n");
                                    }
                                    else if (type == "private")
                                    {
                                        Console.WriteLine("\nGabim: Celesi privat " + name + " nuk ekziston\n");
                                    }
                                }
                                else if (args.Length == 4)
                                {
                                    string expFile = args[3];
DirectoryInfo expDir = Directory.CreateDirectory("exported/");
                                    using (StreamWriter strw = File.CreateText(expDir + expFile)) ;
                                    string publicKey = File.ReadAllText(di + name + ".pub.xml");
File.WriteAllText(expDir + expFile, publicKey);
                                    Console.WriteLine("Celesi publik u ruajt ne fajllin " + expFile);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Gabim: Celesi " + name + " nuk ekziston.");
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Kerkesa duhet te jete: export-key <public|private> <name> dhe [file] opsionale");
                    }
                }
                else if (command == "import-key")
                {
                    try
                    {
                        if (args[1].Length != 0 && args[2].Length != 0)
                        {
                            string name = args[1];
string path = args[2];
string distinguishURL = "http";
                            if (path.Contains(distinguishURL))
                            {
                                string getContent = Import_key.Get(path);
                                if (getContent.Contains("<P>"))
                                {
                                    rsa = new RsaEncryptor();
string publicKey = rsa.GetPublicKey();
File.WriteAllText(di + name + ".xml", getContent);
                                    File.WriteAllText(di + name + ".pub.xml", publicKey);
                                    Console.WriteLine("Celesi privat u ruajt ne fajllin " + di + name + ".xml");
                                    Console.WriteLine("Celesi publik u ruajt ne fajllin " + di + name + ".pub.xml");
                                }
                                else
                                {
                                    File.WriteAllText(di + name + ".pub.xml", getContent);
                                    Console.WriteLine("Celesi publik u ruajt ne fajllin " + di + name + ".pub.xml");
                                }

                            }
                            else
                            {
                                if (File.Exists(path))
                                {
                                    string content = File.ReadAllText(path);/*@"C:\Users\Admin\Desktop\edon.xml"*/
                                    if (content.Contains("<P>"))
                                    {
                                        rsa = new RsaEncryptor();
string publicKey = rsa.GetPublicKey();
File.WriteAllText(di + name + ".xml", content);
                                        File.WriteAllText(di + name + ".pub.xml", publicKey);
                                        Console.WriteLine("Celesi privat u ruajt ne fajllin " + di + name + ".xml");
                                        Console.WriteLine("Celesi publik u ruajt ne fajllin " + di + name + ".pub.xml");
                                    }
                                    else
                                    {
                                        File.WriteAllText(di + name + ".pub.xml", content);
                                        Console.WriteLine("Celesi publik u ruajt ne fajllin " + di + name + ".pub.xml");
                                    }
                                }
                                else
                                    Console.WriteLine("Gabim: Fajlli " + path + " nuk ekziston.");
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Kerkesa duhet te jete: import-key <name> <path>");
                    }
                }

                else if (command == "list-keys")//Komande shtese -> listimi i celesave (needs to convert from path name > name:) 
                {

                    Dictionary<string, string> list_keys = new Dictionary<string, string>();
string[] fCount = Directory.GetFiles(@"../../../keys/", "*.xml");

                    foreach (string k in fCount)
                    {
                        string val = File.ReadAllText(@k, Encoding.UTF8);
list_keys.Add(k, val);

                    }
                    foreach (KeyValuePair<string, string> item in list_keys)
                    {
                        Console.WriteLine("Key: {0}, \nValue: {1}\n\n", item.Key, item.Value);
                    }
                }

                else if (command == "write-message")
                {
                    string input = args[1];

string pubkey = di + input + ".pub.xml";
DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();

string randKey = Convert.ToBase64String(objDes.Key);
string randiv = Convert.ToBase64String(objDes.IV);


                    if (File.Exists(pubkey))
                    {
                        if (args.Length == 3)
                        {

                            string publicKey = File.ReadAllText(di + input + ".pub.xml");
string tekst = args[2];

Console.WriteLine(WR.Base64Encode(input) + "." + WR.Base64Encode(randiv) + "." + WR.rsa_Encrypt(randKey, publicKey) + "." + WR.des_Encrypt(tekst, randKey, randiv));
                        }
                        else if (args.Length == 4)
                        {
                            string publicKey = File.ReadAllText(di + input + ".pub.xml");
string tekst = args[2];
string file = args[3];
DirectoryInfo di2 = Directory.CreateDirectory(@"../../../files/");
                            using (StreamWriter sw = File.CreateText(di2 + file)) ;

                            string g = (WR.Base64Encode(input) + "." + WR.Base64Encode(randiv) + "." + WR.rsa_Encrypt(randKey, publicKey) + "." + WR.des_Encrypt(tekst, randKey, randiv));
File.WriteAllText(di2 + file, g);

                            Console.WriteLine("Mesazhi i enkriptuar u ruajt ne fajllin: files/{0}", file);

                        }
                        else
                        {
                            Console.WriteLine("Numri i argumenteve nuk eshte valid!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Celesi publik: {0} nuk ekziston ", input);
                    }
                }
                else if (command == "read-message")
                {
                    string cipher = args[1];
                    if (Regex.Matches(cipher, @"\.").Count == 3)
                    {
                        var array = cipher.Split(new[] { '.' }, 4);

string firstElem = array[0];
string second = array[1];
string third = array[2];
string fourth = array[3];
                        if (WR.Check_Base64(firstElem) && WR.Check_Base64(second) && WR.Check_Base64(third) && WR.Check_Base64(fourth))
                        {
                            string input = WR.Base64Decode(firstElem);
string privkey = di + input + ".xml";

                            if (File.Exists(privkey))
                            {
                                Console.WriteLine("Marresi: " + input);
                                string privateKey = File.ReadAllText(di + input + ".xml");
                                try
                                {
                                    string iv_get = WR.Base64Decode(second);
                                    try
                                    {
                                        string rsaKey_get = WR.rsa_Decrypt(third, privateKey);
                                        try
                                        {
                                            Console.WriteLine("Dekriptimi: " + WR.des_Decrypt(fourth, rsaKey_get, iv_get));
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("Error: {0}");

                                        }
                                    }
                                    catch (CryptographicException e)
                                    {
                                        Console.WriteLine("Dekriptimi nuk mund te behet me celesin e dhene");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("IV nuk eshte e njejte me ate te enkriptimit!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Celesi privat " + input + " nuk ekziston.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nuk eshte Base64!");
                        }
                    }
                    else if (File.Exists(cipher))
                    {
                        string content = File.ReadAllText(cipher);
                        if (Regex.Matches(content, @"\.").Count == 3)
                        {
                            var array = content.Split(new[] { '.' }, 4);
string firstElem = array[0];
string second = array[1];
string third = array[2];
string fourth = array[3];
                            if (WR.Check_Base64(firstElem) && WR.Check_Base64(second) && WR.Check_Base64(third) && WR.Check_Base64(fourth))
                            {
                                string input = WR.Base64Decode(firstElem);
string privkey = di + input + ".xml";

                                if (File.Exists(privkey))
                                {
                                    Console.WriteLine("Marresi: " + input);
                                    string privateKey = File.ReadAllText(di + input + ".xml");
                                    try
                                    {
                                        string iv_get = WR.Base64Decode(second);
                                        try
                                        {
                                            string rsaKey_get = WR.rsa_Decrypt(third, privateKey);
                                            try
                                            {
                                                Console.WriteLine("Dekriptimi: " + WR.des_Decrypt(fourth, rsaKey_get, iv_get));
                                            }
                                            catch (Exception)
                                            {
                                                Console.WriteLine("Error: {0}");
                                            }
                                        }
                                        catch (CryptographicException e)
                                        {
                                            Console.WriteLine("Dekriptimi nuk mund te behet me celesin e dhene");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("IV nuk eshte e njejte me ate te enkriptimit!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Celesi privat " + input + " nuk ekziston.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nuk eshte Base64!");
                            }

                        }
                    }
                    else
                        Console.WriteLine("Provide valid args!");
                }
                //FAZA 1
                else if (command == "four-square")
                {
                    try
                    {
                        if (args[1] == "encrypt")
                        {
                            try
                            {
                                string plaintext = args[2];
string key1 = args[3];
string key2 = args[4];
Console.WriteLine("Encryption: " + FS.Encrypt(plaintext, key1, key2));
                            }

                            catch (Exception)
                            {
                                Console.WriteLine("You should put arguments in this order: <plaintext> <key1> <key2>" +
                                    "\nIf plaintext is a sentence, put them in \"\", key must be one word and contain only letters!");
                            }
                        }
                        else if (args[1] == "decrypt")
                        {
                            try
                            {
                                string ciphertext = args[2];
string key1 = args[3];
string key2 = args[4];
Console.WriteLine("Decryption: " + FS.Decrypt(ciphertext, key1, key2));
                            }

                            catch (Exception)
                            {
                                Console.WriteLine("You should put arguments in this order: <ciphertext> <key1> <key2>");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your command should be: <encrypt> or <decrypt> ");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("COMMANDS: <encrypt> <decrypt>");
                    }
                }

                else if (command == "case")
                {
                    try
                    {
                        if (args[1] == "lower")
                        {
                            string word = args[2];
Console.WriteLine(word.ToLower());
                        }
                        else if (args[1] == "upper")
                        {
                            string word = args[2];
Console.WriteLine(word.ToUpper());
                        }
                        else if (args[1] == "capitalize")
                        {
                            string word = args[2];
Console.WriteLine(CASE.Capitalize(word));
                        }
                        else if (args[1] == "inverse")
                        {
                            string word = args[2];
Console.WriteLine(CASE.Inverse(word));
                        }
                        else if (args[1] == "alternating")
                        {
                            string word = args[2];
Console.WriteLine(CASE.Alternating(word));
                        }
                        else if (args[1] == "sentence")
                        {
                            string a = args[2];
Console.Write(a.ToLower() + ", " + CASE.Str2(a) + ". " + a.ToUpper() + "! " + CASE.Str4(a) + ".\n" + CASE.Str5(a) + ", " + a.ToLower() + ". " + CASE.Str7(a) + "! " + CASE.Str8(a) + ".");
                        }
                        else
                            Console.WriteLine("Your command was wrong, choice one of the cases: <lower> <upper> <capitalize> <inverse> <alternating> <sentence>");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("CASES: <lower> <upper> <capitalize> <inverse> <alternating> <sentence> ");
                    }
                }
                else if (command == "rail-fence")
                {
                    try
                    {
                        if (args[1] == "encrypt")
                        {
                            string plaint = args[2];
string rail = args[3];
int railsNr = Int32.Parse(rail);
Console.WriteLine(RF.Rencode(plaint, railsNr));
                        }
                        else if (args[1] == "decrypt")
                        {
                            string ciphert = args[2];
string rail = args[3];
int railsNr = Int32.Parse(rail);
Console.WriteLine(RF.Rdecode(ciphert, railsNr));
                        }
                        else
                        {
                            Console.WriteLine("You must choose beetwen: <encrypt> <decrypt>");
                        }


                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Continue: <plaintext> <rails>");
                    }
                }
                else
                {
                    Console.WriteLine("Kerkesa duhet te jete: <create-user> <delete-user> <export-key> <import-key> <write-message> <read-message>");
                }
                /*else
				{
					Console.WriteLine("You should provide a valid METHOD: <four-square> <case> <rail-fence>");
				}*/
			}
		}
	}
}
