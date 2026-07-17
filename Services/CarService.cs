using ExpressVoitures.Domain;
using ExpressVoitures.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;
        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _context.Cars
                .Include(c => c.CarModel)
                    .ThenInclude(m => m.Brand)
                .Include(c => c.Trim)
                .Include(c => c.CarRepairs)
                    .ThenInclude(cr => cr.RepairType)
                .ToListAsync();
        }
        public async Task <Car?> GetCarByIdAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.CarModel)
                    .ThenInclude(m => m.Brand)
                .Include (c => c.Trim)
                .Include(c => c.CarRepairs)
                    .ThenInclude(cr => cr.RepairType)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task AddCarAsync(Car car)
        {
            // Date de disponibilité = maintenant
            car.AvailabilityDate = DateTime.Now;
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }
        public async Task SetUnavailableAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                car.IsAvailable = false;
                //Date de vente = maintenant
                car.SaleDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<decimal> GetSellingPriceAsync(int carId)
        {
            var car = await _context.Cars
                .Include(c => c.CarRepairs)
                .FirstOrDefaultAsync(c => c.Id == carId);

            if (car == null)
            {
                return 0;
            }
            var setting = await _context.Settings.FirstOrDefaultAsync();
            var markup = setting?.Markup ?? 500;
            var totalRepairs = car.CarRepairs.Sum(r => r.Cost);

            return car.PurchasePrice + totalRepairs + markup;
        }
        public async Task<Dictionary<int, decimal>> GetAllSellingPricesAsync(IEnumerable<Car> cars)
        {
            // Une seule requête BDD pour le markup
            var settings = await _context.Settings.FirstOrDefaultAsync();
            var markup = settings?.Markup ?? 500;

            // LINQ — calcul en mémoire, zéro requête supplémentaire
            // cars.Select crée une nouvelle liste transformée
            // ToDictionary la convertit en dictionnaire Id → Prix
            return cars.ToDictionary(
                car => car.Id,
                car => car.PurchasePrice + car.CarRepairs.Sum(r => r.Cost) + markup
            );
        }
    }
}
