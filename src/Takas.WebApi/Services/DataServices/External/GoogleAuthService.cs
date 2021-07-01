using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces.External;

namespace Takas.WebApi.Services.DataServices.External
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private const string UserInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=";
        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<GoogleUserInfoResult> GetUserInfoAsync(string accessToken)
        {
            var fullUrl = UserInfoUrl + accessToken;
            var result = await _httpClientFactory.CreateClient().GetAsync(fullUrl);
            result.EnsureSuccessStatusCode();
            var responseAsString = await result.Content.ReadAsStringAsync();
            var googleUser = JsonConvert.DeserializeObject<GoogleUserInfoResult>(responseAsString);

            return googleUser;
        }
    }
}
