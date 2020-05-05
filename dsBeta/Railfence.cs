using System;
using System.Collections.Generic;
using System.Text;

namespace ds
{
    class RF
    {
        public static string Rencode(string plaintext, int rails)
        {
            string ciphertext = "";
            bool check = false;
            int j = 0;
            int rows = rails;
            int columns = plaintext.Length;
            //char[] plainArray = plaintext.ToCharArray();

            char[,] a = new char[rows, columns];
            for (int i = 0; i < columns; i++)
            {
                if (j == 0 || j == rails - 1)
                    check = !check;

                a[j, i] = plaintext[i];

                if (check)
                    j++;
                else
                    j = 0;
            }



            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k < columns; k++)
                {
                    if (a[i, k] != 0)
                        ciphertext += a[i, k];
                }
            }




            return ciphertext;

        }



        public static string Rdecode(string ciphertext, int rails)
        {

            bool checkdown = false;
            int j = 0;
            int row = rails;
            int col = ciphertext.Length;
            char[,] a = new char[row, col];


            for (int i = 0; i < col; i++)
            {
                if (j == 0 || j == row - 1)
                    checkdown = !checkdown;
                a[j, i] = '*';
                if (checkdown)
                    j++;
                else
                    j--;
            }



            int index = 0;
            for (int i = 0; i < row; i++)
            {
                for (int k = 0; k < col; k++)
                {
                    if (a[i, k] == '*' && index < ciphertext.Length)
                    {
                        a[i, k] = ciphertext[i];
                    }
                }
            }



            checkdown = false;
            string decoded = "";
            j = 0;

            for (int i = 0; i < col; i++)
            {
                if (j == 0 || j == row - 1)
                    checkdown = !checkdown;
                decoded += a[j, i];
                if (checkdown)
                    j++;
                else j--;
            }

            return decoded;

        }
    }
}
