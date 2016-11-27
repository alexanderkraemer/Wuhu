using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[Serializable]
	public class Role
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public Role(int ID, string Name)
		{
			this.ID = ID;
			this.Name = Name;
		}

		public Role(string Name)
		{
			this.Name = Name;
		}
	}
}
