using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ReportingService
{
    public static class ValidationResultExtension
    {
        public static ObjectResult ToWebError(this ValidationResult result)
        {
            List<string> errors = new();
            foreach (var failure in result.Errors)
            {
                errors.Add("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            }
            return new BadRequestObjectResult(errors);
        }
    }
}
