using HomemadeCakes.ModelView;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HomemadeCakes.Business
{
    public class UserLogBusiness
    {
       public async Task<object> LoginAsync(string userID , string pass)
        {
            var userLog = new LoginRequest();
            return userLog;
        }
    }
}
