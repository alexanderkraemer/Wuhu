using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views.Player
{
	/// <summary>
	/// Interaction logic for PlayerEdit.xaml
	/// </summary>
	public partial class PlayerEdit : UserControl
	{
		private PlayerVM player = null;

		public PlayerEdit(PlayerVM player)
		{
			
		}

		public string Header { get; internal set; }

		public void SavePlayerButtonClick(object sender, object e)
		{
			MainWindow.main.Content = new PlayerList();
		}

		public void CancelPlayerButtonClick(object sender, object e)
		{
			MainWindow.main.Content = new PlayerList();
		}

		public void SetTextBoxValue(TextBox textbox, string Value)
		{
			textbox.Text = Value;
		}
	}
}
