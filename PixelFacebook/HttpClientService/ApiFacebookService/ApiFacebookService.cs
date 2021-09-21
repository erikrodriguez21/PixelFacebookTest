using Microsoft.Extensions.Configuration;
using PixelFacebook.HttpClientService.DtoObjets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelFacebook.HttpClientService.ApiFacebookService
{
    public class ApiFacebookService
    {
        public IConfiguration _config { get; }
        private readonly HttpClientService _httpClient;
        private readonly string pixelId = "";
        private readonly string accessToken = "";
        private readonly string testEventCode = "";
        private readonly string urlApi = "https://graph.facebook.com/v11.0/";

        public ApiFacebookService(IConfiguration conf)
        {
            _config = conf;   
            _httpClient = new HttpClientService();
             
            //valores para la api
            pixelId = _config.GetSection("FBpixelId").Value;
            accessToken = _config.GetSection("FBaccessToken").Value;
            testEventCode = _config.GetSection("testEventCode").Value;
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
        public async Task<string> PostPixelFB(string order_id, string value, EventName eventName, string testEventCode = null) 
        {
            try
            {
                string url = "";
                string data = GetDataJson(order_id, value, eventName);
                
                if (string.IsNullOrEmpty(testEventCode))
                {
                    url = $"{urlApi + pixelId}/events?access_token={accessToken}&data={data}"; 
                }
                else
                {
                    url = $"{urlApi + pixelId}/events?access_token={accessToken}&data={data}&test_event_code={testEventCode}";
                }
                
                return await _httpClient.PostAsync(url, "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region methods
        public string GetDataJson(string order_id, string value, EventName eventName)
        {
            try
            {
                var model = new ApiFacebookDto();
                var data = new Datum();
                var custom = new Custom_Data();
                custom.order_id = order_id;
                custom.value = value;
                custom.currency = "usd";
                data.action_source = "website";
                data.event_source_url = "https://localhost:44304/Home/Paso8";
                data.event_name = eventName.ToString();
                data.event_id = eventName.ToString().Replace("_", "");

                data.event_time = DateTimeOffset.Now.ToUnixTimeSeconds();
                data.user_data = new User_Data();
                data.custom_data = custom;
                var list = new List<Datum>();
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
