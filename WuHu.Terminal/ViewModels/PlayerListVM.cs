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
		private IList<PlayerDC> list;

		public PlayerListVM()
		{
			list = new List<PlayerDC>();
			LoadPlayers();
		}

		ObservableCollection<PlayerDC> playerData = new ObservableCollection<PlayerDC>();
		public ObservableCollection<PlayerDC> PlayerData
		{
			get
			{
				if (playerData.Count <= 0)
				{
					foreach (PlayerDC p in list)
					{
						playerData.Add(p);
					}
				}
				return playerData;
			}
		}

		public void LoadPlayers()
		{
			PlayerReference.PlayerServiceClient client = new PlayerReference.PlayerServiceClient();

			foreach (PlayerDC p in client.GetPlayerList())
			{
				list.Add(p);
			}
		}
	}
}
