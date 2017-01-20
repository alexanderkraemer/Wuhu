using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Web;

namespace WuHu.WebAPI
{
	public class TokenObj
	{
		public TokenComb Token { get; set; }
		public DateTime ValidUntil { get; set; }
	}
	[DataContract]
	public class TokenComb
	{
		[DataMember]
		public string Nickname { get; set; }
		[DataMember]
		public string Token { get; set; }
	}

	public class Authentication
	{
		private static Authentication instance;
		private List<TokenObj> TokenList;

		private Authentication()
		{
			this.TokenList = new List<TokenObj>();
		}
		public static Authentication getInstance()
		{
			if (instance == null)
			{
				instance = new Authentication();
			}
			return instance;
		}

		private static Random random = new Random();

		private static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public TokenComb newAuthentication(string nickname)
		{
			var t = new TokenComb();
			t.Token = RandomString(60);
			t.Nickname = nickname;

			var to = new TokenObj();
			to.Token = t;
			to.ValidUntil = DateTime.Now.AddMinutes(30);

			TokenList.RemoveAll(s => s.Token.Nickname == nickname);

			TokenList.Add(to);

			return t;
		}

		public bool isAuthenticateWithHeader(HttpRequestMessage obj)
		{
			var header = obj.Headers;
			string token ="";
			if (header.Contains("Authorization"))
			{
				token = header.GetValues("Authorization").First();
			}
			foreach (var o in TokenList)
			{
				if (o.Token.Token == token)
				{
					if (o.ValidUntil >= DateTime.Now)
					{
						o.ValidUntil = DateTime.Now.AddMinutes(30);
						return true;
					}
				}
			}
			return false;
		}
	}
}