using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ds
{
    class write_read
    {
        private static  DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");

        public static void Write(string[] args)
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
        public static void Read(string[] args)
        {
            DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");
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
                            catch (CryptographicException)
                            {
                                Console.WriteLine("Dekriptimi nuk mund te behet me celesin e dhene");
                            }
                        }
                        catch (Exception)
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
                                catch (CryptographicException)
                                {
                                    Console.WriteLine("Dekriptimi nuk mund te behet me celesin e dhene");
                                }
                            }
                            catch (Exception)
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
    }
}
