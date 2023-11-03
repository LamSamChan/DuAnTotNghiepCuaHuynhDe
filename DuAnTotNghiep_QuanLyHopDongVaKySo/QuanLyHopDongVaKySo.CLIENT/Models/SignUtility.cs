using System;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SuperSignature.Net.Standard;

namespace test.Models
{
    public class SignData
    {
        public string Data { get; set; }
        public string Smooth { get; set; }
    }

    public static class SignUtility
    {
        public static Bitmap GetSignatureBitmap(string signData, string signDataSmooth, IHttpContextAccessor contextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            var ctlSignature = new MouseSignature(contextAccessor, Path.Combine(hostingEnvironment.WebRootPath, "SuperSignature.MouseSignature.lic"))
            {
                SignHighQualityImage = true,
                SmoothSign = true
            };

            var bmpSign = ctlSignature.SaveSignature(signData, signDataSmooth);

            if (ctlSignature.HasError)
                throw new Exception(ctlSignature.InternalError);

            return bmpSign;
        }

    }
}