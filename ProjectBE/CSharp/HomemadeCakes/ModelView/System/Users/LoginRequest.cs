using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.ModelView
{
    public class LoginRequest
    {
        public string UserID { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
