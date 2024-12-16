using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Data.Common;
using System.Data;
using WebApplication2.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using WebApplication2.Token;
using System.Reflection;

namespace WebApplication2.Controllers
{

    [ApiExplorerSettings(GroupName = "v1")]
    [Route("values")]  //路由配置
    [ApiController]  //声明api的特性
    [Authorize]
    [EnableCors("any")]
    [AllowAnonymous]//取消授权  绕过身份验证
    public class loginController : ControllerBase
    {
        private readonly IFreeSql db;
        private readonly IAuthenticateServices auth;
        public loginController(IFreeSql _db, IAuthenticateServices _auth)
        {
            db = _db;
            this.auth = _auth;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="jobname"></param>
        /// <param name="jobaslary"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("login")]
        [AllowAnonymous]//取消授权
        public async Task<object> login(string name, string password)
        {
            Model.USERS list = await db.Select<Model.USERS>().Where(e => e.USERNAME == name && e.PASSWORD == password).FirstAsync();
            if (list != null)
            {
                string token;
                if (auth.IsAuthenticated(list, out token))
                {
                    // 判断用户角色
                    string role = list.ROLE;
                    var result = new
                    {
                        token = token,
                        role = role
                    };
                    return JsonConvert.SerializeObject(result);
                }
                else
                {
                    return JsonConvert.SerializeObject(1);
                }

            }
            return JsonConvert.SerializeObject(false);
        }

        /// <summary>
        /// 获取销售订单信息
        /// </summary>
        /// <param name="name"></param>       
        /// <returns></returns>
        [HttpGet]
        [Route("GetName")]
        public async Task<object> Get(string? ORDERNUMBER = "", string? CUSTOMERNAME = "", string? APPLICATIONSTATUS = "全部", string? REVIEWSTATUS = "全部")
        {
            var list = db.Select<SALESORDERS, CUSTOMERS>()
                .LeftJoin((a, b) => a.CUSTOMERID == b.CUSTOMERID)
                .Where((a, b) =>
                 ((ORDERNUMBER == "") || (a.ORDERNUMBER.Contains(ORDERNUMBER))) &&
                  ((CUSTOMERNAME == "") || (b.CUSTOMERNAME.Contains(CUSTOMERNAME))) &&
                  ((APPLICATIONSTATUS == "全部")) || (a.APPLICATIONSTATUS == APPLICATIONSTATUS) &&
                   ((REVIEWSTATUS == "全部")) || (a.REVIEWSTATUS == REVIEWSTATUS)
             ).ToList((a, b) => new
             {
                 a.ORDERID,
                 a.ORDERNUMBER,
                 b.CUSTOMERNAME,
                 a.CONTACTPERSON,
                 a.CONTACTPHONE,
                 a.TOTALAMOUNT,
                 a.DELIVERYDATE,
                 a.USERNAME,
                 a.APPLICATIONTIME,
                 a.APPLICATIONSTATUS,
                 a.REVIEWER,
                 a.REVIEWTIME,
                 a.REVIEWSTATUS
             });
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser")]
        public async Task<object> DeleteUser(int orderid)
        {
            // 查找指定的销售订单
            var order = await db.Select<Model.SALESORDERS>().Where(so => so.ORDERID == orderid).FirstAsync();

            // 删除订单
            var deleteResult = await db.Delete<Model.SALESORDERS>().Where(so => so.ORDERID == orderid).ExecuteAffrowsAsync();
            if (deleteResult > 0)
            {
                return Ok(new { message = "销售订单删除成功" });
            }
            else
            {
                return JsonConvert.SerializeObject("订单删除失败");
            }
        }
        /// <summary>
        /// 提交授权
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="status"></param>
        /// <param name="reviewer"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateSalesOrderStatus")]
        public async Task<IActionResult> UpdateSalesOrderStatus(int orderid)
        {
            var order = await db.Select<SALESORDERS>().Where(s => s.ORDERID == orderid).FirstAsync();

            // 检查当前订单是否是“未提交”状态
            if (order.APPLICATIONSTATUS == "未提交")
            {
                // 更新订单审核状态
                var updateResult = await db.Update<SALESORDERS>()
                    .Set(a => a.APPLICATIONSTATUS, "待审核")
                    .Where(so => so.ORDERID == orderid)
                    .ExecuteAffrowsAsync();

                if (updateResult > 0)
                {
                    // 记录日志，返回成功消息
                    return Ok(new { message = "提交成功" });
                }
                else
                {
                    return BadRequest(new { message = "提交失败" });
                }
            }
            return BadRequest(new { message = "订单状态不符合提交条件" });
        }
        /// <summary>
        /// 提交审核 驳回
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="status"></param>
        /// <param name="reviewer"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateSalesOrderStatuss")]
        public async Task<IActionResult> UpdateSalesOrderStatuss(int orderid)
        {
            var order = await db.Select<SALESORDERS>().Where(s => s.ORDERID == orderid).FirstAsync();

            // 检查当前订单是否是“未提交”状态
            if (order.APPLICATIONSTATUS == "待审核")
            {
                // 更新订单审核状态
                var updateResult = await db.Update<SALESORDERS>()
                    .Set(b => b.APPLICATIONSTATUS, "已审核")
                    .Set(a => a.REVIEWSTATUS, "驳回")
                    .Where(so => so.ORDERID == orderid)
                    .ExecuteAffrowsAsync();

                if (updateResult > 0)
                {
                    // 记录日志，返回成功消息
                    return Ok(new { message = "审核成功" });
                }
                else
                {
                    return BadRequest(new { message = "审核失败" });
                }
            }
            return BadRequest(new { message = "订单状态不符合提交条件" });
        }
        /// <summary>
        /// 提交审核 通过
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateSalesOrderStatusss")]
        public async Task<IActionResult> UpdateSalesOrderStatusss(int orderid)
        {
            var order = await db.Select<SALESORDERS>().Where(s => s.ORDERID == orderid).FirstAsync();

            // 检查当前订单是否是“未提交”状态
            if (order.APPLICATIONSTATUS == "待审核")
            {
                // 更新订单审核状态
                var updateResult = await db.Update<SALESORDERS>()
                    .Set(b => b.APPLICATIONSTATUS, "已审核")
                    .Set(a => a.REVIEWSTATUS, "通过")
                    .Where(so => so.ORDERID == orderid)
                    .ExecuteAffrowsAsync();

                if (updateResult > 0)
                {
                    // 记录日志，返回成功消息
                    return Ok(new { message = "审核成功" });
                }
                else
                {
                    return BadRequest(new { message = "审核失败" });
                }
            }
            return BadRequest(new { message = "订单状态不符合提交条件" });
        }
        /// <summary>
        /// 根据用户id查看自己的订单
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getidOrderStatus")]
        public async Task<object> getidOrderStatus(string uname)
        {
            var list = db.Select<SALESORDERS, CUSTOMERS>()
                .LeftJoin((a,b)=>a.CUSTOMERID == b.CUSTOMERID).Where((a,b)=>a.USERNAME == uname).ToList((a, b) => new
                {
                    a.ORDERID,
                    a.ORDERNUMBER,
                    b.CUSTOMERNAME,
                    a.CONTACTPERSON,
                    a.CONTACTPHONE,
                    a.TOTALAMOUNT,
                    a.DELIVERYDATE,
                    a.USERNAME,
                    a.APPLICATIONTIME,
                    a.APPLICATIONSTATUS,
                    a.REVIEWER,
                    a.REVIEWTIME,
                    a.REVIEWSTATUS
                });
           return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllPresnent")]
        public Object GetAllPresnent()
        {
            var list = db.Select<Model.CUSTOMERS>()
                .ToList();
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
		/// 物料档案表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        [Route("GetMaterials")]

        public Object GetMaterials()
        {
            var list = db.Select<Model.MATERIALS>()
                .ToList();
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
		/// 添加
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
        [Route("CreateSalesOrder")]
        [EnableCors("any")]

        public async Task<Object> CreateSalesOrder([FromBody] OrderCreateRequest? request)
        {

            // 生成订单编号
            string orderNumber = $"SO{DateTime.Now:yyyyMMddHHmmss}";

            // 创建销售订单主表记录
            var salesOrder = new SALESORDERS
            {
                ORDERNUMBER = orderNumber,
                CUSTOMERID = request.SalesOrder.CUSTOMERID,
                DELIVERYDATE = request.SalesOrder.DELIVERYDATE,  
                //USERID = request.SalesOrder.USERID,
                TOTALAMOUNT = request.OrderDetails.Sum(d => d.QUANTITY * d.AMOUNT),
                USERNAME = request.SalesOrder.USERNAME,
                UPDATEDATE = request.SalesOrder.UPDATEDATE,
                APPLICATIONTIME = request.SalesOrder.APPLICATIONTIME,
                CONTACTPERSON = request.SalesOrder.CONTACTPERSON,
                CONTACTPHONE = request.SalesOrder.CONTACTPHONE,
                REVIEWER = request.SalesOrder.REVIEWER,
                REVIEWSTATUS = request.SalesOrder.REVIEWSTATUS,
                REVIEWTIME = request.SalesOrder.REVIEWTIME,
                APPLICATIONSTATUS = "未提交"
            };

            // 插入主表数据
            //await db.Insert<SALESORDERS>().AppendData(salesOrder).ExecuteAffrowsAsync();
            await db.Insert<SALESORDERS>().AppendData(salesOrder).ExecuteAffrowsAsync();

            // 获取刚插入的订单ID
            var newOrder = await db.Select<SALESORDERS>()
                .Where(o => o.ORDERNUMBER == orderNumber)
                .FirstAsync();

            // 为每个明细设置订单ID并插入
            foreach (var detail in request.OrderDetails)
            {
                detail.ORDERID = newOrder.ORDERID;
                detail.AMOUNT = detail.QUANTITY * detail.AMOUNT;  
            }

            // 插入明细表数据
            await db.Insert<SALESORDERDETAILS>().AppendData(request.OrderDetails).ExecuteAffrowsAsync();

            var result = new
            {
                code = 0,
                msg = "创建成功",
                data = new { orderId = salesOrder.ORDERID }
            };
            return JsonConvert.SerializeObject(result);
        }

        // 用于接收前端请求的包装类
        public class OrderCreateRequest
        {
            public List<SALESORDERDETAILS> OrderDetails { get; set; }
            public SALESORDERS SalesOrder { get; set; }
        }


    }
}





