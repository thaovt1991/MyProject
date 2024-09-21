using System.Security.Principal;

namespace HomemadeCakes.DAL.Interface
{
    public interface IDALPrincipal : IPrincipal
    {
        PJRequestContext RequestContext { get; }
    }
}
