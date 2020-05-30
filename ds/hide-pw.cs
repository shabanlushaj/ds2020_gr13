using System;
using System.Collections.Generic;
using System.Text;

namespace ds
{
    class Hide_pw
    {
        public static string Hide()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            bool continueReading = true;
            char newLineChar = '\r';
            while (continueReading)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                char passwordChar = consoleKeyInfo.KeyChar;
                if (passwordChar == newLineChar)
                {
                    continueReading = false;
                }
                else
                {
                    passwordBuilder.Append(passwordChar.ToString());
                }
            }
            Console.WriteLine("");
            
            string pw = passwordBuilder.ToString();
            if (WR.CheckPassword(pw))
            {
                return pw;

            }
            else
            {
                Console.WriteLine("Passwordi nuk eshte valid");
                return null;
            }
            
        }
    }
}

