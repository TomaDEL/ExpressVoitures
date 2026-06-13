using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Attributes
{
    public class CurrentYearMaxAttribute : ValidationAttribute
    {
        private readonly int _minYear;

        public CurrentYearMaxAttribute(int minYear)
        {
            _minYear = minYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int year)
            {
                var currentYear = DateTime.Now.Year;
                if (year < _minYear || year > currentYear)
                {
                    return new ValidationResult($"L'année doit être comprise entre {_minYear} et {currentYear}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}