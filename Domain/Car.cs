using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Attributes;

namespace ExpressVoitures.Domain
{
    public class Car
    {
        public int Id { get; set; }

        // Numéro d'identification unique du véhicule (gravé sur la voiture)
        public string? VIN { get; set; }

        [Required]
        [CurrentYearMax(1990, ErrorMessage = "L'année doit être entre 1990 et l'année actuelle.")]
        public int Year { get; set; }

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
        [Precision(8, 2)]
        public decimal PurchasePrice { get; set; }

        // Clé étrangère : à quel modèle de voiture appartient cette voiture
        public int CarModelId { get; set; }

        // Navigation : accès au modèle de voiture parente
        public CarModel? CarModel { get; set; }

        // Navigation : vers les réparations de cette voiture
        public ICollection<CarRepair> CarRepairs { get; set; } = new List<CarRepair>();
    }
}
