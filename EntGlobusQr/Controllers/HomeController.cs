using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EntGlobusQr.Models;
using QRCoder;
using System.Drawing;

namespace EntGlobusQr.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            

            Color blue = Color.FromArgb(255, 82, 157, 237);


            //for (int i = 1000; i <= 1600; i++)
            //{
            //    QRCodeGenerator qrGenerator = new QRCodeGenerator();

            //    QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{i}", QRCodeGenerator.ECCLevel.Q);
            //    QRCode qrCode = new QRCode(qrCodeData);

            //    Bitmap qrCodeImage = qrCode.GetGraphic(3, blue, Color.White, false);

            //    qrCodeImage.Save($"C:\\zipgrade\\qr\\{i}.jpg");
            //}

            

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
