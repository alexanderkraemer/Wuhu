using System.Collections.Generic;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface IPresenceDao
	{
        Presence FindById(int player_id, int day_id);
        IList<Presence> FindPresenceByDay(int day_id);
        IList<Presence> FindPresenceByPlayer(int player_id);
        IList<Presence> FindAll();
        bool DeleteById(int player_id, int day_id);
        bool DeleteAll();
        int Insert(Presence presence);
        bool Update(Presence oldDay, Presence newDay);
    }
}
