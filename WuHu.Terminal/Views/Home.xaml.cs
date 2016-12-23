using Syncfusion.Windows.Chart;
using System.Windows.Controls;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views
{
	/// <summary>
	/// Interaction logic for Home.xaml
	/// </summary>
	public partial class Home : UserControl
	{
		public Home()
		{
			InitializeComponent();
			this.DataContext = HomeVM.getInstance();
		}
	}
}
