﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WuHu.Terminal.ViewModels
{
	public class LoginVM
	{
		ICommand _loginCommand;
		private string nickname;
		private string password;

		private static LoginVM instance;
		public static LoginVM getInstance()
		{
			if(instance == null)
			{
				instance = new LoginVM();
			}
			return instance;
		}

		private LoginVM()
		{
			
		}

		public string Nickname
		{
			get { return nickname; }
			set { nickname = value; }
		}

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		private bool isAuthenticated;
		public bool IsAuthenticated
		{
			get { return isAuthenticated; }
			set
			{
				if (value != isAuthenticated)
				{
					isAuthenticated = value;
				}
			}
		}

		public ICommand LoginCommand
		{
			get
			{
				if (_loginCommand == null)
				{
					_loginCommand = new RelayCommand(param =>
					{
						var login_password = param as PasswordBox;
						var password = login_password.Password;
						Password = password;
						Authentication.getInstance().Authenticate(Nickname, Password);
					});
				}
				return _loginCommand;
			}
		}
	}
}