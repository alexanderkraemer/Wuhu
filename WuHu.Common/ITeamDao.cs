using System.Collections.Generic;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface ITeamDao
	{
        Team FindById(int id);
        IList<Team> FindAll();
        bool DeleteById(int id);
        bool DeleteAll();
        int Insert(Team team);
        bool Update(Team team);
    }
}
