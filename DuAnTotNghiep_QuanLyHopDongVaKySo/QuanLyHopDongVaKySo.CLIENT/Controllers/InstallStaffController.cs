using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using System.Drawing.Imaging;
using test.Models;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class InstallStaffController : Controller
    {
        private readonly IIRequirementService _iRequirementService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        public InstallStaffController(IIRequirementService iRequirementService , IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor) {
            _iRequirementService = iRequirementService;
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListInstallRequire()
        {
            return View();
        }
        public IActionResult DetailsRequire()
        {
            return View();
        }
        public IActionResult ListDevice()
        {
            return View();
        }
        public IActionResult DetailsDevice()
        {
            return View();
        }
        public IActionResult PersonalView()
        {
            return View();
        }
       
        public IActionResult DetailsInstall()
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
        [HttpPost]
        public ActionResult SaveSignature([FromBody] SignData sData)
        {
            if (null == sData)
                return NotFound();

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid() + ".png";

            var filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "SignatureImages"), fileName);


            bmpSign.Save(filePath, ImageFormat.Png);

            return Content(fileName);
        }
    }
}
