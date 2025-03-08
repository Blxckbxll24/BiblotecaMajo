using Bibloteca.Models.Domain;

namespace Bibloteca.Servicios.IServices
{
    public interface ICategoryServices
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> CreateCategory(Category category);
        Task<Category> UpdateCategory(int id, Category updatedCategory);
        Task<bool> DeleteCategory(int id);
    }
}
