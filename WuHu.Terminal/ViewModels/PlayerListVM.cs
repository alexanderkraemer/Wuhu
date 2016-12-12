using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.BusinessLogic;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
	public class PlayerListVM
	{
		private IList<PlayerVM> list;

		public PlayerListVM()
		{
			list = new List<PlayerVM>();
			LoadPlayers();
		}

		ObservableCollection<PlayerVM> playerData = new ObservableCollection<PlayerVM>();
		public ObservableCollection<PlayerVM> PlayerData
		{
			get
			{
				if (playerData.Count <= 0)
				{
					foreach (PlayerVM p in list)
					{
						playerData.Add(p);
					}
				}
				return playerData;
			}
		}

		public void LoadPlayers()
		{
			BLPlayer blp = new BLPlayer();
			foreach (Player p in blp.GetPlayerList())
			{
				list.Add(new PlayerVM(p));
			}
		}
	}
}
