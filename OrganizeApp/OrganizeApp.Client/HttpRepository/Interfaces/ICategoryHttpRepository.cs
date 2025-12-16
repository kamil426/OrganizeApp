using OrganizeApp.Shared.Category.Commands;
using OrganizeApp.Shared.Category.Dtos;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;

namespace OrganizeApp.Client.HttpRepository.Interfaces
{
    public interface ICategoryHttpRepository
    {
        Task Add(AddCategoryCommand command);
        Task<IList<CategoryDto>> GetCategories(string userId);
        Task<EditCategoryCommand> GetEditCategory(int id, string userId);
        Task Edit(EditCategoryCommand command);
        Task Delete(int id, string userId);

    }
}
