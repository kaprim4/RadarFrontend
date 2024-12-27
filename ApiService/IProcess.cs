using Domain.DTO;
using Microsoft.AspNetCore.Http;
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
        public async Task<ResponseModel<T>> ProcessAsync(dynamic obj, RequestType type, EndPoint endPoint, bool isAuthorize = false, string token = "", bool isUrlParam = false)
        {
            ResponseModel<T> output = new(true);
            try
            {
                var response = await ConsumeApiAsync(obj, type, endPoint, isAuthorize, token, isUrlParam);
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
        /// Process an API request and deserialize the response into a single object of type T.
        /// </summary>
        public async Task<A> ProcessAsync<A>(dynamic obj, RequestType type, EndPoint endPoint, bool isAuthorize = false, string token = "", bool isUrlParam = false, bool isMultipart = false) where A :class
        {
            try
            {
                var response = isMultipart ? await ConsumeApiMultiPartAsync(obj, type, endPoint, isAuthorize, token, isUrlParam) : await ConsumeApiAsync(obj, type, endPoint, isAuthorize, token, isUrlParam);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<A>(response);
                }
                
                
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
        }


        /// <summary>
        /// Process an API request and deserialize the response into a single object of type T.
        /// </summary>
        public async Task<T> ProcessGetByIdAsync(dynamic obj, RequestType type, EndPoint endPoint, bool isAuthorize = false, string token = "", bool isUrlParam = false)
        {
            try
            {
                var response = await ConsumeApiAsync(obj, type, endPoint, isAuthorize, token, isUrlParam);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<T>(response);
                }
            }
            catch (Exception ex)
            {
                //output.AddErrorMessage(ex.ToString());
            }

            return null;
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
                    return JsonConvert.DeserializeObject<ResponseModel<T>>(response);
                
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
        private async Task<dynamic> ConsumeApiAsync(dynamic payload, RequestType type, EndPoint endPoint, bool isAuthorize = false, string token = "", bool isUrlParam = false)
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
                    RequestType.Get => await client.GetAsync(_endPoint + (isUrlParam ? $"/{payload}" : "")),
                    RequestType.GetById => await client.GetAsync(_baseUrl + _controller +  $"/{payload}"),
                    RequestType.Post => await client.PostAsync(_endPoint, content),
                    RequestType.Patch => await client.PatchAsync(_endPoint, content),
                    RequestType.Put => await client.PutAsync(_endPoint, content),
                    RequestType.Delete => await client.DeleteAsync(_endPoint + $"/{payload}"),
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
                    return response;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

            return null;
        }


        private async Task<dynamic> ConsumeApiMultiPartAsync(
            Document dto,
            RequestType type,
            EndPoint endPoint,
            bool isAuthorize = false,
            string token = "",
            bool isUrlParam = false)
        {
            using HttpClient client = new();

            if (isAuthorize)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(_baseUrl + _controller + "/");
            string _endPoint = endPoint.ToString();

            var form = new MultipartFormDataContent();

            // Add files
            //foreach (var image in dto.Images)
            //{
            //    var fileContent = new StreamContent(image.OpenReadStream());
            //    fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType ?? "application/octet-stream");
            //    form.Add(fileContent, "Images", image.FileName);
            //}

            // Add DTO as string
            form.Add(new StringContent(JsonConvert.SerializeObject(dto)), "Dto");

            try
            {
                var response = await client.PostAsync(_endPoint, form);
                   

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

                    // Read response content
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Check if it's JSON
                    if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
                    {
                        try
                        {
                            // Deserialize JSON error details (if applicable)
                            var errorDetails = JsonConvert.DeserializeObject<Dictionary<string, object>>(errorContent);

                            // Log or process the error details
                            foreach (var key in errorDetails.Keys)
                            {
                                Console.WriteLine($"{key}: {errorDetails[key]}");
                            }
                        }
                        catch
                        {
                            // Handle cases where response body isn't JSON
                            Console.WriteLine($"Error Content: {errorContent}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error Content: {errorContent}");
                    }


                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return response;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }

            return null;
        }
        private MultipartFormDataContent CreateMultipartFormDataContent(object dto, List<IFormFile> files)
        {
            var multipartContent = new MultipartFormDataContent();

            // Add files
            if (files != null && files.Count > 0)
            {
                int index = 0;
                foreach (var file in files)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    multipartContent.Add(streamContent, $"Images[{index}]", file.FileName);
                    index++;
                }
            }

            // Add DTO as JSON string
            if (dto != null)
            {
                var jsonContent = dto;
                multipartContent.Add(new StringContent((string)jsonContent, Encoding.UTF8, "application/json"), "Dto");
            }

            return multipartContent;
        }

        //private MultipartFormDataContent CreateMultipartFormDataContent(object dto)
        //{
        //    var multipartContent = new MultipartFormDataContent();

        //    foreach (var property in dto.GetType().GetProperties())
        //    {
        //        var value = property.GetValue(dto);
        //        if (value == null) continue;

        //        if (value is IEnumerable<IFormFile> formFiles)
        //        {
        //            int index = 0;
        //            foreach (var formFile in formFiles)
        //            {
        //                var streamContent = new StreamContent(formFile.OpenReadStream());
        //                streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
        //                multipartContent.Add(streamContent, $"Images[{index}]", formFile.FileName);
        //                index++;
        //            }
        //        }
        //        else if (value is IFormFile singleFormFile)
        //        {
        //            var streamContent = new StreamContent(singleFormFile.OpenReadStream());
        //            streamContent.Headers.ContentType = new MediaTypeHeaderValue(singleFormFile.ContentType);
        //            multipartContent.Add(streamContent, "Images", singleFormFile.FileName);
        //        }
        //        else
        //        {
        //            multipartContent.Add(new StringContent(value is string ? value.ToString() : JsonConvert.SerializeObject(value)), property.Name);
        //        }
        //    }

        //    return multipartContent;
        //}






    }
}

