using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Domain;
using WuHu.SQLServer;

namespace WuHu.BusinessLogic
{
    public class BLPlayer
    {
		private IDatabase database = null;
		public BLPlayer()
		{
			if(database == null)
			{
				database = DalFactory.CreateDatabase();
			}
		}

		public IList<Player> GetPlayerList()
		{
			IList<Player> list = new List<Player>();

			PlayerDao pd = new PlayerDao(database);

			foreach(Player p in pd.FindAll())
			{
				list.Add(p);
			}
			return list;
		}

    }
}
