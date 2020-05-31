using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ds
{
    class Hide_pw
    {
        public static void Hide()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            bool continueReading = true;
            char newLineChar = '\r';
            while (continueReading)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(false);
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
            Check(pw);
        }
        public static bool Check(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$";
            if (password != null)
            {
                if (Regex.IsMatch(password, pattern))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}

