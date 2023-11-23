using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Services.OperationHistoryEmpService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryEmpController : ControllerBase
    {
        private readonly IOperationHistoryEmpSvc _operationHistoryEmpSvc;

        public HistoryEmpController(IOperationHistoryEmpSvc operationHistoryEmpSvc)
        {
            _operationHistoryEmpSvc = operationHistoryEmpSvc;
        }
    }
}
