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

#region  ע������

Func<IServiceProvider, IFreeSql> fsqlFactory = r =>
{
    IFreeSql fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.Oracle, @"user id=sales;password=123456; data source=//127.0.0.1:1521/orcl;Pooling=true;Min Pool Size=1")
        .UseMonitorCommand(cmd => Console.WriteLine($"Sql��{cmd.CommandText}"))
        .UseAutoSyncStructure(true) //�Զ�ͬ��ʵ��ṹ�����ݿ⣬ֻ��CRUDʱ�Ż����ɱ�
        .Build();
    return fsql;
};
builder.Services.AddSingleton<IFreeSql>(fsqlFactory);
#endregion 

#region ����
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
//����Swagger
//ע��Swagger������������һ��Swagger �ĵ�
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "�����ĵ�",
        Description = "RESTful API"
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "��ȫ���ĵ�",
        Description = "RESTful API"
    });
    c.SwaggerDoc("v3", new OpenApiInfo
    {
        Version = "v3",
        Title = "���ĵ�",
        Description = "RESTful API"
    });
    //// Ϊ Swagger ����xml�ĵ�ע��·��
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
#endregion


#region  Token�����֤
builder.Services.Configure<TokenManagement>(builder.Configuration.GetSection("tokenManagement"));
var token = builder.Configuration.GetSection("tokenManagement").Get<TokenManagement>();   //��ȡappsettings.json��token�����Ϣ

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
//�����м����������Swagger��ָ��Swagger JSON�ս��
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "���� V1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "��ȫ��V1");
    c.SwaggerEndpoint("/swagger/v3/swagger.json", "��V1");

    // c.RoutePrefix = string.Empty;//���ø��ڵ����
});

app.UseHttpsRedirection();

app.UseAuthentication();//��Ȩ

app.UseRouting(); //��Ҫ����·���м����������治�ܵߵ�˳��

app.UseCors("any");			//����Cors�м�����Ҳ�����������ǰ�涨������Ʊ���һ��	

app.UseAuthorization();   //�����֤ 

app.MapControllers();

app.Run();
