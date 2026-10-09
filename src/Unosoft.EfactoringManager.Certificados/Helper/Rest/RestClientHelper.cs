using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Unosoft.EfactoringManager.Certificados.Helper.Rest
{
    public sealed class RestClientHelper
    {
        public RestClientHelper()
        {

        }     

        public async Task<T> GetAsync<T>(string uri, Dictionary<string, string> additionalHeaders = null)
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
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<T>(responseBody);
                    }
                }
            }

        }

        public async Task<Tout> PostAsync<Tin, Tout>(string uri, Tin content, Dictionary<string, string> additionalHeaders = null) where Tin : class
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
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<Tout>(responseBody);
                    }
                }
            }

        }

        public async Task<string> DeleteAsync(string requestUri, Dictionary<string, string> additionalHeaders = null)
        {

            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
               

                using (HttpClient httpClient = new HttpClient(httpClientHandler))
                {
                    AddHeaders(httpClient, additionalHeaders);
                    var httpResponseMessage = await httpClient.DeleteAsync(requestUri);
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }

        }

        public async Task<string> PutAsync<T>(string requestUri, T request, Dictionary<string, string> additionalHeaders = null) where T : class
        {

            using (HttpClientHandler httpClientHandler = new HttpClientHandler())
            {
              
                using (HttpClient httpClient = new HttpClient(httpClientHandler))
                {
                    AddHeaders(httpClient, additionalHeaders);

                    var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    var httpContent = new StringContent(json);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var httpResponseMessage = await httpClient.PutAsync(requestUri, httpContent);
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }

        }

       


        public static string JoinUriSegments(string uri, params string[] segments)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return null;

            if (segments == null || segments.Length == 0)
                return uri;

            return segments.Aggregate(uri, (current, segment) => $"{current.TrimEnd('/')}/{segment.TrimStart('/')}");
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


        public async Task<Tout> Upload<Tout>(string uri, string filePath, Dictionary<string, string> additionalHeaders = null)
        {

            using (var client = new HttpClient())
            {
                AddHeaders(client, additionalHeaders);

                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    //Load the file and set the file's Content-Type header
                    var fileStreamContent = new StreamContent(File.OpenRead(filePath));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    //Add the file
                    multipartFormContent.Add(fileStreamContent, name: "file", fileName: Path.GetFileName(filePath));

                    //Send it
                    var response = await client.PostAsync(uri, multipartFormContent);
                    response.EnsureSuccessStatusCode();
                    
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Tout>(responseBody);
                    
                }
            }

        }

        static bool IsComplex(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsComplex(typeInfo.GetGenericArguments()[0]);
            }
            return !(typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(Guid))
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal)));
        }
    }
}
