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
	public partial class MATERIALS {

		[JsonProperty, Column(Name = "MATERIAL_ID", DbType = "NUMBER(22)", IsPrimary = true)]
		public decimal MATERIALID { get; set; }

		[JsonProperty, Column(Name = "CREATE_DATE", DbType = "DATE(7)")]
		public DateTime? CREATEDATE { get; set; }

		[JsonProperty, Column(Name = "MATERIAL_CODE", DbType = "VARCHAR2(50 BYTE)", IsNullable = false)]
		public string MATERIALCODE { get; set; }

		[JsonProperty, Column(Name = "MATERIAL_NAME", DbType = "VARCHAR2(100 BYTE)", IsNullable = false)]
		public string MATERIALNAME { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR2(50 BYTE)")]
		public string TYPE { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR2(20 BYTE)")]
		public string UNIT { get; set; }

		[JsonProperty, Column(Name = "UNIT_PRICE")]
		public decimal? UNITPRICE { get; set; }

		[JsonProperty, Column(Name = "UPDATE_DATE", DbType = "DATE(7)")]
		public DateTime? UPDATEDATE { get; set; }

	}

}
