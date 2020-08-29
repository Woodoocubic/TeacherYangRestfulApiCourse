using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.APi.Models
{
    //input Dto
    //search, insert, update should use different Dto, for bussiness logic updating and refactoring
    public class CompanyAddDto
    {
        [Display(Name = "company name")]
        [Required(ErrorMessage = " {0} this is a must")]
        [MaxLength(100, ErrorMessage = " {0} must less than {1}")]
        public string Name { get; set; }

        [Display(Name = "company introduction")]
        [MinLength(10)]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "{0} range from {2} to {1}")]
        public string Introduction { get; set; }

        public ICollection<EmployeeAddDto> Employees { get; set; } 
            = new List<EmployeeAddDto>();
    }
}
