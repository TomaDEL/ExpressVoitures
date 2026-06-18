using ExpressVoitures.Domain;

namespace ExpressVoitures.Services
{
    public interface IRepairTypeService
    {
        Task<IEnumerable<RepairType>> GetAllRepairTypesAsync();
        Task<RepairType?> GetRepairTypeByIdAsync(int id);
        Task AddRepairTypeAsync(RepairType repairType);
        Task UpdateRepairTypeAsync(RepairType repairType);
        Task DeleteRepairTypeAsync(int id);
    }
}
