using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ds
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "create-user")
                {
                    if (args.Length == 2)
                    {
                        string uname = args[1];
                        Console.Write("Jepni fjalekalimin: ");
                        string pass1 = Hide_pw.Hide();
                        Console.Write("Perserit fjalekalimin: ");
                        string pass2 = Hide_pw.Hide();
                        if (pass1.Equals(pass2))
                        {
                            User_interact.Userinfo(uname, pass1);
                            USER.Create_user(uname);
                        }
                        else
                        {
                            Console.WriteLine("Fjalekalimet nuk perputhen!");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Numri i argumenteve nuk eshte valid!");
                    }
                }
                else if (args[0] == "delete-user")
                {
                    try
                    {
                        if (args[1].Length != 0)
                        {
                            USER.Delete_user(args[1]);
                        }

                    }
                    catch
                    {
                        Console.WriteLine("Kerkesa duhet te jete: delete-user <emri>");
                        return;

                    }
                }
                else if(args[0] == "login")
                {
                    Login_user.Login(args[1],args[2]);//should be modified :one argument - others dpndcs;
                }
                else if (args[0] == "export-key")
                {
                    IMPORT.Export_key(args);
                }
                else if (args[0] == "import-key")
                {
                    IMPORT.Import_key(args);
                }

                else if (args[0] == "list-keys")//Komande shtese -> listimi i celesave (needs to convert from path name > name:) 
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

                else if (args[0] == "write-message")
                {
                    write_read.Write(args);
                }
                else if (args[0] == "read-message")
                {
                    write_read.Read(args);
                }
                //FAZA 1
                else if (args[0] == "four-square")
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
                            Console.WriteLine("Your args[0] should be: <encrypt> or <decrypt> ");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("args[0]S: <encrypt> <decrypt>");
                    }
                }

                else if (args[0] == "case")
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
                            Console.WriteLine("Your args[0] was wrong, choice one of the cases: <lower> <upper> <capitalize> <inverse> <alternating> <sentence>");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("CASES: <lower> <upper> <capitalize> <inverse> <alternating> <sentence> ");
                    }
                }
                else if (args[0] == "rail-fence")
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
