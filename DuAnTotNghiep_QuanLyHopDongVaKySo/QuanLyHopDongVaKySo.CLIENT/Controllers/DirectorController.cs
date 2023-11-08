using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using test.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class DirectorController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;

        public DirectorController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListContractAwait()
        {
            return View();
        }
        public IActionResult ListContractActive()
        {
            return View();
        }
        public IActionResult InforSign()
        {
            return View();
        }
        public IActionResult DetailsContractAwait()
        {
            return View();
        }
        public IActionResult DetailsApprovedContract()
        {
            return View();
        }
        public IActionResult DetailsActiveContract()
        {
            return View();
        }
        public IActionResult ListContractEffect()
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
