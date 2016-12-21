using System;
using System.Collections.Generic;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface ITournamentDao
	{
		Tournament FindById(int id);
		Tournament FindByDay(DateTime day);
		IList<Tournament> FindAll();
		bool DeleteById(int id);
		bool DeleteAll();
		int Insert(Tournament team);
		bool Update(Tournament team);
	}
}
