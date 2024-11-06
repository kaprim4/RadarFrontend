using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RadarFrontend.Models;

namespace RadarFrontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("https://localhost:5001/api/") };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var payload = JsonConvert.SerializeObject(new { username, password });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/login", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<dynamic>(result).Token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)token);

            return token;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(content);
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            var response = await _httpClient.GetAsync("devices");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Device>>(content);
        }

        // Ajoutez d'autres méthodes pour gérer les rôles, dispositifs, etc.
    }
}

