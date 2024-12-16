using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace WebApplication2.Model {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class CUSTOMERS {

		[JsonProperty, Column(Name = "CUSTOMER_ID", DbType = "NUMBER(22)", IsPrimary = true)]
		public decimal CUSTOMERID { get; set; }

		[JsonProperty, Column(Name = "COMPANY_ADDRESS", DbType = "VARCHAR2(200 BYTE)")]
		public string COMPANYADDRESS { get; set; }

		[JsonProperty, Column(Name = "CONTACT_PERSON", DbType = "VARCHAR2(50 BYTE)")]
		public string CONTACTPERSON { get; set; }

		[JsonProperty, Column(Name = "CONTACT_PHONE", DbType = "VARCHAR2(20 BYTE)")]
		public string CONTACTPHONE { get; set; }

		[JsonProperty, Column(Name = "CREDIT_RATING", DbType = "VARCHAR2(20 BYTE)")]
		public string CREDITRATING { get; set; }

		[JsonProperty, Column(Name = "CUSTOMER_NAME", DbType = "VARCHAR2(100 BYTE)", IsNullable = false)]
		public string CUSTOMERNAME { get; set; }

		[JsonProperty, Column(Name = "CUSTOMER_TYPE", DbType = "VARCHAR2(20 BYTE)")]
		public string CUSTOMERTYPE { get; set; }

	}

}
