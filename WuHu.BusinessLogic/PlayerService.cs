using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WuHu.Common;
using WuHu.Domain;
using WuHu.SQLServer;

namespace WuHu.BusinessLogic
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
	public class PlayerService : IPlayerService
	{
		public PlayerDC generatePlayerDC(Player player)
		{
			PlayerDC p = new PlayerDC();
			p.ID = player.ID;
			p.RoleID = player.RoleID;
			p.FirstName = player.FirstName;
			p.LastName = player.LastName;
			p.Nickname = player.Nickname;
			p.Skills = player.Skills;
			p.Password = player.Password;

			return p;
		}

		public IList<PlayerDC> GetPlayerList()
		{
			IList<PlayerDC> list = new List<PlayerDC>();

			IDatabase database = DalFactory.CreateDatabase();
			PlayerDao dao = new PlayerDao(database);

			foreach(Player p in dao.FindAll())
			{
				list.Add(generatePlayerDC(p));
			}
			return list;
		}
	}
}
