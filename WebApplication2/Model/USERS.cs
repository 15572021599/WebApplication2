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
	public partial class USERS {

		[JsonProperty, Column(Name = "USER_ID", DbType = "NUMBER(22)", IsPrimary = true)]
		public decimal USERID { get; set; }

		[JsonProperty, Column(Name = "CREATE_DATE", DbType = "DATE(7)")]
		public DateTime? CREATEDATE { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR2(100 BYTE)", IsNullable = false)]
		public string PASSWORD { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR2(20 BYTE)", IsNullable = false)]
		public string ROLE { get; set; }

		[JsonProperty, Column(Name = "UPDATE_DATE", DbType = "DATE(7)")]
		public DateTime? UPDATEDATE { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR2(50 BYTE)", IsNullable = false)]
		public string USERNAME { get; set; }

	}

}
