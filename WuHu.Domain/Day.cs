using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[Serializable]
	public class Day
	{
		public int ID { get; }
		public string Name { get; set; }

	
		public Day(int ID, string Name)
		{
			this.ID = ID;
			this.Name = Name;
		}

		public Day (string Name)
		{
			this.Name = Name;
		}

		public new string ToString()
		{
			return "Day " + this.ID + ": " + this.Name;
		}
	}
}