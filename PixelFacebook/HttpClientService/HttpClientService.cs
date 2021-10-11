using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PixelFacebook.HttpClientService
{
    public class HttpClientService
    {
        #region GET
        /// <summary>
        /// Petición get 
        /// <example>
        /// ejemplo:
        /// <code>                
        ///     var res = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
        /// </code>
        /// </example>
        /// </summary>        
        /// <param name="MethodWithParameters"></param>       
        /// <param name="accessToken"></param>
        /// <returns>string</returns>      
        public async Task<string> GetAsync(string MethodWithParameters, string accessToken = null)
        {
            try
            {                
                string response = "";
                using (var client = new HttpClient())
                {
                    CertificateValidation();
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!string.IsNullOrEmpty(accessToken))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var result = await client.GetAsync(MethodWithParameters);
                    var resultContent = await result.Content.ReadAsStringAsync();
                    response = resultContent;
                    if (result.IsSuccessStatusCode)
                    {
                        return resultContent;
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region POST
        /// <summary>
        /// Petición post con serialización de objeto
        /// <example>
        /// ejemplo:
        /// <code>
        ///    Person person = new Person();
        ///    person.id = 1;
        ///    person.title = "";
        ///    person.body = "";
        ///    person.userId = 1;
        ///
        ///    var res2 = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", person);
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="MethodWithParameters"></param>
        /// <param name="Object"></param>
        /// <param name="accessToken"></param>
        /// <returns>string</returns>
        public async Task<string> PostAsync<T>(string MethodWithParameters, T Object, string accessToken = null)
        {
            try
            {
                string response = "";
                using (var client = new HttpClient())
                {
                    CertificateValidation();
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!string.IsNullOrEmpty(accessToken))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    string jsonparameters = JsonConvert.SerializeObject(Object);

                    var content = new StringContent(jsonparameters, System.Text.Encoding.UTF8, "application/json");

                    var result = await client.PostAsync(MethodWithParameters, content);
                    var resultContent = await result.Content.ReadAsStringAsync();
                    response = resultContent;
                    if (result.IsSuccessStatusCode)
                    {
                        return resultContent;
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Petición post con body tipo json
        /// <example>
        /// ejemplo:
        /// <code>
        /// string person = "{\"id\": 1,\"title\": \"...\",\"body\": \"...\",\"userId\": 1}";
        ///
        /// var res2 = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", person);
        /// </code>
        /// </example>
        /// </summary>       
        /// <param name="MethodWithParameters"></param>
        /// <param name="body"></param> 
        /// <param name="accessToken"></param>
        /// <returns>string</returns>       
        public async Task<string> PostAsync(string MethodWithParameters, string body, string accessToken = null)
        {
            try
            {
                string response = "";
                using (var client = new HttpClient())
                {
                    CertificateValidation();
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!string.IsNullOrEmpty(accessToken))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

                    var result = await client.PostAsync(MethodWithParameters, content);
                    var resultContent = await result.Content.ReadAsStringAsync();
                    response = resultContent;
                    if (result.IsSuccessStatusCode)
                    {
                        return resultContent;
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Serialize / deserialize objet
        public string SerializeObjet<T>(T Object)
        {
            try
            {
                return JsonConvert.SerializeObject(Object);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public T DeserializeObject<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Methods
        public string GetClientIPAddress(HttpContext context)
        {
            string ipAddress = context.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(':');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }            
            string[] result = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString().Split(':');
            if (result.Length == 0 || result[0] == "127.0.0.1" || result[0] == "")
            {
                return "123.123.123.123";
            }
            return result[0];
        }

        public async Task<string> GetExternalIp() 
        {
            return await GetAsync("https://api.ipify.org/");
        }

        public void CertificateValidation()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
        }
        #endregion
    }
}
