using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.BusinessLogic
{
	public class BLTournaments
	{
		private static bool isLocked = false;

		public static bool IsLocked
		{
			get { return isLocked; }
			set { isLocked = value;  }
		}
	}
}
