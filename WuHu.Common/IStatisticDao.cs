using System;
using System.Collections.Generic;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface IStatisticDao
	{
        Statistic FindById(int id);
        Statistic FindByPlayer(int player_id);
        Statistic FindByDay(DateTime date);
        IList<Statistic> FindAll();
        bool DeleteById(int id);
        bool DeleteAll();
        int Insert(Statistic statistic);
        bool Update(Statistic statistic);
    }
}
