using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace WebApplication2.Model {

	[JsonObject(MemberSerialization.OptIn), Table(Name = "SALES_ORDER_DETAILS", DisableSyncStructure = true)]
	public partial class SALESORDERDETAILS {

		[JsonProperty, Column(Name = "DETAIL_ID", DbType = "NUMBER(22)", IsPrimary = true)]
		public decimal DETAILID { get; set; }

		[JsonProperty, Column(DbType = "NUMBER(22)")]
		public decimal? AMOUNT { get; set; }

		[JsonProperty, Column(Name = "CREATE_DATE", DbType = "DATE(7)")]
		public DateTime? CREATEDATE { get; set; }

		[JsonProperty, Column(Name = "MATERIAL_ID", DbType = "NUMBER(22)")]
		public decimal MATERIALID { get; set; }

		[JsonProperty, Column(Name = "ORDER_ID", DbType = "NUMBER(22)")]
		public decimal ORDERID { get; set; }

		[JsonProperty, Column(DbType = "NUMBER(22)")]
		public decimal? QUANTITY { get; set; }

		[JsonProperty, Column(Name = "UPDATE_DATE", DbType = "DATE(7)")]
		public DateTime? UPDATEDATE { get; set; }

	}

}
