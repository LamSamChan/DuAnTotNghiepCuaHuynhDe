using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Models;

namespace SuperSignature.MVC.Core.Components
{
    public class SignControl : ViewComponent
    {
        public SignControl()
        {
            
        }

        public IViewComponentResult Invoke()
        {
            var signModel = new SignatureModel
            {
                SignObject = "ctlSignature",
                SignWidth = 540,
                SignHeight = 300,
                SignBackGroundImage = "/images/background.png",
                SignBorderStyle = "dashed",
                SignPenColor = "#000000",
                SignPenSize = 3,
                SignBorderColor = "black",
                SignBorderWidth = 1,
                SignRequiredPoints = 50, // This will govern validity
                SignPenCursor = "/images/pen.cur",
                SignStartMessage = "Welcome !!!!",
                SignSuccessMessage = "Wow !!!!",
                SignErrorMessage = "Keep signing !!!!",
                SignBackColor = "transparent",
                SignRefreshImage = "/images/clear.png",
                SignSmooth = true,
                SignTransparent = true,
                SignZIndex = 1,
                SignImageScaleFactor = 1.0f
            };

            return View(signModel);
        }
    }
}
