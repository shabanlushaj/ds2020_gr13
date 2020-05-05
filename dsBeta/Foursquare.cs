using System;
using System.Collections.Generic;
using System.Text;

namespace ds
{
    class FS
    {
		static string C_check(string s)
		{
			string cleanTxt = "";
			for (int i = 0; i < s.Length; i++)
			{
				char character = s[i];
				if (char.IsLetter(character) && character != 'j')
				{
					cleanTxt += char.ToLower(character);
				}
			}
			return cleanTxt;

		}
		static string Encoding(string key)
		{
			string keyword_encoding = "abcdefghiklmnopqrstuvwxyz";

			for (int i = key.Length - 1; i >= 0; i--)
			{
				if (key[i] != 'j'||char.IsDigit(key[i])||char.IsWhiteSpace(key[i]))
				{
					var index = keyword_encoding.IndexOf(key[i]);
					keyword_encoding = keyword_encoding.Remove(index, 1);
					keyword_encoding = key[i] + keyword_encoding;

				}

			}
			return keyword_encoding;
		}
		private static string Encode_d(string diag, string block1, string block2)
		{
			string encoded_digraph = "";
			string alphabetBlock = "abcdefghiklmnopqrstuvwxyz";
			char firstL = diag[0];
			char secondL = diag[1];
			int firstI = alphabetBlock.IndexOf(firstL);
			int secondI = alphabetBlock.IndexOf(secondL);
			int firstR = firstI / 5;
			int firstC = firstI % 5;
			int secondR = secondI / 5;
			int secondC = secondI % 5;

			int firstE_index = 5 * firstR + secondC;
			int secondE_index = 5 * secondR + firstC;

			char firstE_letter = block1[firstE_index];
			char secondE_letter = block2[secondE_index];


			encoded_digraph += firstE_letter;
			encoded_digraph += secondE_letter;

			return encoded_digraph;
		}

		private static string Decode_d(string diag, string block1, string block2)
		{

			string decoded_digraph = "";
			string alphabetBlock = "abcdefghiklmnopqrstuvwxyz";
			char firstL = diag[0];
			char secondL = diag[1];
			int firstI = block1.IndexOf(firstL);
			int secondI = block2.IndexOf(secondL);
			int firstR = firstI / 5;
			int firstC = firstI % 5;
			int secondR = secondI / 5;
			int secondC = secondI % 5;

			int firstD_index = 5 * firstR + secondC;
			int secondD_index = 5 * secondR + firstC;

			char firstD_letter = alphabetBlock[firstD_index];
			char secondD_letter = alphabetBlock[secondD_index];


			decoded_digraph += firstD_letter;
			decoded_digraph += secondD_letter;

			return decoded_digraph;
		}
		public static string Encrypt(string ptext, string k1, string k2)
		{
			string encoded_message = "";
			string block1 = Encoding(k1);
			string block2 = Encoding(k2);
			string message = C_check(ptext);
			int message_length = message.Length;


			if (message_length % 2 == 1)
			{
				message += "x";
			}

			for (int i = 0; i < message_length; i += 2)
			{
				string digraph = message.Substring(i, 2);
				string encoded_digraph = Encode_d(digraph, block1, block2);
				encoded_message += encoded_digraph;
			}
			return encoded_message;
		}

		public static string Decrypt(string ctext, string k1, string k2)
		{
			string decoded_message = "";
			string block1 = Encoding(k1);
			string block2 = Encoding(k2);
			string message = C_check(ctext);
			int message_length = message.Length;

			for (int i = 0; i < message_length; i += 2)
			{
				string digraph = message.Substring(i, 2);
				string decoded_digraph = Decode_d(digraph, block1, block2);
				decoded_message += decoded_digraph;
			}
			return decoded_message;
		}
	}
}
