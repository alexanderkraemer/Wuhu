using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WuHu.Terminal.Views.Tournaments
{
	/// <summary>
	/// Interaction logic for TournamentList.xaml
	/// </summary>
	public partial class TournamentList : UserControl
	{
		public TournamentList()
		{
			InitializeComponent();
			this.DataContext = TournamentListVM.getInstance();
			
			activePlayerList.SelectionChanged += ActivePlayerClick;
		}

		public void CreateTournamentButtonClick(object sender, object e)
		{
			MainWindow.main.Content = new TournamentCreate();
		}

		public void ActivePlayerClick(object sender, SelectionChangedEventArgs e)
		{
			foreach (object obj in e.AddedItems)
			{
				((TournamentListVM)DataContext).activePlayerSelectedCommand.Execute(obj);
			}
		}
	}
}
