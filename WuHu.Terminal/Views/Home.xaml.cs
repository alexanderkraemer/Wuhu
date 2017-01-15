using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using WuHu.Terminal.ViewModels;
using System.Linq;

namespace WuHu.Terminal.Views
{
	/// <summary>
	/// Interaction logic for Home.xaml
	/// </summary>
	public partial class Home : UserControl
	{
		private static Home instance;

		public static Home getInstance()
		{
			if(instance == null)
			{
				instance = new Views.Home();
			}
			return instance;
		}

		private Home()
		{
			InitializeComponent();
			this.DataContext = HomeVM.getInstance();

			HomeVM.getInstance().PropertyChanged -= UpdateStatistics;
			HomeVM.getInstance().PropertyChanged += UpdateStatistics;
		}

		private void UpdateStatistics(object sender, PropertyChangedEventArgs e)
		{
			if(e.PropertyName == "StatisticList")
			{
				var list = HomeVM.getInstance().StatisticList;

				foreach(var d in list)
				{
					LineSeries ls = new LineSeries();
					ls.Title = d.player.Nickname;
					ls.DependentValuePath = "Skill";
					ls.IndependentValuePath = "Timestamp";
					ls.ItemsSource = d.statList;
					SkillChart.Series.Add(ls);
				}
			}
		}
	}
}
