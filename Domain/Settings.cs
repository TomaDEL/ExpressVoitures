namespace ExpressVoitures.Domain
{
    public class Settings
    {
        public int Id { get; set; }

        // Plus-value ajoutée par Jacques sur chaque voiture (500€ par défaut)
        public decimal Markup { get; set; } = 500;
    }
}
