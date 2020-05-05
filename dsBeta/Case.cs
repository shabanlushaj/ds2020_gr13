using System;
using System.Globalization;
using System.Text;

namespace ds
{
    class CASE
    {
        public static string Capitalize(string capital)
        {
            capital = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(capital.ToLower());
            return capital;
        }
        public static string Inverse(string inv)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < inv.Length; i++)
            {

                if ((inv[i] >= 'a' && inv[i] <= 'z') || inv[i] == ' ')
                {
                    sb.Append(inv[i].ToString().ToUpper());
                }
                else if ((inv[i] >= 'A' && inv[i] <= 'Z') || inv[i] == ' ')
                {
                    sb.Append(inv[i].ToString().ToLower());
                }
            }
            inv = sb.ToString();
            return inv;
        }
        public static string Alternating(string alter)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < alter.Length; i++)
            {
                if (i % 2 == 0)
                {
                    sb.Append(alter[i].ToString().ToLower());
                }
                else
                {
                    sb.Append(alter[i].ToString().ToUpper());
                }
            }
            alter = sb.ToString();
            return alter;

        }



        public static string Str2(string b)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {

                if (i == 0 || i == 7)
                {
                    sb.Append(b[i].ToString().ToUpper());
                }
                else
                {
                    sb.Append(b[i].ToString().ToLower());
                }
            }
            b = sb.ToString();
            return b;
        }
        public static string Str4(string c)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < c.Length; i++)
            {

                if (i == 2 || i == 7 || i == 11)
                {
                    sb.Append(c[i].ToString().ToUpper());
                }
                else
                {
                    sb.Append(c[i].ToString().ToLower());
                }
            }
            c = sb.ToString();
            return c;
        }
        public static string Str5(string d)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < d.Length; i++)
            {

                if (i == 0)
                {
                    sb.Append(d[i].ToString().ToUpper());
                }
                else
                {
                    sb.Append(d[i].ToString().ToLower());
                }
            }
            d = sb.ToString();
            return d;
        }
        public static string Str7(string e)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < e.Length; i++)
            {

                if (i == 0)
                {
                    sb.Append(e[i].ToString().ToUpper());
                }
                else
                {
                    sb.Append(e[i].ToString().ToLower());
                }
            }
            e = sb.ToString();
            return e;
        }
        public static string Str8(string f)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < f.Length; i++)
            {

                if (i == 0)
                {
                    sb.Append(f[i].ToString().ToUpper());
                }
                else
                {
                    sb.Append(f[i].ToString().ToLower());
                }
            }
            f = sb.ToString();
            return f;

        }
    }
}

