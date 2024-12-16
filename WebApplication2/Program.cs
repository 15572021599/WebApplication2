using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using WebApplication2.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region  注入依赖

Func<IServiceProvider, IFreeSql> fsqlFactory = r =>
{
    IFreeSql fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.Oracle, @"user id=sales;password=123456; data source=//127.0.0.1:1521/orcl;Pooling=true;Min Pool Size=1")
        .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))
        .UseAutoSyncStructure(true) //自动同步实体结构到数据库，只有CRUD时才会生成表
        .Build();
    return fsql;
};
builder.Services.AddSingleton<IFreeSql>(fsqlFactory);
#endregion 

#region 跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("any", builder =>
    {
        builder.AllowAnyMethod()
        .SetIsOriginAllowed(_ => true)
        .AllowAnyHeader()
        .AllowCredentials();

    });
});
#endregion

#region Swagger
//配置Swagger
//注册Swagger生成器，定义一个Swagger 文档
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "毒瘤文档",
        Description = "RESTful API"
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "烤全羊文档",
        Description = "RESTful API"
    });
    c.SwaggerDoc("v3", new OpenApiInfo
    {
        Version = "v3",
        Title = "三文档",
        Description = "RESTful API"
    });
    //// 为 Swagger 设置xml文档注释路径
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
#endregion


#region  Token身份验证
builder.Services.Configure<TokenManagement>(builder.Configuration.GetSection("tokenManagement"));
var token = builder.Configuration.GetSection("tokenManagement").Get<TokenManagement>();   //获取appsettings.json中token身份信息

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
        ValidIssuer = token.Issuer,
        ValidAudience = token.Audience,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddSingleton<IAuthenticateServices, TokenAuthenticationService>();

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseSwagger();
//启用中间件服务生成Swagger，指定Swagger JSON终结点
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "毒瘤 V1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "靠全羊V1");
    c.SwaggerEndpoint("/swagger/v3/swagger.json", "三V1");

    // c.RoutePrefix = string.Empty;//设置根节点访问
});

app.UseHttpsRedirection();

app.UseAuthentication();//授权

app.UseRouting(); //需要放在路由中间件上面和下面不能颠倒顺序

app.UseCors("any");			//配置Cors中间件，且策略名称需与前面定义的名称保持一致	

app.UseAuthorization();   //身份验证 

app.MapControllers();

app.Run();
