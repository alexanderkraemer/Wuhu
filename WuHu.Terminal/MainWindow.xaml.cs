using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WuHu.Terminal.ViewModels;
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

			main.Content = Home.getInstance();
			HomeButton.Background = Brushes.SteelBlue;
		}

		private void ChangeButtonColor(Button btn)
		{
			Button[] array = { HomeButton, PlayerButton, MatchesButton, TournamentsButton };
			foreach (Button b in array)
			{
				if (b == btn)
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
			if (TournamentListVM.getInstance().IsEditing)
			{
				TournamentListVM.getInstance().UnlockTournament();
			}
			ChangeButtonColor(HomeButton);
			MainWindow.main.Content = Home.getInstance();
		}

		public void NavPlayerButton(object sender, object e)
		{
			if (TournamentListVM.getInstance().IsEditing)
			{
				TournamentListVM.getInstance().UnlockTournament();
			}
			ChangeButtonColor(PlayerButton);
			
			Authentication.CheckIfLoggedIn(new PlayerList());
		}

		public void NavMatchesButton(object sender, object e)
		{
			if (TournamentListVM.getInstance().IsEditing)
			{
				TournamentListVM.getInstance().UnlockTournament();
			}

			ChangeButtonColor(MatchesButton);
			Authentication.CheckIfLoggedIn(new MatchList());
		}

		public void NavTournamentsButton(object sender, object e)
		{
			Authentication.CheckIfLoggedIn(new TournamentList());
			if(Authentication.isAuthenticated)
			{
				TournamentListVM.getInstance().LockTournament();
			}
			ChangeButtonColor(TournamentsButton);
			
		}
	}
}
