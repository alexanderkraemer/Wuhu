using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

		private void ChangeButtonColor(Button btn)
		{
			Button[] array = { HomeButton, PlayerButton, MatchesButton, TournamentsButton };
			foreach(Button b in array)
			{
				if(b == btn)
				{
					b.Background = Brushes.SteelBlue;
				}
				else
				{
					b.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD"));
				}
			}
			
		}

		public void NavHomeButton(object sender, object e)
		{
			ChangeButtonColor(HomeButton);

			MainWindow.main.Content = new Home();
		}

		public void NavPlayerButton(object sender, object e)
		{
			ChangeButtonColor(PlayerButton);
			MainWindow.main.Content = new PlayerList();
		}

		public void NavMatchesButton(object sender, object e)
		{
			ChangeButtonColor(MatchesButton);
			MainWindow.main.Content = new MatchList();
		}

		public void NavTournamentsButton(object sender, object e)
		{
			ChangeButtonColor(TournamentsButton);
			MainWindow.main.Content = new TournamentList();
		}
	}
}
