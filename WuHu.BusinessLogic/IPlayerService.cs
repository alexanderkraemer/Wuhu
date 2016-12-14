using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WuHu.Domain;

namespace WuHu.BusinessLogic
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IPlayerService
	{
		PlayerDC generatePlayerDC(Player player);

		[OperationContract]
		IList<PlayerDC> GetPlayerList();

		// TODO: Add your service operations here
	}

	// Use a data contract as illustrated in the sample below to add composite types to service operations.
	// You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "WuHu.BusinessLogic.ContractType".
	[DataContract]
	public class PlayerDC
	{

		[DataMember]
		public int ID { get; set; }

		[DataMember]
		public int RoleID { get; set; }

		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		[DataMember]
		public string Nickname { get; set; }

		[DataMember]
		public int Skills { get; set; }

		[DataMember]
		public string PhotoPath { get; set; }

		[DataMember]
		public string Password { get; set; }


		public string FullName
		{
			get { return FirstName + " " + LastName; }
		}
	}
}
