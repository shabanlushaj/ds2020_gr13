using System;
using System.Text;

namespace ds
{
    class secretpw
    {
        public static void secret_password()
        {
            Console.Write("You password please: ");
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
            Console.WriteLine();
            Console.Write("Your password in plain text is {0}", passwordBuilder.ToString());
        }
    }
}
