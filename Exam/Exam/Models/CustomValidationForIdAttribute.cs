using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exam.Models
{
    public class CustomValidationForIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var id = validationContext.ObjectType.GetProperty("Id").GetValue(validationContext.ObjectInstance);
            if (value == null || !value.Equals(id))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("ParentId cann`t equals Id");
        }
    }
}