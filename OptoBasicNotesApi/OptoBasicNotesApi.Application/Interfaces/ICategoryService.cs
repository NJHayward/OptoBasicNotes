using OptoBasicNotesApi.Core.Models;

namespace OptoBasicNotesApi.Application.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all Categories
        /// </summary>
        /// <returns>List of category models</returns>
        Task<IList<Category>> GetAllAsync();

        /// <summary>
        /// Finds a category by its id
        /// </summary>
        /// <param name="ids">The id of the category to find</param>
        /// <returns>The category model</returns>
        Task<IList<Category>> FindByIdsAsync(IList<int> ids);

        /// <summary>
        /// Finds a category by name
        /// </summary>
        /// <param name="categoryName">The name of the category to find</param>
        /// <returns>The category model</returns>
        Task<Category> FindByNameAsync(string categoryName);

        /// <summary>
        /// Creates the category passed in
        /// </summary>
        /// <param name="category">The category to create</param>
        Task CreateCategoryAsync(Category category);
    }
}
