using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Domain
{
    public class CarRepair
    {
        public int Id { get; set; }

        // Clé étrangère vers Car
        public int CarId { get; set; }

        // Clé étrangère vers RepairType
        public int RepairTypeId { get; set; }

        // Coût de la réparation
        [Precision(8, 2)]
        public decimal Cost { get; set; }

        //Navigations
        public Car? Car { get; set; }
        public RepairType? RepairType { get; set; }
    }
}
