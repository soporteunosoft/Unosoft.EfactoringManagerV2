using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.BillyServices.DTO;

namespace Unosoft.EFactoringManager.BillyServices.Helper.Rest
{
    class RestClientHelper
    {
        public RestClientHelper()
        {

        }
        public async Task<BillyResponse<T>> GetAsync<T>(string uri, Dictionary<string, string> additionalHeaders = null)
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {

                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    AddHeaders(client, additionalHeaders);

                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return new BillyResponse<T>()
                        {
                            Response = JsonConvert.DeserializeObject<T>(responseBody),
                            Status = response.StatusCode
                        };
                        //return JsonConvert.DeserializeObject<T>(responseBody);
                    }
                }
            }

        }
        public async Task<BillyResponse<Tout>> PostAsync<Tin, Tout>(string uri, Tin content, Dictionary<string, string> additionalHeaders = null) where Tin : class
        {

            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {

                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    AddHeaders(client, additionalHeaders);
                    var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(uri, serialized))
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return new BillyResponse<Tout>() {
                            Response = JsonConvert.DeserializeObject<Tout>(responseBody),
                            Status = response.StatusCode
                        };
                    }
                }
            }

        }
        private void AddHeaders(HttpClient httpClient, Dictionary<string, string> additionalHeaders)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            //No additional headers to be added
            if (additionalHeaders == null)
                return;

            foreach (KeyValuePair<string, string> current in additionalHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(current.Key, current.Value);
            }
        }
    }
}
