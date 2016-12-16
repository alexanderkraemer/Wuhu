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

namespace WuHu.Terminal.Views.Matches
{
	/// <summary>
	/// Interaction logic for PlayerList.xaml
	/// </summary>
	public partial class MatchList : UserControl
	{
		public MatchList()
		{
			InitializeComponent();
			// this.DataContext = new PlayerListVM();
		}

		public void MatchesListSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox list = sender as ListBox;
			//PlayerDC player = (PlayerDC) list.SelectedValue;

			//this.player = player;
		}

		public void DeleteMatchButtonClick(object sender, object e)
		{

		}

		private void EditMatchButtonClick(object sender, RoutedEventArgs e)
		{
			main.Content = new MatchEdit();
		}
	}
}
