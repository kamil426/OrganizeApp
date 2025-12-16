using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Category.Commands;
using OrganizeApp.Shared.Category.Dtos;
using System.Net.Http.Json;

namespace OrganizeApp.Client.HttpRepository
{
    public class CategoryHttpRepository : ICategoryHttpRepository
    {
        private readonly HttpClient _client;

        public CategoryHttpRepository(HttpClient client)
            => _client = client;

        public async Task Add(AddCategoryCommand command)
            => await _client.PostAsJsonAsync("category", command);

        public async Task Delete(int id, string userId)
            => await _client.DeleteAsync($"category/{id}/{userId}");

        public async Task Edit(EditCategoryCommand command)
            => await _client.PutAsJsonAsync("category/category", command);

        public async Task<IList<CategoryDto>> GetCategories(string userId)
            => await _client.GetFromJsonAsync<IList<CategoryDto>>($"category/categories/{userId}");

        public async Task<EditCategoryCommand> GetEditCategory(int id, string userId)
            => await _client.GetFromJsonAsync<EditCategoryCommand>($"category/category-edit/{id}/{userId}");
    }
}
