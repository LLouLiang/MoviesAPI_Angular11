using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Entities
{
    // Data Model
    // Data Transfer Object => DTOs
    public class Actor
    {
        [Required]
        [StringLength(120)]
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string biography { get; set; }
        public string picture { get; set; }
        public int id { get; set; }
        // Model validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var firstLetter = name[0].ToString();
                if (firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("FirstLetter is not capital", new string[] { nameof(name) });
                }
            }
        }
    }
    

}
