using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Domain
{
    public class Repair
    {
        public int Id { get; set; }

        // Clé étrangère : à quelle voiture appartient cette réparation
        public int CarId { get; set; }

        [Required(ErrorMessage = "La description de la réparation est obligatoire.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le coût de la réparation est obligatoire.")]
        public decimal Cost { get; set; }

        // Navigation : accès à la voiture parente
        public Car? Car { get; set; }
    }
}
