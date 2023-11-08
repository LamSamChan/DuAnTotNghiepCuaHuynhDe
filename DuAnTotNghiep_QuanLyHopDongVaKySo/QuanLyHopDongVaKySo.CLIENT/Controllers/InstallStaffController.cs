using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class InstallStaffController : Controller
    {
        private readonly IIRequirementService _iRequirementService;
        public InstallStaffController(IIRequirementService iRequirementService) {
            _iRequirementService = iRequirementService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PersonalView()
        {
            return View();
        }
        public IActionResult CompleteRecord()
        {
            return View();
        }
        public IActionResult ConfirmRecordInstall ()
        {
            return View();
        }
        public IActionResult DetailsInstallRecord()
        {
            return View();
        }
        public IActionResult DetailsInstallRecordWait()
        {
            return View();
        }
        public async Task<IActionResult> ListInstallRecord()
        {
            List<API.InstallationRequirement> ir = new List<API.InstallationRequirement>();
            try
            {
                ir= await _iRequirementService.GetAll();
                return View(ir);
            }
            catch (Exception ex)
            {

                return View(new List<API.InstallationRequirement>());
            }
        }
        public IActionResult SignByCus()
        {
            return View();

        }
        public IActionResult SignByStaff()
        {
            return View();
        }
    }
}
