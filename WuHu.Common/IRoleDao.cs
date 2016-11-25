using System.Collections.Generic;
using WuHu.Domain;

namespace WuHu.Common
{
	public interface IRoleDao
	{
        Role FindById(int id);
        Role FindByRole(string name);
        IList<Role> FindAll();
        bool DeleteById(int id);
        bool DeleteAll();
        int Insert(Role role);
        bool Update(Role role);
    }
}
