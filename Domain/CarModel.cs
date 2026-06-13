using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Domain
{
    public class CarModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du modèle est obligatoire.")]
        public string Name { get; set; } = string.Empty;

        // Clé étrangère vers Brand
        public int BrandId { get; set; }

        // Navigation vers la marque parente
        public Brand? Brand { get; set; }

        // Navigation vers les voitures de ce modèle
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}