using HomemadeCakes.Model.NumberModels;
using System.Threading.Tasks;

namespace HomemadeCakes.Service.NumberService
{
    public interface INumberService
    {
        Task<int> AddNumber(Number number);
    }
}
