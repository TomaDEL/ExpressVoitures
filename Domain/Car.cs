using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace ExpressVoitures.Domain
{
    public class Car
    {
        public int Id { get; set; }

        // Numéro d'identification unique du véhicule (gravé sur la voiture)
        public string? VIN { get; set; }

        [Required]
        [Range(1990, 9999, ErrorMessage = "L'année doit être entre 1990 et l'année actuelle.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "La marque est obligatoire.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le modèle est obligatoire.")]
        public string Model { get; set; } = string.Empty;

        // Finition (ex : LE, Sport, XLT, ...)
        public string? Trim { get; set; }

        public string? Description { get; set; }

        // Chemin vers la photo de la voiture
        public string? PhotoUrl { get; set; }

        // True = voiture encore disponible à la vente
        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "La date d'achat est obligatoire.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Le prix d'achat est obligatoire.")]
        [Precision(18, 2)]
        public decimal PurchasePrice { get; set; }

        // Navigation : une voiture peut avoir plusieurs réparations
        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}
