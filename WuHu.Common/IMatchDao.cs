using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface IMatchDao
	{
        Match FindById(int id);
        IList<Match> FindAll();
        bool DeleteById(int id);
        bool DeleteAll();
        int Insert(Match match);
        bool Update(Match match);
    }
}
