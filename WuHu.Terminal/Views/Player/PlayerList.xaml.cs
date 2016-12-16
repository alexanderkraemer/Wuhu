using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static WuHu.Terminal.MainWindow;

namespace WuHu.Terminal.Views.Player
{
	/// <summary>
	/// Interaction logic for PlayerList.xaml
	/// </summary>
	public partial class PlayerList : UserControl
	{
		PlayerVM player = null;

		public string Header { get; internal set; }

		public PlayerList()
		{
			InitializeComponent();
			this.DataContext = new PlayerListVM();
		}

		public void PlayerListSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox list = sender as ListBox;
			PlayerVM player = (PlayerVM) list.SelectedValue;

			this.player = player;
		}

		public void DeletePlayerButtonClick(object sender, object e)
		{
			
		}

		private void EditPlayerButtonClick(object sender, RoutedEventArgs e)
		{
			MainWindow.main.Content = new PlayerEdit(this.player);
		}
	}
}
