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
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views.Matches
{
	/// <summary>
	/// Interaction logic for PlayerEdit.xaml
	/// </summary>
	public partial class MatchEdit : UserControl
	{
		public MatchEdit()
		{
			InitializeComponent();
			this.DataContext = MatchListVM.getInstance().CurrentMatch;
		}
	}
}
