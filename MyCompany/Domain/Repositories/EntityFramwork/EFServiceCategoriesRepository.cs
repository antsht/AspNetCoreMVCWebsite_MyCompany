using Microsoft.EntityFrameworkCore;
using MyCompany.Domain.Entities;
using MyCompany.Domain.Repositories.Abstract;

namespace MyCompany.Domain.Repositories.EntityFramwork
{
    public class EFServiceCategoriesRepository(AppDbContext context) : IServiceCategoriesRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<ServiceCategory>> GetServiceCategoriesAsync()
        {
            return await _context.ServiceCategories.Include(x => x.Services).ToListAsync();
        }

        public async Task SaveServiceCategoryAsync(ServiceCategory entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceCategory?> GetServiceCategorieByIdAsync(int id)
        {
            return await _context.ServiceCategories.Include(x => x.Services).FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task DeleteServiceCategoryAsync(int id)
        {
            _context.Entry(new ServiceCategory() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

        }

    }
}
