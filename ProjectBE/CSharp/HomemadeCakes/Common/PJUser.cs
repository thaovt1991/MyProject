using System.Collections.Generic;
using System;
using HomemadeCakes.Common.Interface;

namespace HomemadeCakes.Common
{

    [Serializable]
    public class PJUser : IUser
    {
        public string UserID { get; set; }

        public string BUID { get; set; }

        public string Language { get; set; }

        public string Theme { get; set; }

        public string DefaultHome { get; set; }

        public string Tenant { get; set; }

        public string Domain { get; set; }

        public string ConnectionName { get; set; }

        public string SecurityKey { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpireOn { get; set; }

        public int SessionTimeout { get; set; }

        public string Gender { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public bool Administrator { get; set; }

        public bool FunctionAdmin { get; set; }

        public bool SystemAdmin { get; set; }

        public bool CantChangePW { get; set; }

        public bool Locked { get; set; }

        public bool IsMobile { get; set; }

        public string GroupID { get; set; }

        public DateTime Logon { get; set; }

        public bool NeverExpire { get; set; }

        public Employee Employee { get; set; }

        public Dictionary<string, object> Extends { get; set; }
    }
}
