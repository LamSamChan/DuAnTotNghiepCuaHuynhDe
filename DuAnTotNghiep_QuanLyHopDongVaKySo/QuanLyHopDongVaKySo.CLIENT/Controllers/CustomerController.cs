using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using test.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomerController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CusToSign()
        {
            return View();
        }
        public IActionResult Log()
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
            var filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "Signatures"), fileName);

            bmpSign.Save(filePath, ImageFormat.Png);

            return Content(fileName);
        }
    }
}
