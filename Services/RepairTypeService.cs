using ExpressVoitures.Domain;
using ExpressVoitures.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Services
{
    public class RepairTypeService : IRepairTypeService
    {
        private readonly ApplicationDbContext _context;
        public RepairTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RepairType>> GetAllRepairTypesAsync()
        {
            return await _context.RepairTypes.ToListAsync();
        }

        public async Task<RepairType?> GetRepairTypeByIdAsync(int id)
        {
            return await _context.RepairTypes.FindAsync(id);
        }

        public async Task AddRepairTypeAsync(RepairType repairType)
        {
            _context.RepairTypes.Add(repairType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRepairTypeAsync(RepairType repairType)
        {
            _context.RepairTypes.Update(repairType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRepairTypeAsync(int id)
        {
            var repairType = await _context.RepairTypes.FindAsync(id);
            if (repairType != null)
            {
                _context.RepairTypes.Remove(repairType);
                await _context.SaveChangesAsync();
            }
        }
    }
}