
using HomemadeCakes.Model.Common;
using HomemadeCakes.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Service
{
    public interface IUserService
    {
        Task<object> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
    }
}
