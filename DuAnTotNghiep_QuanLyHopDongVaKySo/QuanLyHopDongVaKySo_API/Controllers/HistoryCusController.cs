using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.OperationHistoryCusService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryCusController : ControllerBase
    {
        private readonly IOperationHistoryCusSvc _operationHistoryCusSvc;

        public HistoryCusController(IOperationHistoryCusSvc operationHistoryCusSvc)
        {
            _operationHistoryCusSvc = operationHistoryCusSvc;
        }

    }
}
