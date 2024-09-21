using System.Security.Principal;
using HomemadeCakes.Common;
using HomemadeCakes.DAL.Interface;

namespace HomemadeCakes.DAL
{
    public class DALPrincipal : IDALPrincipal, IPrincipal
    {
        public PJRequestContext RequestContext { get; }

        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            return false;
        }

        public DALPrincipal()
        {
            RequestContext = new PJRequestContext();
        }

        public DALPrincipal(IPrincipal principal)
        {
            if (principal != null)
            {
                DALPrincipal dALPrincipal = PJJsonHelper.Deserializer<DALPrincipal>(PJJsonHelper.Serializer(principal));
                RequestContext = dALPrincipal.RequestContext;
                Identity = dALPrincipal.Identity;
            }

            if (RequestContext == null)
            {
                RequestContext = new PJRequestContext();
            }
        }
    }
}
