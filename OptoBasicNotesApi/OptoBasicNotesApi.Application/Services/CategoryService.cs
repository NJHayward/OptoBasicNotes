using Microsoft.EntityFrameworkCore;
using OptoBasicNotesApi.Application.Interfaces;
using OptoBasicNotesApi.Core;
using OptoBasicNotesApi.Core.Models;

namespace OptoBasicNotesApi.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IList<Category>> FindByIdsAsync(IList<int> ids)
        {
            return await _context.Categories.Where(x => ids.Contains(x.Id))
                                            .ToListAsync();
        }

        public async Task<Category> FindByNameAsync(string categoryName)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName.ToUpper() == categoryName.ToUpper());
        }

        public async Task CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
}
