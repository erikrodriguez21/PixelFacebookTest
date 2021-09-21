using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PixelFacebook.HttpClientService.ApiFacebookService;
using PixelFacebook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PixelFacebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiFacebookService _api;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _api = new ApiFacebookService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Paso1() 
        {
            return View();
        }

        public IActionResult Paso5() 
        {
            return View();
        }

        public IActionResult Paso8() 
        {
            return View();
        }

        [HttpPost] 
        public IActionResult Paso8(int id) 
        {
            return View();
        }


        public async Task<JsonResult> PixelFacebook(string eventName)
        {
            try
            {
                string jsonRes = "";

                if (eventName == ApiFacebookService.EventName.Paso_1.ToString())
                {
                    jsonRes = await _api.PostPixelFB("", "", ApiFacebookService.EventName.Paso_1);
                }

                if (eventName == ApiFacebookService.EventName.Paso_5.ToString())
                {
                    jsonRes = await _api.PostPixelFB("", "", ApiFacebookService.EventName.Paso_5);
                }

                if (eventName == ApiFacebookService.EventName.Paso_8.ToString())
                {
                    jsonRes = await _api.PostPixelFB("", "", ApiFacebookService.EventName.Paso_8);
                }

                if (eventName == ApiFacebookService.EventName.Solicitud_Enviada.ToString())
                {
                     jsonRes = await _api.PostPixelFB("1234", "10000", ApiFacebookService.EventName.Solicitud_Enviada);
                }


                return Json(jsonRes);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(ex.InnerException.Message);
                }
                else
                {
                    return Json(ex.Message);

                }
            }
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
