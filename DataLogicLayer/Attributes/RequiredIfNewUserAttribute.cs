using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PizzashopMVCNtier.Attributes
{
    public class RequiredIfNewUserAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userIdProperty = validationContext.ObjectType.GetProperty("UserId");
            if (userIdProperty == null)
                throw new ArgumentException("Property 'UserId' not found.");

            var userIdValue = userIdProperty.GetValue(validationContext.ObjectInstance);
            bool isNewUser = userIdValue == null || (long)userIdValue == 0;

            if (isNewUser && string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? "Password is required for new users.");
            }

            return ValidationResult.Success;
        }

        // This method enables client-side validation
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-requiredifnewuser", ErrorMessage ?? "Password is required.");
        }
    }
}
