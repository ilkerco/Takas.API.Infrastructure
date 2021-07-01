using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces.External;

namespace Takas.WebApi.Services.DataServices.External
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private const string UserInfoUrl = "https://graph.facebook.com/v9.0/me?fields=id%2Cname%2Cemail%2Cpicture&access_token=";
        private readonly IHttpClientFactory _httpClientFactory;

        public FacebookAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
        {
            var fullUrl = UserInfoUrl + accessToken;
            var result = await _httpClientFactory.CreateClient().GetAsync(fullUrl);
            result.EnsureSuccessStatusCode();
            var responseAsString = await result.Content.ReadAsStringAsync();
            var facebookUser = JsonConvert.DeserializeObject<FacebookUserInfoResult>(responseAsString);

            return facebookUser;
        }
    }
}
