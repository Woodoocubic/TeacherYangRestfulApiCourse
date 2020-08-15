using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.APi.Models;
using System.ComponentModel.DataAnnotations;

namespace Routine.APi.ValidationAttributes
{
    public class EmployeeNoMustDifferentFromFirstNameAttribute: ValidationAttribute
    {
        //attribute, value->attribute value, on class, value is object
        //validationContext is object of attribute
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employeeAddDto = (EmployeeAddOrUpdateDto) validationContext.ObjectInstance;
            if (employeeAddDto.EmployeeNo == employeeAddDto.FirstName)
            {
                return new ValidationResult(ErrorMessage, new []{ nameof(EmployeeAddOrUpdateDto)});
            }

            return ValidationResult.Success;
        }

    }
}
