using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using System.Net;
using System.Net.Http.Json;

namespace Dima.Web.Handlers
{
    public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/transactions", request);
            return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ??
                new Response<Transaction?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to create transaction");
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            var result = await _httpClient.DeleteAsync($"v1/transactions/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ??
                new Response<Transaction?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to delete transaction");
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
            => await _httpClient.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}") ??
                new Response<Transaction?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to search transaction");

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            const string format = "yyyy-MM-dd";
            var startDate = request.StartDate is not null ?
                request.StartDate.Value.ToString(format) :
                DateTime.Now.GetFirstDay().ToString(format);

            var endDate = request.EndDate is not null ?
                request.EndDate.Value.ToString(format) :
                DateTime.Now.GetLastDay().ToString(format);

            var url = $"v1/transactions?startDate={startDate}&endDate={endDate}";

            return await _httpClient.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url) ??
                new PagedResponse<List<Transaction>?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to search transactions by period"); ;
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ??
                new Response<Transaction?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to update transaction");
        }
    }
}
