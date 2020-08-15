using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Routine.APi.Entities;
using Routine.APi.ValidationAttributes;

namespace Routine.APi.Models
{
    [EmployeeNoMustDifferentFromFirstName(ErrorMessage = "employee id must not same as name")]
    public abstract class EmployeeAddOrUpdateDto: IValidatableObject
    {
        [Display(Name  = "Employee ID")]
        [Required(ErrorMessage = "{0}must fill in")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "{0} length is {1}")]
        public string EmployeeNo { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} must fill in")]
        [MaxLength(50, ErrorMessage = "{0} length no more than {1}")]
        public string FirstName { get; set; }

        [Display(Name = "Name"), Required(ErrorMessage = "{0} must fill in"),
        MaxLength(50, ErrorMessage = "{0} length no more than {1}")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "date of birth")]
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                yield return new ValidationResult("First name and last name must not same", new []{ nameof(LastName), nameof(FirstName)});
            }
        }
    }
}
