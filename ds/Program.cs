using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;

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
                            File.WriteAllText(di + command2 + ".xml", privateKey);
                            File.WriteAllText(di + command2 + ".pub.xml", publicKey);

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
                            //DirectoryInfo dir = Directory.CreateDirectory("keys/");
                            //We have di for directory $top.
                            string privKeysDir = /*dir*/ di + name + ".xml";
                            string pubKeysDir = /*dir*/di + name + ".pub.xml";
                            if (File.Exists(pubKeysDir) && File.Exists(privKeysDir))
                            {
                                if (args.Length == 3)
                                {
                                    /*if (type == "public")
                                    {
                                        string publicKey = File.ReadAllText(di + name + ".pub.xml");
                                        char[] seperator = { '>' };
                                        String[] strlist = publicKey.Split(seperator);
                                        foreach(String s in strlist){Console.Write(s + ">"); Console.WriteLine();}

                                    }*/
                                    if (type == "public")
                                    {
                                        string publicKey = File.ReadAllText(/*dir*/ di + name + ".pub.xml");
                                        publicKey = publicKey.Replace(">", ">" + System.Environment.NewLine);
                                        Console.WriteLine("\n" + publicKey + "\n");
                                    }
                                    else if (type == "private")
                                    {
                                        string privateKey = File.ReadAllText(/*dir*/ di + name + ".xml");
                                        privateKey = privateKey.Replace(">", ">" + System.Environment.NewLine);
                                        Console.WriteLine("\n" + privateKey + "\n");
                                    }
                                }
                                else if (args.Length == 4/*> 3*/)
                                {
                                    string expFile = args[3];
                                    DirectoryInfo expDir = Directory.CreateDirectory("exported/");
                                    using (StreamWriter strw = File.CreateText(expDir + expFile)) ;
                                    string publicKey = File.ReadAllText(/*dir*/ di + name + ".pub.xml");//
                                    File.WriteAllText(expDir + expFile, publicKey);
                                    Console.WriteLine("Celesi publik u ruajt ne fajllin " + expFile + ".xml");
                                }
                            }
                            else if (File.Exists(pubKeysDir) && !File.Exists(privKeysDir))
                            {

                                if (args.Length == 3)
                                {
                                    if (type == "public")
                                    {
                                        string publicKey = File.ReadAllText(/*dir*/ di + name + ".pub.xml");
                                        Console.WriteLine("\n" + publicKey + "\n");
                                    }
                                    else if (type == "private")
                                    {
                                        //string privateKey = File.ReadAllText(/*dir*/ di + name + ".xml");
                                        Console.WriteLine("\nGabim: Celesi privat " + name + " nuk ekziston\n");
                                    }
                                }
                                else if (args.Length == 4/*> 3*/)
                                {
                                    string expFile = args[3];
                                    DirectoryInfo expDir = Directory.CreateDirectory("exported/");
                                    using (StreamWriter strw = File.CreateText(expDir + expFile)) ;
                                    string publicKey = File.ReadAllText(/*dir*/ di + name + ".pub.xml");//
                                    File.WriteAllText(expDir + expFile, publicKey);
                                    Console.WriteLine("Celesi publik u ruajt ne fajllin " + expFile);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Gabim: Celesi publik " + name + " nuk ekziston.");
                            }

                        }
                    }
                    catch
                    {
                        Console.WriteLine("Kerkesa duhet te jete: export-key <public|private> <name> dhe [file] opsionale");
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

                            string g = ("\n" + WR.Base64Encode(input) + "." + WR.Base64Encode(randiv) + "." + WR.rsa_Encrypt(randKey, publicKey) + "." + WR.des_Encrypt(tekst, randKey, randiv));
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
                    try
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
                    catch (Exception)
                    {
                        Console.WriteLine("Mesazhi i dhene nuk permban njeren nga parametrat <name> <iv> <key> <message>");
                    }
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
                    Console.WriteLine("Kerkesa duhet te jete: <create-user> <delete-user> <write-message> <read-message>");
                }
                /*else
				{
					Console.WriteLine("You should provide a valid METHOD: <four-square> <case> <rail-fence>");
				}*/
			}
		}
	}
}
