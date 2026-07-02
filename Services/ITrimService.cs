using ExpressVoitures.Domain;

namespace ExpressVoitures.Services
{
    public interface ITrimService
    {
        Task<IEnumerable<Trim>> GetAllTrimsAsync();
        Task<IEnumerable<Trim?>> GetTrimsByCarModelIdAsync(int carModelId);
        Task<Trim?> GetTrimByIdAsync(int id);
        Task AddTrimAsync(Trim trim);
        Task UpdateTrimAsync(Trim trim);
        Task DeleteTrimAsync(int id);
    }
}
