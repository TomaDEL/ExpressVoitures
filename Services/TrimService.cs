using ExpressVoitures.Data;
using ExpressVoitures.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Services
{
    public class TrimService : ITrimService
    {
        private readonly ApplicationDbContext _context;

        public TrimService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trim>> GetAllTrimsAsync()
        {
            return await _context.Trims
                .Include(t => t.CarModel)
                .ToListAsync();
        }

        // Récupère uniquement les finitions d'un modèle précis
        public async Task<IEnumerable<Trim>> GetTrimsByCarModelIdAsync(int carModelId)
        {
            return await _context.Trims
                .Where(t => t.CarModelId == carModelId)
                .ToListAsync();
        }

        public async Task<Trim?> GetTrimByIdAsync(int id)
        {
            return await _context.Trims
                .Include(t => t.CarModel)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTrimAsync(Trim trim)
        {
            _context.Trims.Add(trim);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTrimAsync(Trim trim)
        {
            _context.Trims.Update(trim);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTrimAsync(int id)
        {
            var trim = await _context.Trims.FindAsync(id);
            if (trim != null)
            {
                _context.Trims.Remove(trim);
                await _context.SaveChangesAsync();
            }
        }
    }
}
