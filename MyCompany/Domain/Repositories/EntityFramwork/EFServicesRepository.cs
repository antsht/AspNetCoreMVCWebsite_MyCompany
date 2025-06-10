using Microsoft.EntityFrameworkCore;
using MyCompany.Domain.Entities;
using MyCompany.Domain.Repositories.Abstract;

namespace MyCompany.Domain.Repositories.EntityFramwork
{
    public class EFServicesRepository(AppDbContext context) : IServicesRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Service>> GetServicesAsync()
        {
            return await _context.Services.Include(x => x.ServiceCategory).ToListAsync();
        }

        public async Task SaveServiceAsync(Service entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            return await _context.Services.Include(x => x.ServiceCategory).FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task DeleteServiceAsync(int id)
        {
            _context.Entry(new Service() { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

        }

    }
}
