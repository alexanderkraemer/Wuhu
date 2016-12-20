using System.Windows;
using System.Windows.Controls;
using WuHu.Terminal.Views;
using WuHu.Terminal.Views.Matches;
using WuHu.Terminal.Views.Player;
using WuHu.Terminal.Views.Tournaments;

namespace WuHu.Terminal
{


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static Frame main;
		public MainWindow()
		{
			InitializeComponent();

			main = MainFrame;

			main.Content = new Home();
		}

		public void NavHomeButton(object sender, object e)
		{
			MainWindow.main.Content = new Home();
		}

		public void NavPlayerButton(object sender, object e)
		{
			MainWindow.main.Content = new PlayerList();
		}

		public void NavMatchesButton(object sender, object e)
		{
			MainWindow.main.Content = new MatchList();
		}

		public void NavTournamentsButton(object sender, object e)
		{
			MainWindow.main.Content = new TournamentList();
		}
	}
}
