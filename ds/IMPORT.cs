using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
//Import key
namespace ds
{
    class IMPORT
    {
        private static RsaEncryptor rsa;
        private static readonly DirectoryInfo di = Directory.CreateDirectory(@"../../../keys/");
       
        public static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }


        public static void Export_key(string[] args)
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

        public static void Import_key(string[] args)
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
                        string getContent = IMPORT.Get(path);
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

    }
}
