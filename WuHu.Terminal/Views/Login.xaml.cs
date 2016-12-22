using System.Windows.Controls;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : UserControl
	{
		public Login()
		{
			InitializeComponent();
			this.DataContext = new LoginVM();
		}
	}
}
