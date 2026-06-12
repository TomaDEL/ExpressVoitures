using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Domain
{
    public class Settings
    {
        public int Id { get; set; }

        // Plus-value ajoutée par Jacques sur chaque voiture (500€ par défaut)
        [Precision(18, 2)]
        public decimal Markup { get; set; } = 500;
    }
}
