using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Dima.Web.Handlers
{
    public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<string>> LoginAsync(LoginRequest request)
        {
            var result = await _client.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
            return result.IsSuccessStatusCode ?
                new Response<string>("You are successfully logged in", (int)HttpStatusCode.OK, "You are successfully logged in") :
                new Response<string>(null, (int)HttpStatusCode.BadRequest, "Username or password incorrect");
        }

        public async Task LogoutAsync()
        {
            var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
            await _client.PostAsync("v1/identity/logout", emptyContent);
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var result = await _client.PostAsJsonAsync("v1/identity/register", request);
            return result.IsSuccessStatusCode ?
                new Response<string>("Registered successfully", (int)HttpStatusCode.OK, "Registered successfully") :
                new Response<string>(null, (int)HttpStatusCode.BadRequest, "Unable to register");
        }
    }
}
