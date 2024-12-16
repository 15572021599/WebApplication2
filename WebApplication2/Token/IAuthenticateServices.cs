using System.Reflection;
using WebApplication2.Model;

namespace WebApplication2.Token
{
    public interface IAuthenticateServices
    {
        //你需要操作的类  登录的类
        bool IsAuthenticated(Model.USERS request, out string token);

    }
}
