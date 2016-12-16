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
	public partial class TeamEdit : UserControl
	{
		public TeamEdit()
		{
			InitializeComponent();
		}

		public void SaveTeamButtonClick(object sender, object e)
		{
			MainWindow.main.Content = new TeamList();
		}
		public void CancelTeamButtonClick(object sender, object e)
		{

		}
	}
}
