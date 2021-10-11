using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
        private readonly ApiFacebookService _apiFB;
        
        string testEvntCode;
        public HomeController(ILogger<HomeController> logger, ApiFacebookService api, IConfiguration config)
        {
            _config = config;
            _logger = logger;
            _apiFB = api;
        }

        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Paso1() 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("GuidPIxelFacebook")))
            {
                HttpContext.Session.SetString("GuidPIxelFacebook", GetNewGUID());
            }
            return View();
        }

        public IActionResult Paso5() 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("GuidPIxelFacebook")))
            {
                HttpContext.Session.SetString("GuidPIxelFacebook", GetNewGUID());
            }
            return View();
        }

        public IActionResult Paso8() 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("GuidPIxelFacebook")))
            {
                HttpContext.Session.SetString("GuidPIxelFacebook", GetNewGUID());
            }
            return View();
        }

        [HttpPost] 
        public IActionResult Paso8(int id) 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("GuidPIxelFacebook")))
            {
                HttpContext.Session.SetString("GuidPIxelFacebook", GetNewGUID());
            }
            return View();
        }

        /// revisar y terminar la peticion
        public async Task<JsonResult> PixelFacebook(string eventName, string userAgent, string email, string urlSource, string monto = "0")
        {
            try
            {
                string guid = HttpContext.Session.GetString("GuidPIxelFacebook");

                // Requires: using Microsoft.AspNetCore.Http;
                if (string.IsNullOrEmpty(guid))
                {
                    HttpContext.Session.SetString("GuidPIxelFacebook", GetNewGUID());
                    guid = HttpContext.Session.GetString("GuidPIxelFacebook");
                }
               

                string testEvntCode = "", jsonRes = "";
                string clientIp = _apiFB.GetClientIp(HttpContext);               
                ApiFacebookService.EventName evtName;

#if TEST || DEBUG
                testEvntCode = "TEST12197"; //codigo de prueba que proporciona facebook, buscar ambiente de desarrollo 
#endif

                string FBpixelId = "2052647431560475";
                string FBaccessToken = "EAAotId4ZAUvQBAJ8ARsrJFzOZApY4tMudroZC4L0qJPAz34vpFHufJUnGOvQGqouPgCQlKvvmFjgb7NzQruSFdNaisroVkkV91jZBwYcKCI3COqTS0GMn4ggukP03CdqrlgtlTSCulMiF3k1AMbZAEbJVbpTAKMBzIIps4r0ux5sMlbU1jeu7";

                if (string.IsNullOrEmpty(FBpixelId) || string.IsNullOrEmpty(FBaccessToken))
                {
                    throw new Exception("No se encontraron datos para FBaccessToken y FBpixelId en las configuraciones");
                }

                evtName = (ApiFacebookService.EventName)Enum.Parse(typeof(ApiFacebookService.EventName), eventName);

                jsonRes = await _apiFB.PostPixelFB(FBaccessToken, FBpixelId, monto, evtName, userAgent, email, urlSource, clientIp, guid, testEvntCode);
                return Json(jsonRes);
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        public JsonResult GetPixelId()
        {
            try
            {
                string pixelId = "2052647431560475"; //pixel id del layout
                return Json(pixelId);
            }
            catch (Exception ex)
            {                
                throw;
            }
        }



        public JsonResult GetFBpixelIdLayout()
        {
            try
            {
                string pixelId = "";
                return Json(pixelId);
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
        public async Task<JsonResult> Test() 
        {
            try
            {
                string jsonRes = await _apiFB.testApi("https://jsonplaceholder.typicode.com/users", HttpContext);
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


        public string GetNewGUID()
        {
            try
            {
                Guid guid = Guid.NewGuid();
                return guid.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
