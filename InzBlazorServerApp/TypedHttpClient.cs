using Inz.DTOModel;
using System.Text.Json;
using System.Text;
using System.Net;

namespace InzBlazorServerApp
{
    public class TypedHttpClient
    {
        private readonly HttpClient _httpClient;

        public TypedHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetLoginToken(LoginDTO loginDTO)
        {
            var apiUri = "api/Login";

            var jsonLogin = JsonSerializer.Serialize(loginDTO);

            var httpContent = new StringContent(jsonLogin, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUri, httpContent);

            var stringResult = response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            { 
                var token = stringResult.Result;
                return token;
            }

            return stringResult.Result;
        }

    }
}
