using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Domain
{
    public class Trim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la finition est obligatoire.")]
        public string Name { get; set; } = string.Empty;

        // Clé étrangère : une finition appartient à un modèle de voiture
        public int CarModelId { get; set; }

        // Navigation vers le modèle parent
        public CarModel? CarModel { get; set; }

        // Navigation vers les voitures ayant cette finition
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
