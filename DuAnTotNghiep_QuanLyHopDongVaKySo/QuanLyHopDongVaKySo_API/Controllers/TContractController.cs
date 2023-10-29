
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Helpers;
namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TContractController:ControllerBase
    {
        private readonly ITemplateContractSvc _TContractSvc;
         private readonly IUploadFileHelper _helpers;
        public TContractController(ITemplateContractSvc TContractSvc,IUploadFileHelper helpers)
        {
            _TContractSvc = TContractSvc;
            _helpers = helpers;
        }

        /// <summary>
        /// Lấy danh sách toàn bộ mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTContractsAsnyc()
        {
            return Ok(new
                {
                    retText = "Lấy danh sách mẫu hợp đồng thành công",
                    data = await _TContractSvc.getAllAsnyc()
                }
            );
        }

        /// <summary>
        /// Lấy mẫu hợp đồng được chọn theo id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTContractAsnyc(int id)
        {
            return Ok(new{
                retText = "Lấy hợp đồng mẫu thành công",
                data = await _TContractSvc.getByIdAsnyc(id)
            });
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="tContract"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTContractAsnyc([FromForm] PostTContract tContract)
        {
             if(ModelState.IsValid)
             {
                int id_Tcontract = await _TContractSvc.addAsnyc(tContract);
                if(id_Tcontract > 0)
                {
                    if(tContract.File != null)
                    {
                        _helpers.UploadFile(tContract.File,"AppData","TContracts");
                        return Ok (new{
                        retText = "Thêm mẫu hợp đồng thành công",
                        data = await _TContractSvc.getByIdAsnyc(id_Tcontract)
                        });
                    }
                }
             }
            return Ok(new {
                retText = "dữ liệu không hợp lệ",
                data = ""
            });
        }

        /// <summary>
        /// Sửa mẫu hợp đồng
        /// </summary>
        /// <param name="tContract"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTContractAsnyc([FromForm] PutTContract tContract)
        {
             if(ModelState.IsValid)
             {
                int id_Tcontract = await _TContractSvc.updateAsnyc(tContract);
                if(id_Tcontract > 0)
                {
                    if(tContract.File != null)
                    {
                        _helpers.UploadFile(tContract.File,"AppData","TContracts");
                        return Ok (new{
                            retText = "sửa mẫu hợp đồng thành công",
                            data = await _TContractSvc.getByIdAsnyc(id_Tcontract)
                        });
                    }
                }
             }
            return Ok(new {
                retText = "dữ liệu không hợp lệ",
                data = ""
            });
        }
    }
}