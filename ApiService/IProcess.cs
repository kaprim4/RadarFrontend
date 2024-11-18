using Domain.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ApiService
{
    public class IProcess<T> : ApiServiceBase where T : class
    {
        private readonly string _baseUrl = "http://localhost:5000/api/";
        private string _controller;

        public IProcess(string controller = "")
        {
            _controller = controller;
        }

        public IProcess<T> SetController(string controller)
        {
            _controller = controller;
            return this;
        }

        /// <summary>
        /// Process an API request and deserialize the response into a single object of type T.
        /// </summary>
        public async Task<ResponseModel<T>> ProcessAsync(dynamic obj, RequestType type, EndPoint endPoint, bool isAuthorize = false, string token = "")
        {
            ResponseModel<T> output = new(true);
            try
            {
                var response = await ConsumeApiAsync(obj, type, endPoint, isAuthorize, token);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ResponseModel<T>>(response);
                }
            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
            }

            return output;
        }

        

        /// <summary>
        /// Process an API request and deserialize the response into a list of objects of type T.
        /// </summary>
        public async Task<ResponseModel<T>> ProcessListAsync(dynamic requestObj, RequestType type, EndPoint endPoint, bool isAuthorize = false, string token = "")
        {
            ResponseModel<T> output = new(true);
            try
            {
                var response = await ConsumeApiAsync(requestObj, type, endPoint, isAuthorize, token);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ResponseModel<List<T>>>(response);
                }
            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
            }

            return output;
        }

        /// <summary>
        /// Authenticate a user and return an authentication response.
        /// </summary>
        public async Task<ResponseAuthModel<object>> AuthAsync(dynamic obj)
        {
            ResponseAuthModel<object> output = new(true);
            try
            {
                var response = await ConsumeApiAsync(obj, RequestType.Post, EndPoint.Login);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ResponseAuthModel<object>>(response);
                }
            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
            }

            return output;
        }

        /// <summary>
        /// Core method for making API requests.
        /// </summary>
        private async Task<dynamic> ConsumeApiAsync(dynamic payload, RequestType type, EndPoint endPoint, bool isAuthorize = false, string token = "")
        {
            using HttpClient client = new();

            if (isAuthorize)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(_baseUrl + _controller + "/");
            string _endPoint = endPoint.ToString();
            string json = payload != null ? JsonConvert.SerializeObject(payload) : string.Empty;
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = type switch
                {
                    RequestType.Get => await client.GetAsync(_endPoint),
                    RequestType.Post => await client.PostAsync(_endPoint, content),
                    RequestType.Patch => await client.PatchAsync(_endPoint, content),
                    RequestType.Put => await client.PutAsync(_endPoint, content),
                    RequestType.Delete => await client.DeleteAsync(_endPoint),
                    _ => throw new NotImplementedException($"Request type {type} is not implemented.")
                };

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    OnUnauthorizedAccessDetected();
                    return null;
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

            return null;
        }
    }
}

