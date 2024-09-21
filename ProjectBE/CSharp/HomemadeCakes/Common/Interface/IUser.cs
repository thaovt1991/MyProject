using System;

namespace HomemadeCakes.Common.Interface
{
    public interface IUser
    {
        string UserID { get; set; }

        string BUID { get; set; }

        string Language { get; set; }

        string Theme { get; set; }

        string DefaultHome { get; set; }

        string ConnectionName { get; set; }

        string Tenant { get; set; }

        string Domain { get; set; }

        string SecurityKey { get; set; }

        string Token { get; set; }

        string RefreshToken { get; set; }

        DateTime ExpireOn { get; set; }

        int SessionTimeout { get; set; }
    }
}
