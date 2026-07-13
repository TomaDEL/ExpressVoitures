using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Domain;

namespace ExpressVoitures.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Chaque DbSet correspond à une table dans la base de données
        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Trim> Trims { get; set; }
        public DbSet<RepairType> RepairTypes { get; set; }
        public DbSet<CarRepair> CarRepairs { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // Désactive le "cascade delete" sur la relation Trim -> Car
            // pour éviter les chemins de cascade multiples
            builder.Entity<Car>()
                .HasOne(c => c.Trim)
                .WithMany(t => t.Cars)
                .HasForeignKey(c => c.TrimId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed : valeur initiale de la plus-valus de Jacques
            builder.Entity<Settings>().HasData(
                new Settings { Id = 1, Markup = 500 }
            );
        }
    }
}
