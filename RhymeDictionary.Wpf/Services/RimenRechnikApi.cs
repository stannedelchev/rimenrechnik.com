using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace RhymeDictionary.Wpf.Services
{
    internal class RimenRechnikApi
    {
        private readonly string baseApiUrl;
        private readonly string baseTokenUrl;

        public RimenRechnikApi(string baseApiUrl, string baseTokenUrl)
        {
            this.baseApiUrl = baseApiUrl;
            this.baseTokenUrl = baseTokenUrl;
        }

        public async Task<AddWordResult> AddWordAsync(string word)
        {
            AddWordResult result = null;
            var token = await this.GetAccessTokenAsync();

            using (var client = new HttpClient() { BaseAddress = new Uri(this.baseApiUrl) })
            {
                client.SetBearerToken(token);

                var json = JsonConvert.SerializeObject(new AddWordInputModel(word));
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var reqResult = await client.PostAsync("words", content);
                var resContent = await reqResult.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<AddWordResult>(resContent);
            }

            return result;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync(this.baseTokenUrl);
            if (disco.IsError)
            {
                throw disco.Exception ?? new Exception(disco.Error);
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, ApplicationWideSettings.ClientId, ApplicationWideSettings.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("admin-api");

            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception ?? new Exception(tokenResponse.Error);
            }

            return tokenResponse?.AccessToken;
        }
    }
}
