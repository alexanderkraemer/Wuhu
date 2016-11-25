using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface IPlayerDao
	{
		Player FindById(int id);
		IList<Player> FindAll();
        bool DeleteById(int id);
        bool DeleteAll();
        int Insert(Player player);
		bool Update(Player player);
        

        Player FindByNickname(string nickname);
    }
}
