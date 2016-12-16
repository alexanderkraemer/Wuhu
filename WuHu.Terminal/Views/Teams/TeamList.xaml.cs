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

namespace WuHu.Terminal.Views.Teams
{
	/// <summary>
	/// Interaction logic for TeamList.xaml
	/// </summary>
	public partial class TeamList : UserControl
	{
		public TeamList()
		{
			InitializeComponent();
		}

		public void TeamListSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox list = sender as ListBox;
			//PlayerDC player = (PlayerDC) list.SelectedValue;

			//this.player = player;
		}

		public void EditTeamButtonClick(object sender, object e)
		{
			MainWindow.main.Content = new TeamEdit();
		}
		public void DeleteTeamButtonClick(object sender, object e)
		{

		}
	}
}
