using AngularMoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Entities
{
    public class Genre: IValidatableObject
    {
        //[Range(1, 9999)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The field with name {0} is required")] // {0} is representing the name
        [StringLength(30)]
        [FirstLetterUpperCase]
        public string Name { get; set; }

        // model validations
        // custom validation attribute -> implemention in folder AngularMoviesAPI.Validations
        // When impement it, we don't need to add attribute annotation, but auto complete model validation
        // cons !!!: because it is created in the model class thus it cannot be used among other models.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Name))
            {
                var firstLetter = Name[0].ToString();
                if(firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("FirstLetter is not capital", new string[] { nameof(Name) });
                }
            }
        }
    }
}
