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

        public async Task<string> testApi(string url)
        {

            var res = await _httpClient.GetExternalIp();          
            return $"IP: {res} \n" + await _httpClient.GetAsync(url); 
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
        public async Task<string> PostPixelFB(string value, EventName eventName, string userAgent, string email, string urlSource, string testEventCode = null) 
        {
            try
            {
               
                string data = await GetDataJson(value, eventName, userAgent, email, urlSource), url = "";

                if (string.IsNullOrEmpty(testEventCode))
                {
                    url = $"{_urlApi + _pixelId}/events?access_token={_accessToken}&data={data}";
                }
                else
                {
                    url = $"{_urlApi + _pixelId}/events?access_token={_accessToken}&data={data}&test_event_code={testEventCode}";
                }

                return await _httpClient.PostAsync(url, "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region methods

        public async Task<string> GetDataJson(string value, EventName eventName, string userAgent, string email, string urlSource)
        {
            try
            {
                var model = new ApiFacebookDto();
                var data = new Datum();
                var custom = new Custom_Data();
                var list = new List<Datum>();
                //monto
                custom.value = value;
                custom.currency = "mxn";
                //action source
                data.action_source = "website";                
                //user data
                data.user_data = new User_Data();
                data.user_data.client_ip_address = await _httpClient.GetExternalIp(); //optiene la ip del servidor
                data.user_data.client_user_agent = userAgent;
                data.user_data.em = new string[] { email };
                //event
                data.event_id = eventName.ToString().Replace("_", "");
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
