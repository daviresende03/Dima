using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Net;
using System.Net.Http.Json;

namespace Dima.Web.Handlers
{
    public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);

        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/categories", request);
            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ??
                new Response<Category?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to create category");
        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            var result = await _httpClient.DeleteAsync($"v1/categories/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ??
                new Response<Category?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to delete category");
        }

        public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
            => await _httpClient.GetFromJsonAsync<PagedResponse<List<Category>?>>("v1/categories") ?? 
                new PagedResponse<List<Category>?>(null, (int) HttpStatusCode.BadRequest, "An error ocurred while trying to search categories");

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
            => await _httpClient.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}") ??
                new Response<Category?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to search category");

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync($"v1/categories/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ??
                new Response<Category?>(null, (int)HttpStatusCode.BadRequest, "An error ocurred while trying to update category");
        }
    }
}
