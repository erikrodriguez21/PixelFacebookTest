using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public IConfiguration _config { get; }
        private readonly ILogger<HomeController> _logger;
        private readonly ApiFacebookService _api;
        
        string testEvntCode;
        public HomeController(ILogger<HomeController> logger, ApiFacebookService api, IConfiguration config)
        {
            _config = config;
            _logger = logger;
            _api = api;
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

        /// revisar y terminar la peticion
        public async Task<JsonResult> PixelFacebook(string eventName, string userAgent, string email, string urlSource, string monto = "0")
        {
            try
            {
                
                testEvntCode = _config.GetSection("testEventCode").Value; 
                string jsonRes = "";

                if (eventName == ApiFacebookService.EventName.Paso_1.ToString())
                {
                    jsonRes = await _api.PostPixelFB(monto, ApiFacebookService.EventName.Paso_1, userAgent, email, urlSource, testEvntCode);
                }

                if (eventName == ApiFacebookService.EventName.Paso_5.ToString())
                {
                    jsonRes = await _api.PostPixelFB(monto, ApiFacebookService.EventName.Paso_5, userAgent, email, urlSource, testEvntCode);
                }

                if (eventName == ApiFacebookService.EventName.Paso_8.ToString())
                {
                    jsonRes = await _api.PostPixelFB(monto, ApiFacebookService.EventName.Paso_8, userAgent, email, urlSource, testEvntCode);
                }

                if (eventName == ApiFacebookService.EventName.Solicitud_Enviada.ToString())
                {
                     jsonRes = await _api.PostPixelFB(monto, ApiFacebookService.EventName.Solicitud_Enviada, userAgent, email, urlSource, testEvntCode);
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

        public async Task<JsonResult> Test() 
        {
            try
            {
                //string ip = HttpContext.Connection.RemoteIpAddress.ToString();
                string jsonRes = await _api.testApi("https://jsonplaceholder.typicode.com/users");
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

        public IActionResult jsResult(string evnt) 
        {
           
            string script = "var userAgent = window.navigator.userAgent;" +
                "alert('"+ evnt +" --- ' + userAgent)";
            return new JavaScriptResult(script);
        }

        public class JavaScriptResult : ContentResult
        {
            public JavaScriptResult(string script)
            {
                this.Content = script;
                this.ContentType = "application/javascript";
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
