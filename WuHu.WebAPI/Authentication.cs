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
			TokenComb token = new TokenComb();
			if (header.Contains("Authorization"))
			{
				string json = header.GetValues("Authorization").First();

				token = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenComb>(json);
				/*if (tokenObj is TokenComb)
				{
					token = (TokenComb)tokenObj;
				}
				else if (tokenObj is string)
				{
					var o = new TokenComb();
					o.Token = TokenObj;
					o.Nickname = 
					token = n;
				}
				*/
			}
			foreach (var o in TokenList)
			{
				if (o.Token.Nickname == token.Nickname && o.Token.Token == token.Token)
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