using ExpressVoitures.Domain;

namespace ExpressVoitures.Models
{
    public class CarIndexViewModel
    {
        public IEnumerable<Car> Cars { get; set; } = new List<Car>();
        public Dictionary<int, decimal> SellingPrices { get; set; } = new ();

        // Méthode utilitaire pour récupérer le prix d'une voiture
        public decimal GetSellingPrice(int carId)
        {
            return SellingPrices.TryGetValue(carId, out var price) ? price : 0;
        }
    }
}
