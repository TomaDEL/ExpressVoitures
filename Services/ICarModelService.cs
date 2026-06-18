using ExpressVoitures.Domain;

namespace ExpressVoitures.Services
{
    public interface ICarModelService
    {
        Task<IEnumerable<CarModel>> GetAllCarModelsAsync();
        Task<IEnumerable<CarModel>> GetCarModelsByBrandIdAsync(int brandId);
        Task<CarModel?> GetCarModelByIdAsync(int id);
        Task AddCarModelAsync(CarModel carModel);
        Task UpdateCarModelAsync(CarModel carModel);
        Task DeleteCarModelAsync(int id);
    }
}
