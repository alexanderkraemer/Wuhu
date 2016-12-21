﻿using System;
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

namespace WuHu.Terminal.Views.Tournaments
{
	/// <summary>
	/// Interaction logic for TournamentCreate.xaml
	/// </summary>
	public partial class TournamentCreate : UserControl
	{
		public TournamentCreate()
		{
			InitializeComponent();
			this.DataContext = new TournamentVM();
		}
	}
}
