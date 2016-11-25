using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface IDayDao
	{
        Day FindById(int id);
        Day FindByDayname(string name);
        IList<Day> FindAll();
        bool DeleteById(int id);
        bool DeleteAll();
        int Insert(Day day);
        bool Update(Day day);
    }
}
