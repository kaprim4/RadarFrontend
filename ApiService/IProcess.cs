using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using Newtonsoft.Json;
using ApiService.DTO;

namespace ApiService
{
    public class IProcess<T>  where T : class
    {
        public string _table { get; set; }
        private readonly string _baseUrl;
        public IProcess(string table)
        {
            _table = table;
        }

        public async Task<ResponseModel> ProcessAsync(T obj, RequestType type, EndPoint endPoint)
        {
            ResponseModel output = new(true);
            try
            {
                var response = await ConsumeApiAsync<T>(obj, type, endPoint);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ResponseModel>(response);
                }
            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
            }

            return output;
        }

       


        private static async Task<dynamic> ConsumeApiAsync<T>(T entity, RequestType type, EndPoint endPoint) where T : class
        {
            using HttpClient client = new();

            client.BaseAddress = new Uri("https://yourapiurl.com");
            string _endPoint = endPoint.ToString();
            var json = JsonConvert.SerializeObject(entity);
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

    public enum RequestType
    {
        Get,
        Post,
        Patch,
        Put,
        Delete,
    }

    public enum EndPoint
    {
        List,
        Add,
        Update,
        Delete,
    }
}