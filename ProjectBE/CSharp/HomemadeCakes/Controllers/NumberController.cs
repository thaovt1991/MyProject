using HomemadeCakes.Service.NumberService;
using Microsoft.AspNetCore.Mvc;

namespace HomemadeCakes.Controllers
{
    public class NumberController : Controller
    {
        private readonly NumberService _numberService;

        public NumberController(NumberService numberService)
        {
            _numberService = numberService;
        }
    }
}
