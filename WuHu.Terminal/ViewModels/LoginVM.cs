using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WuHu.Terminal.ViewModels
{
	public class LoginVM
	{
		ICommand _loginCommand;
		private string username;
		private string password;

		public LoginVM()
		{

		}

		public string Username
		{
			set { username = value; }
		}

		public string Password
		{
			set { password = value; }
		}

		public ICommand LoginCommand
		{
			get
			{
				if (_loginCommand == null)
				{
					_loginCommand = new RelayCommand(param =>
					{
						Password = param.Dat;
					});
				}
				return _loginCommand;
			}
		}
	}
}
