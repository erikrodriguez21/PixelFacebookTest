using PixelFacebook.HttpClientService.DtoObjets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelFacebook.HttpClientService.ApiFacebookService
{
    public class ApiFacebookService
    {
        private readonly HttpClientService _httpClient;
        private readonly string pixelId = "387608306145949";
        private readonly string accessToken = "EAAFXZAl0f0v8BAP6U4auKRnR6gZCmDEyyIcngIOtR22ZCo75SrTNOrGcH1MSITokXixtNrhAsKfAoihHE0F14eAprtLZAcOzoZBDxBIdCH3vByPSH2DAoR9ZA474q71B1v5TjJSyttmzd9yPcyRCTs7BOxy5iEgU7g0HbGXMCZBfoAQk5U6JcjdEsvih0ZBJBPQZD";
        private readonly string urlApi = "https://graph.facebook.com/v11.0/";

        public ApiFacebookService()
        {
            _httpClient = new HttpClientService();
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
        /// <param name="idCredito"></param>
        /// <param name="monto"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public async Task<string> PostPixelFB(string idCredito, string monto, EventName eventName)
        {
            try
            {
                string data = GetDataJson(idCredito, monto, eventName);
                string url = $"{urlApi + pixelId}/events?access_token={accessToken}&data={data}"; //&test_event_code=TEST21206";
                return await _httpClient.PostAsync(url, "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region methods
        public string GetDataJson(string idCredito, string monto, EventName eventName)
        {
            try
            {
                var model = new ApiFacebookDto();
                var data = new Datum();
                var custom = new Custom_Data();
                custom.idCredito = idCredito;
                custom.monto = monto;
                data.action_source = "website";

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
