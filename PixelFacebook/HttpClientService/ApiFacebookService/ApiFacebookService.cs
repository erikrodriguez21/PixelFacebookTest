using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PixelFacebook.HttpClientService.DtoObjets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PixelFacebook.HttpClientService.ApiFacebookService
{
    public class ApiFacebookService
    {
        public IConfiguration _config { get; }
        private readonly HttpClientService _httpClient;
        private readonly string _pixelId = "";
        private readonly string _accessToken = ""; 
        //private readonly string _testEventCode = "";
        private readonly string _urlApi = "https://graph.facebook.com/v11.0/";

        public ApiFacebookService(IConfiguration conf)
        {
            _config = conf;   
            _httpClient = new HttpClientService();
             
            //valores para la api
            _pixelId = _config.GetSection("FBpixelId").Value;
            _accessToken = _config.GetSection("FBaccessToken").Value;
            //_testEventCode = _config.GetSection("testEventCode").Value;
        }

        public async Task<string> testApi(string url, HttpContext context)
        {
            try
            {
                string clientIp = _httpClient.GetClientIPAddress(context);
                if (clientIp == "::1")
                {
                    clientIp = "127.0.0.1";
                }
                var res = await _httpClient.GetExternalIp();
                return $"Client IP: {clientIp} \nApp Serice IP: {res} \n" + await _httpClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// Petición POST para pixel de facebook
        /// <example>
        /// ejemplo:
        /// <code>                
        ///     string result = await _apiFB.PostPixelFB("11", "15000000", ApiFacebookService.EventName.Paso_8);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="value"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public async Task<string> PostPixelFB(string accessToken, string pixelID, string value, EventName eventName, string userAgent, string email, string urlSource,
           string clientIp, string guid, string testEventCode = null)
        {
            try
            {
                string url = "";
                string data = GetDataJson(value, eventName, userAgent, email, urlSource, clientIp, guid);

                if (string.IsNullOrEmpty(testEventCode))
                {
                    url = $"{_urlApi + pixelID}/events?access_token={accessToken}&data={data}";
                }
                else
                {
                    url = $"{_urlApi + pixelID}/events?access_token={accessToken}&data={data}&test_event_code={testEventCode}";
                }

                return await _httpClient.PostAsync(url, "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetClientIp(HttpContext context)
        {
            return _httpClient.GetClientIPAddress(context);
        }


        #region methods
        public string GetDataJson(string value, EventName eventName, string userAgent, string email, string urlSource, string clientIp, string guid)
        {
            try
            {
                var model = new ApiFacebookDto();
                var data = new Datum();
                var custom = new Custom_Data();
                var list = new List<Datum>();
                //monto
                custom.value = value;
                custom.currency = eventName != EventName.Solicitud_Enviada ? null : "mxn";
                //action source
                data.action_source = "website";
                //user data
                data.user_data = new User_Data();
                data.user_data.client_ip_address = clientIp; //optiene la ip del cliente
                data.user_data.client_user_agent = userAgent;
                data.user_data.em = new string[] { email };
                //event
                data.event_id = eventName.ToString().Replace("_", "") + guid;
                data.event_source_url = urlSource;
                data.event_name = eventName.ToString();
                data.event_time = DateTimeOffset.Now.ToUnixTimeSeconds();
                data.custom_data = custom;
                list.Add(data);
                model.data = list;

                return _httpClient.SerializeObjet(model.data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region enums
        public enum EventName
        {
            Paso_1 = 1,
            Paso_5 = 5,
            Paso_8 = 8,
            Solicitud_Enviada = 9

        }
        #endregion
    }
}
