using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Domain
{
    public class RepairType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la réparation est obligatoire.")]
        public string Name { get; set; } = string.Empty;

        // Navigation vers toutes les utilisations de ce type de réparation
        public ICollection<CarRepair> CarRepairs { get; set; } = new List<CarRepair>();
    }
}
