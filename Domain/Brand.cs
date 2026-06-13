using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Domain
{
    public class Brand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la marque est obligatoire.")]
        public string Name { get; set; } = string.Empty;

        // Navigation vers les modèles de cette marque
        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
    }
}
