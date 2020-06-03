using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ds
{
    class tokens
    {
        private static readonly RSACryptoServiceProvider rsk = new RSACryptoServiceProvider();
        public static string CreateToken()
        {
             var token = new JwtBuilder()
			.WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
			.WithSecret(secret)
			.AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
			.AddClaim("claim2", "claim2-value")
			.Encode();

			return token;
        }
        public static string DecodeToken(string token)
        {
            var json = new JwtBuilder()
			.WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
			.WithSecret(secret)
			.MustVerifySignature()
			.Decode(token);                     
            return json;
        }
    }
}