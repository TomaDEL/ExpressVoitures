using ExpressVoitures.Domain;

namespace ExpressVoitures.Services
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task SetUnavailableAsync(int id);
        Task<decimal> GetSellingPriceAsync(int carId);
    }
}
