using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public class CategoriesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Category> _categories;
        public CategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
            _categories = context.categories;
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categories.ToListAsync();
        }
        public async Task<Category?> GetCategory(Guid id)
        {
            return await _categories.FindAsync(id);
        }
        public async Task<Category> AddCategory(Category category)
        {
            var entry = await _categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Category?> UpdateCategory(Guid id, Category category)
        {
            var existingCategory = await _categories.FindAsync(id);
            if (existingCategory == null)
            {
                return null;
            }
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            await _context.SaveChangesAsync();
            return existingCategory;
        }
        public async Task<Category?> DeleteCategory(Guid id)
        {
            var existingCategory = await _categories.FindAsync(id);
            if (existingCategory == null)
            {
                return null;
            }
            _categories.Remove(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }
        public async void saveChanges()
        {
            await _context.SaveChangesAsync();
        }   
    }
}
