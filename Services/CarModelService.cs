using ExpressVoitures.Domain;
using ExpressVoitures.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly ApplicationDbContext _context;
        public CarModelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarModel>> GetAllCarModelsAsync()
        {
            return await _context.CarModels
                .Include(m => m.Brand)
                .ToListAsync();
        }

        // Récupère uniquement les modèles d'une marque précise
        // C'est cette méthode qui alimente le menu déroulant dynamique
        public async Task<IEnumerable<CarModel>> GetCarModelsByBrandIdAsync(int brandId)
        {
            return await _context.CarModels
                .Where(m => m.BrandId == brandId)
                .ToListAsync();
        }

        public async Task<CarModel?> GetCarModelByIdAsync(int id)
        {
            return await _context.CarModels
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddCarModelAsync(CarModel carModel)
        {
            _context.CarModels.Add(carModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarModelAsync(CarModel carModel)
        {
            _context.CarModels.Update(carModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarModelAsync(int id)
        {
            var carModel = await _context.CarModels.FindAsync(id);
            if (carModel != null)
            {
                _context.CarModels.Remove(carModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}