using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace WebApplication2.Model {

	[JsonObject(MemberSerialization.OptIn), Table(Name = "SALES_ORDERS", DisableSyncStructure = true)]
	public partial class SALESORDERS {

		[JsonProperty, Column(Name = "ORDER_ID", DbType = "NUMBER(22)", IsPrimary = true)]
		public decimal ORDERID { get; set; }

		[JsonProperty, Column(Name = "APPLICATION_STATUS", DbType = "VARCHAR2(20 BYTE)")]
		public string APPLICATIONSTATUS { get; set; }

		[JsonProperty, Column(Name = "APPLICATION_TIME", DbType = "DATE(7)")]
		public DateTime? APPLICATIONTIME { get; set; }

		[JsonProperty, Column(Name = "CONTACT_PERSON", DbType = "VARCHAR2(50 BYTE)")]
		public string CONTACTPERSON { get; set; }

		[JsonProperty, Column(Name = "CONTACT_PHONE", DbType = "VARCHAR2(20 BYTE)")]
		public string CONTACTPHONE { get; set; }

		[JsonProperty, Column(Name = "CUSTOMER_ID", DbType = "NUMBER(22)")]
		public decimal CUSTOMERID { get; set; }

		[JsonProperty, Column(Name = "DELIVERY_DATE", DbType = "DATE(7)")]
		public DateTime? DELIVERYDATE { get; set; }

		[JsonProperty, Column(Name = "ORDER_NUMBER", DbType = "VARCHAR2(50 BYTE)", IsNullable = false)]
		public string ORDERNUMBER { get; set; }

		[JsonProperty, Column(Name = "REVIEW_STATUS", DbType = "VARCHAR2(20 BYTE)")]
		public string REVIEWSTATUS { get; set; }

		[JsonProperty, Column(Name = "REVIEW_TIME", DbType = "DATE(7)")]
		public DateTime? REVIEWTIME { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR2(50 BYTE)")]
		public string REVIEWER { get; set; }

		[JsonProperty, Column(Name = "TOTAL_AMOUNT", DbType = "NUMBER(22)")]
		public decimal? TOTALAMOUNT { get; set; }

		[JsonProperty, Column(Name = "UPDATE_DATE", DbType = "DATE(7)")]
		public DateTime? UPDATEDATE { get; set; }

		[JsonProperty, Column(Name = "USER_ID", DbType = "NUMBER(22)")]
		public decimal USERID { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR2(50 BYTE)", IsNullable = false)]
		public string USERNAME { get; set; }

	}

}
